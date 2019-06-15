using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AMQPBizTalkAdapter.Helpers
{
    class Helper
    {
        public static T Deserialize<T>(string toDeserialize)
        {

            var serializer = new XmlSerializer(typeof(T));
            object result;

            using (TextReader reader = new StringReader(toDeserialize.Trim()))
            {
                result = serializer.Deserialize(reader);
            }

            return (T)result;

        }

        public static void GetBasicPropertiesFromMessage(SendMessageRequest request, ref RabbitMQ.Client.IBasicProperties basicProperties, Encoding encoding)
        {
            if (!string.IsNullOrEmpty(request.BasicProperties.MessageId))
                basicProperties.MessageId = request.BasicProperties.MessageId;
            // basicProperties.Persistent = true;
            if (!string.IsNullOrEmpty(request.BasicProperties.ReplyTo))
                basicProperties.ReplyTo = request.BasicProperties.ReplyTo;
            if (request.BasicProperties.Timestamp != 0)
                basicProperties.Timestamp = new RabbitMQ.Client.AmqpTimestamp(request.BasicProperties.Timestamp);
            else
            {
                //AmqpTimestamp is a unixTime
                long unixTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                basicProperties.Timestamp = new RabbitMQ.Client.AmqpTimestamp(unixTime);
            }

            if (!string.IsNullOrEmpty(request.BasicProperties.Type))
                basicProperties.Type = request.BasicProperties.Type;
            if (!string.IsNullOrEmpty(request.BasicProperties.UserId))
                basicProperties.UserId = request.BasicProperties.UserId;
            if (!string.IsNullOrEmpty(request.BasicProperties.AppId))
                basicProperties.AppId = request.BasicProperties.AppId;
            if (!string.IsNullOrEmpty(request.BasicProperties.ClusterId))
                basicProperties.ClusterId = request.BasicProperties.ClusterId;
            if (!string.IsNullOrEmpty(request.BasicProperties.ContentEncoding))
                basicProperties.ContentEncoding = request.BasicProperties.ContentEncoding;
            if (!string.IsNullOrEmpty(request.BasicProperties.ContentType))
                basicProperties.ContentType = request.BasicProperties.ContentType;
            if (!string.IsNullOrEmpty(request.BasicProperties.CorrelationId))
                basicProperties.CorrelationId = request.BasicProperties.CorrelationId;

            basicProperties.DeliveryMode = request.BasicProperties.DeliveryMode;

            if (!string.IsNullOrEmpty(request.BasicProperties.Expiration))
                basicProperties.Expiration = request.BasicProperties.Expiration;

            if (request.BasicProperties.Headers != null)
            {
                if (basicProperties.Headers == null)
                    basicProperties.Headers = new Dictionary<string, object>();
                foreach (BasicPropertiesItem item in request.BasicProperties.Headers)
                {
                    if (!basicProperties.Headers.ContainsKey(item.Key))
                    {
                        if (!string.IsNullOrEmpty(item.Value))
                            basicProperties.Headers.Add(item.Key, item.Value);
                    }
                }
            }
            else
            {
                basicProperties.Headers = new Dictionary<string, object>();
            }
        }


        public static string RemoveCDATASection(string body)
        {
            if (string.IsNullOrEmpty(body))
                return body;
            body = body.Replace("&lt;", "<").Replace("&gt;", ">");
            if (body.StartsWith("<![CDATA[", StringComparison.OrdinalIgnoreCase) && body.EndsWith("]]>", StringComparison.OrdinalIgnoreCase))
            {
                //We must remove only <![CDATA[ et ]]>
                body = body.Remove(0, "<![CDATA[".Length - 1);
                body = body.Remove(body.Length - "]]>".Length, "]]>".Length);
            }
            return body;
        }

        public static Message CreateWcfMessage(ReceiveMessage message,Uri uri)
        {
             System.Xml.XmlReader reader = System.Xml.XmlReader.Create(new StringReader(message.ToString()));
            // create WCF message  
            Message chMessage = Message.CreateMessage(MessageVersion.Default
                        , MetaDataHelper.ReceiveOperationNodeId
                        , reader);

            chMessage.Headers.To = uri;
            return chMessage;
        }

        public static ReceiveMessage GetReceiveMessage(RabbitMQ.Client.Events.BasicDeliverEventArgs e, MethodTracer methodTracer, Encoding messageEncoding,string messageId)
        {

            if (e == null || e.Body == null)
            {
                return null;
            }
            ReceiveMessage message = new ReceiveMessage();
            message.Body = messageEncoding.GetString(e.Body);
            message.DeliveryTag = e.DeliveryTag;
            message.ConsumerTag = e.ConsumerTag;
            message.Exchange = e.Exchange;
            message.Redelivered = e.Redelivered;
            message.RoutingKey = e.RoutingKey;
            if (e.BasicProperties != null)
            {
                message.BasicProperties = new BasicProperties();
                message.BasicProperties.AppId = e.BasicProperties.AppId;
                message.BasicProperties.ClusterId = e.BasicProperties.ClusterId;
                message.BasicProperties.ContentEncoding = e.BasicProperties.ContentEncoding;
                message.BasicProperties.ContentType = e.BasicProperties.ContentType;
                message.BasicProperties.CorrelationId = e.BasicProperties.CorrelationId;
                message.BasicProperties.DeliveryMode = e.BasicProperties.DeliveryMode;
                message.BasicProperties.Expiration = e.BasicProperties.Expiration;
                message.BasicProperties.MessageId = e.BasicProperties.MessageId;
                message.BasicProperties.Priority = e.BasicProperties.Priority;
                message.BasicProperties.ReplyTo = e.BasicProperties.ReplyTo;
                message.BasicProperties.Timestamp = e.BasicProperties.Timestamp.UnixTime;
                message.BasicProperties.Type = e.BasicProperties.Type;
                message.BasicProperties.UserId = e.BasicProperties.UserId;
                message.BasicProperties.ProtocolClassId = e.BasicProperties.ProtocolClassId;
                message.BasicProperties.ProtocolClassName = e.BasicProperties.ProtocolClassName;
                if (e.BasicProperties.Headers != null && e.BasicProperties.Headers.Count > 0)
                {
                    List<BasicPropertiesItem> items = new List<BasicPropertiesItem>();
                    foreach (string key in e.BasicProperties.Headers.Keys)
                    {
                        object value = e.BasicProperties.Headers[key];
                        if (value != null)
                        {
                            BasicPropertiesItem item = new BasicPropertiesItem();
                            item.Key = key;
                                item.Value = messageEncoding.GetString((byte[])value);
                                items.Add(item);
                        }
                    }
                    message.BasicProperties.Headers = items.ToArray();
                }
            }

            Guid Id = Guid.NewGuid();
            if (!"GenNewGUID".Equals(messageId) &&
                Guid.TryParse(messageId, out Id))
            {
                message.BasicProperties.MessageId = Id.ToString();
            }
            return message;
        }
    }
}
