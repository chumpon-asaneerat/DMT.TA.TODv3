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
            // Approve and close Window
            var win = AccountApp.Windows.ConfirmApproveMessageBox;
            win.Setup();
            if (win.ShowDialog() == false) return;

            //TAxTODMQService.Instance.WriteQueue();
            DialogResult = true;
        }

        private void cmdReject_Click(object sender, RoutedEventArgs e)
        {
            // Reject and close Window
            var win = AccountApp.Windows.ConfirmRejectMessageBox;
            win.Setup();
            if (win.ShowDialog() == false) return;

            //TAxTODMQService.Instance.WriteQueue();
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

        public void Setup(Models.TSBExchangeTransaction request, Models.TSBExchangeTransaction approve = null)
        {
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
