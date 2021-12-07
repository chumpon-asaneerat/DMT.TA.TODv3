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
    /// Interaction logic for InternalExchangeWindow.xaml
    /// </summary>
    public partial class InternalExchangeWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public InternalExchangeWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private TSBReplaceCreditManager _manager = new TSBReplaceCreditManager();

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (null != _manager)
            {
                if (!_manager.CanReplaceOut)
                {
                    var win = TAApp.Windows.MessageBox;
                    win.Setup("จำนวนเงินขอแลกออกในบางรายการ เกินจำนวนที่่ด่านมีอยู่ กรุณาตรวจสอบข้อมูล", "Toll Admin");
                    win.ShowDialog();
                    return;
                }

                if (!_manager.IsEquals)
                {
                    var win = TAApp.Windows.MessageBox;
                    win.Setup("จำนวนเงินขอแลก ไม่เท่ากัน กรุณาตรวจสอบข้อมูล", "Toll Admin");
                    win.ShowDialog();
                    return;
                }
                _manager.Save();
            }

            DialogResult = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            // Set TSB.
            _manager.TSB = TSB.GetCurrent().Value();
            // Set Current Supervisor
            _manager.Supervisor = TAApp.User.Current;
            
            // Set description (Replace out)
            _manager.ReplaceOut.Description = "เงินขอแลกออก";
            this.plazaEntry.IsEnabled = true;
            this.plazaEntry.DataContext = _manager.ReplaceOut;

            // Set description (Replace in)
            _manager.ReplaceIn.Description = "เงินขอแลกเข้า";
            this.exchangeEntry.IsEnabled = true;
            this.exchangeEntry.DataContext = _manager.ReplaceIn;
        }

        #endregion
    }
}
