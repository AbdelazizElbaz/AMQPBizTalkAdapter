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
        public AmqpNetLiteConnectionFactory(Address address)
        {
            this.Address = address;
        }
        public Amqp.Connection CreateConnection()
        {
            return new Connection(this.Address);
        }
    }
}
