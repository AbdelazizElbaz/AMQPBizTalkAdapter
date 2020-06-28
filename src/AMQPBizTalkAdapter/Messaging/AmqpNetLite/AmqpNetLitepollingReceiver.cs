using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter
{
    class AmqpNetLitePollingReceiver : AmqpNetLiteReceiver, IReceiver
    {
        readonly Object ObjectLocker;
        readonly Encoding messageEncoding;
        readonly Uri connectionUri;

        readonly int msgPollingLot;
        private DateTime nextTimeToExecute = DateTime.UtcNow;
        private int executeIntervalInSeconds;
    
        private bool closed = false;

        public AmqpNetLitePollingReceiver(AMQPBizTalkAdapterConnection connection,
           QueueTypeEnum queueType, string queue, string keyRouting, string subscriptionId, MethodTracer methodTracer, Encoding encoding, int lot, int intervalInSeconds)
            : base(connection, queueType, queue, subscriptionId)
        {
            this.executeIntervalInSeconds = intervalInSeconds;
            this.ObjectLocker = new object();
            this.messageEncoding = encoding;
            this.connectionUri = connection.ConnectionUri.Uri;
            this.msgPollingLot = lot;
        }
       
        public void StartListener(TimeSpan timeout, MethodTracer methodTracer)
        {
            lock (ObjectLocker)
            {

                try
                {
                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Start listening uri= {0}", this.connectionUri));
                    this.Start(timeout,methodTracer);
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
                                List<Amqp.Message> eMsgs = this.DequeueMessages(msgPollingLot);
                                List<ReceiveMessage> messages = new List<ReceiveMessage>();
                              
                                foreach (var e in eMsgs)
                                {
                                    ReceiveMessage wso2Message = Helpers.Helper.GetReceiveMessage(e, methodTracer,
                                    this.messageEncoding, "GenNewGUID");
                                    if (wso2Message != null)
                                    {
                                        messages.Add(wso2Message);
                                    }
                                }

                                wcfMessage = Helpers.Helper.CreateWcfMessage(messages, this.connectionUri);

                                result = true;
                                this.nextTimeToExecute = this.nextTimeToExecute.AddSeconds((double)this.executeIntervalInSeconds);

                                //Send Ack to amqp MB
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
