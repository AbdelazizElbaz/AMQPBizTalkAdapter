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



        [System.Configuration.ConfigurationProperty("encoding", DefaultValue = "UTF8")]
        public string Encoding
        {
            get
            {
                return ((string)(base["Encoding"]));
            }
            set
            {
                base["Encoding"] = value;
            }
        }



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



        [System.Configuration.ConfigurationProperty("inboundOperationType", DefaultValue = "Notification")]
        public string InboundOperationType
        {
            get
            {
                return ((string)(base["InboundOperationType"]));
            }
            set
            {
                base["InboundOperationType"] = value;
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
            this["EnableTrace"] = adapterBinding.EnableTrace;
            this["ContentType"] = adapterBinding.ContentType;
            this["Encoding"] = adapterBinding.Encoding;
            this["MessageId"] = adapterBinding.MessageId;
            this["Interval"] = adapterBinding.Interval;
            this["PoolingLot"] = adapterBinding.PoolingLot;
            this["InboundOperationType"] = adapterBinding.InboundOperationType;
        }

        /// <summary>
        /// Applies the configuration
        /// </summary>
        protected override void OnApplyConfiguration(Binding binding)
        {
            if (binding == null)
                throw new ArgumentNullException("binding");

            AMQPBizTalkAdapterBinding adapterBinding = (AMQPBizTalkAdapterBinding)binding;
            adapterBinding.EnableTrace = (System.Boolean)this["EnableTrace"];
            adapterBinding.ContentType = (System.String)this["ContentType"];
            adapterBinding.Encoding = (System.String)this["Encoding"];
            adapterBinding.MessageId = (System.String)this["MessageId"];
            adapterBinding.Interval = (System.Int32)this["Interval"];
            adapterBinding.PoolingLot = (System.Int32)this["PoolingLot"];
            adapterBinding.InboundOperationType = (System.String)this["InboundOperationType"];
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
                    configProperties.Add(new ConfigurationProperty("EnableTrace", typeof(System.Boolean), false, null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("ContentType", typeof(System.String), "text/xml", null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("Encoding", typeof(System.String), "UTF8", null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("MessageId", typeof(System.String), "GenNewGuid", null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("Interval", typeof(System.Int32), (System.Int32)30, null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("PoolingLot", typeof(System.Int32), (System.Int32)10, null, null, ConfigurationPropertyOptions.None));
                    configProperties.Add(new ConfigurationProperty("InboundOperationType", typeof(System.String), "Notification", null, null, ConfigurationPropertyOptions.None));
                    this.properties = configProperties;
                }
                return this.properties;
            }
        }


        #endregion StandardBindingElement Members
    }
}
