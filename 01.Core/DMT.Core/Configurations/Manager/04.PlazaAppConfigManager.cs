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
}
