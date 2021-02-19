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
using DMT.Services;
using DMT.Models;
using DMT.Models.ExtensionMethods;

using NLib;
using NLib.IO;
using NLib.Services;
using NLib.Reflection;

using RestSharp;
using System.Threading.Tasks;
using System.Threading;

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
            SyncCouponFromServer();
        }

        #endregion

        #region Private Methods

        private void RaiseProgressEvent(int current, int max)
        {
            OnProgress.Call(this, new ProgressEventArgs() { Current = current, Max = max });
        }

        private NRestResult<List<TAServerCouponTransaction>, NRestOut> GetTransactions(
            int pageNo, int rowsPerPage, int? flag)
        {
            var tsb = TSB.GetCurrent().Value();
            if (null != tsb)
            {
                var search = Search.TAxTOD.Coupon.Gets.Create(tsb.TSBId, null, null, null, flag, pageNo, rowsPerPage);
                var ret = couponOps.Gets(search);
                return ret;
            }
            else
            {
                var ret = new NRestResult<List<TAServerCouponTransaction>, NRestOut>();
                ret.Success();
                return ret;
            }
        }

        private void InternalSyncCouponFromServer()
        {
            if (IsSync) return;

            MethodBase med = MethodBase.GetCurrentMethod();
            IsSync = true;

            int cnt = LocalCouponCount();
            bool readAll = (ForceSync || (cnt == 0));
            int? flag = (readAll) ? new int?() : new int?(0);

            med.Info("Local Db Coupon count: {0}, ForceSync: {1}.", cnt, ForceSync);

            try
            {
                int iCurrent = 0;
                int iMaxPage = 100;
                int iRowsPerPage = 10;

                RaiseProgressEvent(0, 100); // Reset Progress.

                #region Find Max Records

                var maxRet = GetTransactions(1, iRowsPerPage, flag);
                if (null == maxRet || !maxRet.Ok ||
                    null == maxRet.Output || !maxRet.Output.MaxPage.HasValue)
                {
                    IsSync = false;
                    return;
                }
                iMaxPage = maxRet.Output.MaxPage.Value;
                if (iMaxPage <= 0)
                {
                    IsSync = false;
                    return;
                }

                #endregion

                int pageNo;
                while (iCurrent < iMaxPage && this.IsRunning)
                {
                    pageNo = (readAll) ? iCurrent : 1; // if read all page number must be increment.

                    RaiseProgressEvent(iCurrent, iMaxPage); // Update current Progress.

                    // read only first because when update to total page will change each time.
                    var pageRet = GetTransactions(pageNo, iRowsPerPage, flag);
                    if (null == pageRet || !pageRet.Ok ||
                        null == pageRet.Output) break;
                    var taCoupons = pageRet.data;
                    var output = pageRet.Output;
                    if (null != output && null != taCoupons)
                    {
                        var coupons = taCoupons.ToLocals();
                        if (coupons != null)
                        {
                            var saveRet = TSBCouponTransaction.SyncTransactions(coupons);
                            if (saveRet.Failed) break;
                            // OK so send update back to TA Server
                            coupons.ForEach(coupon =>
                            {
                                couponOps.Received(coupon.CouponId);
                                RaiseProgressEvent(iCurrent, iMaxPage); // Update current Progress.

                                ApplicationManager.Instance.Sleep(50); // sleep a little.
                                ApplicationManager.Instance.DoEvents();
                            });
                        }
                        
                        ApplicationManager.Instance.Sleep(50); // sleep a little.
                        ApplicationManager.Instance.DoEvents();

                        RaiseProgressEvent(iCurrent, iMaxPage); // Update current Progress.
                    }
                    ++iCurrent;
                }

                RaiseProgressEvent(0, 100); // Reset Progress.
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            ForceSync = false; // Reset flag.
            IsSync = false;
        }

        private async void InternalSyncCouponFromServerAsync() // returns void
        {
            var uiContext = TaskScheduler.FromCurrentSynchronizationContext();
            await Task.Factory.StartNew(() => 
            {
                InternalSyncCouponFromServer();
            }, CancellationToken.None, TaskCreationOptions.None, uiContext);
        }

        private void SyncCouponFromServer() // not blocks, not waits
        {
            InternalSyncCouponFromServerAsync();
        }

        private int LocalCouponCount()
        {
            return TSBCouponTransaction.GetCouponCount().Value();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start.
        /// </summary>
        public void Start()
        {
            this.IsRunning = true;
            this.IsSync = false;
            this.ForceSync = false;

            SyncCouponFromServer(); // call sync when start.

            if (null == _timer) _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(15);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }
        /// <summary>
        /// Shutdown.
        /// </summary>
        public void Shutdown()
        {
            this.ForceSync = false;
            this.IsSync = false;
            this.IsRunning = false;
            if (null != _timer)
            {
                _timer.Tick -= _timer_Tick;
                _timer.Stop();
            }
            _timer = null;
        }
        /// <summary>
        /// Force Re Sync all coupon from server.
        /// </summary>
        public void ReSyncAll()
        {
            if (ForceSync) return;
            ForceSync = true;
            if (this.IsSync) return; // on sync process ignore it.
            SyncCouponFromServer(); // call imediately if no in sync process.
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
        /// <summary>
        /// Gets or sets force sync.
        /// </summary>
        public bool ForceSync { get; private set; }

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
