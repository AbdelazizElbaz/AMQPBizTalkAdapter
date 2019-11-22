/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterOutboundHandler.cs
/// Description :  This class is used for sending data to the target system
/// -----------------------------------------------------------------------------------------------------------

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;

using Microsoft.ServiceModel.Channels.Common;
#endregion

namespace AMQPBizTalkAdapter
{
    public class AMQPBizTalkAdapterOutboundHandler : AMQPBizTalkAdapterHandlerBase, IOutboundHandler
    {
        private readonly string mTraceEventCode = "{0}/outbound/" + Guid.NewGuid().ToString();
        /// <summary>
        /// Initializes a new instance of the AMQPBizTalkAdapterOutboundHandler class
        /// </summary>
        public AMQPBizTalkAdapterOutboundHandler(AMQPBizTalkAdapterConnection connection
            , MetadataLookup metadataLookup)
        : base(connection, metadataLookup)
        {
        }

        #region IOutboundHandler Members

        /// <summary>
        /// Executes the request message on the target system and returns a response message.
        /// If there isn’t a response, this method should return null
        /// </summary>
        public Message Execute(Message message, TimeSpan timeout)
        {
            //
            //TODO: Implement Execute
            //
            using (MethodTracer methodTracer = new MethodTracer(this.mTraceEventCode, AMQPBizTalkAdapterTracer.Trace, this.Connection.ConnectionFactory.Adapter.EnableTrace))
            {
                // Trace input message  
                methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "AmqpAdapterOutboundHandler.Execute");
                // Timeout is not supported in this sample  
                OperationMetadata om = this.MetadataLookup.GetOperationDefinitionFromInputMessageAction(message.Headers.Action, timeout);
                if (om == null)
                    throw new AdapterException("Invalid operation metadata for " + message.Headers.Action);
                if (timeout.Equals(TimeSpan.Zero))
                    throw new AdapterException("time out is zero");



                AmqpPublisher MessagePublisher = new AmqpPublisher(this.Connection);
                var Encoding = System.Text.Encoding.GetEncoding((int)this.Connection.ConnectionFactory.Adapter.Encoding);
                MessagePublisher.Publish(message, methodTracer, timeout, Encoding);

                return null;
            }
        }

        #endregion IOutboundHandler Members
    }
}
