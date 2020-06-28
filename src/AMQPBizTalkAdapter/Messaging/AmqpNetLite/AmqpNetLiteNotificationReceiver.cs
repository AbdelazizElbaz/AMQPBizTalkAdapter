using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter.Messaging.AmqpNetLite
{
    class AmqpNetLiteNotificationReceiver : AmqpNetLiteReceiver, IReceiver
    {
        #region Private

        readonly object ObjectLocker;
        readonly Uri connectionUri;
        internal bool closed = false;
        readonly string messageId;
        readonly AMQPBizTalkAdapterConnection connection;
        QueueTypeEnum queueType;
        MethodTracer tracer;
        string queueName;
        string subscriptionIdentifier;
        string routingKey;
        Encoding messageEncoding;
        #endregion
        #region Public
        public void StartListener(TimeSpan timeout, MethodTracer methodTracer)
        {
            lock (ObjectLocker)
            {

                try
                {
                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Start listening uri= {0}", this.connectionUri));

                    this.Start(timeout, methodTracer);
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
            this.Stop(timeout, methodTracer);
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
                                    receiveMessage.BasicProperties.MessageId, this.connectionUri));
                                    result = true;
                                    //Send Ack 
                                    AckDelivery(e);
                                    methodTracer.TraceReturn(result);
                                    break;
                                }
                                else
                                {
                                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Message body is  null, NMSMessageId= {0}", e.Properties.MessageId));
                                    result = false;
                                }
                            }
                            else
                            {
                                methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "Message null or no message available");
                                result = false;
                            }
                        }
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

        public AmqpNetLiteNotificationReceiver(AMQPBizTalkAdapterConnection connection,
            QueueTypeEnum queueType, string queue, string keyRouting, string subscriptionId, MethodTracer methodTracer, Encoding encoding, string msgId)
            :base(connection, queueType,queue,subscriptionId)
        {
            this.connection = connection; ;
            this.connectionUri = connection.ConnectionUri.Uri;
            this.tracer = methodTracer;
            this.queueType = queueType;
            this.ObjectLocker = new object();
            this.routingKey = keyRouting;
            this.queueName = queue;
            this.subscriptionIdentifier = subscriptionId;
            this.messageEncoding = encoding;
            this.messageId = msgId;
        }
        #endregion
    }
}
