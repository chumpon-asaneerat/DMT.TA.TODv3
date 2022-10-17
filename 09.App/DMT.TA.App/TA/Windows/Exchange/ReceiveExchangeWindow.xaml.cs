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
                if (appv.BHTTotal != recv.BHTTotal)
                {
                    var win = TAApp.Windows.MessageBox;
                    win.Owner = this; // change owner
                    win.Setup("ยอดรวมเงินที่ได้รับจริง ไม่เท่ากับ ยอดรวมรายการอนุมัติจากบัญชี กรุณาตรวจสอบข้อมูล", "Toll Admin");
                    win.ShowDialog();
                    return success;
                }

                if (appv.ExchangeBHT != exch.BHTTotal)
                {
                    var win = TAApp.Windows.MessageBox;
                    win.Owner = this; // change owner
                    win.Setup("จำนวนเงินขอแลกออก ไม่เท่ากับ เงินขอแลก กรุณาตรวจสอบข้อมูล", "Toll Admin");
                    win.ShowDialog();
                    return success;
                }

                var tsbBal = TSBCreditBalance.GetCurrent(TAAPI.TSB).Value(); // Find current TSB balance.
                if (tsbBal.AmountST25 - exch.AmountST25 < 0 ||
                    tsbBal.AmountST50 - exch.AmountST50 < 0 ||
                    tsbBal.AmountBHT1 - exch.AmountBHT1 < 0 ||
                    tsbBal.AmountBHT2 - exch.AmountBHT2 < 0 ||
                    tsbBal.AmountBHT5 - exch.AmountBHT5 < 0 ||
                    tsbBal.AmountBHT10 - exch.AmountBHT10 < 0 ||
                    tsbBal.AmountBHT20 - exch.AmountBHT20 < 0 ||
                    tsbBal.AmountBHT50 - exch.AmountBHT50 < 0 ||
                    tsbBal.AmountBHT100 - exch.AmountBHT100 < 0 ||
                    tsbBal.AmountBHT500 - exch.AmountBHT500 < 0 ||
                    tsbBal.AmountBHT1000 - exch.AmountBHT1000 < 0)
                {
                    var win = TAApp.Windows.MessageBox;
                    win.Owner = this; // change owner
                    win.Setup("จำนวนเงินขอแลกออกในด่าน ไม่เพียงพอแลก กรุณาตรวจสอบข้อมูล", "Toll Admin");
                    win.ShowDialog();
                    return success;
                }

                var confirm = TAApp.Windows.ConfirmChiefReceiveMoney;
                confirm.Owner = this; // change owner
                confirm.Setup(appv.GrandTotalBHT);
                if (confirm.ShowDialog() == false)
                {
                    // failed to verify user
                    return success;
                }
                // save received.
                _manager.SaveReceived();
                success = true; // setup flag.
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
