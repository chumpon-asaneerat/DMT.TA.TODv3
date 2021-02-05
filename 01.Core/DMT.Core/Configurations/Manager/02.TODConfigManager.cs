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
    #region TODConfigManager

    /// <summary>
    /// TOD Config Manager class.
    /// </summary>
    public class TODConfigManager : JsonConfigFileManger<TODAppPlazaConfig>,
        IDMTConfig, ISCWConfig, ITAAppConfig, ITODAppConfig
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
}
