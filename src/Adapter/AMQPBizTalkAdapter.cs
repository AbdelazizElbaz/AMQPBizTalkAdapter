/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapter.cs
/// Description :  The main adapter class which inherits from Adapter
/// -----------------------------------------------------------------------------------------------------------

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Description;

using Microsoft.ServiceModel.Channels.Common;
#endregion

namespace AMQPBizTalkAdapter
{
   
    public class AMQPBizTalkAdapter : Adapter
    {
        // Scheme associated with the adapter
        internal const string SCHEME = "amqp";
        // Namespace for the proxy that will be generated from the adapter schema
        internal const string SERVICENAMESPACE = "amqp://biztalkadapter.ampq/2018";
        // Initializes the AdapterEnvironmentSettings class
        private static AdapterEnvironmentSettings environmentSettings = new AdapterEnvironmentSettings();

        #region Custom Generated Fields

        private bool enableTrace;


        private string contentType;


        private string encoding;


        private string messageId;


        private int interval;


        private int poolingLot;


        private string inboundOperationType;

        #endregion Custom Generated Fields

        #region  Constructor

        /// <summary>
        /// Initializes a new instance of the AMQPBizTalkAdapter class
        /// </summary>
        public AMQPBizTalkAdapter()
        : base(environmentSettings)
        {
            Settings.Metadata.DefaultMetadataNamespace = SERVICENAMESPACE;
        }

        /// <summary>
        /// Initializes a new instance of the AMQPBizTalkAdapter class with a binding
        /// </summary>
        public AMQPBizTalkAdapter(AMQPBizTalkAdapter binding)
            : base(binding)
        {
            this.EnableTrace = binding.EnableTrace;
            this.ContentType = binding.ContentType;
            this.Encoding = binding.Encoding;
            this.MessageId = binding.MessageId;
            this.Interval = binding.Interval;
            this.PoolingLot = binding.PoolingLot;
            this.InboundOperationType = binding.InboundOperationType;
        }

        #endregion Constructor

        #region Custom Generated Properties

        [System.Configuration.ConfigurationProperty("enableTrace", DefaultValue = false)]
        public bool EnableTrace
        {
            get
            {
                return this.enableTrace;
            }
            set
            {
                this.enableTrace = value;
            }
        }



        [System.Configuration.ConfigurationProperty("contentType", DefaultValue = "text/xml")]
        public string ContentType
        {
            get
            {
                return this.contentType;
            }
            set
            {
                this.contentType = value;
            }
        }



        [System.Configuration.ConfigurationProperty("encoding", DefaultValue = "UTF8")]
        public string Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                this.encoding = value;
            }
        }



        [System.Configuration.ConfigurationProperty("messageId", DefaultValue = "GenNewGuid")]
        public string MessageId
        {
            get
            {
                return this.messageId;
            }
            set
            {
                this.messageId = value;
            }
        }



        [System.Configuration.ConfigurationProperty("interval", DefaultValue = 30)]
        public int Interval
        {
            get
            {
                return this.interval;
            }
            set
            {
                this.interval = value;
            }
        }



        [System.Configuration.ConfigurationProperty("poolingLot", DefaultValue = 10)]
        public int PoolingLot
        {
            get
            {
                return this.poolingLot;
            }
            set
            {
                this.poolingLot = value;
            }
        }



        [System.Configuration.ConfigurationProperty("inboundOperationType", DefaultValue = "Notification")]
        public string InboundOperationType
        {
            get
            {
                return this.inboundOperationType;
            }
            set
            {
                this.inboundOperationType = value;
            }
        }

        #endregion Custom Generated Properties

        #region Public Properties

        /// <summary>
        /// Gets the URI transport scheme that is used by the adapter
        /// </summary>
        public override string Scheme
        {
            get
            {
                return SCHEME;
            }
        }

        #endregion Public Properties

        #region Protected Methods

        /// <summary>
        /// Creates a ConnectionUri instance from the provided Uri
        /// </summary>
        protected override ConnectionUri BuildConnectionUri(Uri uri)
        {
            return new AMQPBizTalkAdapterConnectionUri(uri);
        }

        /// <summary>
        /// Builds a connection factory from the ConnectionUri and ClientCredentials
        /// </summary>
        protected override IConnectionFactory BuildConnectionFactory(
            ConnectionUri connectionUri
            , ClientCredentials clientCredentials
            , System.ServiceModel.Channels.BindingContext context)
        {
            return new AMQPBizTalkAdapterConnectionFactory(connectionUri, clientCredentials, this);
        }

        /// <summary>
        /// Returns a clone of the adapter object
        /// </summary>
        protected override Adapter CloneAdapter()
        {
            return new AMQPBizTalkAdapter(this);
        }

        /// <summary>
        /// Indicates whether the provided TConnectionHandler is supported by the adapter or not
        /// </summary>
        protected override bool IsHandlerSupported<TConnectionHandler>()
        {
            return (
                  typeof(IOutboundHandler) == typeof(TConnectionHandler)
                || typeof(IInboundHandler) == typeof(TConnectionHandler)
                || typeof(IMetadataResolverHandler) == typeof(TConnectionHandler)
                || typeof(IMetadataBrowseHandler) == typeof(TConnectionHandler)
                || typeof(IMetadataSearchHandler) == typeof(TConnectionHandler));
        }

        /// <summary>
        /// Gets the namespace that is used when generating schema and WSDL
        /// </summary>
        protected override string Namespace
        {
            get
            {
                return SERVICENAMESPACE;
            }
        }

        #endregion Protected Methods
    }
}
