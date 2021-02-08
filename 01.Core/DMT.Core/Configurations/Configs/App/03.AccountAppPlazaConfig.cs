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

namespace DMT.Configurations
{
    #region AccountAppServiceConfig

    /// <summary>
    /// The AccountAppServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class AccountAppServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountAppServiceConfig() : base()
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
            if (null == obj || !(obj is AccountAppServiceConfig)) return false;
            return this.GetString() == (obj as AccountAppServiceConfig).GetString();
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

    #region AccountHeaderBars

    /// <summary>
    /// The TODHeaderBars class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class AccountHeaderBars
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountHeaderBars()
        {

        }

        #endregion

        #region Public Properties

        #endregion
    }

    #endregion

    #region AccountStatusBars

    /// <summary>
    /// The AccountStatusBars class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class AccountStatusBars
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountStatusBars()
        {
            this.AppInfo = new StatusBarConfig() { Visible = true };
            this.ClientInfo = new StatusBarConfig() { Visible = true };
            this.LocalDb = new StatusBarConfig() { Visible = true };
            this.RabbitMQ = new StatusBarConfig() { Visible = false };
            this.SCW = new StatusBarConfig() { Visible = false };
            this.TAServer = new StatusBarConfig() { Visible = false };
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

        #endregion
    }

    #endregion

    #region AccountUIConfig

    /// <summary>
    /// The AccountUIConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class AccountUIConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountUIConfig()
        {
            this.HeaderBars = new AccountHeaderBars();
            this.StatusBars = new AccountStatusBars();
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets Account header bars.</summary>
        public AccountHeaderBars HeaderBars { get; set; }
        /// <summary>Gets or sets Account status bars.</summary>
        public AccountStatusBars StatusBars { get; set; }

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
            this.Services = new AccountAppServiceConfig();
            this.UIConfig = new AccountUIConfig();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Account App Service Config.
        /// </summary>
        public AccountAppServiceConfig Services { get; set; }
        /// <summary>
        /// Gets or sets Account UI Config.
        /// </summary>
        public AccountUIConfig UIConfig { get; set; }

        #endregion
    }

    #endregion
}
