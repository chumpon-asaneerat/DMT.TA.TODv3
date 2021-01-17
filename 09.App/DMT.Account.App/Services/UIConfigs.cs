#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

#endregion

namespace DMT.Configurations
{
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
            this.AppInfo = new StatusBarConfig() { Name = "AppInfo", Visible = true };
            this.ClientInfo = new StatusBarConfig() { Name = "ClientInfo", Visible = true };
            this.LocalDb = new StatusBarConfig() { Name = "LocalDb", Visible = false };
            this.SCW = new StatusBarConfig() { Name = "SCW", Visible = false };
            this.TAServer = new StatusBarConfig() { Name = "TAServer", Visible = true };
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets AppInfo status bar.</summary>
        public StatusBarConfig AppInfo { get; set; }
        /// <summary>Gets or sets ClientInfo status bar.</summary>
        public StatusBarConfig ClientInfo { get; set; }
        /// <summary>Gets or sets LocalDb status bar.</summary>
        public StatusBarConfig LocalDb { get; set; }
        /// <summary>Gets or sets SCW status bar.</summary>
        public StatusBarConfig SCW { get; set; }
        /// <summary>Gets or sets TAServer status bar.</summary>
        public StatusBarConfig TAServer { get; set; }

        #endregion
    }

    #endregion

    #region AccountUIConfigManager

    /// <summary>
    /// Account UI Config Manager class.
    /// </summary>
    public class AccountUIConfigManager : JsonConfigFileManger<AccountUIConfig>
    {
        #region Static Instance Access

        private static AccountUIConfigManager _instance = null;

        /// <summary>
        /// Gets ConfigManager instance access.
        /// </summary>
        public static AccountUIConfigManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(AccountUIConfigManager))
                    {
                        _instance = new AccountUIConfigManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private string _fileName = NJson.LocalConfigFile("Account.app.ui.config.json");

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private AccountUIConfigManager() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~AccountUIConfigManager()
        {
            Shutdown();
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
        /// Gets AppInfo Config.
        /// </summary>
        public StatusBarConfig AppInfo
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.AppInfo : null;
            }
        }
        /// <summary>
        /// Gets ClientInfo Config.
        /// </summary>
        public StatusBarConfig ClientInfo
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.ClientInfo : null;
            }
        }
        /// <summary>
        /// Gets LocalDb Config.
        /// </summary>
        public StatusBarConfig LocalDb
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.LocalDb : null;
            }
        }
        /// <summary>
        /// Gets SCW Config.
        /// </summary>
        public StatusBarConfig SCW
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.SCW : null;
            }
        }
        /// <summary>
        /// Gets TAServer Config.
        /// </summary>
        public StatusBarConfig TAServer
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.TAServer : null;
            }
        }

        #endregion
    }

    #endregion
}
