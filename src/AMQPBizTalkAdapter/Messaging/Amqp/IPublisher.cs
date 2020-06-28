using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter
{
    interface IPublisher
    {
        void Publish(System.ServiceModel.Channels.Message wcfmessage, MethodTracer methodTracer, TimeSpan openTimeOut, Encoding mEncoding);
    }
}
