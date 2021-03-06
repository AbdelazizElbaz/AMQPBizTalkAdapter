/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterBindingElement.cs
/// Description :  Provides a base class for the configuration elements.
/// -----------------------------------------------------------------------------------------------------------

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Channels;
using System.Configuration;
using System.Globalization;

using Microsoft.ServiceModel.Channels.Common;
#endregion

namespace AMQPBizTalkAdapter
{
    public class AMQPBizTalkAdapterBindingElement : StandardBindingElement
    {
        private ConfigurationPropertyCollection properties;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AMQPBizTalkAdapterBindingElement class
        /// </summary>
        public AMQPBizTalkAdapterBindingElement()
            : base(null)
        {
        }


        /// <summary>
        /// Initializes a new instance of the AMQPBizTalkAdapterBindingElement class with a configuration name
        /// </summary>
        public AMQPBizTalkAdapterBindingElement(string configurationName)
            : base(configurationName)
        {
        }

        #endregion Constructors

        #region Custom Generated Properties
        [System.ComponentModel.Description("Amqp protocol version.")]
        [System.ComponentModel.Category("Message")]
        [System.Configuration.ConfigurationProperty("AMQP", DefaultValue = ProtocolVersion.AMQP_0_9_1)]
        public ProtocolVersion AMQP
        {
            get
            {
                return ((ProtocolVersion)(base["AMQP"]));
            }
            set
            {
                base["AMQP"] = value;
            }
        }
        [System.ComponentModel.Description("Enable all traces Levels.")]
        [System.ComponentModel.Category("Diagnostics")]
        [System.Configuration.ConfigurationProperty("enableTrace", DefaultValue = false)]
        public bool EnableTrace
        {
            get
            {
                return ((bool)(base["EnableTrace"]));
            }
            set
            {
                base["EnableTrace"] = value;
            }
        }


        [System.ComponentModel.Description("Content Type : text/Xml...")]
        [System.ComponentModel.Category("Message")]
        [System.Configuration.ConfigurationProperty("contentType", DefaultValue = "text/xml")]
        public string ContentType
        {
            get
            {
                return ((string)(base["ContentType"]));
            }
            set
            {
                base["ContentType"] = value;
            }
        }


        [System.ComponentModel.Description("Message body and Header encoding")]
        [System.ComponentModel.Category("Message")]
        [System.Configuration.ConfigurationProperty("encoding", DefaultValue = EncodingEnum.UTF8)]
        public EncodingEnum Encoding
        {
            get
            {
                return ((EncodingEnum)(base["Encoding"]));
            }
            set
            {
                base["Encoding"] = value;
            }
        }


        [System.ComponentModel.Description("GenNewGUID : the SendPort will generate a new guid for every message, you can put a unique Id for all messages.")]
        [System.ComponentModel.Category("Message")]
        [System.Configuration.ConfigurationProperty("messageId", DefaultValue = "GenNewGuid")]
        public string MessageId
        {
            get
            {
                return ((string)(base["MessageId"]));
            }
            set
            {
                base["MessageId"] = value;
            }
        }


        [System.ComponentModel.Description("Time interval in seconds between successive receive")]
        [System.ComponentModel.Category("Pooling")]
        [System.Configuration.ConfigurationProperty("interval", DefaultValue = 30)]
        public int Interval
        {
            get
            {
                return ((int)(base["Interval"]));
            }
            set
            {
                base["Interval"] = value;
            }
        }


        [System.ComponentModel.Description("The number of messages to be retrieved for each polls.")]
        [System.ComponentModel.Category("Pooling")]
        [System.Configuration.ConfigurationProperty("poolingLot", DefaultValue = 10)]
        public int PoolingLot
        {
            get
            {
                return ((int)(base["PoolingLot"]));
            }
            set
            {
                base["PoolingLot"] = value;
            }
        }


        [System.ComponentModel.Description("Indicates the inbound operation to be performed : Pooling or Notification")]
        [System.ComponentModel.Category("Configuration")]
        [System.Configuration.ConfigurationProperty("inboundOperationType", DefaultValue = "Notification")]
        public InboundOperationType InboundOperationType
        {
            get
            {
                return ((InboundOperationType)(base["InboundOperationType"]));
            }
            set
            {
                base["InboundOperationType"] = value;
            }
        }

        [System.ComponentModel.Description("RoutingKey to filter messages from topic")]
        [System.ComponentModel.Category("Message")]
        [System.Configuration.ConfigurationProperty("RoutingKey", DefaultValue = "QueueName")]
        public string RoutingKey
        {
            get
            {
                return ((string)(base["RoutingKey"]));
            }
            set
            {
                base["RoutingKey"] = value;
            }
        }
        #endregion Custom Generated Properties

        #region Protected Properties

        /// <summary>
        /// Gets the type of the BindingElement
        /// </summary>
        protected override Type BindingElementType
        {
            get
            {
                return typeof(AMQPBizTalkAdapterBinding);
            }
        }

        #endregion Protected Properties

        #region StandardBindingElement Members

        /// <summary>
        /// Initializes the binding with the configuration properties
        /// </summary>
        protected override void InitializeFrom(Binding binding)
        {
            base.InitializeFrom(binding);
            AMQPBizTalkAdapterBinding adapterBinding = (AMQPBizTalkAdapterBinding)binding;

            this["AMQP"] = adapterBinding.AMQP;
            this["EnableTrace"] = adapterBinding.EnableTrace;
            this["ContentType"] = adapterBinding.ContentType;
            this["Encoding"] = adapterBinding.Encoding;
            this["MessageId"] = adapterBinding.MessageId;
            this["Interval"] = adapterBinding.Interval;
            this["PoolingLot"] = adapterBinding.PoolingLot;
            this["InboundOperationType"] = adapterBinding.InboundOperationType;
            this["RoutingKey"] = adapterBinding.RoutingKey;
        }

        /// <summary>
        /// Applies the configuration
        /// </summary>
        protected override void OnApplyConfiguration(Binding binding)
        {
            if (binding == null)
                throw new ArgumentNullException("binding");

            AMQPBizTalkAdapterBinding adapterBinding = (AMQPBizTalkAdapterBinding)binding;

            adapterBinding.AMQP = (ProtocolVersion)this["AMQP"];
            adapterBinding.EnableTrace = (System.Boolean)this["EnableTrace"];
            adapterBinding.ContentType = (System.String)this["ContentType"];
            adapterBinding.Encoding = (EncodingEnum)this["Encoding"];
            adapterBinding.MessageId = (System.String)this["MessageId"];
            adapterBinding.Interval = (System.Int32)this["Interval"];
            adapterBinding.PoolingLot = (System.Int32)this["PoolingLot"];
            adapterBinding.InboundOperationType = (InboundOperationType)this["InboundOperationType"];
            adapterBinding.RoutingKey = (System.String)this["RoutingKey"];
        }

        /// <summary>
        /// Returns a collection of the configuration properties
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                if (this.properties == null)
                {
                    ConfigurationPropertyCollection configProperties = base.Properties;

                    configProperties.Add(new ConfigurationProperty("AMQP", typeof(ProtocolVersion), ProtocolVersion.AMQP_0_9_1, null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("EnableTrace", typeof(System.Boolean), false, null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("ContentType", typeof(System.String), "text/xml", null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("Encoding", typeof(EncodingEnum), EncodingEnum.UTF8, null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("MessageId", typeof(System.String), "GenNewGuid", null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("Interval", typeof(System.Int32), (System.Int32)30, null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("PoolingLot", typeof(System.Int32), (System.Int32)10, null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("InboundOperationType", typeof(InboundOperationType), InboundOperationType.Notification, null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("RoutingKey", typeof(string), "QueueName", null, null, ConfigurationPropertyOptions.None));
                    this.properties = configProperties;
                }
                return this.properties;
            }
        }


        #endregion StandardBindingElement Members
    }
    public enum InboundOperationType
    {
        Polling,
        Notification
    }
}
