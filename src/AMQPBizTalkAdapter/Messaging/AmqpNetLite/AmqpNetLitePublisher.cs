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
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "Call ActivemqNetPublisher.Publish");
                    XmlDictionaryReader inputReader = wcfmessage.GetReaderAtBodyContents();
                    string msgcontent = inputReader.ReadOuterXml().Trim();
                    var request = Helpers.Helper.Deserialize<SendMessageRequest>(msgcontent);
                    inputReader.Close();


                    //We must remove CDATA Section before send message 
                    request.Body = Helpers.Helper.RemoveCDATASection(request.Body);

                    byte[] messageBytes = mEncoding.GetBytes(request.Body);
                    var amqpmessage = new Amqp.Message(messageBytes);
                    //var nmmessage = this.nmsession.CreateBytesMessage(messageBytes);
                    amqpmessage.Properties = new Amqp.Framing.Properties();
                    amqpmessage.Properties.MessageId = request.BasicProperties.MessageId;

                    var AmqpNetLitConnectionFactory = connection.CreateAmqpNetLitConnectionFactory(openTimeOut);
                    var amqpConnection = new Amqp.Connection(new Amqp.Address("http://admin:admin@localhost:5672"));//  AmqpNetLitConnectionFactory.CreateConnection();
                                                                                                                    // Create the AMQP session
                    var amqpsession = new Amqp.Session(amqpConnection);
                    if (IsQueue)
                    {
                        methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "Publish Message to Queue :   " + this.QueueName);
                        //Queue
                        var producer = new Amqp.SenderLink(amqpsession, "Client", this.QueueName);// bydefault it send to queue
                        producer.Send(amqpmessage);
                    }
                    else
                    {
                        methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, "Publish Message to Topic :   " + this.QueueName);
                        //Topic
                        var producer = new Amqp.SenderLink(amqpsession, this.QueueName, string.Format("topic://{0}", this.QueueName));// topic://topicname to send to topic
                        producer.Send(amqpmessage, openTimeOut);
                    }
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
