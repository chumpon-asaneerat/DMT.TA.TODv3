﻿#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

using NLib;

using DMT.Models;
using DMT.Configurations;
using DMT.Services;
using System.Threading.Tasks;
using System.Windows.Threading;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// The Message Queue Resend Service class (TOD App).
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
            TAApp
        }

        #endregion

        #region Internal Variables

        private TODResnedConfigManager _cfgMgr = TODResnedConfigManager.Instance;
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
            timer.Start();
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
            // TAxTOD
            if (null != _cfgMgr.TAApp)
            {
                ts = DateTime.Now - _lastUpdateds[MQ.TAApp];
                if (ts.TotalSeconds > _cfgMgr.TAApp.IntervalSeconds)
                {
                    // Check Unsync TSBShift, UserShift
                    // TODO: Uncomment if need auto sync TSB to TAServer (but required to check TA Stored Proc because it has duplicate error)
                    /*
                    var tsbShifts = Models.TSBShift.GetUnSyncTSBShifts().Value();
                    if (null != tsbShifts && tsbShifts.Count > 0)
                    {
                        med.Info("Generate Unsync TSBShift message(s).");
                        tsbShifts.ForEach(tsbshift => 
                        { 
                            TAxTODMQService.Instance.WriteQueue(tsbshift);
                            // Mark as sync.
                            tsbshift.ToTAServer = 1;
                            Models.TSBShift.Save(tsbshift);
                        });
                    }
                    */
                    var usrShifts = Models.UserShift.GetUnSyncUserShifts().Value();
                    if (null != usrShifts && usrShifts.Count > 0)
                    {
                        med.Info("Generate Unsync UserShift message(s).");
                        usrShifts.ForEach(usrshift => 
                        { 
                            TAxTODMQService.Instance.WriteQueue(usrshift);
                            // Mark as sync.
                            usrshift.ToTAServer = 1;
                            Models.UserShift.Save(usrshift);
                        });
                    }

                    var revenues = Models.RevenueEntry.GetUnSyncRevenueEnties().Value();
                    if (null != revenues && revenues.Count > 0)
                    {
                        med.Info("Generate Unsync RevenueEntry message(s).");
                        revenues.ForEach(revenue =>
                        {
                            TAxTODMQService.Instance.WriteQueue(revenue);
                            // Mark as sync.
                            revenue.ToTAServer = 1;
                            Models.RevenueEntry.Save(revenue);
                        });
                    }

                    // Call resend
                    med.Info("TAApp resend message(s).");
                    TAMQService.Instance.ResendMessages();
                    _lastUpdateds[MQ.TAApp] = DateTime.Now; // Update new time
                }
            }
            else
            {
                med.Info("TAApp message resend config is null.");
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

            if (!_lastUpdateds.ContainsKey(MQ.TAApp))
                _lastUpdateds.Add(MQ.TAApp, DateTime.MinValue);
            else _lastUpdateds[MQ.TAApp] = DateTime.MinValue;
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