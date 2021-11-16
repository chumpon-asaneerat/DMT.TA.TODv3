#define RUN_IN_THREAD

#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

using NLib;

using DMT.Configurations;
using DMT.Services;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// The Message Queue Resend Service class (Account App).
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
            TAxTOD
        }

        #endregion

        #region Internal Variables

        private AccountResendConfigManager _cfgMgr = AccountResendConfigManager.Instance;
        private Dictionary<MQ, DateTime> _lastUpdateds = new Dictionary<MQ, DateTime>();

#if RUN_IN_THREAD
        private Thread _th = null;
        private bool _running = false;
#else
        private DispatcherTimer timer = null;
        private bool _processing = false;
#endif

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private MQResendService() : base()
        {
            ResetTimes();
#if !RUN_IN_THREAD
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
#endif
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MQResendService()
        {
#if !RUN_IN_THREAD
            if (null != timer)
            {
                timer.Tick -= timer_Tick;
                timer.Stop();
            }
            timer = null;
#endif
            Shutdown();
        }

        #endregion

        #region Private Method

        private void _cfgMgr_ConfigChanged(object sender, EventArgs e)
        {
            ResetTimes();
        }

#if RUN_IN_THREAD
        private void Processing()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            while (null != _th && _running && !ApplicationManager.Instance.IsExit)
            {
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
                        lock (this)
                        {
                            _lastUpdateds[MQ.SCW] = DateTime.Now; // Update new time
                        }
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
                        lock (this)
                        {
                            _lastUpdateds[MQ.TAxTOD] = DateTime.Now; // Update new time
                        }
                    }
                }
                else
                {
                    med.Info("TAxTOD message resend config is null.");
                }

                ApplicationManager.Instance.Sleep(50);
                ApplicationManager.Instance.DoEvents();
            }
            Shutdown();
        }
#else
        void timer_Tick(object sender, EventArgs e)
        {
            if (_processing)
                return;

            _processing = true;

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

            _processing = false;
        }
#endif
        private void ResetTimes()
        {
            lock (this)
            {
                if (!_lastUpdateds.ContainsKey(MQ.SCW))
                    _lastUpdateds.Add(MQ.SCW, DateTime.MinValue);
                else _lastUpdateds[MQ.SCW] = DateTime.MinValue;

                if (!_lastUpdateds.ContainsKey(MQ.TAxTOD))
                    _lastUpdateds.Add(MQ.TAxTOD, DateTime.MinValue);
                else _lastUpdateds[MQ.TAxTOD] = DateTime.MinValue;
            }
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
#if RUN_IN_THREAD
            if (null != _th)
                return;
            _th = new Thread(Processing);
            _th.Name = "MQ Resend";
            _th.Priority = ThreadPriority.BelowNormal;
            _th.IsBackground = true;
            _running = true;
            _th.Start();
#endif
        }
        /// <summary>
        /// Shutdown service.
        /// </summary>
        public void Shutdown()
        {
#if RUN_IN_THREAD
            _running = false;
            if (null != _th)
            {
                try
                {
                    _th.Abort();
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
                catch (Exception)
                {
                    //Console.WriteLine(ex);
                }
                finally
                {

                }
            }
            _th = null;
#endif
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
