/// -----------------------------------------------------------------------------------------------------------
/// Module      :  AMQPBizTalkAdapterTrace.cs
/// Description :  Implements adapter tracing
/// -----------------------------------------------------------------------------------------------------------

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.ServiceModel.Channels.Common;
#endregion

namespace AMQPBizTalkAdapter
{
    // Use AMQPBizTalkAdapterUtilities.Trace in the code to trace the adapter
    public class AMQPBizTalkAdapterUtilities
    {
        //
        // Initializes a new instane of  Microsoft.ServiceModel.Channels.Common.AdapterTrace using the specified name for the source
        //
        static AdapterTrace trace = new AdapterTrace("AMQPBizTalkAdapter");

        /// <summary>
        /// Gets the AdapterTrace
        /// </summary>
        public static AdapterTrace Trace
        {
            get
            {
                return trace;
            }
        }

    }


}

