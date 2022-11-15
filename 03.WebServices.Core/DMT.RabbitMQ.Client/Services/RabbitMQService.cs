#region Using

using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Timers;
using System.Threading.Tasks;

using NLib;
using NLib.IO;

using DMT.Configurations;
using DMT.Models;
using DMT.Services;

#endregion

namespace DMT.Services
{
    // TODO: Need to implements common class to provide each message to generate file, read data from file and process data

    /// <summary>
    /// The Rabbit MQ Service class.
    /// </summary>
    public class RabbitMQService : JsonMessageTransferService
    {
        #region Singelton

        private static RabbitMQService _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static RabbitMQService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(RabbitMQService))
                    {
                        _instance = new RabbitMQService();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private RabbitMQClient rabbitClient = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private RabbitMQService() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~RabbitMQService()
        {
            Shutdown();
        }

        #endregion

        #region Private Methods

        #region RabbitClient Message Handler

        private void RabbitClient_OnMessageArrived(object sender, QueueMessageEventArgs e)
        {
            if (null == e) return;

            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("RabbitMQ: New Message arrived.");
            // Save message.
            string fileName = "msg." + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss.ffffff",
                System.Globalization.DateTimeFormatInfo.InvariantInfo);
            med.Info("RabbitMQ: Write file:" + fileName);
            WriteFile(fileName, e.Message);
        }

        #endregion

        #endregion

        #region Override Methods and Properties

        /// <summary>
        /// Gets Folder Name (sub directory name).
        /// </summary>
        protected override string FolderName { get { return "rabbit.mq.msgs"; } }
        /// <summary>
        /// Process Json (string).
        /// </summary>
        /// <param name="fullFileName">The json full file name.</param>
        /// <param name="jsonString">The json data in string.</param>
        protected override void ProcessJson(string fullFileName, string jsonString)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            // Extract Header.
            var msg = jsonString.FromJson<Models.RabbitMQMessage>();
            if (null != msg)
            {
                if (msg.parameterName == "STAFF")
                {
                    med.Info("RabbitMQ: Detected STAFF file. Write to local database.");
                    // Update To Local Database.
                    var mq = jsonString.FromJson<Models.RabbitMQStaffMessage>();
                    if (null != mq)
                    {
                        var staffs = Models.RabbitMQStaff.ToLocals(mq.staff);
                        if (null != staffs && staffs.Count > 0)
                        {
                            Task.Run(() =>
                            {
                                Models.User.SaveUsers(staffs);
                            });
                        }
                        // process success backup file.
                        MoveToBackup(fullFileName);
                    }
                    else
                    {
                        // process success error file.
                        med.Err("RabbitMQ: Cannot convert to STAFF message.");
                        MoveToError(fullFileName);
                    }
                }
                else
                {
                    // process not staff list so Not Supports file.
                    med.Err("RabbitMQ: message is not STAFF message.");
                    MoveToNotSupports(fullFileName);
                }
            }
            else
            {
                // process success error file.
                med.Err("RabbitMQ: message is null or cannot convert to json object.");
                MoveToError(fullFileName);
            }
        }
        /// <summary>
        /// Resend Json (string) from error folder.
        /// </summary>
        /// <param name="fullFileName">The json full file name.</param>
        /// <param name="jsonString">The json data in string.</param>
        protected override void ResendJson(string fullFileName, string jsonString)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Err("RabbitMQ: Not Supports resend.");
        }
        /// <summary>
        /// OnStart.
        /// </summary>
        protected override void OnStart() 
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            // Init Rabbit Client
            if (null == rabbitClient)
            {
                if (null != RabbitMQ && RabbitMQ.Enabled)
                {
                    med.Info("RabbitMQ: Rabbit Host Info: " + RabbitMQ.GetString());
                    try
                    {
                        rabbitClient = new RabbitMQClient();
                        rabbitClient.HostName = RabbitMQ.HostName;
                        rabbitClient.PortNumber = RabbitMQ.PortNumber;
                        rabbitClient.VirtualHost = RabbitMQ.VirtualHost;
                        rabbitClient.UserName = RabbitMQ.UserName;
                        rabbitClient.Password = RabbitMQ.Password;
                        rabbitClient.OnMessageArrived += RabbitClient_OnMessageArrived;
                        if (rabbitClient.Connect() && rabbitClient.Listen(RabbitMQ.QueueName))
                        {
                            med.Info("RabbitMQ: Success connected to : " + RabbitMQ.GetString());
                            med.Info("RabbitMQ: Listen to Queue: " + RabbitMQ.QueueName);
                        }
                        else
                        {
                            med.Err("RabbitMQ: failed connected to : " + RabbitMQ.HostName);
                        }

                    }
                    catch (Exception ex)
                    {
                        med.Err(ex);
                    }
                }
            }
        }
        /// <summary>
        /// OnShutdown.
        /// </summary>
        protected override void OnShutdown() 
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            // Free Rabbit Client
            try
            {
                if (null != rabbitClient)
                {
                    rabbitClient.OnMessageArrived -= RabbitClient_OnMessageArrived;
                    rabbitClient.Disconnect();

                }
            }
            catch { }

            med.Info("RabbitMQ: Shutdown");

            rabbitClient = null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Checks is Rabbit MQ Connected.
        /// </summary>
        public bool Connected
        {
            get { return (null != rabbitClient && rabbitClient.Connected); }
        }
        /// <summary>
        /// Gets or sets RabbitMQ Service Config.
        /// </summary>
        public RabbitMQServiceConfig RabbitMQ { get; set; }

        #endregion
    }
}
