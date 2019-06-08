using Microsoft.ServiceModel.Channels.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter
{
    internal class AMQPBizTalkAdapterTracer
    {
        //
        // Initializes a new instane of  Microsoft.ServiceModel.Channels.Common.AdapterTrace using the specified name for the source
        //
        public const string AdapterTraceEventSource = "WCF-AMQP";
        static AdapterTrace trace = new AdapterTrace(AdapterTraceEventSource);


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
        internal static MethodTracer CreateMethodTracer(string uri, bool IsOutBound, bool enableTrace)
        {
            String EventCode = IsOutBound == true ? string.Format("Receive Location with uri ={0}", uri) : string.Format("Send port witth uri ={0}", uri);
            return new MethodTracer(AMQPBizTalkAdapterTracer.Trace, new StackFrame(1, true).GetMethod().Name, EventCode, enableTrace);
        }
    }
    internal class MethodTracer : IDisposable
    {
        private readonly AdapterTrace mTrace;

        private readonly string mMethodName;

        private readonly string mTraceEventCode;

        public readonly bool ShouldTraceVerbose;
        public readonly bool EnbaleTraceAll;

        EventLog eventLogTracer;
        private EventLog EventLogTracer
        {

            get
            {
                if (eventLogTracer == null)
                {
                    eventLogTracer = new EventLog();
                    eventLogTracer.Log = "Application";
                    eventLogTracer.Source = AMQPBizTalkAdapterTracer.AdapterTraceEventSource;
                }
                return eventLogTracer;
            }

        }

        TraceSource traceSource;
        private TraceSource TraceSource
        {

            get
            {
                if (traceSource == null)
                {
                    traceSource = new TraceSource(AMQPBizTalkAdapterTracer.AdapterTraceEventSource);
                    traceSource.Switch = new SourceSwitch("SourceSwitch");
                    traceSource.Switch.Level = SourceLevels.All;
                    EventLogTraceListener eventLogTraceListener = new EventLogTraceListener(AMQPBizTalkAdapterTracer.AdapterTraceEventSource);
                    traceSource.Listeners.Add(eventLogTraceListener);
                }
                return traceSource;
            }

        }


        public MethodTracer(AdapterTrace traceObject, bool enableTrace)
            : this(traceObject, new StackFrame(1, true).GetMethod().Name, null, enableTrace)
        {
        }

        public MethodTracer(string traceEventCode, AdapterTrace traceObject, bool enableTrace)
            : this(traceObject, new StackFrame(1, true).GetMethod().Name, traceEventCode, enableTrace)
        {
        }

        public MethodTracer(AdapterTrace traceObject, string methodName, string traceEventCode, bool enableTrace)
        {
            this.mTrace = traceObject;
            this.mMethodName = methodName;
            this.mTraceEventCode = traceEventCode;
            this.EnbaleTraceAll = enableTrace;
            if (this.ShouldTraceVerbose)
            {
                this.mTrace.Trace(TraceEventType.Verbose, this.mTraceEventCode, this.mMethodName + "::Enter");
            }
            if (enableTrace)
            {
                this.mTrace = new AdapterTrace(this.TraceSource);
            }
            this.ShouldTraceVerbose = this.mTrace.ShouldTrace(TraceEventType.Verbose);
        }

        public void TraceReturn(bool returnValue)
        {
            if (this.ShouldTraceVerbose)
            {
                if (returnValue)
                {
                    this.mTrace.Trace(TraceEventType.Verbose, this.mTraceEventCode, this.mMethodName + "::Return: TRUE");
                    return;
                }
                this.mTrace.Trace(TraceEventType.Verbose, this.mTraceEventCode, this.mMethodName + "::Return: FALSE");
            }
        }

        public void TraceData(TraceEventType traceEventType, object data)
        {
            if (!this.EnbaleTraceAll && (traceEventType == TraceEventType.Warning && traceEventType == TraceEventType.Error))
            {
                if (traceEventType == TraceEventType.Warning)
                    this.EventLogTracer.WriteEntry(string.Format("{0} : {1}::Warning : {2} ", this.mTraceEventCode, this.mMethodName, data.ToString()), EventLogEntryType.Warning);
                else
                    this.EventLogTracer.WriteEntry(string.Format("{0} : {1}::Exception : {2} ", this.mTraceEventCode, this.mMethodName, data.ToString()), EventLogEntryType.Error);
            }
            this.mTrace.Trace(traceEventType, this.mTraceEventCode, this.mMethodName + ": " + ((data == null) ? "(null)" : data.ToString()));
        }

        public void TraceArray(TraceEventType traceEventType, string prefix, string[] array)
        {
            if (this.mTrace.ShouldTrace(traceEventType))
            {
                string text = (array == null) ? "{}" : ("{" + string.Join(",", array) + "}");
                this.mTrace.Trace(traceEventType, this.mTraceEventCode, string.Concat(new string[]
                {
                    this.mMethodName,
                    ": ",
                    prefix,
                    ": ",
                    text
                }));
            }
        }

        public void TraceException(Exception exception)
        {
            if (!this.EnbaleTraceAll)
                this.EventLogTracer.WriteEntry(string.Format("{0} : {1}::Exception : {2} ", this.mTraceEventCode, this.mMethodName, exception.ToString()), EventLogEntryType.Error);

            this.mTrace.Trace(TraceEventType.Error, this.mTraceEventCode, this.mMethodName + "::Exception", null, exception);
        }
        public void TraceWarning(string warningMessage)
        {
            if (!this.EnbaleTraceAll)
                this.EventLogTracer.WriteEntry(string.Format("{0} : {1}::{2} ", this.mTraceEventCode, this.mMethodName, warningMessage), EventLogEntryType.Warning);

            this.mTrace.Trace(TraceEventType.Warning, this.mTraceEventCode, this.mMethodName + "::" + warningMessage);
        }

        public void TraceError(string errorMessage)
        {
            if (!this.EnbaleTraceAll)
                this.EventLogTracer.WriteEntry(string.Format("{0} : {1}::{2} ", this.mTraceEventCode, this.mMethodName, errorMessage), EventLogEntryType.Error);


            this.mTrace.Trace(TraceEventType.Error, this.mTraceEventCode, this.mMethodName + "::" + errorMessage);
        }

        void IDisposable.Dispose()
        {
            if (this.ShouldTraceVerbose)
            {
                this.mTrace.Trace(TraceEventType.Verbose, this.mTraceEventCode, this.mMethodName + "::Exit");
            }
        }

    }

}
