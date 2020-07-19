using Amqp;
using Amqp.Framing;
using Amqp.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter
{
    internal class AmqpNetLiteReceiver
    {
        System.Timers.Timer receiveTimer;

        AMQPBizTalkAdapterConnection amqpBizTalkAdapterConnection;
        private readonly object locker = new object();
        bool closed = false;
        Amqp.Connection amqpconnection;
        Amqp.Session amqpsession;
        Amqp.ReceiverLink receiver;
        QueueTypeEnum queueType;
        string queueName;
        string subscriptionIdentifier;
        Queue<Amqp.Message> inboundQueue;
        TimeSpan startTimeout;
        private AmqpNetLiteReceiver() { }
        public AmqpNetLiteReceiver(AMQPBizTalkAdapterConnection bizTalkconnection, QueueTypeEnum queueType, string queueName, string subscriptionIdentifier)
        {
            this.amqpBizTalkAdapterConnection = bizTalkconnection;
            this.queueType = queueType;
            this.queueName = queueName;
            this.subscriptionIdentifier = subscriptionIdentifier;
            inboundQueue = new Queue<Message>();
        }

        private Source CreateDurableSource()
        {
            Source source = new Source();

            Symbol[] outcomes = new Symbol[] {new Symbol("amqp:accepted:list"),
                                          new Symbol("amqp:rejected:list"),
                                          new Symbol("amqp:released:list"),
                                          new Symbol("amqp:modified:list") };

            Modified defaultOutcome = new Modified();
            defaultOutcome.DeliveryFailed = true;
            defaultOutcome.UndeliverableHere = false;
            if (queueType == QueueTypeEnum.Queue)
            {
                source.Capabilities = new[] { new Symbol("queue") };
            }
            else if (queueType == QueueTypeEnum.Topic)
            {
                source.Capabilities = new[] { new Symbol("topic") };
            }

            // Configure Source.
            source.DefaultOutcome = defaultOutcome;
            source.Outcomes = outcomes;
            // Create a Durable Consumer Source.
            source.Address = this.queueType == QueueTypeEnum.Queue ? this.queueName : string.Format("topic://{0}", this.queueName);

            //durable
            source.ExpiryPolicy = new Symbol("never");
            source.Durable = 2;//Durable
            source.DistributionMode = new Symbol("copy");

            return source;
        }

        public void Start(TimeSpan timeout, MethodTracer methodTracer)
        {
            try
            {
                lock (locker)
                {
                    startTimeout = timeout;
                    this.amqpconnection = amqpBizTalkAdapterConnection.CreateAmqpNetLitConnectionFactory(timeout).CreateConnection();
                    methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Start listening uri= {0}", this.amqpBizTalkAdapterConnection.ConnectionUri.ToString()));
                    this.amqpsession = new Session(this.amqpconnection);
                    this.receiver = new ReceiverLink(this.amqpsession, this.subscriptionIdentifier, CreateDurableSource(), null);
                    //receiveTimer = new System.Timers.Timer(100);
                    //receiveTimer.Elapsed += ReceiveTimer_Elapsed;
                   // receiveTimer.Start();
                }
            }
            catch (Exception exception)
            {
                methodTracer.TraceData(TraceEventType.Error, "Error in AmqpNetLiteReceiver.Start");
                methodTracer.TraceException(exception);
                throw;
            }
        }

        /*private void ReceiveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            receiveTimer.Stop();
            if (!this.closed)
            {
                if (receiver.IsClosed)
                {
                    this.amqpconnection = amqpBizTalkAdapterConnection.CreateAmqpNetLitConnectionFactory(startTimeout).CreateConnection();
                    this.amqpsession = new Session(this.amqpconnection);
                    this.receiver = new ReceiverLink(this.amqpsession, this.subscriptionIdentifier, CreateDurableSource(), null);
                }
                Message message = receiver.Receive(new TimeSpan(0, 0, 1));
                if (message != null)
                {
                    lock (locker) { inboundQueue.Enqueue(message); }
                }
            }

           // receiveTimer.Start();
        }*/

        public void Stop(TimeSpan timeout, MethodTracer methodTracer)
        {
            try
            {
                methodTracer.TraceData(System.Diagnostics.TraceEventType.Verbose, string.Format("Stop listening uri= {0}", this.amqpBizTalkAdapterConnection.ConnectionUri));
                lock (locker)
                {
                    this.closed = true;

                    /*if (receiver != null)
                        receiver.Close();
                    if (amqpsession != null)
                        amqpsession.Close();
                    if (amqpconnection != null)
                        this.amqpconnection.Close();*/
                }
            }
            catch (Exception exception)
            {
                methodTracer.TraceData(TraceEventType.Error, "Error in AmqpNetLiteReceiver.Stop");
                methodTracer.TraceException(exception);
                throw;
            }
        }

        public List<Message> DequeueMessages(int lot)
        {
            List<Message> msgs = new List<Message>();
            lock (locker)
            {
                // int count = inboundQueue.Count >= lot ? lot : inboundQueue.Count;
                //for (int i = 0; i < count; i++)
                // {
                //   msgs.Add(inboundQueue.Dequeue());
                //}

                if (!this.closed)
                {
                    if (receiver.IsClosed)
                    {
                        this.amqpconnection = amqpBizTalkAdapterConnection.CreateAmqpNetLitConnectionFactory(startTimeout).CreateConnection();
                        this.amqpsession = new Session(this.amqpconnection);
                        this.receiver = new ReceiverLink(this.amqpsession, this.subscriptionIdentifier, CreateDurableSource(), null);
                    }
                    Message message = receiver.Receive(new TimeSpan(0, 0, 1));
                    if (message != null)
                    {
                        lock (locker) { inboundQueue.Enqueue(message); }
                    }
                }
                for (int i = 0; i < lot; i++)
                 {
                   msgs.Add(this.receiver.Receive(startTimeout));
                }
                
            }
            return msgs;
        }
        public void AckDelivery(Message msg)
        {
            this.receiver.Accept(msg);
        }
        public void AckDelivery(List<Message> msgs)
        {
            foreach (Message msg in msgs)
                this.receiver.Accept(msg);
        }
        public void NackDelivery(Message msg)
        {
            this.receiver.Reject(msg);
        }
        public bool IsAvailablesMessages
        {
            get
            {
                //lock (locker)
                //{
                    //return inboundQueue.Count > 0;
                    return true;
                //}
            }

        }
    }
}
