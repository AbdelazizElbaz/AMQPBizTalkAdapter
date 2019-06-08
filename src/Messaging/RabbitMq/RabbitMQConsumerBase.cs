
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMQPBizTalkAdapter
{
    class AmqpReceiverBase
    {
        #region Private
        const string exchangeQueue = "amq.direct";
        const string exchangeTopic = "amq.topic";
        object ObjectLocker;
        bool closed = false;
        RabbitMQ.Client.ConnectionFactory connectionFactory;
        Queue<BasicDeliverEventArgs> inboundQueue;
        QueueTypeEnum queueType;
        MethodTracer tracer;
        string queueName;
        string subscriptionIdentifier;
        string routingKey;

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            lock (ObjectLocker)
            {
                if (!this.closed)
                {
                    tracer.TraceData(TraceEventType.Verbose, "New Message arrived DeliveryTag={0} " + e.DeliveryTag);
                    inboundQueue.Enqueue(e);
                }
            }
        }
        #endregion
        #region Public

        public IConnection RabbitMQConnection { get; private set; }
        public IModel RabbitMQModel { get; private set; }
        public EventingBasicConsumer consumer { get; private set; }

        public void Start(TimeSpan timeout, MethodTracer methodTracer)
        {
            lock (ObjectLocker)
            {

                try
                {
                    methodTracer.TraceData(TraceEventType.Verbose, "StartListener");

                    this.RabbitMQConnection = connectionFactory.CreateConnection();
                    this.RabbitMQModel = RabbitMQConnection.CreateModel();

                    if (queueType==QueueTypeEnum.Topic)
                    {
                        //Topic 

                        //Declare a Topic to retrieve messages.
                        this.RabbitMQModel.ExchangeDeclare(exchangeTopic, "topic", true, false, null);
                        this.RabbitMQModel.QueueDeclare(subscriptionIdentifier, true, false, false, null);
                        this.RabbitMQModel.QueueBind(subscriptionIdentifier, exchangeTopic, routingKey);

                        this.consumer = new EventingBasicConsumer(RabbitMQModel);
                        this.consumer.Received += Consumer_Received;
                        this.RabbitMQModel.BasicConsume(subscriptionIdentifier, false, consumer);
                    }
                    else
                    {

                        //Queue

                        //Declare a queue to retrieve messages.
                        this.RabbitMQModel.ExchangeDeclare(exchangeQueue, "direct", true, false, null);
                        this.RabbitMQModel.QueueDeclare(queueName, true, false, false, null);
                        this.RabbitMQModel.QueueBind(queueName, exchangeQueue, queueName);
                        this.consumer = new EventingBasicConsumer(RabbitMQModel);
                        this.consumer.Received += Consumer_Received;
                        this.RabbitMQModel.BasicConsume(queueName, false, consumer);
                    }

                    


                }
                catch (Exception exception)
                {
                    methodTracer.TraceData(TraceEventType.Error, "Error in RabbitMQConsumerBase.StartListener");
                    methodTracer.TraceException(exception);
                    throw;
                }
            }
        }
        public void Stop(TimeSpan timeout, MethodTracer methodTracer)
        {
            try
            {
                this.closed = true;

                lock (ObjectLocker)
                {
                    if (consumer != null)
                    {
                        consumer.HandleBasicCancelOk(consumer.ConsumerTag);
                    }
                    if (RabbitMQModel != null)
                        RabbitMQModel.Abort();

                    if (RabbitMQConnection != null)
                        RabbitMQConnection.Abort();

                }
            }
            catch (Exception exception)
            {
                methodTracer.TraceData(TraceEventType.Error, "Error in RabbitMQConsumerBase.StopListener");
                methodTracer.TraceException(exception);
                throw;
            }

        }
        public bool CheckAvailablesMessages()
        {
            lock (ObjectLocker)
            {
                return inboundQueue.Count > 0;
            }
        }
        public List<BasicDeliverEventArgs> GetMessages(uint lot)
        {
            List<BasicDeliverEventArgs> msgs = new List<BasicDeliverEventArgs>();
            lock (ObjectLocker)
            {
                uint count = inboundQueue.Count >= lot ? lot : (uint)inboundQueue.Count;
                for (int i = 0; i < count; i++)
                {
                    msgs.Add(inboundQueue.Dequeue());
                }
            }
            return msgs;
        }

        #endregion
        #region Constructor
        public AmqpReceiverBase(RabbitMQ.Client.ConnectionFactory rabbitMQconnectionFactory,
            QueueTypeEnum queueType, MethodTracer methodTracer,string queue,string topicRoutingKey,string subscriptionId)
        {
            this.connectionFactory = rabbitMQconnectionFactory;
            this.tracer = methodTracer;
            this.queueType = queueType;
            this.ObjectLocker = new object();
            this.routingKey = topicRoutingKey;
            this.queueName = queue;
            this.inboundQueue = new Queue<BasicDeliverEventArgs>();
            this.subscriptionIdentifier = subscriptionId;
        }


        public void AckDelivery(ulong DeliveryTag)
        {
            RabbitMQModel.BasicAck(DeliveryTag, false);
        }
        public void AckDelivery(List<ulong> DeliveryTags)
        {
            foreach (ulong tag in DeliveryTags)
                RabbitMQModel.BasicAck(tag, false);
        }
        public void NackDelivery(ulong DeliveryTag)
        {
            RabbitMQModel.BasicNack(DeliveryTag, false, true);
        }


        #endregion
    }
}
