﻿#region Using

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

    #region PlazaAppConfigManager

    /// <summary>
    /// Plaza App Config Manager class.
    /// </summary>
    public class PlazaAppConfigManager : JsonConfigFileManger<PlazaAppConfig>,
        IDMTConfig, ISCWConfig, ITAxTODConfig, ITAAppConfig, ITODAppConfig
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

    #region TAConfigManager

    /// <summary>
    /// TA Config Manager class.
    /// </summary>
    public class TAConfigManager : JsonConfigFileManger<TAAppPlazaConfig>,
        IDMTConfig, ISCWConfig, ITAxTODConfig, ITAAppConfig, ITODAppConfig
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

    #region TODConfigManager

    /// <summary>
    /// TOD Config Manager class.
    /// </summary>
    public class TODConfigManager : JsonConfigFileManger<TODAppPlazaConfig>,
        IDMTConfig, ISCWConfig, ITAxTODConfig, ITAAppConfig, ITODAppConfig
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