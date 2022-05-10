#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.Windows
{
    /// <summary>
    /// Interaction logic for TSBRequestExchangeWindow.xaml
    /// </summary>
    public partial class TSBRequestExchangeWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBRequestExchangeWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private int _requestId = 0;
        private Models.TSBExchangeTransaction _request = null;
        private Models.TSBExchangeTransaction _approve = null;
        private bool _IsNewApprove = false;

        #endregion

        #region Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Button Handlers

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            if (_approve.BHTTotal != _approve.GrandTotalBHT)
            {
                // Check total sum.
                var win2 = AccountApp.Windows.MessageBox;
                win2.Setup("ยอดรวมของเหรียญ/ธนบัตร และจำนวนเงินรวมไม่เท่ากัน กรุณาตรวจสอบข้อมูล.", "DMT - TA (Account)");
                win2.ShowDialog();
                return;
            }

            // Approve and close Window
            var win = AccountApp.Windows.ConfirmApproveMessageBox;
            win.Setup();
            if (win.ShowDialog() == false) return;

            var header = new Models.TAAExchangeHeader();
            header.AdditionalBHT = _approve.AdditionalBHT;
            header.BorrowBHT = _approve.BorrowBHT;
            header.ExchangeBHT = _approve.ExchangeBHT;
            header.FinishFlag = 0;
            header.PeriodBegin = _approve.PeriodBegin;
            header.PeriodEnd = _approve.PeriodEnd;
            header.Remark = _approve.Remark;
            header.RequestId = _requestId;
            header.TSBId = _approve.TSBId;
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
                item.TSBId = _approve.TSBId;
                item.RequestId = _requestId;
                item.CurrencyDenomId = currency.currencyDenomId;

                if (currency.denomValue == (decimal)0.25)
                {
                    item.CurrencyValue = _approve.AmountST25;
                    item.CurrencyCount = _approve.CountST25;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == (decimal)0.5)
                {
                    item.CurrencyValue = _approve.AmountST50;
                    item.CurrencyCount = _approve.CountST50;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 1)
                {
                    item.CurrencyValue = _approve.AmountBHT1;
                    item.CurrencyCount = _approve.CountBHT1;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 2)
                {
                    item.CurrencyValue = _approve.AmountBHT2;
                    item.CurrencyCount = _approve.CountBHT2;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 5)
                {
                    item.CurrencyValue = _approve.AmountBHT5;
                    item.CurrencyCount = _approve.CountBHT5;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 10)
                {
                    item.CurrencyValue = _approve.AmountBHT10;
                    item.CurrencyCount = _approve.CountBHT10;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 20)
                {
                    item.CurrencyValue = _approve.AmountBHT20;
                    item.CurrencyCount = _approve.CountBHT20;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 50)
                {
                    item.CurrencyValue = _approve.AmountBHT50;
                    item.CurrencyCount = _approve.CountBHT50;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 100)
                {
                    item.CurrencyValue = _approve.AmountBHT100;
                    item.CurrencyCount = _approve.CountBHT100;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 500)
                {
                    item.CurrencyValue = _approve.AmountBHT500;
                    item.CurrencyCount = _approve.CountBHT500;
                    approveItems.Add(item);
                }
                else if (currency.denomValue == 1000)
                {
                    item.CurrencyValue = _approve.AmountBHT1000;
                    item.CurrencyCount = _approve.CountBHT1000;
                    approveItems.Add(item);
                }
            });

            // Write Queue
            TAxTODMQService.Instance.WriteQueue(header);
            TAxTODMQService.Instance.WriteQueue(approveItems);

            DialogResult = true;
        }

        private void cmdReject_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_approve.Remark))
            {
                var win2 = AccountApp.Windows.MessageBox;
                win2.Setup("กรุณาระบุเหตุผลการไม่อนุมัติ.", "DMT - TA (Account)");
                win2.ShowDialog();
                return;
            }

            // Reject and close Window
            var win = AccountApp.Windows.ConfirmRejectMessageBox;
            win.Setup(true);
            if (win.ShowDialog() == false) return;

            var header = new Models.TAAExchangeHeader();
            header.AdditionalBHT = _request.AdditionalBHT;
            header.BorrowBHT = _request.BorrowBHT;
            header.ExchangeBHT = _request.ExchangeBHT;
            header.FinishFlag = 1;
            header.PeriodBegin = _request.PeriodBegin;
            header.PeriodEnd = _request.PeriodEnd;
            header.Remark = _approve.Remark; // use remark from approve item.
            header.RequestId = _requestId;
            header.TSBId = _request.TSBId;
            header.UserId = AccountApp.User.Current.UserId;
            header.Status = "C";
            header.TranactionDate = DateTime.Now;

            // Write Queue
            TAxTODMQService.Instance.WriteQueue(header);

            DialogResult = true;
        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            // Close Window
            DialogResult = false;
        }

        #endregion

        #region Private Methods

        private void CreateNewApprove()
        {
            if (null != _request)
            {
                _approve = new TSBExchangeTransaction();

                _approve.TransactionDate = DateTime.Now;
                _approve.TransactionType = TSBExchangeTransaction.TransactionTypes.Approve;

                _approve.AmountST25 = _request.AmountST25;
                _approve.AmountST50 = _request.AmountST50;
                _approve.AmountBHT1 = _request.AmountBHT1;
                _approve.AmountBHT2 = _request.AmountBHT2;
                _approve.AmountBHT5 = _request.AmountBHT5;
                _approve.AmountBHT10 = _request.AmountBHT10;
                _approve.AmountBHT20 = _request.AmountBHT20;
                _approve.AmountBHT50 = _request.AmountBHT50;
                _approve.AmountBHT100 = _request.AmountBHT100;
                _approve.AmountBHT500 = _request.AmountBHT500;
                _approve.AmountBHT1000 = _request.AmountBHT1000;

                _approve.AdditionalBHT = _request.AdditionalBHT;
                _approve.BorrowBHT = _request.BorrowBHT;
                _approve.ExchangeBHT = _request.ExchangeBHT;
                _approve.TSBId = _request.TSBId;
                _approve.UserId = AccountApp.User.Current.UserId; // current approve user.

                _approve.PeriodBegin = _request.PeriodBegin;
                _approve.PeriodEnd = _request.PeriodEnd;
            }
        }

        #endregion

        #region Public Method

        public void Setup(int requestId,
            Models.TSBExchangeTransaction request, Models.TSBExchangeTransaction approve = null)
        {
            _requestId = requestId;
            _request = request;
            _approve = approve;

            if (null != _request)
            {
                this.Title = "รายละเอียดคำร้องการขอ/แลก เงินยืมทอน - " + _request.TSBNameTH;
                _IsNewApprove = (null == _approve);
                if (_IsNewApprove)
                {
                    CreateNewApprove();
                }

                entry.Setup(_request, _approve);
            }
            else
            {
                this.Title = "รายละเอียดคำร้องการขอ/แลก เงินยืมทอน - ";
            }
        }

        #endregion
    }
}
