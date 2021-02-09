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
    #region TODAppServiceConfig

    /// <summary>
    /// The TODAppServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TODAppServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TODAppServiceConfig() : base()
        {
            this.DMT = new DMTConfig();
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
            if (null == obj || !(obj is TODAppServiceConfig)) return false;
            return this.GetString() == (obj as TODAppServiceConfig).GetString();
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
                code += string.Format("SCW: {0}",
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

    #region TODPlazaConfig (Need for TOD App Plaza Config class)

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

    #region TODHeaderBars

    /// <summary>
    /// The TODHeaderBars class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TODHeaderBars
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TODHeaderBars()
        {
            this.PlazaInfo = new PlazaInfoConfig()
            {
                TSBId = "09",
                PlazaId = "091",
                PlazaNameEN = "ANUSORN SATHAN",
                PlazaNameTH = "อนุสรณ์สถาน"
            };
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets AppInfo status bar.</summary>
        public PlazaInfoConfig PlazaInfo { get; set; }

        #endregion
    }

    #endregion

    #region TODStatusBars

    /// <summary>
    /// The TODStatusBars class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TODStatusBars
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TODStatusBars()
        {
            this.AppInfo = new StatusBarConfig() { Visible = true };
            this.ClientInfo = new StatusBarConfig() { Visible = true };
            this.LocalDb = new StatusBarConfig() { Visible = true };
            this.RabbitMQ = new StatusBarConfig() { Visible = false };
            this.SCW = new StatusBarConfig() { Visible = false };
            this.TAApp = new StatusBarConfig() { Visible = false };
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
        /// <summary>Gets or sets TA App status bar.</summary>
        public StatusBarConfig TAApp { get; set; }

        #endregion
    }

    #endregion

    #region TODUIConfig

    /// <summary>
    /// The TODUIConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TODUIConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TODUIConfig()
        {
            this.HeaderBars = new TODHeaderBars();
            this.StatusBars = new TODStatusBars();
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets TOD header bars.</summary>
        public TODHeaderBars HeaderBars { get; set; }
        /// <summary>Gets or sets TOD status bars.</summary>
        public TODStatusBars StatusBars { get; set; }

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
            this.Services = new TODAppServiceConfig();

            this.Plazas = new List<TODPlazaConfig>();
            this.Plazas.Add(new TODPlazaConfig() { PlazaId = 15 });
            this.Plazas.Add(new TODPlazaConfig() { PlazaId = 16 });

            this.UIConfig = new TODUIConfig();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// <summary>
        /// Gets or sets TOD App Service Config.
        /// </summary>
        public TODAppServiceConfig Services { get; set; }
        /// <summary>
        /// Gets or sets Plazas.
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<TODPlazaConfig> Plazas { get; set; }
        /// <summary>
        /// Gets or sets TOD UI Config.
        /// </summary>
        public TODUIConfig UIConfig { get; set; }

        #endregion
    }

    #endregion
}
