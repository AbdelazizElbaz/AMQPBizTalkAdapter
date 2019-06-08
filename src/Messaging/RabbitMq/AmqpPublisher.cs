using Microsoft.ServiceModel.Channels.Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AMQPBizTalkAdapter
{
    class AmqpPublisher
    {
        const string exchangeQueue = "amq.direct";
        const string exchangeTopic = "amq.topic";
        AMQPBizTalkAdapterConnection connection;
        private bool IsQueue {
            get {
                return this.connection.ConnectionUri.QueueType == QueueTypeEnum.Queue;
            }
        }
        private string QueueName
        {
            get
            {
                return this.connection.ConnectionUri.QueueName;
            }
        }

        public AmqpPublisher(AMQPBizTalkAdapterConnection adapterConnection)
        {
            this.connection = adapterConnection;

        }
        public void Publish(System.ServiceModel.Channels.Message wcfmessage, MethodTracer methodTracer, TimeSpan openTimeOut,  Encoding mEncoding)
        {
            try
            {
                methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "Call AmqpPublisher.Publish");
                XmlDictionaryReader inputReader = wcfmessage.GetReaderAtBodyContents();

                var request = Helpers.Helper.Deserialize<SendMessageRequest>(inputReader.ReadOuterXml().Trim());
                inputReader.Close();

                using (var rmqconnection = connection.CreateRabbitMQConnectionFactory(openTimeOut).CreateConnection())
                {
                    using (IModel Model = rmqconnection.CreateModel())
                    {
                        IBasicProperties basicProperties = Model.CreateBasicProperties();

                        Helpers.Helper.GetBasicPropertiesFromMessage(request, ref basicProperties, mEncoding);

                        //We must remove CDATA Section before send message 
                        request.Body = Helpers.Helper.RemoveCDATASection(request.Body);

                        byte[] messageBytes = mEncoding.GetBytes(request.Body);

                        if (IsQueue)
                        {
                            methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "Publish Message to Queue :   " + this.QueueName);
                            //Queue
                            Model.ExchangeDeclare("amq.direct", "direct");
                            Model.BasicPublish("amq.direct", this.QueueName, basicProperties, messageBytes);
                        }
                        else
                        {
                            methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "Publish Message to Topic :   " + this.QueueName);
                            //Topic
                            Model.ExchangeDeclare(exchangeTopic, "topic");
                            Model.BasicPublish(exchangeTopic, this.QueueName, basicProperties, messageBytes);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                methodTracer.TraceError(ex.ToString());
                throw new AdapterException(ex.Message, ex);
            }
            methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "End AmqpPublisher.Publish");
        }

    }
}
