#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Reflection;

using DMT.Models;
using DMT.Services;

using NLib;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.Account.Pages.Exchange
{
    using ops = DMT.Services.Operations.TAxTOD;

    /// <summary>
    /// Interaction logic for TSBRequestExchangeViewPage.xaml
    /// </summary>
    public partial class TSBRequestExchangeViewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBRequestExchangeViewPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private List<TAAExchangeSummary> requests = new List<TAAExchangeSummary>();
        private List<TAAExchangeSummary> approves = new List<TAAExchangeSummary>();


        #endregion

        #region TabControl Handler

        private void tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabs.SelectedIndex == 0)
            {
                cmdApprove.Visibility = Visibility.Visible;
                cmdNotApprove.Visibility = Visibility.Visible;
            }
            else
            {
                cmdApprove.Visibility = Visibility.Collapsed;
                cmdNotApprove.Visibility = Visibility.Collapsed;
            }

        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            // approve all selected
            if (null == requests || requests.Count <= 0) return;
            int iCnt = 0;
            requests.ForEach(item => 
            {
                if (item.Selected) iCnt++;
            });

            if (iCnt <= 0)
            {
                var msgbox = AccountApp.Windows.MessageBox;
                msgbox.Setup("กรุณาเลือกรายการ", "DMT - TA (Account)");
                msgbox.ShowDialog();
                return;
            }

            var win = AccountApp.Windows.ConfirmApproveMessageBox;
            win.Setup("ยืนยันการอนุมัติคำร้องของด่านที่เลือก");
            if (win.ShowDialog() == false)
                return;

            ApproveAll();
        }

        private void cmdNotApprove_Click(object sender, RoutedEventArgs e)
        {
            // reject all selected
            if (null == requests || requests.Count <= 0) return;
            int iCnt = 0;
            requests.ForEach(item =>
            {
                if (item.Selected) iCnt++;
            });

            if (iCnt <= 0)
            {
                var msgbox = AccountApp.Windows.MessageBox;
                msgbox.Setup("กรุณาเลือกรายการ", "DMT - TA (Account)");
                msgbox.ShowDialog();
                return;
            }

            var win = AccountApp.Windows.ConfirmRejectMessageBox;
            win.Setup("ยืนยันการไม่อนุมัติคำร้องของด่านที่เลือก");
            if (win.ShowDialog() == false)
                return;

            string reason = string.Empty;
            RejectAll(reason);
        }

        private void cmdRequestDetail_Click(object sender, RoutedEventArgs e)
        {
            // Show Detail windows
            if (null == sender || !(sender is Button)) return;
            var item = (sender as Button).DataContext as Models.TAAExchangeSummary;
            if (null != item)
            {
                int reqId = (item.RequestId.HasValue) ? item.RequestId.Value : -1;
                string tsbId = item.TSBId;
                TSB tsb = TSB.GetTSB(tsbId).Value();

                // create request document
                var doc = new TSBExchangeTransaction();
                doc.TransactionDate = (item.RequestDate.HasValue) ? item.RequestDate.Value : DateTime.MinValue;
                doc.TransactionType = TSBExchangeTransaction.TransactionTypes.Request;
                doc.Remark = item.RequestRemark;
                doc.AdditionalBHT = (item.AdditionalBHT.HasValue) ? item.AdditionalBHT.Value : decimal.Zero;
                doc.BorrowBHT = (item.BorrowBHT.HasValue) ? item.BorrowBHT.Value : decimal.Zero;
                doc.ExchangeBHT = (item.ExchangeBHT.HasValue) ? item.ExchangeBHT.Value : decimal.Zero;
                doc.PeriodBegin = item.PeriodBegin;
                doc.PeriodEnd = item.PeriodEnd;
                doc.TSBId = tsb.TSBId;
                doc.TSBNameEN = tsb.TSBNameEN;
                doc.TSBNameTH = tsb.TSBNameTH;

                // get request details
                var details = ops.Exchange.GetRequestItems(tsbId, reqId).Value();
                if (null != details && details.Count > 0)
                {
                    details.ForEach(detail =>
                    {
                        if (detail.CurrencyDenomId == 1)
                        {
                            doc.AmountST25 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 2)
                        {
                            doc.AmountST50 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 3)
                        {
                            doc.AmountBHT1 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 4)
                        {
                            doc.AmountBHT2 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 5)
                        {
                            doc.AmountBHT5 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 6)
                        {
                            doc.AmountBHT10 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 7)
                        {
                            // Skip 10 BHT Notes.
                        }
                        else if (detail.CurrencyDenomId == 8)
                        {
                            doc.AmountBHT20 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 9)
                        {
                            doc.AmountBHT50 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 10)
                        {
                            doc.AmountBHT100 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 11)
                        {
                            doc.AmountBHT500 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 12)
                        {
                            doc.AmountBHT1000 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                    });
                }

                var win = AccountApp.Windows.RequestExchangeWindow;
                win.Setup(doc);
                win.ShowDialog();

                LoadAll(); // refresh.
            }
        }

        private void cmdApproveDetail_Click(object sender, RoutedEventArgs e)
        {
            // Show Detail windows
            if (null == sender || !(sender is Button)) return;
            var item = (sender as Button).DataContext as Models.TAAExchangeSummary;
            if (null != item)
            {
                int reqId = (item.RequestId.HasValue) ? item.RequestId.Value : -1;
                string tsbId = item.TSBId;
                TSB tsb = TSB.GetTSB(tsbId).Value();

                // create approve document
                var doc = new TSBExchangeTransaction();
                doc.TransactionDate = (item.RequestDate.HasValue) ? item.RequestDate.Value : DateTime.MinValue;
                doc.TransactionType = TSBExchangeTransaction.TransactionTypes.Approve;
                doc.Remark = item.RequestRemark;
                doc.AdditionalBHT = (item.AdditionalBHT.HasValue) ? item.AdditionalBHT.Value : decimal.Zero;
                doc.BorrowBHT = (item.BorrowBHT.HasValue) ? item.BorrowBHT.Value : decimal.Zero;
                doc.ExchangeBHT = (item.ExchangeBHT.HasValue) ? item.ExchangeBHT.Value : decimal.Zero;
                doc.PeriodBegin = item.PeriodBegin;
                doc.PeriodEnd = item.PeriodEnd;
                doc.TSBId = tsb.TSBId;
                doc.TSBNameEN = tsb.TSBNameEN;
                doc.TSBNameTH = tsb.TSBNameTH;

                // get request details
                var details = ops.Exchange.GetRequestItems(tsbId, reqId).Value();
                if (null != details && details.Count > 0)
                {
                    details.ForEach(detail =>
                    {
                        if (detail.CurrencyDenomId == 1)
                        {
                            doc.AmountST25 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 2)
                        {
                            doc.AmountST50 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 3)
                        {
                            doc.AmountBHT1 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 4)
                        {
                            doc.AmountBHT2 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 5)
                        {
                            doc.AmountBHT5 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 6)
                        {
                            doc.AmountBHT10 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 7)
                        {
                            // Skip 10 BHT Notes.
                        }
                        else if (detail.CurrencyDenomId == 8)
                        {
                            doc.AmountBHT20 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 9)
                        {
                            doc.AmountBHT50 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 10)
                        {
                            doc.AmountBHT100 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 11)
                        {
                            doc.AmountBHT500 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                        else if (detail.CurrencyDenomId == 12)
                        {
                            doc.AmountBHT1000 = (detail.CurrencyValue.HasValue) ? detail.CurrencyValue.Value : decimal.Zero;
                        }
                    });
                }

                var win = AccountApp.Windows.RequestExchangeWindow;
                win.Setup(doc);
                win.ShowDialog();

                LoadAll(); // refresh.
            }
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = AccountApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void LoadRequestList()
        {
            gridRequest.ItemsSource = null;
            // get by (R)equest status.
            requests = ops.Exchange.Gets("R").Value();
            gridRequest.ItemsSource = requests;
        }

        private void LoadApproveList()
        {
            gridApprove.ItemsSource = null;
            // get by (A)pprove status.
            approves = ops.Exchange.Gets("A").Value();
            gridApprove.ItemsSource = approves;
        }

        private void ApproveAll(string reason = "")
        {
            if (null == requests || requests.Count <= 0) return;
            requests.ForEach(item =>
            {
                if (item.Selected)
                {
                    var header = new Models.TAAExchangeHeader();
                    header.AdditionalBHT = item.AdditionalBHT;
                    header.BorrowBHT = item.BorrowBHT;
                    header.ExchangeBHT = item.ExchangeBHT;
                    header.FinishFlag = 1;
                    header.PeriodBegin = item.PeriodBegin;
                    header.PeriodEnd = item.PeriodEnd;
                    header.Remark = reason;
                    header.RequestId = item.RequestId;
                    header.TSBId = item.TSBId;
                    header.UserId = AccountApp.User.Current.UserId;
                    header.Status = "A";

                    List<TAAExchangeItem> reqitems = ops.Exchange.GetRequestItems(item.TSBId, item.RequestId.Value).Value();

                    // Write Queue
                    TAxTODMQService.Instance.WriteQueue(header);
                    TAxTODMQService.Instance.WriteQueue(reqitems);
                }
            });
        }

        private void RejectAll(string reason)
        {
            if (null == requests || requests.Count <= 0) return;
            requests.ForEach(item =>
            {
                if (item.Selected)
                {
                    var header = new Models.TAAExchangeHeader();
                    header.AdditionalBHT = item.AdditionalBHT;
                    header.BorrowBHT = item.BorrowBHT;
                    header.ExchangeBHT = item.ExchangeBHT;
                    header.FinishFlag = 1;
                    header.PeriodBegin = item.PeriodBegin;
                    header.PeriodEnd = item.PeriodEnd;
                    header.Remark = reason;
                    header.RequestId = item.RequestId;
                    header.TSBId = item.TSBId;
                    header.UserId = AccountApp.User.Current.UserId;
                    header.Status = "C";

                    // Write Queue
                    TAxTODMQService.Instance.WriteQueue(header);
                }
            });
        }

        private void LoadAll()
        {
            LoadRequestList();
            LoadApproveList();
        }

        #endregion

        #region Public Method
        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="chief">The Current User.</param>
        public void Setup(User chief)
        {
            LoadAll();
        }

        #endregion
    }
}
