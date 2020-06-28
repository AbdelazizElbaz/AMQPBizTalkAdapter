/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterBindingElementExtensionElement.cs
/// Description :  This class is provided to surface Adapter as a binding element, so that it 
///                can be used within a user-defined WCF "Custom Binding".
///                In configuration file, it is defined under
///                <system.serviceModel>
///                  <extensions>
///                     <bindingElementExtensions>
///                         <add name="{name}" type="{this}, {assembly}"/>
///                     </bindingElementExtensions>
///                  </extensions>
///                </system.serviceModel>
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
    using System;
    using System.Configuration;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Configuration;

    public class AMQPBizTalkAdapterBindingElementExtensionElement : BindingElementExtensionElement
    {

        #region  Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public AMQPBizTalkAdapterBindingElementExtensionElement()
        {
        }

        #endregion Constructor

        #region Custom Generated Properties


        [System.ComponentModel.Description("AMQP Protocol versiom.")]
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

        #region BindingElementExtensionElement Methods
        /// <summary>
        /// Return the type of the adapter (binding element)
        /// </summary>
        public override Type BindingElementType
        {
            get
            {
                return typeof(AMQPBizTalkAdapter);
            }
        }
        /// <summary>
        /// Returns a collection of the configuration properties
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get
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
                return configProperties;
            }
        }

        /// <summary>
        /// Instantiate the adapter.
        /// </summary>
        /// <returns></returns>
        protected override BindingElement CreateBindingElement()
        {
            AMQPBizTalkAdapter adapter = new AMQPBizTalkAdapter();
            this.ApplyConfiguration(adapter);
            return adapter;
        }

        /// <summary>
        /// Apply the configuration properties to the adapter.
        /// </summary>
        /// <param name="bindingElement"></param>
        public override void ApplyConfiguration(BindingElement bindingElement)
        {
            base.ApplyConfiguration(bindingElement);
            AMQPBizTalkAdapter adapterBinding = ((AMQPBizTalkAdapter)(bindingElement));
            adapterBinding.AMQP = (ProtocolVersion)this["AMQP"];
            adapterBinding.EnableTrace = (System.Boolean)this["EnableTrace"];
            adapterBinding.ContentType = (System.String)this["ContentType"];
            adapterBinding.Encoding = (EncodingEnum)this["Encoding"];
            adapterBinding.MessageId = (System.String)this["MessageId"];
            adapterBinding.Interval = (System.Int32)this["Interval"];
            adapterBinding.PoolingLot = (System.Int32)this["PoolingLot"];
            adapterBinding.InboundOperationType = (InboundOperationType)this["InboundOperationType"];
            adapterBinding.RoutingKey = (string)this["RoutingKey"];
        }

        /// <summary>
        /// Initialize the binding properties from the adapter.
        /// </summary>
        /// <param name="bindingElement"></param>
        protected override void InitializeFrom(BindingElement bindingElement)
        {
            base.InitializeFrom(bindingElement);
            AMQPBizTalkAdapter adapterBinding = ((AMQPBizTalkAdapter)(bindingElement));

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
        /// Copy the properties to the custom binding
        /// </summary>
        /// <param name="from"></param>
        public override void CopyFrom(ServiceModelExtensionElement from)
        {
            base.CopyFrom(from);
            AMQPBizTalkAdapterBindingElementExtensionElement adapterBinding = ((AMQPBizTalkAdapterBindingElementExtensionElement)(from));
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

        #endregion BindingElementExtensionElement Methods
    }
}

