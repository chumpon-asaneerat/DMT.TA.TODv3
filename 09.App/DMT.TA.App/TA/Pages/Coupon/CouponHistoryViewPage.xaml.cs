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

namespace DMT.TA.Pages.Coupon
{
    using ops = DMT.Services.Operations.TAxTOD.Coupon;

    /// <summary>
    /// Interaction logic for CouponHistoryViewPage.xaml
    /// </summary>
    public partial class CouponHistoryViewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CouponHistoryViewPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _chief = null;

        #endregion

        #region Button Handlers

        private void cmdRefresh_Click(object sender, RoutedEventArgs e)
        {
            Resync();
        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TAApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void Resync()
        {
            CouponSyncService.Instance.ReSyncAll();
        }

        private void LoadShifts()
        {
           cbShifts.ItemsSource = TAAPI.Shifts;
        }

        private void Search()
        {
            string sapItemCode = string.IsNullOrWhiteSpace(txtSAPItemCode.Text) ? null : txtSAPItemCode.Text.Trim();
            string sapIntrSerial = string.IsNullOrWhiteSpace(txtSAPIntrSerial.Text) ? null : txtSAPIntrSerial.Text.Trim();
            string sapTransferNo = string.IsNullOrWhiteSpace(txtSAPTransferNo.Text) ? null : txtSAPTransferNo.Text.Trim();
            string sapARInvoice = string.IsNullOrWhiteSpace(txtSAPARInvoice.Text) ? null : txtSAPARInvoice.Text.Trim();
            int? itemStatusDigit = new int?();
            int? tollWayId = 9;
            DateTime? workingDateFrom = (dtWorkDateFrom.Value.HasValue) ? dtWorkDateFrom.Value.Value : new DateTime?();
            DateTime? workingDateTo = (dtWorkDateTo.Value.HasValue) ? dtWorkDateTo.Value.Value : new DateTime?();
            int? shiftId = (null != cbShifts.SelectedItem && cbShifts.SelectedItem is Models.Shift) ?
                (cbShifts.SelectedItem as Models.Shift).ShiftId : new int ?();
            var searchOp = Models.Search.TAxTOD.Coupon.Inquiry.Create(sapItemCode, sapIntrSerial, sapTransferNo,
                itemStatusDigit, tollWayId, workingDateFrom, workingDateTo, sapARInvoice, shiftId);
            var ret = ops.Inquiry(searchOp);
            grid.ItemsSource = ret.Value();
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

            LoadShifts();

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtSAPItemCode.SelectAll();
                txtSAPItemCode.Focus();
            }));
        }

        #endregion
    }
}
