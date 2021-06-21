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
    public class TODResnedConfigManager : JsonConfigFileManger<TODMessageResendConfig>
    {
        #region Static Instance Access

        private static TODResnedConfigManager _instance = null;

        /// <summary>
        /// Gets ConfigManager instance access.
        /// </summary>
        public static TODResnedConfigManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TODResnedConfigManager))
                    {
                        _instance = new TODResnedConfigManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private string _fileName = NJson.LocalConfigFile("TOD.message.resend.config.json");

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private TODResnedConfigManager() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TODResnedConfigManager()
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
