/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterConnection.cs
/// Description :  Defines the connection to the target system.
/// -----------------------------------------------------------------------------------------------------------

#region Using Directives
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Microsoft.ServiceModel.Channels.Common;
#endregion

namespace AMQPBizTalkAdapter
{
    public class AMQPBizTalkAdapterConnection : IConnection
    {
        #region Private Fields

        private AMQPBizTalkAdapterConnectionFactory connectionFactory;
        private string connectionId;

        #endregion Private Fields

        /// <summary>
        /// Initializes a new instance of the AMQPBizTalkAdapterConnection class with the AMQPBizTalkAdapterConnectionFactory
        /// </summary>
        public AMQPBizTalkAdapterConnection(AMQPBizTalkAdapterConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
            this.connectionId = Guid.NewGuid().ToString();
        }

        #region Public Properties

        /// <summary>
        /// Gets the ConnectionFactory
        /// </summary>
        public AMQPBizTalkAdapterConnectionFactory ConnectionFactory
        {
            get
            {
                return this.connectionFactory;
            }
        }

        #endregion Public Properties

        #region IConnection Members

        /// <summary>
        /// Closes the connection to the target system
        /// </summary>
        public void Close(TimeSpan timeout)
        {
            //
            //TODO: Implement physical closing of the connection
            //
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Returns a value indicating whether the connection is still valid
        /// </summary>
        public bool IsValid(TimeSpan timeout)
        {
            //
            //TODO: Implement physical checking for the validity of the opened connection
            //
            throw new NotImplementedException("The method or operation is not implemented.");

        }

        /// <summary>
        /// Opens the connection to the target system.
        /// </summary>
        public void Open(TimeSpan timeout)
        {
            //
            //TODO: Implement physical opening of the connection
            //
            throw new NotImplementedException("The method or operation is not implemented.");

        }

        /// <summary>
        /// Clears the context of the Connection. This method is called when the connection is set back to the connection pool
        /// </summary>
        public void ClearContext()
        {
            //
            //TODO: Implement clear context to set the connection back to the pool.
            //
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Builds a new instance of the specified IConnectionHandler type
        /// </summary>
        public TConnectionHandler BuildHandler<TConnectionHandler>(MetadataLookup metadataLookup)
             where TConnectionHandler : class, IConnectionHandler
        {

            if (typeof(IOutboundHandler).IsAssignableFrom(typeof(TConnectionHandler)))
            {
                return new AMQPBizTalkAdapterOutboundHandler(this, metadataLookup) as TConnectionHandler;
            }
            if (typeof(IInboundHandler).IsAssignableFrom(typeof(TConnectionHandler)))
            {
                return new AMQPBizTalkAdapterInboundHandler(this, metadataLookup) as TConnectionHandler;
            }
            if (typeof(IMetadataResolverHandler).IsAssignableFrom(typeof(TConnectionHandler)))
            {
                return new AMQPBizTalkAdapterMetadataResolverHandler(this, metadataLookup) as TConnectionHandler;
            }
            if (typeof(IMetadataBrowseHandler).IsAssignableFrom(typeof(TConnectionHandler)))
            {
                return new AMQPBizTalkAdapterMetadataBrowseHandler(this, metadataLookup) as TConnectionHandler;
            }
            if (typeof(IMetadataSearchHandler).IsAssignableFrom(typeof(TConnectionHandler)))
            {
                return new AMQPBizTalkAdapterMetadataSearchHandler(this, metadataLookup) as TConnectionHandler;
            }

            return default(TConnectionHandler);
        }

        /// <summary>
        /// Aborts the connection to the target system
        /// </summary>
        public void Abort()
        {
            //
            //TODO: Implement abort logic. DO NOT throw an exception from this method
            //
            throw new NotImplementedException("The method or operation is not implemented.");
        }


        /// <summary>
        /// Gets the Id of the Connection
        /// </summary>
        public String ConnectionId
        {
            get
            {
                return connectionId;
            }
        }

        #endregion IConnection Members
    }
}
