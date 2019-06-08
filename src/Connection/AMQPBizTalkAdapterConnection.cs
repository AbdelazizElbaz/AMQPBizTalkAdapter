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
        private string userName;
        private string passWord;
        private  AMQPBizTalkAdapterConnectionUri connectionUri;
        #endregion Private Fields

        /// <summary>
        /// Initializes a new instance of the AMQPBizTalkAdapterConnection class with the AMQPBizTalkAdapterConnectionFactory
        /// </summary>
        public AMQPBizTalkAdapterConnection(AMQPBizTalkAdapterConnectionFactory connectionFactory, AMQPBizTalkAdapterConnectionUri connectionUri, string user, string pass)
        {
            this.connectionFactory = connectionFactory;
            this.connectionId = Guid.NewGuid().ToString();
            this.userName = user;
            this.passWord = pass;
            this.connectionUri = connectionUri;
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
            return true;

        }

        /// <summary>
        /// Opens the connection to the target system.
        /// </summary>
        public void Open(TimeSpan timeout)
        {
            //
            //TODO: Implement physical opening of the connection
            //


        }

        /// <summary>
        /// Clears the context of the Connection. This method is called when the connection is set back to the connection pool
        /// </summary>
        public void ClearContext()
        {
            //
            //TODO: Implement clear context to set the connection back to the pool.
            //
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


        public RabbitMQ.Client.ConnectionFactory CreateRabbitMQConnectionFactory(TimeSpan openTimeOut)
        {
            RabbitMQ.Client.ConnectionFactory factory = new RabbitMQ.Client.ConnectionFactory();
            factory.VirtualHost = "/carbon";
            factory.UserName = this.userName;
            factory.Password = this.passWord;
            factory.HostName = this.connectionUri.HostName;
            factory.Port = this.connectionUri.Port;
            factory.Protocol = RabbitMQ.Client.Protocols.AMQP_0_9_1; ;
            factory.RequestedConnectionTimeout = (int)openTimeOut.TotalMilliseconds;
            return factory;
        }
        #endregion IConnection Members
    }
}
