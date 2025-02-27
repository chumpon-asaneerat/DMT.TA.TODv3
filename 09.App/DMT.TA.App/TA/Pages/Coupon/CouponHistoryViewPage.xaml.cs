﻿#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
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

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        private CultureInfo culture = new CultureInfo("th-TH");
        private XmlLanguage language = XmlLanguage.GetLanguage("th-TH");

        private User _chief = null;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup DateTime Picker
            // Work From
            dtWorkDateFrom.CultureInfo = culture;
            dtWorkDateFrom.Language = language;
            // Work To
            dtWorkDateTo.CultureInfo = culture;
            dtWorkDateTo.Language = language;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            ClearInputs();
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TAApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void LoadTSBs()
        {
            var tsbs = TSB.GetTSBs().Value();
            if (null != tsbs)
            {
                tsbs.Insert(0, new Models.TSB() { TSBId = "00", TSBNameEN = "[All]", TSBNameTH = "[ไม่ระบุด่าน]" });
                tsbs.Add(new Models.TSB() { TSBId = "99", TSBNameEN = "[Other]", TSBNameTH = "[อื่น ๆ ]" });
            }

            cbTSBs.ItemsSource = tsbs;
            //if (null != tsbs) tsbs.SelectedIndex = 0;
        }

        private void LoadShifts()
        {
            var shifts = TAAPI.Shifts;
            if (null != shifts)
            {
                shifts.Insert(0, new Models.Shift() { ShiftId = 0, ShiftNameEN = "[All]", ShiftNameTH = "[เลือกทั้งหมด]" });
            }
            cbShifts.ItemsSource = shifts;
            if (null != shifts) cbShifts.SelectedIndex = 0;
        }

        private void LoadInquiryStatus()
        {
            var status = InquiryStatus.Gets();
            cbStatus.ItemsSource = status;
            if (null != status) cbStatus.SelectedIndex = 0;
        }

        private void Reset()
        {
            txtSAPItemCode.Text = string.Empty;
            txtSAPIntrSerial.Text = string.Empty;
            txtSAPTransferNo.Text = string.Empty;
            txtSAPARInvoice.Text = string.Empty;

            dtWorkDateFrom.Value = new DateTime?();
            dtWorkDateTo.Value = new DateTime?();
        }

        private void ClearInputs()
        {
            Reset();

            //CheckComboDefaultItem(cbTSBs); // comment out used auto select current TSB instead.
            AutoSelectCurrentTSB();

            CheckComboDefaultItem(cbStatus);
            CheckComboDefaultItem(cbShifts);

            txtStockBalance.Text = "0"; // clear stock.
            grid.ItemsSource = null; // clear grid.
        }

        private void CheckComboDefaultItem(ComboBox cb)
        {
            if (null == cb) return;
            cb.SelectedIndex = -1;
            if (null != cb.ItemsSource)
            {
                var lst = cb.ItemsSource as IList;
                if (null != lst && lst.Count > 0) cb.SelectedIndex = 0;
            }
        }

        private void AutoSelectCurrentTSB()
        {
            var tsb = TSB.GetCurrent().Value();

            cbTSBs.SelectedIndex = -1;
            if (null != cbTSBs.ItemsSource)
            {
                var tsbs = cbTSBs.ItemsSource as List<TSB>;
                int idx = -1;
                if (null != tsbs && null != tsb)
                {
                    try
                    {
                        idx = tsbs.FindIndex((eachTSB) => { return eachTSB.TSBId == tsb.TSBId; });
                    }
                    catch
                    {
                        idx = -1;
                    }
                }
                if (idx == -1)
                {
                    cbTSBs.SelectedIndex = 0;
                    cbTSBs.IsEnabled = true;
                }
                else
                {
                    cbTSBs.SelectedIndex = idx;
                    cbTSBs.IsEnabled = false;
                }
            }
        }

        private void Search()
        {
            grid.ItemsSource = null;

            string sapItemCode = string.IsNullOrWhiteSpace(txtSAPItemCode.Text) ? null : txtSAPItemCode.Text.Trim();
            string sapIntrSerial = string.IsNullOrWhiteSpace(txtSAPIntrSerial.Text) ? null : txtSAPIntrSerial.Text.Trim();
            string sapTransferNo = string.IsNullOrWhiteSpace(txtSAPTransferNo.Text) ? null : txtSAPTransferNo.Text.Trim();
            string sapARInvoice = string.IsNullOrWhiteSpace(txtSAPARInvoice.Text) ? null : txtSAPARInvoice.Text.Trim();

            var status = (null != cbStatus.SelectedItem && cbStatus.SelectedItem is Models.InquiryStatus) ?
                cbStatus.SelectedItem as Models.InquiryStatus : null;
            int ? itemStatusDigit = (null != status) ? status.ItemStatusDigit : new int?();

            DateTime? workingDateFrom = (dtWorkDateFrom.Value.HasValue) ? dtWorkDateFrom.Value.Value.Date : new DateTime?();
            DateTime? workingDateTo = (dtWorkDateTo.Value.HasValue) ? dtWorkDateTo.Value.Value.Date : new DateTime?();

            var tsb = (null != cbTSBs.SelectedItem && cbTSBs.SelectedItem is Models.TSB) ?
                cbTSBs.SelectedItem as Models.TSB : null;
            int ? tollWayId = (null != tsb && tsb.TSBId != "00") ? Convert.ToInt32(tsb.TSBId) : new int?();

            var shift = (null != cbShifts.SelectedItem && cbShifts.SelectedItem is Models.Shift) ?
                cbShifts.SelectedItem as Models.Shift : null;
            int ? shiftId = (null != shift) ? shift.ShiftId : new int?();

            var searchOp = Models.Search.TAxTOD.Coupon.Inquiry.Create(sapItemCode, sapIntrSerial, sapTransferNo,
                sapARInvoice, itemStatusDigit, tollWayId, shiftId, workingDateFrom, workingDateTo);
            var ret = ops.Inquiry(searchOp);
            var coupons = ret.Value();
            grid.ItemsSource = coupons;

            if (null != coupons)
            {
                int cnt = coupons.Count;
                /*
                int cnt = coupons.Count(coupon => 
                { 
                    return (coupon.ItemStatusDigit.HasValue && coupon.ItemStatusDigit.Value == 1);
                });
                */
                txtStockBalance.Text = string.Format("{0:n0}", cnt);
            }
            else txtStockBalance.Text = "0";
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

            LoadTSBs(); // TollwayId
            LoadShifts();
            LoadInquiryStatus();
            ClearInputs();

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
