using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AMQPBizTalkAdapter
{
     public class AmqpNetLitePublisher : IPublisher 
    {
        AMQPBizTalkAdapterConnection connection;
        Amqp.Connection amqpConnection;
        Amqp.Session amqpsession;
        Amqp.SenderLink  producer;
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
        public AmqpNetLitePublisher(AMQPBizTalkAdapterConnection adapterConnection)
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

                this.amqpConnection = connection.CreateAmqpNetLitConnectionFactory(openTimeOut).CreateConnection();
                // Create the AMQP session
                this.amqpsession = new Amqp.Session(this.amqpConnection);

                //We must remove CDATA Section before send message 
                request.Body = Helpers.Helper.RemoveCDATASection(request.Body);

                byte[] messageBytes = mEncoding.GetBytes(request.Body);
                var amqpmessage = new Amqp.Message(messageBytes);
                //var nmmessage = this.nmsession.CreateBytesMessage(messageBytes);
                amqpmessage.Properties = new Amqp.Framing.Properties();
                amqpmessage.Properties.MessageId= request.BasicProperties.MessageId;
                if (IsQueue)
                {
                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "Publish Message to Queue :   " + this.QueueName);
                    //Queue
                    this.producer=  new Amqp.SenderLink(this.amqpsession,this.QueueName, this.QueueName);// bydefault it send to queue
                    this.producer.Send(amqpmessage);
                }
                else
                {
                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "Publish Message to Topic :   " + this.QueueName);
                    //Topic
                    this.producer = new Amqp.SenderLink(this.amqpsession, this.QueueName,string.Format("topic://{0}",this.QueueName) );// topic://topicname to send to topic
                    this.producer.Send(amqpmessage);
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
