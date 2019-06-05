/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterConnectionUri.cs
/// Description :  This is the class for representing an adapter connection uri
/// -----------------------------------------------------------------------------------------------------------

#region Using Directives
using System;

using Microsoft.ServiceModel.Channels.Common;
#endregion

namespace AMQPBizTalkAdapter
{
    /// <summary>
    /// This is the class for building the AMQPBizTalkAdapterConnectionUri
    /// </summary>
    public class AMQPBizTalkAdapterConnectionUri : ConnectionUri
    {

        #region Custom Generated Fields

        private string hostName = "localhost";


        private int port = 5544;


        private string queueName = "testTopic";


        private string queueType = "Topic";


        private string consumerIdentifier = "testtopic1";

        #endregion Custom Generated Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ConnectionUri class
        /// </summary>
        public AMQPBizTalkAdapterConnectionUri() { }

        /// <summary>
        /// Initializes a new instance of the ConnectionUri class with a Uri object
        /// </summary>
        public AMQPBizTalkAdapterConnectionUri(Uri uri)
            : base()
        {

        }

        #endregion Constructors

        #region Custom Generated Properties

        public string HostName
        {
            get
            {
                return this.hostName;
            }
            set
            {
                this.hostName = value;
            }
        }



        public int Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }



        public string QueueName
        {
            get
            {
                return this.queueName;
            }
            set
            {
                this.queueName = value;
            }
        }



        public string QueueType
        {
            get
            {
                return this.queueType;
            }
            set
            {
                this.queueType = value;
            }
        }



        public string ConsumerIdentifier
        {
            get
            {
                return this.consumerIdentifier;
            }
            set
            {
                this.consumerIdentifier = value;
            }
        }

        #endregion Custom Generated Properties

        #region ConnectionUri Members

        /// <summary>
        /// Getter and Setter for the Uri
        /// </summary>
        public override Uri Uri
        {
            get
            {
                //
                //TODO: Return the composed uri in valid format
                //
                throw new NotImplementedException("The method or operation is not implemented.");
            }
            set
            {
                //
                //TODO: Parse the uri into its relevant parts to produce a valid Uri object. (For example scheme, host, query).
                //
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        #endregion ConnectionUri Members

    }
}