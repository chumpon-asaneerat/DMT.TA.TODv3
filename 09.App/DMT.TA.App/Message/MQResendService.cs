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
using System.Windows.Threading;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// The Message Queue Resend Service class (TA App).
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

        #region Enum (private)

        private enum MQ
        {
            SCW,
            TAxTOD,
            TODApps
        }

        #endregion

        #region Internal Variables

        private TAResnedConfigManager _cfgMgr = TAResnedConfigManager.Instance;
        private DispatcherTimer timer = null;
        private Dictionary<MQ, DateTime> _lastUpdateds = new Dictionary<MQ, DateTime>();

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private MQResendService() : base()
        {
            ResetTimes();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MQResendService()
        {
            if (null != timer)
            {
                timer.Tick -= timer_Tick;
                timer.Stop();
            }
            timer = null;

            Shutdown();
        }

        #endregion

        #region Private Method

        private void _cfgMgr_ConfigChanged(object sender, EventArgs e)
        {
            ResetTimes();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            TimeSpan ts;
            // SCW
            if (null != _cfgMgr.SCW)
            {
                ts = DateTime.Now - _lastUpdateds[MQ.SCW];
                if (ts.TotalSeconds > _cfgMgr.SCW.IntervalSeconds)
                {
                    // Call resend
                    med.Info("SCW resend message(s).");
                    SCWMQService.Instance.ResendMessages();
                    _lastUpdateds[MQ.SCW] = DateTime.Now; // Update new time
                }
            }
            else
            {
                med.Info("SCW message resend config is null.");
            }
            // TAxTOD
            if (null != _cfgMgr.TAxTOD)
            {
                ts = DateTime.Now - _lastUpdateds[MQ.TAxTOD];
                if (ts.TotalSeconds > _cfgMgr.TAxTOD.IntervalSeconds)
                {
                    // Call resend
                    med.Info("TAxTOD resend message(s).");
                    TAxTODMQService.Instance.ResendMessages();
                    _lastUpdateds[MQ.TAxTOD] = DateTime.Now; // Update new time
                }
            }
            else
            {
                med.Info("TAxTOD message resend config is null.");
            }
            // TODApps
            if (null != _cfgMgr.TODApps)
            {
                ts = DateTime.Now - _lastUpdateds[MQ.TODApps];
                if (ts.TotalSeconds > _cfgMgr.TODApps.IntervalSeconds)
                {
                    // Call resend
                    med.Info("TODApps resend message(s).");
                    TODMQService.Instance.ResendMessages();
                    _lastUpdateds[MQ.TODApps] = DateTime.Now; // Update new time
                }
            }
            else
            {
                med.Info("TODApps message resend config is null.");
            }
        }

        private void ResetTimes()
        {
            if (!_lastUpdateds.ContainsKey(MQ.SCW)) 
                _lastUpdateds.Add(MQ.SCW, DateTime.MinValue);
            else _lastUpdateds[MQ.SCW] = DateTime.MinValue;

            if (!_lastUpdateds.ContainsKey(MQ.TAxTOD))
                _lastUpdateds.Add(MQ.TAxTOD, DateTime.MinValue);
            else _lastUpdateds[MQ.TAxTOD] = DateTime.MinValue;

            if (!_lastUpdateds.ContainsKey(MQ.TODApps))
                _lastUpdateds.Add(MQ.TODApps, DateTime.MinValue);
            else _lastUpdateds[MQ.TODApps] = DateTime.MinValue;
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
            ResetTimes();
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
