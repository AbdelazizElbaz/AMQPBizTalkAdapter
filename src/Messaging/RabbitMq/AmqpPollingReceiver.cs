using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter
{
    class AmqpPollingReceiver : IReceiver
    {
        public void StartListener(TimeSpan timeout, MethodTracer methodTracer)
        {
            throw new NotImplementedException();
        }

        public void StopListener(TimeSpan timeout, MethodTracer methodTracer)
        {
            throw new NotImplementedException();
        }

        public bool TryReceive(TimeSpan timeout, MethodTracer methodTracer, out Message wcfMessage)
        {
            throw new NotImplementedException();
        }

        public bool WaitForMessage(TimeSpan timeout, MethodTracer methodTracer)
        {
            throw new NotImplementedException();
        }
    }
}
