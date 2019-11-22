/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterInboundHandler.cs
/// Description :  This class implements an interface for listening or polling for data.
/// -----------------------------------------------------------------------------------------------------------
/// 
#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.ServiceModel.Channels.Common;
#endregion

namespace AMQPBizTalkAdapter
{
    public class AMQPBizTalkAdapterInboundHandler : AMQPBizTalkAdapterHandlerBase, IInboundHandler
    {
        private readonly string mTraceEventCode = "{0}/Inbound/" + Guid.NewGuid().ToString();
        readonly bool enableTrace;
       readonly IReceiver msgReceiver;
        /// <summary>
        /// Initializes a new instance of the AMQPBizTalkAdapterInboundHandler class
        /// </summary>
        public AMQPBizTalkAdapterInboundHandler(AMQPBizTalkAdapterConnection connection
            , MetadataLookup metadataLookup,AMQPBizTalkAdapter adapter)
            : base(connection, metadataLookup)
        {
            using (MethodTracer methodTracer = new MethodTracer(this.mTraceEventCode, AMQPBizTalkAdapterTracer.Trace, adapter.EnableTrace))
            {
                mTraceEventCode = string.Format(mTraceEventCode, connection.ConnectionUri.Uri);
                enableTrace = adapter.EnableTrace;
                //synclocker = new Object();
                if (adapter.InboundOperationType == InboundOperationType.Notification)
                {
                    this.msgReceiver = new AmqpNotificationReceiver(connection,connection.ConnectionUri.QueueType,connection.ConnectionUri.QueueName,
                        adapter.RoutingKey,connection.ConnectionUri.SubscriptionIdentifier, methodTracer, GetMessageEncoding(adapter.Encoding),adapter.MessageId);
                }
                else
                {
                    this.msgReceiver = new AmqpPollingReceiver(connection, connection.ConnectionUri.QueueType, connection.ConnectionUri.QueueName,
                        adapter.RoutingKey, connection.ConnectionUri.SubscriptionIdentifier, methodTracer, GetMessageEncoding(adapter.Encoding), adapter.PoolingLot, adapter.Interval);
                }
            }

        }

        #region IInboundHandler Members

        /// <summary>
        /// Start the listener
        /// </summary>
        public void StartListener(string[] actions, TimeSpan timeout)
        {
            //
            // Implement start adapter listener logic.
            //
            using (MethodTracer methodTracer = new MethodTracer(this.mTraceEventCode, AMQPBizTalkAdapterTracer.Trace, enableTrace))
            {
                try
                {
                    if (this.msgReceiver != null)
                    {
                        //start listening 
                        msgReceiver.StartListener(timeout, methodTracer);
                    }
                }
                catch (Exception ex)
                {
                    methodTracer.TraceError(string.Format("Error in AMQPBizTalkAdapterInboundHandler.StartListener : [{0}]", ex.ToString()));
                    throw new AdapterException("Error in AMQPBizTalkAdapterInboundHandler.StartListener", ex);
                }
            }
        }

        /// <summary>
        /// Stop the listener
        /// </summary>
        public void StopListener(TimeSpan timeout)
        {
            //
            // Implement stop adapter listener logic.
            //
            using (MethodTracer methodTracer = new MethodTracer(this.mTraceEventCode, AMQPBizTalkAdapterTracer.Trace, enableTrace))
            {
                try
                {
                    if (this.msgReceiver != null)
                    {
                        //start listening 
                        msgReceiver.StopListener(timeout, methodTracer);
                    }
                }
                catch (Exception ex)
                {
                    methodTracer.TraceError(string.Format("Error in AMQPBizTalkAdapterInboundHandler.StopListener : [{0}]", ex.ToString()));
                    throw new AdapterException("Error in AMQPBizTalkAdapterInboundHandler.StopListener", ex);
                }
            }
        }

        /// <summary>
        /// Tries to receive a message within a specified interval of time. 
        /// </summary>
        public bool TryReceive(TimeSpan timeout, out System.ServiceModel.Channels.Message message, out IInboundReply reply)
        {
            using (MethodTracer methodTracer = new MethodTracer(this.mTraceEventCode, AMQPBizTalkAdapterTracer.Trace, enableTrace))
            {
                try
                {
                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Call TryReceive [{0}] TimeOut=[{1}] ", mTraceEventCode, timeout.ToString()));
                    message = null;
                    reply = new AMQPBizTalkAdapterInboundReply();
                    return msgReceiver.TryReceive(timeout, methodTracer, out message);
                }
                catch (Exception ex)
                {
                    methodTracer.TraceError("Error in AMQPBizTalkAdapterInboundHandler.TryReceive : " + ex.ToString());
                    throw new AdapterException("Error in AMQPBizTalkAdapterInboundHandler.TryReceive", ex);
                }
            }
        }

        /// <summary>
        /// Returns a value that indicates whether a message has arrived within a specified interval of time.
        /// </summary>
        public bool WaitForMessage(TimeSpan timeout)
        {
            //
            //TODO: Implement Wait for message logic.
            //
            using (MethodTracer methodTracer = new MethodTracer(this.mTraceEventCode, AMQPBizTalkAdapterTracer.Trace, enableTrace))
            {
                try
                {
                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Call WaitForMessage [{0}] ", mTraceEventCode));

                    return msgReceiver.WaitForMessage(timeout, methodTracer);

                }
                catch (Exception ex)
                {
                    methodTracer.TraceError(string.Format("Error in AMQPBizTalkAdapterInboundHandler.WaitForMessage : [{0}]" , ex.ToString()));
                    throw new AdapterException("Error in AMQPBizTalkAdapterInboundHandler.WaitForMessage", ex);
                }
            }
        }

        #endregion IInboundHandler Members
        private Encoding GetMessageEncoding(EncodingEnum encoding)
        {
            try
            {
                return Encoding.GetEncoding((int)encoding);
            }
            catch
            { }
            return Encoding.UTF8;
        }
    }
    internal class AMQPBizTalkAdapterInboundReply : InboundReply
    {
        #region InboundReply Members

        /// <summary>
        /// Abort the inbound reply call
        /// </summary>
        public override void Abort()
        {
            //
            //TODO: Implement abort logic.
            //
           
        }

        /// <summary>
        /// Reply message implemented
        /// </summary>
        public override void Reply(System.ServiceModel.Channels.Message message
            , TimeSpan timeout)
        {
            //
            //TODO: Implement reply logic.
            //
           

        }


        #endregion InboundReply Members
    }

}
