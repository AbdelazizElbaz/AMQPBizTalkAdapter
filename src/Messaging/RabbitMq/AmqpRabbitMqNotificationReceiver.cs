using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter
{
    internal class AmqpRabbitMqNotificationReceiver : AmqpReceiverBase, IReceiver
    {
        readonly Object locker;
        readonly Encoding messageEncoding;
        readonly Uri connectionUri;
        readonly string messageId;
        public AmqpRabbitMqNotificationReceiver(AMQPBizTalkAdapterConnection connection,
            QueueTypeEnum queueType,string queue,string keyRouting, string subscriptionId, MethodTracer methodTracer,Encoding encoding,string msgId) : 
            base(connection, queueType, methodTracer, queue, keyRouting,subscriptionId )
        {
            this.locker = new object();
            this.messageEncoding = encoding;
            this.connectionUri = connection.ConnectionUri.Uri;
            this.messageId = msgId;

        }
        #region IReceiver
        public void StartListener(TimeSpan timeout, MethodTracer methodTracer)
        {
            this.Start(timeout, methodTracer);
        }

        public void StopListener(TimeSpan timeout, MethodTracer methodTracer)
        {
            this.closed = true;
            this.Stop(timeout, methodTracer);
        }

        public bool TryReceive(TimeSpan timeout, MethodTracer methodTracer, out Message wcfMessage)
        {

            TimeOutHelper timeHelper = new TimeOutHelper(timeout);
            bool result = false;
            wcfMessage = null;
            while (true)
            {
                lock (locker)
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
                                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format( "MessageReceived MessageId= [{0}] ",receiveMessage.BasicProperties.MessageId));
                                    //Return message to BizTalk
                                    wcfMessage = Helpers.Helper.CreateWcfMessage(receiveMessage, this.connectionUri);

                                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Send Ack DeliveryTag [{0}] to  Uri={1}",
                                    receiveMessage.DeliveryTag, this.connectionUri));
                                    result = true;
                                    //Send Ack 
                                    AckDelivery(e.DeliveryTag);
                                    methodTracer.TraceReturn(result);
                                    break;
                                }
                                else
                                {
                                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Message body is  null, DeliveryTag= {0}", e.DeliveryTag));
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
                Thread.Sleep(1000);
            }

            methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("End TryReceive uri= {0}", this.connectionUri));
            return result;
        }

        public bool WaitForMessage(TimeSpan timeout, MethodTracer methodTracer)
        {
            return this.IsAvailablesMessages;
        }
        #endregion
    }
}
