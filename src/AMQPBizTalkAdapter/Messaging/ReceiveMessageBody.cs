using Microsoft.ServiceModel.Channels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AMQPBizTalkAdapter
{
    public class ReceiveMessageBody : BodyWriter, IDisposable
    {
        private const string ReponseName = "AmqpMessageEnveloppe";
        private const string Result = "Result";
        private List<ReceiveMessage> wso2Messages;

        public ReceiveMessageBody(List<ReceiveMessage> wso2Messages)
           : base(false)
        {
            this.wso2Messages = wso2Messages;
        }
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {

            try
            {
                writer.WriteStartElement(ReponseName, AMQPBizTalkAdapter.SERVICENAMESPACE);
                writer.WriteStartElement(Result, AMQPBizTalkAdapter.SERVICENAMESPACE);
                foreach (var msg in this.wso2Messages)
                {
                    writer.WriteRaw(Helpers.Helper.SerializeToString<ReceiveMessage>(msg));
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            catch (FormatException ex2)
            {
                throw new XmlReaderGenerationException(ex2.Message, ex2);
            }
            catch (ArgumentException ex3)
            {
                throw new XmlReaderGenerationException(ex3.Message, ex3);
            }
            finally
            {
                this.Dispose();
            }
        }
        public void Dispose()
        {
            this.wso2Messages = null;
        }
    }
}
