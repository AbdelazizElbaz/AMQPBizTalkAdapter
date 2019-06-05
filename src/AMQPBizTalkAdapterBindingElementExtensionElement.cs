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
                configProperties.Add(new ConfigurationProperty("EnableTrace", typeof(System.Boolean), false, null, null, ConfigurationPropertyOptions.None));
                configProperties.Add(new ConfigurationProperty("ContentType", typeof(System.String), "text/xml", null, null, ConfigurationPropertyOptions.None));
                configProperties.Add(new ConfigurationProperty("Encoding", typeof(System.String), "UTF8", null, null, ConfigurationPropertyOptions.None));
                configProperties.Add(new ConfigurationProperty("MessageId", typeof(System.String), "GenNewGuid", null, null, ConfigurationPropertyOptions.None));
                configProperties.Add(new ConfigurationProperty("Interval", typeof(System.Int32), (System.Int32)30, null, null, ConfigurationPropertyOptions.None));
                configProperties.Add(new ConfigurationProperty("PoolingLot", typeof(System.Int32), (System.Int32)10, null, null, ConfigurationPropertyOptions.None));
                configProperties.Add(new ConfigurationProperty("InboundOperationType", typeof(System.String), "Notification", null, null, ConfigurationPropertyOptions.None));
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
            adapterBinding.EnableTrace = (System.Boolean)this["EnableTrace"];
            adapterBinding.ContentType = (System.String)this["ContentType"];
            adapterBinding.Encoding = (System.String)this["Encoding"];
            adapterBinding.MessageId = (System.String)this["MessageId"];
            adapterBinding.Interval = (System.Int32)this["Interval"];
            adapterBinding.PoolingLot = (System.Int32)this["PoolingLot"];
            adapterBinding.InboundOperationType = (System.String)this["InboundOperationType"];
        }

        /// <summary>
        /// Initialize the binding properties from the adapter.
        /// </summary>
        /// <param name="bindingElement"></param>
        protected override void InitializeFrom(BindingElement bindingElement)
        {
            base.InitializeFrom(bindingElement);
            AMQPBizTalkAdapter adapterBinding = ((AMQPBizTalkAdapter)(bindingElement));
            this["EnableTrace"] = adapterBinding.EnableTrace;
            this["ContentType"] = adapterBinding.ContentType;
            this["Encoding"] = adapterBinding.Encoding;
            this["MessageId"] = adapterBinding.MessageId;
            this["Interval"] = adapterBinding.Interval;
            this["PoolingLot"] = adapterBinding.PoolingLot;
            this["InboundOperationType"] = adapterBinding.InboundOperationType;
        }

        /// <summary>
        /// Copy the properties to the custom binding
        /// </summary>
        /// <param name="from"></param>
        public override void CopyFrom(ServiceModelExtensionElement from)
        {
            base.CopyFrom(from);
            AMQPBizTalkAdapterBindingElementExtensionElement adapterBinding = ((AMQPBizTalkAdapterBindingElementExtensionElement)(from));
            this["EnableTrace"] = adapterBinding.EnableTrace;
            this["ContentType"] = adapterBinding.ContentType;
            this["Encoding"] = adapterBinding.Encoding;
            this["MessageId"] = adapterBinding.MessageId;
            this["Interval"] = adapterBinding.Interval;
            this["PoolingLot"] = adapterBinding.PoolingLot;
            this["InboundOperationType"] = adapterBinding.InboundOperationType;
        }

        #endregion BindingElementExtensionElement Methods
    }
}

