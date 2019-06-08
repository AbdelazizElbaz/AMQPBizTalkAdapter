using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter.Messaging
{
    class AmqpPublisher
    {
        AMQPBizTalkAdapterConnection connection;

        public AmqpPublisher(AMQPBizTalkAdapterConnection adapterConnection)
        {
            this.connection = adapterConnection;

        }

       
    }
}
