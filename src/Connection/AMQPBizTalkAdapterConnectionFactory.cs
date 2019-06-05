/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterConnectionFactory.cs
/// Description :  Defines the connection factory for the target system.
/// -----------------------------------------------------------------------------------------------------------

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Selectors;
using System.ServiceModel.Description;

using Microsoft.ServiceModel.Channels.Common;
#endregion

namespace AMQPBizTalkAdapter
{
    public class AMQPBizTalkAdapterConnectionFactory : IConnectionFactory
    {
        #region Private Fields

        // Stores the client credentials
        private ClientCredentials clientCredentials;
        // Stores the adapter class
        private AMQPBizTalkAdapter adapter;

        #endregion Private Fields

        /// <summary>
        /// Initializes a new instance of the AMQPBizTalkAdapterConnectionFactory class
        /// </summary>
        public AMQPBizTalkAdapterConnectionFactory(ConnectionUri connectionUri
            , ClientCredentials clientCredentials
            , AMQPBizTalkAdapter adapter)
        {
            this.clientCredentials = clientCredentials;
            this.adapter = adapter;
        }

        #region Public Properties

        /// <summary>
        /// Gets the adapter
        /// </summary>
        public AMQPBizTalkAdapter Adapter
        {
            get
            {
                return this.adapter;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates the connection to the target system
        /// </summary>
        public IConnection CreateConnection()
        {
            return new AMQPBizTalkAdapterConnection(this);
        }

        #endregion Public Methods
    }
}
