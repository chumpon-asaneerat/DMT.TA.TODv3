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
    #region TAAppServiceConfig

    /// <summary>
    /// The TAAppServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TAAppServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TAAppServiceConfig() : base()
        {
            this.DMT = new DMTConfig();
            this.SCW = new SCWWebServiceConfig();
            this.RabbitMQ = new RabbitMQServiceConfig();
            this.TAxTOD = new TAxTODWebServiceConfig();
            this.TAApp = new TAAppWebServiceConfig();

            this.TODApps = new List<TODAppWebServiceConfig>();

            this.TODApps.Add(new TODAppWebServiceConfig()
            {
                Service = new WebServiceConfig()
                {
                    Protocol = "http",
                    HostName = "localhost",
                    PortNumber = 9002,
                    UserName = "DMTUSER",
                    Password = "DMTPASS"
                }
            });
            this.TODApps.Add(new TODAppWebServiceConfig()
            {
                Service = new WebServiceConfig()
                {
                    Protocol = "http",
                    HostName = "localhost",
                    PortNumber = 9003,
                    UserName = "DMTUSER",
                    Password = "DMTPASS"
                }
            });
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
            if (null == obj || !(obj is TAAppServiceConfig)) return false;
            return this.GetString() == (obj as TAAppServiceConfig).GetString();
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
            if (null == this.RabbitMQ)
            {
                code += "RabbitMQ null" + Environment.NewLine;
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
                code += string.Format("SCW: {0}",
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
        /// <summary>
        /// Gets or sets TA App Service Config (local server).
        /// </summary>
        public TAAppWebServiceConfig TAApp { get; set; }

        /// <summary>
        /// Gets or sets TODApps.
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<TODAppWebServiceConfig> TODApps { get; set; }

        #endregion
    }

    #endregion

    #region TAHeaderBars

    /// <summary>
    /// The TODHeaderBars class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TAHeaderBars
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TAHeaderBars()
        {
            this.PlazaInfo = new PlazaInfoConfig()
            {
                TSBId = "09",
                PlazaId = "091",
                PlazaNameEN = "ANUSORN SATHAN",
                PlazaNameTH = "อนุสรน์สถาน"
            };
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets AppInfo status bar.</summary>
        public PlazaInfoConfig PlazaInfo { get; set; }

        #endregion
    }

    #endregion

    #region TAStatusBars

    /// <summary>
    /// The TAStatusBars class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TAStatusBars
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TAStatusBars()
        {
            this.AppInfo = new StatusBarConfig() { Visible = true };
            this.ClientInfo = new StatusBarConfig() { Visible = true };
            this.LocalDb = new StatusBarConfig() { Visible = true };
            this.RabbitMQ = new StatusBarConfig() { Visible = false };
            this.SCW = new StatusBarConfig() { Visible = false };
            this.TAServer = new StatusBarConfig() { Visible = true };
            this.TODApp = new StatusBarConfig() { Visible = false };
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets AppInfo status bar.</summary>
        public StatusBarConfig AppInfo { get; set; }
        /// <summary>Gets or sets ClientInfo status bar.</summary>
        public StatusBarConfig ClientInfo { get; set; }
        /// <summary>Gets or sets LocalDb status bar.</summary>
        public StatusBarConfig LocalDb { get; set; }
        /// <summary>Gets or sets RabbitMQ status bar.</summary>
        public StatusBarConfig RabbitMQ { get; set; }
        /// <summary>Gets or sets SCW status bar.</summary>
        public StatusBarConfig SCW { get; set; }
        /// <summary>Gets or sets TAServer status bar.</summary>
        public StatusBarConfig TAServer { get; set; }
        /// <summary>Gets or sets TOD App status bar.</summary>
        public StatusBarConfig TODApp { get; set; }

        #endregion
    }

    #endregion

    #region TAUIConfig

    /// <summary>
    /// The TAUIConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TAUIConfig
    {
        #region Constructor

        /// <summary>
        /// TAUIConfig.
        /// </summary>
        public TAUIConfig()
        {
            this.HeaderBars = new TAHeaderBars();
            this.StatusBars = new TAStatusBars();
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets TA header bars.</summary>
        public TAHeaderBars HeaderBars { get; set; }
        /// <summary>Gets or sets TA status bars.</summary>
        public TAStatusBars StatusBars { get; set; }

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
            this.Services = new TAAppServiceConfig();
            this.UIConfig = new TAUIConfig();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets TA App Service Config.
        /// </summary>
        public TAAppServiceConfig Services { get; set; }
        /// <summary>
        /// Gets or sets TA UI Config.
        /// </summary>
        public TAUIConfig UIConfig { get; set; }

        #endregion
    }

    #endregion
}
