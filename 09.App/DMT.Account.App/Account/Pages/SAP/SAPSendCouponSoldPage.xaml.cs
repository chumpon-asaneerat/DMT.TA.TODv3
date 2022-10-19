#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

using ops = DMT.Services.Operations.TAxTOD.SAP;

namespace DMT.Account.Pages.SAP
{
    /// <summary>
    /// Interaction logic for SAPSendCouponSoldPage.xaml
    /// </summary>
    public partial class SAPSendCouponSoldPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SAPSendCouponSoldPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _chief = null;
        private int _tollwayId = 9;

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void cmdSendToSAP_Click(object sender, RoutedEventArgs e)
        {
            SendToSAP();
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = AccountApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void Reset()
        {
            dtSoldDate.Value = new DateTime?(DateTime.Now);
            grid.ItemsSource = null;
            gridSum.ItemsSource = null;

            if (null != cbTSBs.ItemsSource && cbTSBs.ItemsSource is IList && (cbTSBs.ItemsSource as IList).Count > 0)
            {
                cbTSBs.SelectedIndex = 0;
            }
        }

        private void GetSAPTSBs()
        {
            cbTSBs.ItemsSource = null;
            var ret = ops.GetTSBs();
            if (null != ret && ret.Ok)
            {
                List<Models.SAPTSB> src = ret.Value();
                List<Models.SAPTSB> list;
                if (null != src)
                {
                    list = src.OrderBy(item => item.TSBId).ToList();
                }
                else list = src;

                cbTSBs.ItemsSource = list;
            }
        }

        private void Search()
        {
            DateTime? dt = (dtSoldDate.Value.HasValue) ? dtSoldDate.Value.Value : new DateTime?();

            if (!dt.HasValue)
            {
                var win = AccountApp.Windows.MessageBox;
                win.Setup("กรุณาระบุวันที่ขาย.", "DMT - TA (Account)");
                win.ShowDialog();
                // Focus on SoldDate input.
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    dtSoldDate.SelectAll();
                    dtSoldDate.Focus();
                }));
                return;
            }

            if (null == cbTSBs.SelectedItem)
            {
                var win = AccountApp.Windows.MessageBox;
                win.Setup("กรุณาเลือกด่าน.", "DMT - TA (Account)");
                win.ShowDialog();
                // Focus on TSB Combobox.
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    cbTSBs.Focus();
                }));
                return;
            }

            grid.ItemsSource = null;
            gridSum.ItemsSource = null;
            _tollwayId = (cbTSBs.SelectedItem as SAPTSB).TollwayID;
            var ret = ops.GetCouponSolds(Models.Search.TAxTOD.SAP.CouponSolds.Create(dt, _tollwayId));
            if (null != ret && ret.Ok)
            {
                var list = ret.Value();
                grid.ItemsSource = list;

                txtTotalRows.Text = "0";
                if (null != list && list.Count > 0)
                {
                    txtTotalRows.Text = list.Count.ToString("n0");
                }
                // get summary list.
                var sum = SAPCouponSoldSummaryItem.Calculate(list);
                gridSum.ItemsSource = sum;
            }
        }

        private void SendToSAP()
        {
            DateTime? dt = (dtSoldDate.Value.HasValue) ? dtSoldDate.Value.Value : new DateTime?();
            if (!dt.HasValue)
            {
                // No date selected.
                var win = AccountApp.Windows.MessageBox;
                win.Setup("กรุณาระบุวันที่ขาย.", "DMT - TA (Account)");
                win.ShowDialog();
                // Focus on SoldDate input.
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    dtSoldDate.SelectAll();
                    dtSoldDate.Focus();
                }));
                return;
            }

            if (null == cbTSBs.SelectedItem)
            {
                // No TSB selected.
                var win = AccountApp.Windows.MessageBox;
                win.Setup("กรุณาเลือกด่าน.", "DMT - TA (Account)");
                win.ShowDialog();
                // Focus on TSB Combobox.
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    cbTSBs.Focus();
                }));
                return;
            }

            _tollwayId = (cbTSBs.SelectedItem as SAPTSB).TollwayID;

            SAPSaveAR json = new SAPSaveAR();
            json.DocDate = DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            json.DocDueDate = dt.Value.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            // fixed
            json.CardCode = "C-Coupon P";
            json.CardName = "C-Coupon P";
            json.TollwayID = _tollwayId;

            TAxTODMQService.Instance.WriteQueue(json);
            // Show success gnerate message.
            {
                var win = AccountApp.Windows.MessageBox;
                win.Setup("เสร็จสิ้นการสร้างรายการนำส่งไปยัง SAP แล้ว.", "DMT - TA (Account)");
                win.ShowDialog();
            }

            Reset(); // clear all inputs.
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="chief">The Current User.</param>
        public void Setup(User chief)
        {
            _chief = chief;

            if (null != _chief)
            {

            }

            GetSAPTSBs();
            Reset();

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                dtSoldDate.SelectAll();
                dtSoldDate.Focus();
            }));
        }

        #endregion
    }
}
