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
                return (null != Value && null != Value.Services) ? Value.Services.DMT : null;
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
                return (null != Value && null != Value.Services) ? Value.Services.RabbitMQ : null;
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
                return (null != Value && null != Value.Services) ? Value.Services.SCW : null;
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
                return (null != Value && null != Value.Services) ? Value.Services.TAxTOD : null;
            }
        }

        #endregion
    }

    #endregion
}
