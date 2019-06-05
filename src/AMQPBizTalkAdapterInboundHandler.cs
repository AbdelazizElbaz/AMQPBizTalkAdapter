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
        /// <summary>
        /// Initializes a new instance of the AMQPBizTalkAdapterInboundHandler class
        /// </summary>
        public AMQPBizTalkAdapterInboundHandler(AMQPBizTalkAdapterConnection connection
            , MetadataLookup metadataLookup)
            : base(connection, metadataLookup)
        {
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
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Stop the listener
        /// </summary>
        public void StopListener(TimeSpan timeout)
        {
            //
            //TODO: Implement stop adapter listener logic.
            //
            throw new NotImplementedException("The method or operation is not implemented.");
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
