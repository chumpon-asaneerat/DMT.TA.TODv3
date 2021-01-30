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
    #region DMTConfig (Common DMT Consts Information)

    /// <summary>
    /// The DMT Config class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class DMTConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public DMTConfig()
        {
            this.network = "4";
            this.tsb = "97";
            this.terminal = "49701";
            this.networkId = 31;
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
            if (null == obj || !(obj is DMTConfig)) return false;
            return this.GetString() == (obj as DMTConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string code = string.Format("network:{0}, tsb:{1}, terminal:{2}, networkId:{3}",
                this.network, this.tsb, this.terminal, this.networkId);
            return code;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets network.
        /// </summary>
        public string network { get; set; }
        /// <summary>
        /// Gets or sets tsb.
        /// </summary>
        public string tsb { get; set; }
        /// <summary>
        /// Gets or sets terminal.
        /// </summary>
        public string terminal { get; set; }
        /// <summary>
        /// Gets or sets networkId.
        /// </summary>
        public int networkId { get; set; }

        #endregion
    }

    #endregion

    #region WebServiceConfig (Common Web Service Config)

    /// <summary>
    /// The WebServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class WebServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public WebServiceConfig()
        {
            this.Protocol = "http";
            this.HostName = "localhost";
            this.PortNumber = 9000;
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
            if (null == obj || !(obj is WebServiceConfig)) return false;
            return this.GetString() == (obj as WebServiceConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string code = string.Format("{0}://{1}:{2}",
                this.Protocol, this.HostName, this.PortNumber);
            return code;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets protocol.
        /// </summary>
        public string Protocol { get; set; }
        /// <summary>
        /// Gets or sets Host Name or IP Address.
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// Gets or sets port number.
        /// </summary>
        public int PortNumber { get; set; }
        /// <summary>
        /// Gets or sets User Name.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; }

        #endregion
    }

    #endregion

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

    #region TAxTODWebServiceConfig (For TAxTOD Web Service)

    /// <summary>
    /// The TAxTODWebServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TAxTODWebServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TAxTODWebServiceConfig()
        {
            this.Service = new WebServiceConfig()
            {
                Protocol = "http",
                //HostName = "localhost",
                //PortNumber = 3000,
                HostName = "172.30.52.61",
                PortNumber = 8000,
                UserName = "DMTUSER",
                Password = "DMTPASS"
            };
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
            if (null == obj || !(obj is TAxTODWebServiceConfig)) return false;
            return this.GetString() == (obj as TAxTODWebServiceConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            if (null != this.Service)
                return string.Format("{0}", this.Service.GetString());
            else return "TAxTOD http is null.";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Http service.
        /// </summary>
        public WebServiceConfig Service { get; set; }

        #endregion
    }

    #endregion

    #region SCWWebServiceConfig (For SCW Web Service)

    /// <summary>
    /// The SCWWebServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SCWWebServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SCWWebServiceConfig()
        {
            this.Service = new WebServiceConfig()
            {
                Protocol = "http",
                //HostName = "172.30.192.9",
                //PortNumber = 8110,
                HostName = "172.30.192.9",
                PortNumber = 8000,
                UserName = "DMTUSER",
                Password = "DMTPASS"
            };
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
            if (null == obj || !(obj is SCWWebServiceConfig)) return false;
            return this.GetString() == (obj as SCWWebServiceConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            if (null != this.Service)
                return string.Format("{0}", this.Service.GetString());
            else return "DC http is null.";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Http service.
        /// </summary>
        public WebServiceConfig Service { get; set; }

        #endregion
    }

    #endregion

    #region TAAppWebServiceConfig (For TA App Web Service)

    /// <summary>
    /// The TAAppWebServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TAAppWebServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TAAppWebServiceConfig()
        {
            this.Service = new WebServiceConfig()
            {
                Protocol = "http",
                HostName = "localhost",
                PortNumber = 9001,
                UserName = "DMTUSER",
                Password = "DMTPASS"
            };
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
            if (null == obj || !(obj is TAAppWebServiceConfig)) return false;
            return this.GetString() == (obj as TAAppWebServiceConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            if (null != this.Service)
                return string.Format("{0}", this.Service.GetString());
            else return "TA App http is null.";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Http service.
        /// </summary>
        public WebServiceConfig Service { get; set; }

        #endregion
    }

    #endregion


    #region TODAppWebServiceConfig (For TOD App Web Service)

    /// <summary>
    /// The TODPlazaConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TODPlazaConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TODPlazaConfig() : base() 
        {
            this.PlazaId = 0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Plaza Id.
        /// </summary>
        public int PlazaId { get; set; }

        #endregion
    }

    #endregion

    #region TODAppWebServiceConfig (For TOD App Web Service)

    /// <summary>
    /// The TODAppWebServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TODAppWebServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TODAppWebServiceConfig()
        {
            this.Service = new WebServiceConfig()
            {
                Protocol = "http",
                HostName = "localhost",
                PortNumber = 9002,
                UserName = "DMTUSER",
                Password = "DMTPASS"
            };
            Plazas = new List<TODPlazaConfig>();
            Plazas.Add(new TODPlazaConfig() { PlazaId = 0 });
            Plazas.Add(new TODPlazaConfig() { PlazaId = 0 });
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
            if (null == obj || !(obj is TODAppWebServiceConfig)) return false;
            return this.GetString() == (obj as TODAppWebServiceConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            if (null != this.Service)
                return string.Format("{0}", this.Service.GetString());
            else return "TOD App http is null.";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Http service.
        /// </summary>
        public WebServiceConfig Service { get; set; }
        /// <summary>
        /// Gets or sets Plazas.
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<TODPlazaConfig> Plazas { get; set; }

        #endregion
    }

    #endregion
}
