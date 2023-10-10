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

        private NRestResult<List<TAAApproveSummary>> GetApproves(string tsbId, int requestId)
        {
            var ret = ops.GetApproves(tsbId, requestId);
            return ret;
        }

        private NRestResult<List<TAAApproveItem>> GetApproveItems(string tsbId, int reqId)
        {
            var ret = ops.GetApproveItems(tsbId, reqId);
            return ret;
        }

        private TSBExchangeTransaction CreateApproveTransaction(TAAApproveSummary header, List<TAAApproveItem> values)
        {
            if (null == values)
                return null;

            string tsbId = header.TSBId;
            int reqId = header.RequestId.Value;
            TSBExchangeTransaction ret = TSBExchangeTransaction.GetRequestApproveTransaction(header.TSBId, reqId).Value();
            if (null == ret)
            {
                ret = new TSBExchangeTransaction(); // not exists create new one.
            }

            ret.TransactionType = TSBExchangeTransaction.TransactionTypes.Approve;
            ret.FinishFlag = TSBExchangeTransaction.FinishedFlags.Avaliable;
            ret.TransactionDate = (header.ApproveDate.HasValue) ? header.ApproveDate.Value : DateTime.Now;
            ret.TSBId = header.TSBId;
            ret.UserId = header.ApproveBy;
            ret.PeriodBegin = header.PeriodBegin;
            ret.PeriodEnd = header.PeriodEnd;
            ret.BorrowBHT = (header.AppBorrowBHT.HasValue) ? header.AppBorrowBHT.Value : decimal.Zero;
            ret.ExchangeBHT = (header.AppExchangeBHT.HasValue) ? header.AppExchangeBHT.Value : decimal.Zero;
            ret.AdditionalBHT = (header.AppAdditionalBHT.HasValue) ? header.AppAdditionalBHT.Value : decimal.Zero;
            ret.Remark = header.ApproveRemark;

            values.ForEach(value => 
            {
                int demonId = (value.CurrencyDenomId.HasValue) ? value.CurrencyDenomId.Value : -1;
                decimal amt = (value.ApproveValue.HasValue) ? value.ApproveValue.Value : decimal.Zero;

                switch (demonId)
                {
                    case 1:
                        ret.AmountST25 = amt;
                        break;
                    case 2:
                        ret.AmountST50 = amt;
                        break;
                    case 3:
                        ret.AmountBHT1 = amt;
                        break;
                    case 4:
                        ret.AmountBHT2 = amt;
                        break;
                    case 5:
                        ret.AmountBHT5 = amt;
                        break;
                    case 6:
                        ret.AmountBHT10 = amt;
                        break;
                    case 7:
                        // 10 BHT Bill
                        //ret.AmountBHT10 = amt;
                        break;
                    case 8:
                        ret.AmountBHT20 = amt;
                        break;
                    case 9:
                        ret.AmountBHT50 = amt;
                        break;
                    case 10:
                        ret.AmountBHT100 = amt;
                        break;
                    case 11:
                        ret.AmountBHT500 = amt;
                        break;
                    case 12:
                        ret.AmountBHT1000 = amt;
                        break;
                }
            });
            return ret;
        }

        private void InternalSyncFromServer()
        {
            if (IsSync) return;

            MethodBase med = MethodBase.GetCurrentMethod();

            TimeSpan ts = DateTime.Now - _lastSync;
            IsSync = true;
            int hours = 0;
            int mins = 0;
            int secs = 20;
            int totalSeconds = (hours * 3600) + (mins * 60) + secs;

            try
            {
                // run every specificed period
                if (ForceSync || ts.TotalSeconds >= totalSeconds)
                {
                    var tsb = TSB.GetCurrent().Value();
                    if (tsb != null)
                    {
                        // Gets both Request, Approve group to sync from server.
                        // in some case account may edit approved data so need to resync again.
                        var localGroups = TSBExchangeGroup.GetRequestExchangeGroups(tsb, false).Value();
                        if (null != localGroups && localGroups.Count > 0)
                        {
                            localGroups.ForEach(localGroup =>
                            {
                                try
                                {
                                    int reqId = localGroup.PkId;
                                    // cast to List
                                    var approves = GetApproves(tsb.TSBId, reqId).Value();
                                    // get first item
                                    var approve = (null != approves) ? approves.FirstOrDefault() : null;

                                    if (null != approve)
                                    {
                                        // need to update local group status
                                        if (approve.Status == "A")
                                        {
                                            // Approve
                                            localGroup.RequestType = TSBExchangeGroup.RequestTypes.Account;
                                            localGroup.State = TSBExchangeGroup.StateTypes.Approve;
                                            localGroup.FinishFlag = TSBExchangeGroup.FinishedFlags.Avaliable;

                                            // load approve details from server
                                            var items = GetApproveItems(tsb.TSBId, reqId).Value();
                                            var tran = CreateApproveTransaction(approve, items);

                                            // save changes
                                            TSBExchangeGroup.SaveTSBExchangeGroup(localGroup);
                                            // set FKs
                                            if (null != tran)
                                            {
                                                tran.GroupId = localGroup.GroupId;
                                                TSBExchangeTransaction.SaveTransaction(tran);
                                            }
                                        }
                                        else if (approve.Status == "C")
                                        {
                                            // Reject by Account dept.
                                            localGroup.RequestType = TSBExchangeGroup.RequestTypes.Account;
                                            localGroup.State = TSBExchangeGroup.StateTypes.Reject;
                                            localGroup.FinishFlag = TSBExchangeGroup.FinishedFlags.Completed;

                                            // save changes
                                            TSBExchangeGroup.SaveTSBExchangeGroup(localGroup);
                                        }
                                        else
                                        {
                                            // other case.
                                        }
                                    }
                                }
                                catch (Exception ex1)
                                {
                                    med.Err("Error get approve from TA Server: TSB: {0}, RequestId: {1}", 
                                        tsb.TSBId, localGroup.PkId);
                                    med.Err(ex1);
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
