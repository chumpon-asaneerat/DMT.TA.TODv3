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
    using ops = Services.Operations.TAxTOD.Exchange; // reference to static class.

    #region ExchangeSyncService

    /// <summary>
    /// The Exchange Sync Service class.
    /// </summary>
    public class ExchangeSyncService
    {
        #region Singelton

        private static ExchangeSyncService _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static ExchangeSyncService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(ExchangeSyncService))
                    {
                        _instance = new ExchangeSyncService();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private DispatcherTimer _timer = null;
        private DateTime _lastSync = DateTime.MinValue;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private ExchangeSyncService() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~ExchangeSyncService()
        {
            Shutdown();
        }

        #endregion

        #region Timer Handler

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (IsSync) return; // on sync ignore.
            SyncFromServer();
        }

        #endregion

        #region Private Methods

        private NRestResult<List<TAAApproveSummary>> GetApproves(string tsbId)
        {
            var ret = ops.GetApproves(tsbId, DateTime.Today);
            return ret;
        }

        private NRestResult<List<TAAApproveItem>> GetApproveItems(string tsbId, int reqId)
        {
            var ret = ops.GetApproveItems(tsbId, reqId);
            return ret;
        }

        private void InternalSyncFromServer()
        {
            if (IsSync) return;

            MethodBase med = MethodBase.GetCurrentMethod();

            TimeSpan ts = DateTime.Now - _lastSync;

            IsSync = true;

            try
            {
                // run every hour
                if (ForceSync || ts.TotalHours > 1)
                {
                    var tsb = TSB.GetCurrent().Value();
                    if (tsb != null)
                    {
                        var approves = GetApproves(tsb.TSBId).Value();
                        if (null != approves && approves.Count > 0)
                        {
                            approves.ForEach(approve =>
                            {
                                int reqId = (approve.RequestId.HasValue) ? approve.RequestId.Value : -1;
                                if (reqId > -1)
                                {
                                    var group = TSBExchangeGroup.GetTSBExchangeGroup(tsb, reqId).Value();
                                    if (null != group && group.State == TSBExchangeGroup.StateTypes.Request)
                                    {
                                        // update only request group.

                                        // get approve details from server.
                                        var items = GetApproveItems(tsb.TSBId, reqId);
                                        if (null != items)
                                        {

                                        }
                                    }
                                }
                            });
                        }
                    }
                    _lastSync = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            ForceSync = false; // Reset flag.
            IsSync = false;
        }

        private async void InternalSyncFromServerAsync() // returns void
        {
            var uiContext = TaskScheduler.FromCurrentSynchronizationContext();
            await Task.Factory.StartNew(() =>
            {
                InternalSyncFromServer();
            }, CancellationToken.None, TaskCreationOptions.None, uiContext);
        }

        private void SyncFromServer() // not blocks, not waits
        {
            InternalSyncFromServerAsync();
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

            SyncFromServer(); // call sync when start.

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
            SyncFromServer(); // call imediately if no in sync process.
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
    }

    #endregion
}
