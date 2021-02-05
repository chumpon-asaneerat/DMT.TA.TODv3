#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using NLib;
using NLib.IO;
using Newtonsoft.Json;
using NLib.Controls.Design;

#endregion

namespace DMT.Configurations
{
    #region RabbitMQServiceConfig (For RabbitMQ Client)

    /// <summary>
    /// The RabbitMQServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class RabbitMQServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RabbitMQServiceConfig()
        {
            HostName = "172.30.73.11";
            PortNumber = 5672;
            VirtualHost = "cbe";
            QueueName = "qp.parameters.th03x009.taa01";
            UserName = "taa";
            Password = "taa123";
            Enabled = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// IsEquals.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsEquals(object obj)
        {
            if (null == obj || !(obj is RabbitMQServiceConfig)) return false;
            return this.GetString() == (obj as RabbitMQServiceConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string code = string.Format("Host:{0}, Port: {1}, VHost:{2}, QueueName: {3}",
                this.HostName, this.PortNumber, this.VirtualHost, this.QueueName);
            return code;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Host Name.
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// Gets or sets Port Number.
        /// </summary>
        public int PortNumber { get; set; }
        /// <summary>
        /// Gets or sets Virtual Host Name.
        /// </summary>
        public string VirtualHost { get; set; }
        /// <summary>
        /// Gets or sets Queue Name.
        /// </summary>
        public string QueueName { get; set; }
        /// <summary>
        /// Gets or sets User Name.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets Enabled.
        /// </summary>
        public bool Enabled { get; set; }

        #endregion
    }

    #endregion
}
