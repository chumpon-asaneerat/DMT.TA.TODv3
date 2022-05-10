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

namespace DMT.TA.Windows.Exchange
{
    /// <summary>
    /// Interaction logic for ReceiveExchangeWindow.xaml
    /// </summary>
    public partial class ReceiveExchangeWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReceiveExchangeWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private TSBRequestCreditManager _manager = null;

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            // Close Window
            DialogResult = false;
        }

        private void cmdExchange_Click(object sender, RoutedEventArgs e)
        {
            // Accept Exchange and close Window
            if (SaveTransactions())
            {
                DialogResult = true;
            }
        }

        #endregion

        #region Private Methods

        private bool SaveTransactions()
        {
            bool success = false;

            var appv = (null != _manager) ? _manager.Approve : null;
            var recv = (null != _manager) ? _manager.Receive : null;
            var exch = (null != _manager) ? _manager.ExchangeOut : null;
            if (null != appv && null != recv && null != exch)
            {
                if (appv.ExchangeBHT != exch.BHTTotal)
                {
                    var win = TAApp.Windows.MessageBox;
                    win.Setup("จำนวนเงินขอแลกออก ไม่เท่ากับ เงินขอแลก กรุณาตรวจสอบข้อมูล", "Toll Admin");
                    win.ShowDialog();
                }
                // save received.
                _manager.SaveReceived();
            }

            return success;
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="manager">The TSB Request Exchange Manager.</param>
        public void Setup(TSBRequestCreditManager manager)
        {
            this._manager = manager;

            if (null != _manager && null != _manager.Request)
            {
                // Need notify error when some items is missing.
            }

            entry.Setup(manager);
        }

        #endregion
    }
}
