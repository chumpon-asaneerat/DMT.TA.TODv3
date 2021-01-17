#region Using

using System;
using System.Text;
using System.Reflection;
using NLib;
// RabbitMQ
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

#endregion

namespace DMT.Services
{
    #region Delegate and Event Args class.

    /// <summary>
    /// The QueueMessage Event Handler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    public delegate void QueueMessageEventHandler(object sender, QueueMessageEventArgs e);
    /// <summary>
    /// The QueueMessage Event Args.
    /// </summary>
    public class QueueMessageEventArgs
    {
        /// <summary>
        /// Gets or sets message.
        /// </summary>
        public string Message { get; set; }
    }

    #endregion

    #region RabbitMQClient

    /// <summary>
    /// The Rabbit MQ Client class.
    /// </summary>
    public class RabbitMQClient
    {
        #region Helper Class

        /// <summary>
        /// The MessageReceiver helper class.
        /// </summary>
        class MessageReceiver : DefaultBasicConsumer
        {
            /// <summary>
            /// The RabbitMQReceiceMessage delegate
            /// </summary>
            /// <param name="szMessage"></param>
            public delegate void RabbitMQReceiceMessage(string szMessage);
            /// <summary>
            /// The rabbitMQRecvMessage event.
            /// </summary>
            public event RabbitMQReceiceMessage rabbitMQRecvMessage;

            private readonly IModel _channel;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="channel"></param>
            public MessageReceiver(IModel channel)
            {
                _channel = channel;
            }

            /// <summary>
            /// Handle Basic Deliver.
            /// </summary>
            /// <param name="consumerTag"></param>
            /// <param name="deliveryTag"></param>
            /// <param name="redelivered"></param>
            /// <param name="exchange"></param>
            /// <param name="routingKey"></param>
            /// <param name="properties"></param>
            /// <param name="body"></param>
            public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered,
                string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
            {
                _channel.BasicAck(deliveryTag, false);
                rabbitMQRecvMessage(Encoding.UTF8.GetString(body.ToArray()));

                base.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body);
            }
        }

        #endregion

        #region Internal Variables

        private ConnectionFactory _factory = null;
        private IConnection _connection = null;
        private IModel _channel = null;
        private EventingBasicConsumer _consumer = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RabbitMQClient() : base()
        {
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~RabbitMQClient()
        {
            this.Disconnect();
        }

        #endregion

        #region Private Methods

        private void CreateFactory()
        {
            if (null != this._factory) return;
            this._factory = new ConnectionFactory()
            {
                HostName = this.HostName,
                UserName = this.UserName,
                Password = this.Password,
                Port = this.PortNumber,
                // VirtualHost Note:
                // "/" -> default
                // "cbe" -> required on production!!!!
                VirtualHost = this.VirtualHost,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(1)
            };
        }

        private void MessageReceiverOnRabbitMqRecvMessage(string szMessage)
        {
            OnMessageArrived.Call(this, new QueueMessageEventArgs() { Message = szMessage });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Connect
        /// </summary>
        /// <returns>Returns true if successfully connectd to RabbitMQ.</returns>
        public bool Connect()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                if (null == this._factory) CreateFactory(); // create factory.
                this._connection = (null != this._factory) ? this._factory.CreateConnection() : null;
                this._channel = (null != this._connection) ? this._connection.CreateModel() : null;
            }
            catch (Exception ex)
            {
                if (null != _factory)
                {
                    Console.WriteLine("Connect failed: Host:{0}, Port:{1}.",
                        _factory.HostName, _factory.Port);
                }
                //Console.WriteLine(ex);
                med.Err(ex);
                Disconnect(); // free channel and connection.
                return false;
            }
            return (null != this._channel);
        }
        /// <summary>
        /// Disconnect.
        /// </summary>
        public void Disconnect()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            #region Consumer

            if (null != this._consumer)
            {
                // No close or dispose method.
            }
            this._consumer = null;

            #endregion

            #region Close and Dispose channel

            if (null != this._channel)
            {
                try
                {
                    this._channel.Close();
                    this._channel.Dispose();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex);
                    med.Err(ex);
                }
            }
            this._channel = null;

            #endregion

            #region Close and Dispose Connection

            if (null != this._connection)
            {
                try
                {
                    this._connection.Close();
                    this._connection.Dispose();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex);
                    med.Err(ex);
                }
            }
            this._connection = null;

            #endregion

            #region Factory

            if (null != this._factory)
            {
                // No close or dispose method.
            }
            this._factory = null;

            #endregion
        }
        /// <summary>
        /// Listen.
        /// </summary>
        /// <param name="queueName">The Queue Name.</param>
        /// <returns></returns>
        public bool Listen(string queueName)
        {
            if (null == this._channel)
            {
                if (!this.Connect())
                {
                    Console.WriteLine("Listen -> connect failed. QueueName: {0}", queueName);
                    return false;
                }
            }

            this._channel.BasicQos(0, 1, false);
            var messageReceiver = new MessageReceiver(this._channel);
            this._channel.BasicConsume(queueName, false, messageReceiver);
            messageReceiver.rabbitMQRecvMessage += MessageReceiverOnRabbitMqRecvMessage;

            return true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Host Name or IP Address.
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// Gets or sets Port Number.
        /// </summary>
        public int PortNumber { get; set; }
        /// <summary>
        /// Gets or sets Virtual Host Name (default is '/').
        /// </summary>
        public string VirtualHost { get; set; }
        /// <summary>
        /// Gets or sets User Name.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Checks is connected.
        /// </summary>
        public bool Connected { get { return (null != this._channel && null != this._connection); } }

        #endregion

        #region Public Events

        /// <summary>
        /// The OnMessageArrived event.
        /// </summary>
        public event QueueMessageEventHandler OnMessageArrived;

        #endregion
    }

    #endregion
}
