#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.Windows
{
    /// <summary>
    /// Interaction logic for TSBApproveExchangeWindow.xaml
    /// </summary>
    public partial class TSBApproveExchangeWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBApproveExchangeWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private int _reqId = 0;

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            // Close Window
            DialogResult = false;
        }

        private void cmdReject_Click(object sender, RoutedEventArgs e)
        {
            var _approve = entry.Current;
            RejectApprove(_approve);
        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            // Begin edit document.
            entry.BeginEdit();
            // Change buttons Visibility
            cmdReject.Visibility = Visibility.Collapsed;
            cmdEdit.Visibility = Visibility.Collapsed;
            cmdCancelEdit.Visibility = Visibility.Visible;
            cmdSaveEdit.Visibility = Visibility.Visible;
        }

        private void cmdCancelEdit_Click(object sender, RoutedEventArgs e)
        {
            // Cancel edit document.
            entry.CancelEdit();
            // Restore buttons Visibility
            cmdReject.Visibility = Visibility.Visible;
            cmdEdit.Visibility = Visibility.Visible;
            cmdCancelEdit.Visibility = Visibility.Collapsed;
            cmdSaveEdit.Visibility = Visibility.Collapsed;
        }

        private void cmdSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            // Save edit document to TA Server.
            entry.EndEdit();
            var _approve = entry.Current;
            SaveApprove(_approve);

            // Restore buttons Visibility
            cmdReject.Visibility = Visibility.Visible;
            cmdEdit.Visibility = Visibility.Visible;
            cmdCancelEdit.Visibility = Visibility.Collapsed;
            cmdSaveEdit.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Private Methods

        private void RejectApprove(Models.TSBExchangeTransaction approve)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            if (null == approve)
            {
                med.Err("The approve instance is null.");
                return;
            }

            // Confirm Reject
            var win = AccountApp.Windows.ConfirmRejectMessageBox;
            win.Setup(false);
            if (win.ShowDialog() == false) return;
            // get reason.
            string reson = win.Reason;

            var header = new Models.TAAExchangeHeader();
            header.AdditionalBHT = approve.AdditionalBHT;
            header.BorrowBHT = approve.BorrowBHT;
            header.ExchangeBHT = approve.ExchangeBHT;
            header.FinishFlag = 1;
            header.PeriodBegin = approve.PeriodBegin;
            header.PeriodEnd = approve.PeriodEnd;
            header.Remark = (!string.IsNullOrWhiteSpace(reson)) ? reson : approve.Remark;
            header.RequestId = _reqId;
            header.TSBId = approve.TSBId;
            header.UserId = AccountApp.User.Current.UserId;
            header.Status = "C";
            header.TranactionDate = DateTime.Now;

            // Write Queue
            TAxTODMQService.Instance.WriteQueue(header);

            DialogResult = true;
        }

        private void SaveApprove(Models.TSBExchangeTransaction approve)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            if (null == approve)
            {
                med.Err("The approve instance is null.");
                return;
            }

            if (approve.BHTTotal != approve.GrandTotalBHT)
            {
                // Check total sum.
                var win2 = AccountApp.Windows.MessageBox;
                win2.Owner = this; // change owner
                win2.Setup("ยอดรวมของเหรียญ/ธนบัตร และจำนวนเงินรวมไม่เท่ากัน กรุณาตรวจสอบข้อมูล.", "DMT - TA (Account)");
                win2.ShowDialog();
                return;
            }

            // Confirm Approve
            var win = AccountApp.Windows.ConfirmApproveMessageBox;
            win.Setup();
            if (win.ShowDialog() == false) return;

            var header = new Models.TAAExchangeHeader();
            header.AdditionalBHT = approve.AdditionalBHT;
            header.BorrowBHT = approve.BorrowBHT;
            header.ExchangeBHT = approve.ExchangeBHT;
            header.FinishFlag = 0;
            header.PeriodBegin = approve.PeriodBegin;
            header.PeriodEnd = approve.PeriodEnd;
            header.Remark = approve.Remark; // use remark from approve item.
            header.RequestId = _reqId;
            header.TSBId = approve.TSBId;
            header.UserId = AccountApp.User.Current.UserId;
            header.Status = "A";
            header.TranactionDate = DateTime.Now;

            List<TAAApproveExchangeItem> approveItems = new List<TAAApproveExchangeItem>();
            List<MCurrency> currencies = MCurrency.GetCurrencies().Value();
            currencies.ForEach(currency =>
            {
                if (currency.currencyDenomId == 7)
                    return; // ignore 10BHT note

                var item = new TAAApproveExchangeItem();
                item.TSBId = approve.TSBId;
                item.RequestId = _reqId;
                item.CurrencyDenomId = currency.currencyDenomId;

                if (currency.denomValue == (decimal)0.25)
                {
                    item.CurrencyValue = approve.AmountST25;
                    item.CurrencyCount = approve.CountST25;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == (decimal)0.5)
                {
                    item.CurrencyValue = approve.AmountST50;
                    item.CurrencyCount = approve.CountST50;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 1)
                {
                    item.CurrencyValue = approve.AmountBHT1;
                    item.CurrencyCount = approve.CountBHT1;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 2)
                {
                    item.CurrencyValue = approve.AmountBHT2;
                    item.CurrencyCount = approve.CountBHT2;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 5)
                {
                    item.CurrencyValue = approve.AmountBHT5;
                    item.CurrencyCount = approve.CountBHT5;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 10)
                {
                    item.CurrencyValue = approve.AmountBHT10;
                    item.CurrencyCount = approve.CountBHT10;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 20)
                {
                    item.CurrencyValue = approve.AmountBHT20;
                    item.CurrencyCount = approve.CountBHT20;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 50)
                {
                    item.CurrencyValue = approve.AmountBHT50;
                    item.CurrencyCount = approve.CountBHT50;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 100)
                {
                    item.CurrencyValue = approve.AmountBHT100;
                    item.CurrencyCount = approve.CountBHT100;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 500)
                {
                    item.CurrencyValue = approve.AmountBHT500;
                    item.CurrencyCount = approve.CountBHT500;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 1000)
                {
                    item.CurrencyValue = approve.AmountBHT1000;
                    item.CurrencyCount = approve.CountBHT1000;
                    approveItems.Add(item);
                }
            });

            // Write Queue
            TAxTODMQService.Instance.WriteQueue(header);
            TAxTODMQService.Instance.WriteQueue(approveItems);

            DialogResult = true;
        }

        #endregion

        #region Public Method

        public void Setup(int reqId, Models.TSBExchangeTransaction approve, bool editable)
        {
            if (!editable)
            {
                // view only
                cmdReject.Visibility = Visibility.Collapsed;
                cmdEdit.Visibility = Visibility.Collapsed;
                cmdCancelEdit.Visibility = Visibility.Collapsed;
                cmdSaveEdit.Visibility = Visibility.Collapsed;
            }
            else
            {
                // allow edit
                cmdReject.Visibility = Visibility.Visible;
                cmdEdit.Visibility = Visibility.Visible;
                cmdCancelEdit.Visibility = Visibility.Collapsed;
                cmdSaveEdit.Visibility = Visibility.Collapsed;
            }

            // keep current request id.
            _reqId = reqId;
            if (null != approve)
            {
                this.Title = "รายละเอียดคำร้องการขอ/แลก เงินยืมทอนที่อนุมัติแล้ว - " + approve.TSBNameTH;
                entry.Setup(approve);
            }
            else
            {
                this.Title = "รายละเอียดคำร้องการขอ/แลก เงินยืมทอนที่อนุมัติแล้ว ";
            }
        }

        #endregion
    }
}
