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
    /// TOD Resned Config Manager class.
    /// </summary>
    public class TAResnedConfigManager : JsonConfigFileManger<TAMessageResendConfig>
    {
        #region Static Instance Access

        private static TAResnedConfigManager _instance = null;

        /// <summary>
        /// Gets ConfigManager instance access.
        /// </summary>
        public static TAResnedConfigManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TAResnedConfigManager))
                    {
                        _instance = new TAResnedConfigManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private string _fileName = NJson.LocalConfigFile("TA.message.resend.config.json");

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private TAResnedConfigManager() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TAResnedConfigManager()
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

        #endregion
    }

    #endregion
}
