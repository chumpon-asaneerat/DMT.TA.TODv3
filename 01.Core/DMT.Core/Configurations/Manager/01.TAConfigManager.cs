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
    // TODO: Need TOD List for call back web service.

    #region TAConfigManager

    /// <summary>
    /// TA Config Manager class.
    /// </summary>
    public class TAConfigManager : JsonConfigFileManger<TAAppPlazaConfig>,
        IDMTConfig, ISCWConfig, ITAxTODConfig, ITAAppConfig
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

        #endregion
    }

    #endregion
}
