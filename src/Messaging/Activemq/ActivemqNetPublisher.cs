using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AMQPBizTalkAdapter
{
     class ActivemqNetPublisher : IPublisher
    {
        AMQPBizTalkAdapterConnection connection;
        Apache.NMS.IConnection   nmsConnection;
        Apache.NMS.ISession nmsession;
        Apache.NMS.IMessageProducer producer;
        private bool IsQueue
        {
            get
            {
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
        public ActivemqNetPublisher(AMQPBizTalkAdapterConnection adapterConnection)
        {
            this.connection = adapterConnection;

        }
        public void Publish(System.ServiceModel.Channels.Message wcfmessage, MethodTracer methodTracer, TimeSpan openTimeOut, Encoding mEncoding)
        {
            try
            {
                methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "Call ActivemqNetPublisher.Publish");
                XmlDictionaryReader inputReader = wcfmessage.GetReaderAtBodyContents();

                var request = Helpers.Helper.Deserialize<SendMessageRequest>(inputReader.ReadOuterXml().Trim());
                inputReader.Close();

                this.nmsConnection = connection.CreateNmsConnectionFactory(openTimeOut).CreateConnection();
                // Create the AMQP session
                this.nmsConnection.Start();
                this.nmsession = this.nmsConnection.CreateSession();


                // Give a name to the sender
                // var senderSubscriptionId = this.connection.ConnectionUri.SubscriptionIdentifier;



         

               

               // IBasicProperties basicProperties = Model.CreateBasicProperties();

                 //Helpers.Helper.GetBasicPropertiesFromMessage(request, ref basicProperties, mEncoding);

                //We must remove CDATA Section before send message 
                request.Body = Helpers.Helper.RemoveCDATASection(request.Body);

                byte[] messageBytes = mEncoding.GetBytes(request.Body);

                var nmmessage = this.nmsession.CreateBytesMessage(messageBytes);
                nmmessage.NMSMessageId = request.BasicProperties.MessageId;



                if (IsQueue)
                {
                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "Publish Message to Queue :   " + this.QueueName);
                    //Queue
                    ActiveMQQueue activemqQueue = new ActiveMQQueue(this.QueueName);
                    this.producer = this.nmsession.CreateProducer(activemqQueue);
                    this.producer.DeliveryMode = Apache.NMS.MsgDeliveryMode.Persistent;
                    this.producer.Send(nmmessage);
                }
                else
                {
                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "Publish Message to Topic :   " + this.QueueName);
                    //Topic
                    ActiveMQTopic activemqTopic = new ActiveMQTopic(this.QueueName);
                    this.producer = this.nmsession.CreateProducer(activemqTopic);
                    this.producer.DeliveryMode = Apache.NMS.MsgDeliveryMode.Persistent;
                    this.producer.Send(nmmessage);
                   
                }


            }
            catch (Exception ex)
            {
                methodTracer.TraceError(ex.ToString());
                throw new Microsoft.ServiceModel.Channels.Common.AdapterException(ex.Message, ex);
            }
            methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "End ActivemqNetPublisher.Publish");
        }
    }
}
