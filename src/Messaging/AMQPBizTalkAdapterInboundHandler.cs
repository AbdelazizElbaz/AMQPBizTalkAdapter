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
                    this.msgReceiver = new AmqpNotification(connection,connection.ConnectionUri.QueueType,connection.ConnectionUri.QueueName,
                        adapter.RoutingKey,connection.ConnectionUri.SubscriptionIdentifier, methodTracer, GetMessageEncoding(adapter.Encoding),adapter.MessageId);
                }
                else
                {
                    //Pooling int the second version
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
            //TODO: Implement start adapter listener logic.
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
                    methodTracer.TraceError(string.Format("Error in IReceiver.StartListener : [{0}]" , ex.ToString()));
                    throw new AdapterException("Error in IReceiver.StartListener", ex);
                }
            }
        }

        /// <summary>
        /// Stop the listener
        /// </summary>
        public void StopListener(TimeSpan timeout)
        {
            //
            //TODO: Implement stop adapter listener logic.
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
                    methodTracer.TraceError(string.Format("Error in IReceiver.StopListener : [{0}]", ex.ToString()));
                    throw new AdapterException("Error in IReceiver.StopListener", ex);
                }
            }
        }

        /// <summary>
        /// Tries to receive a message within a specified interval of time. 
        /// </summary>
        public bool TryReceive(TimeSpan timeout, out System.ServiceModel.Channels.Message message, out IInboundReply reply)
        {
            reply = new AMQPBizTalkAdapterInboundReply();
            //
            //TODO: Implement Try Receive logic.
            //
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Returns a value that indicates whether a message has arrived within a specified interval of time.
        /// </summary>
        public bool WaitForMessage(TimeSpan timeout)
        {
            //
            //TODO: Implement Wait for message logic.
            //
            throw new NotImplementedException("The method or operation is not implemented.");
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
            throw new NotImplementedException("The method or operation is not implemented.");
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
            throw new NotImplementedException("The method or operation is not implemented.");

        }


        #endregion InboundReply Members
    }

}
