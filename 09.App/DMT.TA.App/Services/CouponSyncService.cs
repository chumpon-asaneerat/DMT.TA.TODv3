#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
//using System.Windows.Forms;
//using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Threading;

using DMT.Configurations;
using DMT.Controls;
using DMT.Services;
using DMT.Models;
using DMT.Models.ExtensionMethods;

using NLib;
using NLib.IO;
using NLib.Services;
using NLib.Reports.Rdlc;
using NLib.Reflection;

using RestSharp;

#endregion

namespace DMT.Services
{
    using couponOps = Services.Operations.TAxTOD.Coupon; // reference to static class.

    #region EventHandler and EventArgs

    /// <summary>
    /// The Progress EventArgs class.
    /// </summary>
    public class ProgressEventArgs
    {
        /// <summary>
        /// Gets or sets current value.
        /// </summary>
        public int Current { get; set; }
        /// <summary>
        /// Gets or sets max value.
        /// </summary>
        public int Max { get; set; }
    }

    /// <summary>
    /// The Progress EventHandler.
    /// </summary>
    /// <param name="sender">The Sender instance.</param>
    /// <param name="e">The EventArgs instance.</param>
    public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

    #endregion

    #region CouponSyncService

    /// <summary>
    /// The Coupon Sync Service class.
    /// </summary>
    public class CouponSyncService
    {
        #region Singelton

        private static CouponSyncService _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static CouponSyncService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(CouponSyncService))
                    {
                        _instance = new CouponSyncService();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private DispatcherTimer _timer = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private CouponSyncService() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~CouponSyncService()
        {
            Shutdown();
        }

        #endregion

        #region Timer Handler

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (IsSync) return; // on sync ignore.
            Sync();
        }

        #endregion

        #region Private Methods

        private void RaiseProgressEvent(int current, int max)
        {
            OnProgress.Call(this, new ProgressEventArgs() { Current = current, Max = max });
        }

        private void Sync()
        {
            if (IsSync) return;

            MethodBase med = MethodBase.GetCurrentMethod();
            IsSync = true;
            try
            {
                var search = Search.TAxTOD.Coupon.Gets.Create(TAAPI.TSB.TSBId, null, null, null, 1, 20);
                var ret = couponOps.Gets(search);
                if (ret.Ok)
                {
                    var coupons = ret.data;
                    var output = ret.Output;
                    if (null != output && null != coupons)
                    {
                        if (output.TotalRecords.HasValue && output.TotalRecords.Value > 0)
                        {
                            int iMax = output.TotalRecords.Value;
                            for (int i = 0; i < iMax; i++)
                            {
                                var coupon = coupons[i];

                                // Update to database


                                var status = couponOps.Received(coupon.SerialNo);
                                if (status.Ok)
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            IsSync = false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start.
        /// </summary>
        public void Start()
        {
            this.IsRunning = true;
            if (null == _timer) _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        /// <summary>
        /// Shutdown.
        /// </summary>
        public void Shutdown()
        {
            this.IsRunning = false;
            if (null != _timer)
            {
                _timer.Tick -= _timer_Tick;
                _timer.Stop();
            }
            _timer = null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets is on sync.
        /// </summary>
        public bool IsSync { get; private set; }
        /// <summary>
        /// Gets is service running.
        /// </summary>
        public bool IsRunning { get; private set; }

        #endregion

        #region Public Events

        /// <summary>
        /// OnProgress event.
        /// </summary>
        public event ProgressEventHandler OnProgress;

        #endregion
    }

    #endregion
}
