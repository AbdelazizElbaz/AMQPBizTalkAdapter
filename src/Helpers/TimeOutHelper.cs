using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter
{
    internal class TimeOutHelper
    {

        private TimeSpan timeout;
        private DateTime creationTime;
        //private bool isInfinite=false;

        private static TimeSpan infiniteTimeout = TimeSpan.FromMilliseconds(2147483647);

        public static TimeSpan Infinite
        {
            get
            {
                return TimeOutHelper.infiniteTimeout;
            }
        }

        public TimeSpan RemainingTimeout
        {
            get
            {
                /* if (this.isInfinite)
                 {
                     return TimeOutHelper.Infinite;
                 }*/
                return this.timeout.Subtract(DateTime.UtcNow.Subtract(this.creationTime));
            }
        }

        public TimeOutHelper(TimeSpan timeout)
        {
            this.creationTime = DateTime.UtcNow;
            this.timeout = timeout;
        }

        public TimeSpan GetRemainingTimeoutAndThrowIfExpired(string exceptionMessage)
        {
            /* if (this.isInfinite)
             {
                 return TimeOutHelper.Infinite;
             }*/
            TimeSpan remainingTimeout = this.RemainingTimeout;
            if (remainingTimeout < TimeSpan.Zero)
            {
                throw new TimeoutException(exceptionMessage);
            }
            return remainingTimeout;
        }

        public bool IsExpired
        {
            get
            {
                // return !this.isInfinite && this.RemainingTimeout < TimeSpan.Zero;
                return this.RemainingTimeout < TimeSpan.Zero;
            }
        }
    }


}
