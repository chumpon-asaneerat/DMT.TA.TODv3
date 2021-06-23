#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

using NLib;

using DMT.Configurations;
using DMT.Services;
using System.Threading.Tasks;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// The Message Queue Resend Service class.
    /// </summary>
    public class MQResendService
    {
        #region Singelton

        private static MQResendService _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static MQResendService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(MQResendService))
                    {
                        _instance = new MQResendService();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private TAResnedConfigManager _cfgMgr = TAResnedConfigManager.Instance;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private MQResendService() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MQResendService()
        {

        }

        #endregion

        #region Private Method

        private void _cfgMgr_ConfigChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start service.
        /// </summary>
        public void Start()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null == _cfgMgr)
            {
                med.Info("Message Resend Config manager is null.");
                return;
            }
            _cfgMgr.LoadConfig();
            _cfgMgr.ConfigChanged += _cfgMgr_ConfigChanged;
            _cfgMgr.Start();
            med.Info("Message Resend service started.");
        }
        /// <summary>
        /// Shutdown service.
        /// </summary>
        public void Shutdown()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null == _cfgMgr)
            {
                med.Info("Message Resend Config manager is null.");
                return;
            }
            _cfgMgr.Shutdown();
            _cfgMgr.ConfigChanged -= _cfgMgr_ConfigChanged;
            med.Info("Message Resend service shutdown.");
        }

        #endregion
    }
}
