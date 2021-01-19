#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

#endregion

namespace DMT.Configurations
{
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
            this.StatusBars = new TAStatusBars();
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets TOD status bars.</summary>
        public TAStatusBars StatusBars { get; set; }

        #endregion
    }

    #endregion

    #region TAUIConfigManager

    /// <summary>
    /// TA UI Config Manager class.
    /// </summary>
    public class TAUIConfigManager : JsonConfigFileManger<TAUIConfig>
    {
        #region Static Instance Access

        private static TAUIConfigManager _instance = null;

        /// <summary>
        /// Gets ConfigManager instance access.
        /// </summary>
        public static TAUIConfigManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TAUIConfigManager))
                    {
                        _instance = new TAUIConfigManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private string _fileName = NJson.LocalConfigFile("TA.app.ui.config.json");

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private TAUIConfigManager() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TAUIConfigManager()
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
                if (null == Value || null == Value.StatusBars) LoadConfig();
                return (null != Value && null != Value.StatusBars) ? Value.StatusBars.AppInfo : null;
            }
        }
        /// <summary>
        /// Gets ClientInfo Config.
        /// </summary>
        public StatusBarConfig ClientInfo
        {
            get
            {
                if (null == Value || null == Value.StatusBars) LoadConfig();
                return (null != Value && null != Value.StatusBars) ? Value.StatusBars.ClientInfo : null;
            }
        }
        /// <summary>
        /// Gets LocalDb Config.
        /// </summary>
        public StatusBarConfig LocalDb
        {
            get
            {
                if (null == Value || null == Value.StatusBars) LoadConfig();
                return (null != Value && null != Value.StatusBars) ? Value.StatusBars.LocalDb : null;
            }
        }
        /// <summary>
        /// Gets SCW Config.
        /// </summary>
        public StatusBarConfig SCW
        {
            get
            {
                if (null == Value || null == Value.StatusBars) LoadConfig();
                return (null != Value && null != Value.StatusBars) ? Value.StatusBars.SCW : null;
            }
        }
        /// <summary>
        /// Gets TAServer Config.
        /// </summary>
        public StatusBarConfig TAServer
        {
            get
            {
                if (null == Value || null == Value.StatusBars) LoadConfig();
                return (null != Value && null != Value.StatusBars) ? Value.StatusBars.TAServer : null;
            }
        }
        /// <summary>
        /// Gets TODApp Config.
        /// </summary>
        public StatusBarConfig TODApp
        {
            get
            {
                if (null == Value || null == Value.StatusBars) LoadConfig();
                return (null != Value && null != Value.StatusBars) ? Value.StatusBars.TODApp : null;
            }
        }

        #endregion
    }

    #endregion
}
