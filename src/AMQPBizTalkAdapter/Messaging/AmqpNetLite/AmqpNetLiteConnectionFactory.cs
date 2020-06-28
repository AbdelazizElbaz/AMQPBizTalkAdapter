using Amqp;
using RabbitMQ.Client.Framing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter
{
    public class AmqpNetLiteConnectionFactory
    {
        public Address Address { get; set; }
        public string clientId;
        public AmqpNetLiteConnectionFactory(Address address,string clientId)
        {
            this.Address = address;
            this.clientId = clientId;
        }
        public Amqp.Connection CreateConnection()
        {
            return new Connection(this.Address, Amqp.Sasl.SaslProfile.Anonymous, new Amqp.Framing.Open() { ContainerId = clientId },null);
        }
    }
}
