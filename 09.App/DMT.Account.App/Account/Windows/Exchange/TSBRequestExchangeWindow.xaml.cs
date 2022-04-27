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

        #region Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Button Handlers

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            // Approve and close Window
            DialogResult = true;
        }

        private void cmdReject_Click(object sender, RoutedEventArgs e)
        {
            // Reject and close Window
            DialogResult = true;
        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            // Close Window
            DialogResult = false;
        }

        #endregion

        #region Public Method

        public void Setup(Models.TSBExchangeTransaction request)
        {
            if (null != request)
            {
                this.Title = "รายละเอียดคำร้องการขอ/แลก เงินยืมทอน - " + request.TSBNameTH;
                var appExchange = new TSBExchangeTransaction();

                appExchange.TransactionDate = DateTime.Now;
                appExchange.TransactionType = TSBExchangeTransaction.TransactionTypes.Approve;

                appExchange.AmountST25 = request.AmountST25;
                appExchange.AmountST50 = request.AmountST50;
                appExchange.AmountBHT1 = request.AmountBHT1;
                appExchange.AmountBHT2 = request.AmountBHT2;
                appExchange.AmountBHT5 = request.AmountBHT5;
                appExchange.AmountBHT10 = request.AmountBHT10;
                appExchange.AmountBHT20 = request.AmountBHT20;
                appExchange.AmountBHT50 = request.AmountBHT50;
                appExchange.AmountBHT100 = request.AmountBHT100;
                appExchange.AmountBHT500 = request.AmountBHT500;
                appExchange.AmountBHT1000 = request.AmountBHT1000;

                appExchange.AdditionalBHT = request.AdditionalBHT;
                appExchange.BorrowBHT = request.BorrowBHT;
                appExchange.ExchangeBHT = request.ExchangeBHT;
                appExchange.TSBId = request.TSBId;
                appExchange.UserId = AccountApp.User.Current.UserId; // current approve user.

                appExchange.PeriodBegin = request.PeriodBegin;
                appExchange.PeriodEnd = request.PeriodEnd;

                entry.Setup(request, appExchange);
            }
            else
            {
                this.Title = "รายละเอียดคำร้องการขอ/แลก เงินยืมทอน - ";
            }
        }

        #endregion
    }
}
