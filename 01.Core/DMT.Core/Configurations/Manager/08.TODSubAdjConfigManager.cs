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
    #region TODSupAdjConfigManager

    /// <summary>
    /// TOD SupAdj Config Manager class.
    /// </summary>
    public class TODSupAdjConfigManager : JsonConfigFileManger<SupAdjWebSocketConfig>
    {
        #region Static Instance Access

        private static TODSupAdjConfigManager _instance = null;

        /// <summary>
        /// Gets ConfigManager instance access.
        /// </summary>
        public static TODSupAdjConfigManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TODSupAdjConfigManager))
                    {
                        _instance = new TODSupAdjConfigManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private string _fileName = NJson.LocalConfigFile("supadj.config.json");

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private TODSupAdjConfigManager() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TODSupAdjConfigManager()
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
        /// Gets SupAdj Config.
        /// </summary>
        public SupAdjWebSocketConfig SupAdj
        {
            get
            {
                if (null == Value) LoadConfig();
                return Value;
            }
        }

        #endregion
    }

    #endregion
}
