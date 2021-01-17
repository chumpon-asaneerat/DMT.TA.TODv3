#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

#endregion

namespace DMT.Configurations
{
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
            this.AppInfo = new StatusBarConfig() { Name = "AppInfo", Visible = true };
            this.ClientInfo = new StatusBarConfig() { Name = "ClientInfo", Visible = true };
            this.LocalDb = new StatusBarConfig() { Name = "LocalDb", Visible = true };
            this.SCW = new StatusBarConfig() { Name = "SCW", Visible = false };
            this.TAApp = new StatusBarConfig() { Name = "TAApp", Visible = false };
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
        /// <summary>Gets or sets TA App status bar.</summary>
        public StatusBarConfig TAApp { get; set; }

        #endregion
    }

    #endregion

    #region TODUIConfigManager

    /// <summary>
    /// TOD UI Config Manager class.
    /// </summary>
    public class TODUIConfigManager : JsonConfigFileManger<TODUIConfig>
    {
        #region Static Instance Access

        private static TODUIConfigManager _instance = null;

        /// <summary>
        /// Gets ConfigManager instance access.
        /// </summary>
        public static TODUIConfigManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TODUIConfigManager))
                    {
                        _instance = new TODUIConfigManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private string _fileName = NJson.LocalConfigFile("TOD.app.ui.config.json");

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private TODUIConfigManager() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TODUIConfigManager()
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
        /// Gets TAApp Config.
        /// </summary>
        public StatusBarConfig TAApp
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
}
