using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}
