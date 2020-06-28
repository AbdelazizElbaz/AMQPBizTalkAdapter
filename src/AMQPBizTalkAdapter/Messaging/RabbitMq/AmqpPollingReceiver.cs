using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter
{
    class AmqpPollingReceiver : AmqpReceiverBase, IReceiver
    {
        readonly Object locker;
        readonly Encoding messageEncoding;
        readonly Uri connectionUri;
     
        readonly int msgPollingLot;
        private DateTime nextTimeToExecute = DateTime.UtcNow;
        private int executeIntervalInSeconds;
        public AmqpPollingReceiver(AMQPBizTalkAdapterConnection connection,
           QueueTypeEnum queueType, string queue, string keyRouting, string subscriptionId, MethodTracer methodTracer, Encoding encoding, int lot, int intervalInSeconds) :
           base(connection, queueType, methodTracer, queue, keyRouting, subscriptionId)
        {
            this.executeIntervalInSeconds = intervalInSeconds;
            this.locker = new object();
            this.messageEncoding = encoding;
            this.connectionUri = connection.ConnectionUri.Uri;
            this.msgPollingLot = lot;
        }
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
            bool result = false;

            TimeOutHelper timeHelper = new TimeOutHelper(timeout);
            while (true)
            {
                lock (locker)
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
                                List<BasicDeliverEventArgs> eMsgs = this.DequeueMessages(msgPollingLot);
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
                                this.AckDelivery(DeliveryTags);

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
