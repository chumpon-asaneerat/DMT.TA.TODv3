#region Using

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using NLib;
using NLib.IO;
using Newtonsoft.Json;
using NLib.Controls.Design;

#endregion

namespace DMT.Services
{
    #region Plaza Config and related classes

    #region Each config class

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

    #region LocalHostWebServiceConfig (Local IP Web Service Config) - ?????

    /// <summary>
    /// The LocalHostWebServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class LocalHostWebServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalHostWebServiceConfig()
        {
            this.Protocol = "http";
            this.PortNumber = 9001;
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
            if (null == obj || !(obj is LocalHostWebServiceConfig)) return false;
            return this.GetString() == (obj as LocalHostWebServiceConfig).GetString();
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
        [JsonIgnore]
        public string HostName
        {
            get
            {
                var ip = NLib.Utils.NetworkUtils.GetLocalIPAddress();
                return (null != ip) ? ip.ToString() : "unknown";
            }
            set { }
        }
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

    #region LocalWebServiceConfig (For Local Plaza Web Service)

    /// <summary>
    /// The LocalWebServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class LocalWebServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalWebServiceConfig() 
        {
            this.Service = new WebServiceConfig()
            {
                Protocol = "http",
                HostName = "localhost",
                PortNumber = 9000,
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
            if (null == obj || !(obj is LocalWebServiceConfig)) return false;
            return this.GetString() == (obj as LocalWebServiceConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            if (null != this.Service)
                return string.Format("{0}", this.Service.GetString());
            else return "Local http is null.";
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
                HostName = "localhost",
                PortNumber = 3000,
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
                HostName = "172.30.192.9",
                PortNumber = 8110,
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

        #endregion
    }

    #endregion

    #endregion

    #region PlazaServiceConfig (Combine configuration used in Local Plaza Windows Service)

    /// <summary>
    /// The PlazaServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class PlazaServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlazaServiceConfig() : base()
        {
            this.DMT = new DMTConfig();
            this.Plaza = new LocalWebServiceConfig();
            this.TAxTOD = new TAxTODWebServiceConfig();
            this.SCW = new SCWWebServiceConfig();
            this.RabbitMQ = new RabbitMQServiceConfig();
            this.TAApp = new TAAppWebServiceConfig();
            this.TODApp = new TODAppWebServiceConfig();
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
            if (null == obj || !(obj is PlazaServiceConfig)) return false;
            return this.GetString() == (obj as PlazaServiceConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string code = string.Empty;
            // Application
            if (null == this.DMT)
            {
                code += "DMT: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("DMT: {0}",
                    this.DMT.GetString()) + Environment.NewLine;
            }
            // Local Plaza server
            if (null == this.Plaza)
            {
                code += "Plaza: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("Plaza: {0}",
                    this.Plaza.GetString()) + Environment.NewLine;
            }
            // TAxTOD server
            if (null == this.TAxTOD)
            {
                code += "TAxTOD: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("TAxTOD: {0}", 
                    this.TAxTOD.GetString()) + Environment.NewLine;
            }
            // SCW server
            if (null == this.SCW)
            {
                code += "SCW: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("DC: {0}", 
                    this.SCW.GetString()) + Environment.NewLine;
            }
            // RabbitMQ
            if (null == this.RabbitMQ)
            {
                code += "RabbitMQ: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("RabbitMQ: {0}",
                    this.RabbitMQ.GetString()) + Environment.NewLine;
            }
            // TA Application (Plaza)
            if (null == this.TAApp)
            {
                code += "TAApp: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("TAApp: {0}",
                    this.TAApp.GetString()) + Environment.NewLine;
            }
            // TOD Application (Plaza)
            if (null == this.TODApp)
            {
                code += "TODApp: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("TODApp: {0}",
                    this.TODApp.GetString()) + Environment.NewLine;
            }
            return code;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets DMT Config.
        /// </summary>
        public DMTConfig DMT { get; set; }
        /// <summary>
        /// Gets or sets Rabbit MQ Service Config.
        /// </summary>
        public RabbitMQServiceConfig RabbitMQ { get; set; }
        /// <summary>
        /// Gets or sets Local Plaza Service Config.
        /// </summary>
        public LocalWebServiceConfig Plaza { get; set; }
        /// <summary>
        /// Gets or sets TAxTOD Service Config.
        /// </summary>
        public TAxTODWebServiceConfig TAxTOD { get; set; }
        /// <summary>
        /// Gets or sets SCW Service Config.
        /// </summary>
        public SCWWebServiceConfig SCW { get; set; }
        /// <summary>
        /// Gets or sets TA App Service Config (local server).
        /// </summary>
        public TAAppWebServiceConfig TAApp { get; set; }
        /// <summary>
        /// Gets or sets TOD App Service Config (local server).
        /// </summary>
        public TODAppWebServiceConfig TODApp { get; set; }

        #endregion
    }

    #endregion

    #region PlazaAppPConfig (Combine configuration used in TA Plaza applicaltion)

    /// <summary>
    /// The PlazaAppPConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class PlazaAppConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlazaAppConfig() : base()
        {
            this.DMT = new DMTConfig();
            this.Plaza = new LocalWebServiceConfig();
            this.SCW = new SCWWebServiceConfig();
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
            if (null == obj || !(obj is PlazaAppConfig)) return false;
            return this.GetString() == (obj as PlazaAppConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string code = string.Empty;
            // Application
            if (null == this.DMT)
            {
                code += "DMT: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("DMT: {0}",
                    this.DMT.GetString()) + Environment.NewLine;
            }
            // Local
            if (null == this.Plaza)
            {
                code += "Plaza null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("Plaza: {0}",
                    this.Plaza.GetString()) + Environment.NewLine;
            }
            // SCW server
            if (null == this.SCW)
            {
                code += "SCW: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("DC: {0}",
                    this.SCW.GetString()) + Environment.NewLine;
            }
            return code;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets DMT Config.
        /// </summary>
        public DMTConfig DMT { get; set; }
        /// <summary>
        /// Gets or sets Local Plaza Service Config.
        /// </summary>
        public LocalWebServiceConfig Plaza { get; set; }
        /// <summary>
        /// Gets or sets SCW Service Config.
        /// </summary>
        public SCWWebServiceConfig SCW { get; set; }

        #endregion
    }

    #endregion

    #region TAAppPlazaConfig (Combine configuration used in TA Plaza applicaltion)

    /// <summary>
    /// The TAAppPlazaConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TAAppPlazaConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TAAppPlazaConfig() : base()
        {
            this.DMT = new DMTConfig();
            this.Plaza = new LocalWebServiceConfig();
            this.SCW = new SCWWebServiceConfig();
            this.TAApp = new TAAppWebServiceConfig();
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
            if (null == obj || !(obj is TAAppPlazaConfig)) return false;
            return this.GetString() == (obj as TAAppPlazaConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string code = string.Empty;
            // Application
            if (null == this.DMT)
            {
                code += "DMT: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("DMT: {0}",
                    this.DMT.GetString()) + Environment.NewLine;
            }
            // Local
            if (null == this.Plaza)
            {
                code += "Plaza null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("Plaza: {0}",
                    this.Plaza.GetString()) + Environment.NewLine;
            }
            // SCW server
            if (null == this.SCW)
            {
                code += "SCW: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("DC: {0}",
                    this.SCW.GetString()) + Environment.NewLine;
            }
            // TA Application (Plaza)
            if (null == this.TAApp)
            {
                code += "TAApp: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("TAApp: {0}",
                    this.TAApp.GetString()) + Environment.NewLine;
            }
            return code;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets DMT Config.
        /// </summary>
        public DMTConfig DMT { get; set; }
        /// <summary>
        /// Gets or sets Local Plaza Service Config.
        /// </summary>
        public LocalWebServiceConfig Plaza { get; set; }
        /// <summary>
        /// Gets or sets SCW Service Config.
        /// </summary>
        public SCWWebServiceConfig SCW { get; set; }
        /// <summary>
        /// Gets or sets TA App Service Config (local server).
        /// </summary>
        public TAAppWebServiceConfig TAApp { get; set; }

        #endregion
    }

    #endregion

    #region TODAppPlazaConfig (Combine configuration used in TOD Plaza applicaltion)

    /// <summary>
    /// The TODPlazaConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TODAppPlazaConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TODAppPlazaConfig() : base()
        {
            this.DMT = new DMTConfig();
            this.Plaza = new LocalWebServiceConfig();
            this.SCW = new SCWWebServiceConfig();
            this.TODApp = new TODAppWebServiceConfig();
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
            if (null == obj || !(obj is TODAppPlazaConfig)) return false;
            return this.GetString() == (obj as TODAppPlazaConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string code = string.Empty;
            // Application
            if (null == this.DMT)
            {
                code += "DMT: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("DMT: {0}",
                    this.DMT.GetString()) + Environment.NewLine;
            }
            // Local
            if (null == this.Plaza)
            {
                code += "Plaza: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("Plaza: {0}",
                    this.Plaza.GetString()) + Environment.NewLine;
            }
            // SCW server
            if (null == this.SCW)
            {
                code += "SCW: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("DC: {0}",
                    this.SCW.GetString()) + Environment.NewLine;
            }
            // TOD Application (Plaza)
            if (null == this.TODApp)
            {
                code += "TODApp: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("TODApp: {0}",
                    this.TODApp.GetString()) + Environment.NewLine;
            }
            return code;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets DMT Config.
        /// </summary>
        public DMTConfig DMT { get; set; }
        /// <summary>
        /// Gets or sets Plaza Local Service Config.
        /// </summary>
        public LocalWebServiceConfig Plaza { get; set; }
        /// <summary>
        /// Gets or sets SCW Service Config.
        /// </summary>
        public SCWWebServiceConfig SCW { get; set; }
        /// <summary>
        /// Gets or sets TOD App Service Config (local server).
        /// </summary>
        public TODAppWebServiceConfig TODApp { get; set; }

        #endregion
    }

    #endregion

    #region AccountAppPlazaConfig (Combine configuration used in TA Account applicaltion)

    /// <summary>
    /// The AccountAppPlazaConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class AccountAppPlazaConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountAppPlazaConfig() : base()
        {
            this.DMT = new DMTConfig();
            this.RabbitMQ = new RabbitMQServiceConfig();
            this.SCW = new SCWWebServiceConfig();
            this.TAxTOD = new TAxTODWebServiceConfig();
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
            if (null == obj || !(obj is TAAppPlazaConfig)) return false;
            return this.GetString() == (obj as TAAppPlazaConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string code = string.Empty;
            // Application
            if (null == this.DMT)
            {
                code += "DMT: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("DMT: {0}",
                    this.DMT.GetString()) + Environment.NewLine;
            }
            // RabbitMQ
            if (null == this.RabbitMQ)
            {
                code += "RabbitMQ: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("RabbitMQ: {0}",
                    this.RabbitMQ.GetString()) + Environment.NewLine;
            }
            // SCW server
            if (null == this.SCW)
            {
                code += "SCW: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("DC: {0}",
                    this.SCW.GetString()) + Environment.NewLine;
            }
            // TAxTOD Server
            if (null == this.TAxTOD)
            {
                code += "TAxTOD: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("TAxTOD: {0}",
                    this.TAxTOD.GetString()) + Environment.NewLine;
            }
            return code;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets DMT Config.
        /// </summary>
        public DMTConfig DMT { get; set; }
        /// <summary>
        /// Gets or sets Rabbit MQ Service Config.
        /// </summary>
        public RabbitMQServiceConfig RabbitMQ { get; set; }
        /// <summary>
        /// Gets or sets SCW Service Config.
        /// </summary>
        public SCWWebServiceConfig SCW { get; set; }
        /// <summary>
        /// Gets or sets TAxTOD Service Config.
        /// </summary>
        public TAxTODWebServiceConfig TAxTOD { get; set; }

        #endregion
    }

    #endregion

    #endregion

    #region Plaza Config and related interfaces

    #region IDMTConfig Interface

    /// <summary>
    /// The IDMTConfig inferface.
    /// </summary>
    public interface IDMTConfig
    {
        /// <summary>
        /// Gets DMT Config.
        /// </summary>
        DMTConfig DMT { get; }
    }

    #endregion

    #region IRabbitMQConfig Interface

    /// <summary>
    /// The IRabbitMQConfig inferface.
    /// </summary>
    public interface IRabbitMQConfig
    {
        /// <summary>
        /// Gets RabbitMQ Config.
        /// </summary>
        RabbitMQServiceConfig RabbitMQ { get; }
    }

    #endregion

    #region IPlazaConfig Interface

    /// <summary>
    /// The IPlazaConfig inferface.
    /// </summary>
    public interface IPlazaConfig
    {
        /// <summary>
        /// Gets Local Plaza Config.
        /// </summary>
        LocalWebServiceConfig Plaza { get; }
    }

    #endregion

    #region ISCWConfig Interface

    /// <summary>
    /// The ISCWConfig inferface.
    /// </summary>
    public interface ISCWConfig
    {
        /// <summary>
        /// Gets SCW Config.
        /// </summary>
        SCWWebServiceConfig SCW { get; }
    }

    #endregion

    #region ITAxTODConfig Interface

    /// <summary>
    /// The ITAxTODConfig inferface.
    /// </summary>
    public interface ITAxTODConfig
    {
        /// <summary>
        /// Gets TAxTOD Config.
        /// </summary>
        TAxTODWebServiceConfig TAxTOD { get; }
    }

    #endregion

    #region ITAAppConfig Interface

    /// <summary>
    /// The ITAAppConfig inferface.
    /// </summary>
    public interface ITAAppConfig
    {
        /// <summary>
        /// Gets TAApp Config.
        /// </summary>
        TAAppWebServiceConfig TAApp { get; }
    }

    #endregion

    #region ITODAppConfig Interface

    /// <summary>
    /// The ITODAppConfig inferface.
    /// </summary>
    public interface ITODAppConfig
    {
        /// <summary>
        /// Gets TODApp Config.
        /// </summary>
        TODAppWebServiceConfig TODApp { get; }
    }

    #endregion

    #endregion

    #region JsonConfigFileManger (abstract)

    /// <summary>
    /// The JsonConfigFileManger abstract class.
    /// </summary>
    /// <typeparam name="T">The Config Class Type.</typeparam>
    public abstract class JsonConfigFileManger<T>
        where T : new()
    {
        #region Internal Variables

        private T _cfg = new T();

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public JsonConfigFileManger() : base()
        {
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~JsonConfigFileManger()
        {
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets Config File Name.
        /// </summary>
        public abstract string FileName { get; }
        /// <summary>
        /// Raise Config Changed Event.
        /// </summary>
        protected void RaiseConfigChanged()
        {
            // Raise event.
            ConfigChanged.Call(this, EventArgs.Empty);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load Config from file.
        /// </summary>
        public virtual void LoadConfig()
        {
            lock (this)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    // save back to file.
                    if (!NJson.ConfigExists(FileName))
                    {
                        // File not exist.
                        if (null == _cfg)
                        {
                            _cfg = new T();
                        }
                        NJson.SaveToFile(_cfg, FileName);
                    }
                    else
                    {
                        // Check file size.
                        long len = new FileInfo(FileName).Length;
                        if (len <= 0)
                        {
                            // File size is zero.
                            if (null == _cfg)
                            {
                                _cfg = new T();
                            }
                            NJson.SaveToFile(_cfg, FileName);
                        }
                        else
                        {
                            _cfg = NJson.LoadFromFile<T>(FileName);
                        }
                    }
                    // Raise event.
                    RaiseConfigChanged();
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            }
        }
        /// <summary>
        /// Save Config to file.
        /// </summary>
        public virtual void SaveConfig()
        {
            lock (this)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    // save back to file.
                    if (null == _cfg)
                    {
                        _cfg = new T();
                    }
                    NJson.SaveToFile(_cfg, FileName);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets current config.
        /// </summary>
        public T Value { get { return _cfg; } set { } }

        #endregion

        #region Public Events

        /// <summary>
        /// The ConfigChanged Event Handler.
        /// </summary>
        public event EventHandler ConfigChanged;

        #endregion
    }

    #endregion

    #region PlazaServiceConfigManager

    /// <summary>
    /// The PlazaServiceConfigManager class.
    /// </summary>
    public class PlazaServiceConfigManager : JsonConfigFileManger<PlazaServiceConfig>,
        IDMTConfig, IRabbitMQConfig, IPlazaConfig, ISCWConfig, ITAxTODConfig,
        ITAAppConfig, ITODAppConfig
    {
        #region Static Instance Access

        private static PlazaServiceConfigManager _instance = null;

        /// <summary>
        /// Gets ConfigManager instance access.
        /// </summary>
        public static PlazaServiceConfigManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(PlazaServiceConfigManager))
                    {
                        _instance = new PlazaServiceConfigManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private string _fileName = NJson.LocalConfigFile("plaza.service.config.json");

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private PlazaServiceConfigManager() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~PlazaServiceConfigManager()
        {
            //Shutdown();
        }

        #endregion

        #region Override Methods and Properties

        /// <summary>
        /// Gets Config File Name.
        /// </summary>
        public override string FileName { get { return _fileName; } }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets DMT Config.
        /// </summary>
        public DMTConfig DMT 
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.DMT : null;
            }
        }
        /// <summary>
        /// Gets RabbitMQ Config.
        /// </summary>
        public RabbitMQServiceConfig RabbitMQ
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.RabbitMQ : null;
            }
        }
        /// <summary>
        /// Gets Plaza Config.
        /// </summary>
        public LocalWebServiceConfig Plaza
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.Plaza : null;
            }
        }
        /// <summary>
        /// Gets SCW Config.
        /// </summary>
        public SCWWebServiceConfig SCW
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.SCW : null;
            }
        }
        /// <summary>
        /// Gets TAxTOD Config.
        /// </summary>
        public TAxTODWebServiceConfig TAxTOD
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.TAxTOD : null;
            }
        }
        /// <summary>
        /// Gets TAApp Config.
        /// </summary>
        public TAAppWebServiceConfig TAApp
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.TAApp : null;
            }
        }
        /// <summary>
        /// Gets TODApp Config.
        /// </summary>
        public TODAppWebServiceConfig TODApp
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.TODApp : null;
            }
        }

        #endregion
    }

    #endregion

    #region PlazaAppConfigManager

    /// <summary>
    /// Plaza App Config Manager class.
    /// </summary>
    public class PlazaAppConfigManager : JsonConfigFileManger<PlazaAppConfig>,
        IDMTConfig, IPlazaConfig, ISCWConfig
    {
        #region Static Instance Access

        private static PlazaAppConfigManager _instance = null;

        /// <summary>
        /// Gets ConfigManager instance access.
        /// </summary>
        public static PlazaAppConfigManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(PlazaAppConfigManager))
                    {
                        _instance = new PlazaAppConfigManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private string _fileName = NJson.LocalConfigFile("plaza.app.config.json");

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private PlazaAppConfigManager() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~PlazaAppConfigManager()
        {
            //Shutdown();
        }

        #endregion

        #region Override Methods and Properties

        /// <summary>
        /// Gets Config File Name.
        /// </summary>
        public override string FileName { get { return _fileName; } }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets DMT Config.
        /// </summary>
        public DMTConfig DMT
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.DMT : null;
            }
        }
        /// <summary>
        /// Gets Plaza Config.
        /// </summary>
        public LocalWebServiceConfig Plaza
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.Plaza : null;
            }
        }
        /// <summary>
        /// Gets SCW Config.
        /// </summary>
        public SCWWebServiceConfig SCW
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.SCW : null;
            }
        }

        #endregion
    }

    #endregion

    #region TAConfigManager

    /// <summary>
    /// TA Config Manager class.
    /// </summary>
    public class TAConfigManager : JsonConfigFileManger<TAAppPlazaConfig>,
        IDMTConfig, IPlazaConfig, ISCWConfig
    {
        #region Static Instance Access

        private static TAConfigManager _instance = null;

        /// <summary>
        /// Gets ConfigManager instance access.
        /// </summary>
        public static TAConfigManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TAConfigManager))
                    {
                        _instance = new TAConfigManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private string _fileName = NJson.LocalConfigFile("TA.app.config.json");

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private TAConfigManager() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TAConfigManager()
        {
            //Shutdown();
        }

        #endregion

        #region Override Methods and Properties

        /// <summary>
        /// Gets Config File Name.
        /// </summary>
        public override string FileName { get { return _fileName; } }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets DMT Config.
        /// </summary>
        public DMTConfig DMT
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.DMT : null;
            }
        }
        /// <summary>
        /// Gets Plaza Config.
        /// </summary>
        public LocalWebServiceConfig Plaza
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.Plaza : null;
            }
        }
        /// <summary>
        /// Gets SCW Config.
        /// </summary>
        public SCWWebServiceConfig SCW
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.SCW : null;
            }
        }
        /// <summary>
        /// Gets TAApp Config.
        /// </summary>
        public TAAppWebServiceConfig TAApp
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.TAApp : null;
            }
        }

        #endregion
    }

    #endregion

    #region TODConfigManager

    /// <summary>
    /// TOD Config Manager class.
    /// </summary>
    public class TODConfigManager : JsonConfigFileManger<TODAppPlazaConfig>,
        IDMTConfig, IPlazaConfig, ISCWConfig
    {
        #region Static Instance Access

        private static TODConfigManager _instance = null;

        /// <summary>
        /// Gets ConfigManager instance access.
        /// </summary>
        public static TODConfigManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TODConfigManager))
                    {
                        _instance = new TODConfigManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private string _fileName = NJson.LocalConfigFile("TOD.app.config.json");

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private TODConfigManager() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TODConfigManager()
        {
            //Shutdown();
        }

        #endregion

        #region Override Methods and Properties

        /// <summary>
        /// Gets Config File Name.
        /// </summary>
        public override string FileName { get { return _fileName; } }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets DMT Config.
        /// </summary>
        public DMTConfig DMT
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.DMT : null;
            }
        }
        /// <summary>
        /// Gets Plaza Config.
        /// </summary>
        public LocalWebServiceConfig Plaza
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.Plaza : null;
            }
        }
        /// <summary>
        /// Gets SCW Config.
        /// </summary>
        public SCWWebServiceConfig SCW
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.SCW : null;
            }
        }
        /// <summary>
        /// Gets TODApp Config.
        /// </summary>
        public TODAppWebServiceConfig TODApp
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.TODApp : null;
            }
        }

        #endregion
    }

    #endregion

    #region AccountConfigManager

    /// <summary>
    /// Account Config Manager class.
    /// </summary>
    public class AccountConfigManager : JsonConfigFileManger<AccountAppPlazaConfig>,
        IDMTConfig, IRabbitMQConfig, ITAxTODConfig, ISCWConfig
    {
        #region Static Instance Access

        private static AccountConfigManager _instance = null;

        /// <summary>
        /// Gets ConfigManager instance access.
        /// </summary>
        public static AccountConfigManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TAConfigManager))
                    {
                        _instance = new AccountConfigManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private string _fileName = NJson.LocalConfigFile("Account.app.config.json");

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private AccountConfigManager() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~AccountConfigManager()
        {
            //Shutdown();
        }

        #endregion

        #region Override Methods and Properties

        /// <summary>
        /// Gets Config File Name.
        /// </summary>
        public override string FileName { get { return _fileName; } }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets DMT Config.
        /// </summary>
        public DMTConfig DMT
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.DMT : null;
            }
        }
        /// <summary>
        /// Gets RabbitMQ Config.
        /// </summary>
        public RabbitMQServiceConfig RabbitMQ
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.RabbitMQ : null;
            }
        }
        /// <summary>
        /// Gets SCW Config.
        /// </summary>
        public SCWWebServiceConfig SCW
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.SCW : null;
            }
        }
        /// <summary>
        /// Gets TAxTOD Config.
        /// </summary>
        public TAxTODWebServiceConfig TAxTOD
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.TAxTOD : null;
            }
        }

        #endregion
    }

    #endregion
}
