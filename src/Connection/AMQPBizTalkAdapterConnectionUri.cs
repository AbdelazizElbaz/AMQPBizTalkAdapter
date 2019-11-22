/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterConnectionUri.cs
/// Description :  This is the class for representing an adapter connection uri
/// -----------------------------------------------------------------------------------------------------------

#region Using Directives
using System;
using System.ComponentModel;
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


        private QueueTypeEnum queueType = QueueTypeEnum.Queue;


        private string subscriptionIdentifier = "clienttopic1";

        private Uri _uri;

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
            this.Uri = uri;
        }

        #endregion Constructors

        #region Custom Generated Properties
        [Browsable(true), Category("Connect Descriptor"), Description("AMQP Message Brocker Server host Name")]
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


        [Browsable(true), Category("Connect Descriptor"), Description("AMQP Message Brocker Server Port")]
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


        [Browsable(true), Category("Connect Descriptor"), Description("QueueName or TopicName")]
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


        [Browsable(true), Category("QueueType"), Description("Queue or Topic ")]
        public QueueTypeEnum QueueType
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


        [Browsable(true), Category("SubscriptionIdentifier"), Description("Topic subscription identifier")]
        public string SubscriptionIdentifier
        {
            get
            {
                return this.subscriptionIdentifier;
            }
            set
            {
                this.subscriptionIdentifier = value;
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
                return this.GetUri();
            }
            set
            {
                //
                //TODO: Parse the uri into its relevant parts to produce a valid Uri object. (For example scheme, host, query).
                //
                if (ValidateUri(value))
                {
                    this.Port = ((value.Port == -1) ? 1883 : value.Port);
                    this.HostName = value.Host;
                    string queueSegment = value.Segments[1].TrimEnd(new char[]
                    {
                    '/'
                    });
                    string queue = value.Segments[2].TrimEnd(new char[]
                    {
                    '/'
                    });
                    if (queueSegment.Equals("Queue", StringComparison.InvariantCultureIgnoreCase))
                        //Queue
                        QueueType = QueueTypeEnum.Queue;
                    else if (queueSegment.Equals("Topic", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //Topic 
                        QueueType = QueueTypeEnum.Topic;
                        this.subscriptionIdentifier = value.Query.Remove(0, 1);
                    }
                    this.QueueName = queue;
                    _uri = value;
                }
            }
        }

        public override string SampleUriString {
            get
            {
                return string.Format("{0}{1}{2}", "amqp://localhost:1111/Queue/Queue1", Environment.NewLine, "amqp://localhost:1111/Topic/topic1?subscriptionIdentifier1");
            }
        }


        #endregion ConnectionUri Members
        #region Prviate
        bool ValidateUri(Uri uri)
        {
            if (uri == null)
                throw new InvalidUriException("Uri is null or empty");
            //scheme
            if (!string.Equals(uri.Scheme, "AMQP", StringComparison.OrdinalIgnoreCase))
                throw new InvalidUriException("Invalide scheme value");
            if (string.IsNullOrEmpty(uri.Host))
                throw new InvalidUriException("HostName is null or empty");

            if (uri.Segments.Length == 3)
            {
                string queueSegment = uri.Segments[1].TrimEnd(new char[]
                    {
                    '/'
                    });

                if (queueSegment.Equals("Queue", StringComparison.OrdinalIgnoreCase))
                {
                    //Queue
                    if (!string.IsNullOrEmpty(uri.Query))
                    {
                        throw new InvalidUriException("You can't have query value for Queue , it's allowed only for Topic");
                    }
                }
                else if (queueSegment.Equals("Topic", StringComparison.OrdinalIgnoreCase))
                {
                    //Topic 
                    if (string.IsNullOrEmpty(uri.Query) ||
                        uri.Query.Contains(@"/") ||
                        !uri.Query.StartsWith("?") ||
                        uri.Query.Equals("?"))
                    {
                        throw new InvalidUriException("Invalid Topic subscription identifier");
                    }
                }
                else
                    throw new InvalidUriException(string.Format("You must have a Uri like {1}{2}", uri.ToString(), Environment.NewLine, this.SampleUriString));

                string queue = uri.Segments[2].TrimEnd(new char[]
                    {
                    '/'
                    });
                if (string.IsNullOrEmpty(queue))
                    throw new InvalidUriException(string.Format("You must have a Uri like {1}{2}", uri.ToString(), Environment.NewLine, this.SampleUriString));

                return true;
            }
            else
                throw new InvalidUriException(string.Format("You must have a Uri like {1}{2}", uri.ToString(), Environment.NewLine, this.SampleUriString));
            #endregion
        }
        public Uri GetUri()
        {
            string uritxt = string.Empty;
            if (!string.IsNullOrEmpty(this.QueueName))
            {
                uritxt = string.Concat(new object[]
                {
                    "amqp://",
                        this.HostName,
                        ":",
                        this.port,
                        "/",
                        QueueType==QueueTypeEnum.Queue ? "Queue" :"Topic",
                        "/",
                        Uri.EscapeDataString(this.QueueName),
                        QueueType==QueueTypeEnum.Queue ? "" : string.Format("?{0}",this.SubscriptionIdentifier),
                    });
            }
            else
            {
                throw new InvalidUriException("QueueName is Empty");
            }
            return new Uri(uritxt);
        }
    }
    public enum QueueTypeEnum
    {
        Queue,
        Topic
    }
}