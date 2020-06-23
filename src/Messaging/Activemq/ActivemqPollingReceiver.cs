using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter
{
    class ActivemqPollingReceiver : IReceiver
    {
        readonly Object ObjectLocker;
        readonly Encoding messageEncoding;
        readonly Uri connectionUri;

        readonly int msgPollingLot;
        private DateTime nextTimeToExecute = DateTime.UtcNow;
        private int executeIntervalInSeconds;


        readonly AMQPBizTalkAdapterConnection connection;
        Queue<Apache.NMS.IMessage> inboundQueue;
        QueueTypeEnum queueType;
        MethodTracer tracer;
        string queueName;
        string subscriptionIdentifier;
        string routingKey;
        Apache.NMS.IConnection nmsConnection;
        Apache.NMS.ISession nmsession;
        Apache.NMS.IMessageConsumer consumer;
        private bool closed=false;

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
        public ActivemqPollingReceiver(AMQPBizTalkAdapterConnection connection,
           QueueTypeEnum queueType, string queue, string keyRouting, string subscriptionId, MethodTracer methodTracer, Encoding encoding, int lot, int intervalInSeconds)
        {
            this.executeIntervalInSeconds = intervalInSeconds;
            this.ObjectLocker = new object();
            this.messageEncoding = encoding;
            this.connectionUri = connection.ConnectionUri.Uri;
            this.msgPollingLot = lot;
        }
        public void AckDelivery(List<Apache.NMS.IMessage> messages)
        {
            foreach(Apache.NMS.IMessage msg in messages)
            {
                msg.Acknowledge();
            }
        }
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
            bool result = false;

            TimeOutHelper timeHelper = new TimeOutHelper(timeout);
            while (true)
            {
                lock (ObjectLocker)
                {
                    if (timeHelper.IsExpired || this.closed)
                    {
                        methodTracer.TraceReturn(false);
                        result = false;
                        methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "timeHelper.IsExpired ");
                        wcfMessage = null;
                        break;
                    }
                    if (DateTime.UtcNow >= this.nextTimeToExecute)
                    {
                        try
                        {
                            if (this.IsAvailablesMessages)
                            {
                                List<Apache.NMS.IMessage> eMsgs = this.DequeueMessages(msgPollingLot);
                                List<ReceiveMessage> messages = new List<ReceiveMessage>();
                                List<ulong> DeliveryTags = new List<ulong>();
                                foreach (var e in eMsgs)
                                {
                                    ReceiveMessage wso2Message = Helpers.Helper.GetReceiveMessage(e, methodTracer,
                                    this.messageEncoding, "GenNewGUID");
                                    if (wso2Message != null)
                                    {
                                        messages.Add(wso2Message);
                                        DeliveryTags.Add(wso2Message.DeliveryTag);
                                    }
                                }

                                wcfMessage = Helpers.Helper.CreateWcfMessage(messages, this.connectionUri);

                                result = true;
                                this.nextTimeToExecute = this.nextTimeToExecute.AddSeconds((double)this.executeIntervalInSeconds);

                                //Send Ack to WSO2
                                this.AckDelivery(eMsgs);

                                methodTracer.TraceReturn(true);

                                break;
                            }
                            else
                            {
                                methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("No message available nextTimeToExecute={0}", this.nextTimeToExecute.ToLongTimeString()));
                                while (DateTime.UtcNow >= this.nextTimeToExecute)
                                {
                                    this.nextTimeToExecute = this.nextTimeToExecute.AddSeconds((double)this.executeIntervalInSeconds);
                                }
                                result = false;
                                wcfMessage = null;
                            }

                        }
                        catch (Exception ex)
                        {

                            methodTracer.TraceException(ex);
                            while (this.nextTimeToExecute <= DateTime.UtcNow)
                            {
                                this.nextTimeToExecute = this.nextTimeToExecute.AddSeconds((double)this.executeIntervalInSeconds);
                            }
                            throw;
                        }
                    }

                }
                System.Threading.Thread.Sleep(1000);
            }
            return result;
        }

        public bool WaitForMessage(TimeSpan timeout, MethodTracer methodTracer)
        {
            return this.IsAvailablesMessages;
        }
    }
}
