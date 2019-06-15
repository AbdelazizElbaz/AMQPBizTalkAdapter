using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace AMQPBizTalkAdapter
{
    interface IReceiver
    {
        void StartListener(TimeSpan timeout, MethodTracer methodTracer);
        void StopListener(TimeSpan timeout, MethodTracer methodTracer);
        bool TryReceive(TimeSpan timeout, MethodTracer methodTracer, out Message wcfMessage);
        bool WaitForMessage(TimeSpan timeout, MethodTracer methodTracer);
    }
}
