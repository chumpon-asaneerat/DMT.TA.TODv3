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
    /// Account Resend Config Manager class.
    /// </summary>
    public class AccountResendConfigManager : JsonConfigFileManger<AccountMessageResendConfig>
    {
        #region Static Instance Access

        private static AccountResendConfigManager _instance = null;

        /// <summary>
        /// Gets ConfigManager instance access.
        /// </summary>
        public static AccountResendConfigManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(AccountResendConfigManager))
                    {
                        _instance = new AccountResendConfigManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private string _fileName = NJson.LocalConfigFile("Account.message.resend.config.json");

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private AccountResendConfigManager() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~AccountResendConfigManager()
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
        /// Gets SCW Message Resend Config.
        /// </summary>
        public MessageResendConfig SCW
        {
            get
            {
                if (null == Value) LoadConfig();
                return (null != Value) ? Value.SCW : null;
            }
        }
        /// <summary>
        /// Gets TAxTOD Message Resend Config.
        /// </summary>
        public MessageResendConfig TAxTOD
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
