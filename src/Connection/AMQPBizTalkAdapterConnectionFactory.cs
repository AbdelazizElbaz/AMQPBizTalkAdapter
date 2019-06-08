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
using System.ServiceModel.Security;
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
        // Stores the adapter connectionUri
        private AMQPBizTalkAdapterConnectionUri connectionUri;
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
            this.connectionUri = new AMQPBizTalkAdapterConnectionUri(connectionUri.Uri);
            // it support SSO 
            if (clientCredentials != null)
            {
                UserNamePasswordClientCredential userName = clientCredentials.UserName;
                if (string.IsNullOrEmpty(userName.UserName))
                {
                    throw new CredentialsException("Username is expected.");
                }
            }
            else
            {
                throw new CredentialsException("ClientCredentials is not specified.");
            }
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
            return new AMQPBizTalkAdapterConnection(
                                                    this,
                                                    connectionUri,
                                                    clientCredentials.UserName.UserName,
                                                    clientCredentials.UserName.Password
                                                    );
        }

        #endregion Public Methods
    }
}
