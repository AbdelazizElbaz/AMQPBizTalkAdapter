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
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        #endregion IOutboundHandler Members
    }
}
