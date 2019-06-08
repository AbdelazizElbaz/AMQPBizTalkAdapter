/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterBinding.cs
/// Description :  This is the class used while creating a binding for an adapter
/// -----------------------------------------------------------------------------------------------------------

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

using Microsoft.ServiceModel.Channels.Common;
#endregion

namespace AMQPBizTalkAdapter
{
    public class AMQPBizTalkAdapterBinding : AdapterBinding
    {
        // Scheme in Binding does not have to be the same as Adapter Scheme. 
        // Over write this value as appropriate.
        private const string BindingScheme = "amqp";

        /// <summary>
        /// Initializes a new instance of the AdapterBinding class
        /// </summary>
        public AMQPBizTalkAdapterBinding() { }

        /// <summary>
        /// Initializes a new instance of the AdapterBinding class with a configuration name
        /// </summary>
        public AMQPBizTalkAdapterBinding(string configName)
        {
            ApplyConfiguration(configName);
        }

        /// <summary>
        /// Applies the current configuration to the AMQPBizTalkAdapterBindingCollectionElement
        /// </summary>
        private void ApplyConfiguration(string configurationName)
        {
            /*
            //
            // TODO : replace the <The config name of your adapter> below with the configuration name of your adapter
            //
            BindingsSection bindingsSection = (BindingsSection)System.Configuration.ConfigurationManager.GetSection("system.serviceModel/bindings");
            AMQPBizTalkAdapterBindingCollectionElement bindingCollectionElement = (AMQPBizTalkAdapterBindingCollectionElement)bindingsSection["<The config name of your adapter>"];
            AMQPBizTalkAdapterBindingElement element = bindingCollectionElement.Bindings[configurationName];
            if (element != null)
            {
                element.ApplyConfiguration(this);
            }
            */
            throw new NotImplementedException("The method or operation is not implemented.");
        }



        #region Private Fields

        private AMQPBizTalkAdapter binding;

        #endregion Private Fields

        #region Custom Generated Fields

        private bool enableTrace;


        private string contentType;


        private string encoding;


        private string messageId;


        private int interval;


        private int poolingLot;


        private string inboundOperationType;

        #endregion Custom Generated Fields

        #region Public Properties

        /// <summary>
        /// Gets the URI transport scheme that is used by the channel and listener factories that are built by the bindings.
        /// </summary>
        public override string Scheme
        {
            get
            {
                return BindingScheme;
            }
        }

        /// <summary>
        /// Returns a value indicating whether this binding supports metadata browsing.
        /// </summary>
        public override bool SupportsMetadataBrowse
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns a value indicating whether this binding supports metadata retrieval.
        /// </summary>
        public override bool SupportsMetadataGet
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns a value indicating whether this binding supports metadata searching.
        /// </summary>
        public override bool SupportsMetadataSearch
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns the custom type of the ConnectionUri.
        /// </summary>
        public override Type ConnectionUriType
        {
            get
            {
                return typeof(AMQPBizTalkAdapterConnectionUri);
            }
        }

        #endregion Public Properties

        #region Custom Generated Properties
        [System.ComponentModel.Description("Enable all traces Levels.")]
        [System.ComponentModel.Category("Diagnostics")]
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


        [System.ComponentModel.Description("Content Type : text/Xml...")]
        [System.ComponentModel.Category("Message")]
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


        [System.ComponentModel.Description("Message body and Header encoding")]
        [System.ComponentModel.Category("Message")]
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


        [System.ComponentModel.Description("GenNewGUID : the SendPort will generate a new guid for every message, you can put a unique Id for all messages.")]
        [System.ComponentModel.Category("Message")]
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


        [System.ComponentModel.Description("Time interval in seconds between successive receive")]
        [System.ComponentModel.Category("Pooling")]
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


        [System.ComponentModel.Description("The number of messages to be retrieved for each polls.")]
        [System.ComponentModel.Category("Pooling")]
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


        [System.ComponentModel.Description("Indicates the inbound operation to be performed : Pooling or Notification")]
        [System.ComponentModel.Category("Configuration")]
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

        #region Private Properties

        private AMQPBizTalkAdapter BindingElement
        {
            get
            {
                if (binding == null)
                    binding = new AMQPBizTalkAdapter();
                binding.EnableTrace = this.EnableTrace;
                binding.ContentType = this.ContentType;
                binding.Encoding = this.Encoding;
                binding.MessageId = this.MessageId;
                binding.Interval = this.Interval;
                binding.PoolingLot = this.PoolingLot;
                binding.InboundOperationType = this.InboundOperationType;
                return binding;
            }
        }

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        /// Creates a clone of the existing BindingElement and returns it
        /// </summary>
        public override BindingElementCollection CreateBindingElements()
        {
            BindingElementCollection bindingElements = new BindingElementCollection();
            //Only create once
            bindingElements.Add(this.BindingElement);
            //Return the clone
            return bindingElements.Clone();
        }

        #endregion Public Methods

    }
}
