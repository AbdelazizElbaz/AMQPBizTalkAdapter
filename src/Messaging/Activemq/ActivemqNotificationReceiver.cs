using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter.Messaging.AmqpNetLite
{
    class ActivemqNotificationReceiver : IReceiver
    {
        #region Private

        readonly object ObjectLocker;
        readonly Uri connectionUri;
        internal bool closed = false;
        readonly string messageId;
        readonly AMQPBizTalkAdapterConnection connection;
        Queue<Apache.NMS.IMessage> inboundQueue;
        QueueTypeEnum queueType;
        MethodTracer tracer;
        string queueName;
        string subscriptionIdentifier;
        string routingKey;
        Encoding messageEncoding;
        Apache.NMS.IConnection nmsConnection;
        Apache.NMS.ISession nmsession;
        Apache.NMS.IMessageConsumer consumer;

        void OnMessageCallback(Apache.NMS.IMessage message)
        {
            lock (ObjectLocker)
            {
                if (!this.closed)
                {
                    tracer.TraceData(TraceEventType.Verbose, "New Message arrived NMSMessageId={0} " + message.NMSMessageId);
                    inboundQueue.Enqueue(message);
                }
            }
        }
        public bool IsAvailablesMessages
        {
            get
            {

                lock (ObjectLocker)
                {
                    return inboundQueue.Count > 0;
                }
            }

        }
        public List<Apache.NMS.IMessage> DequeueMessages(int lot)
        {
            List<Apache.NMS.IMessage> msgs = new List<Apache.NMS.IMessage>();
            lock (ObjectLocker)
            {
                int count = inboundQueue.Count >= lot ? lot : inboundQueue.Count;
                for (int i = 0; i < count; i++)
                {
                    msgs.Add(inboundQueue.Dequeue());
                }
            }
            return msgs;
        }
        public void AckDelivery(Apache.NMS.IMessage e)
        {
            e.Acknowledge();
        }
        #endregion
        #region Public
        public void StartListener(TimeSpan timeout, MethodTracer methodTracer)
        {
            lock (ObjectLocker)
            {

                try
                {
                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Start listening uri= {0}", this.connectionUri));

                    this.nmsConnection = connection.CreateNmsConnectionFactory(timeout).CreateConnection();
                    this.nmsession = this.nmsConnection.CreateSession();

                    if (queueType == QueueTypeEnum.Topic)
                    {
                        //Topic 

                        //Declare a Topic to retrieve messages.
                        ActiveMQTopic activemqTopic = new ActiveMQTopic(this.queueName);
                        this.consumer = this.nmsession.CreateDurableConsumer(activemqTopic, this.subscriptionIdentifier, null, false);
                        this.consumer.Listener += OnMessageCallback;
                    }
                    else
                    {
                        //Queue

                        ActiveMQQueue activemqQueue = new ActiveMQQueue(this.queueName);
                        this.consumer = this.nmsession.CreateConsumer(activemqQueue);
                        this.consumer.Listener += OnMessageCallback;
                    }




                }
                catch (Exception exception)
                {
                    methodTracer.TraceData(TraceEventType.Error, "Error in RabbitMQConsumerBase.StartListener");
                    methodTracer.TraceException(exception);
                    throw;
                }
            }

        }

        public void StopListener(TimeSpan timeout, MethodTracer methodTracer)
        {
            methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Stop listening uri= {0}", this.connectionUri));
            this.closed = true;
            this.consumer.Close();
            this.nmsession.Close();
            this.nmsConnection.Close();
        }

        public bool TryReceive(TimeSpan timeout, MethodTracer methodTracer, out System.ServiceModel.Channels.Message wcfMessage)
        {
            TimeOutHelper timeHelper = new TimeOutHelper(timeout);
            bool result = false;
            wcfMessage = null;
            while (true)
            {
                lock (ObjectLocker)
                {
                    try
                    {
                        methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Start TryReceive uri= {0}", this.connectionUri));
                        //time Expired
                        if (timeHelper.IsExpired || this.closed)
                        {
                            methodTracer.TraceReturn(false);
                            methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "timeHelper.IsExpired ");
                            result = false;
                            break;
                        }
                        if (this.IsAvailablesMessages)
                        {
                            var e = this.DequeueMessages(1).FirstOrDefault();
                            if (e != null)
                            {
                                ReceiveMessage receiveMessage = Helpers.Helper.GetReceiveMessage(e, methodTracer,
                                    this.messageEncoding, this.messageId);
                                if (receiveMessage != null)
                                {
                                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("MessageReceived MessageId= [{0}] ", receiveMessage.BasicProperties.MessageId));
                                    //Return message to BizTalk
                                    wcfMessage = Helpers.Helper.CreateWcfMessage(receiveMessage, this.connectionUri);

                                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Send Ack NMSMessageId [{0}] to  Uri={1}",
                                    e.NMSMessageId, this.connectionUri));
                                    result = true;
                                    //Send Ack 
                                    AckDelivery(e);
                                    methodTracer.TraceReturn(result);
                                    break;
                                }
                                else
                                {
                                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Message body is  null, NMSMessageId= {0}", e.NMSMessageId));
                                    result = false;
                                }
                            }
                            else
                            {
                                methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "Message null or no message available");
                                result = false;
                            }
                        }
                        //}
                    }
                    catch (Exception ex)
                    {
                        methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("End TryReceive uri= {0}", this.connectionUri));
                        methodTracer.TraceException(ex);
                        throw;
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }

            methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("End TryReceive uri= {0}", this.connectionUri));
            return result;
        }

        public bool WaitForMessage(TimeSpan timeout, MethodTracer methodTracer)
        {
            return this.IsAvailablesMessages;
        }

        public ActivemqNotificationReceiver(AMQPBizTalkAdapterConnection connection,
            QueueTypeEnum queueType, string queue, string keyRouting, string subscriptionId, MethodTracer methodTracer, Encoding encoding, string msgId)
        {
            this.connection = connection; ;
            this.connectionUri = connection.ConnectionUri.Uri;
            this.tracer = methodTracer;
            this.queueType = queueType;
            this.ObjectLocker = new object();
            this.routingKey = keyRouting;
            this.queueName = queue;
            this.inboundQueue = new Queue<Apache.NMS.IMessage>();
            this.subscriptionIdentifier = subscriptionId;
            this.messageEncoding = encoding;
        }
        #endregion
    }
}
