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
    /// Interaction logic for RequestExchangeWindow.xaml
    /// </summary>
    public partial class RequestExchangeWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RequestExchangeWindow()
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
            var win = TAApp.Windows.MessageBoxYesNo;
            win.Setup("ยืนยันยกเลิกคำร้องขอแลกเปลี่ยนเงิน", "DMT - Toll Admin");
            if (win.ShowDialog() == false)
                return;

            if (null != _manager && null != _manager.Request)
            {
                // Cancel Request and close window
                _manager.CancelRequest();

                DialogResult = false;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (null != _manager && null != _manager.Request)
            {
                if (!_manager.IsMatchAmount())
                {
                    var win = TAApp.Windows.MessageBox;
                    win.Setup("ยอดเงินรวมไม่ตรงกัน กรุณาตรวจสอบข้อมูล.", "DMT - Toll Admin");
                    win.ShowDialog();
                    return;
                }

                // update period begin/end
                _manager.Request.PeriodBegin =  entry.PeriodBegin;
                _manager.Request.PeriodEnd = entry.PeriodEnd;

                if (!_manager.CheckPeriod())
                {
                    var win = TAApp.Windows.MessageBox;
                    win.Setup("กรุณาระบุวันที่ใช้เงิน และวันที่คืนเงิน.", "DMT - Toll Admin");
                    win.ShowDialog();
                    return;
                }
                // Save or Update Request and close window
                _manager.SaveRequest();

                DialogResult = true;
            }
        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            // Close Window
            DialogResult = false;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="manager">The TSB Request Exchange Manager.</param>
        public void Setup(TSBRequestCreditManager manager)
        {
            this._manager = manager;

            if (null != _manager && null != _manager.Request)
            {
                entry.PeriodBegin = _manager.Request.PeriodBegin;
                entry.PeriodEnd = _manager.Request.PeriodEnd;
            }

            entry.Setup(manager);
        }

        #endregion
    }
}
