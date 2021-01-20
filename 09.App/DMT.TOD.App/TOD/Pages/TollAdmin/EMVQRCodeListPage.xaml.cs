#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;

using DMT.Configurations;
using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;
using System.Threading;

#endregion

namespace DMT.TOD.Pages.TollAdmin
{
    using scwOps = Services.Operations.SCW.TOD;

    /// <summary>
    /// Interaction logic for EMVQRCodeListPage.xaml
    /// </summary>
    public partial class EMVQRCodeListPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public EMVQRCodeListPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        private CultureInfo culture = new CultureInfo("th-TH");

        private User _selectUser = null;
        private TSB _tsb = null;
        private List<Plaza> _plazas = null;
        private string _laneFilter = string.Empty;

        #endregion

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup DateTime Picker.
            dtEntryDate.CultureInfo = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        #endregion

        #region RadioButton Handlers

        private void rbEMV_Click(object sender, RoutedEventArgs e)
        {
            RefreshEMV_QRCODE();
        }

        private void rbQRCode_Click(object sender, RoutedEventArgs e)
        {
            RefreshEMV_QRCODE();
        }

        #endregion

        #region DateTime Picker Handlers

        private void dtEntryDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            RefreshEMV_QRCODE();
        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdPaymentClear_Click(object sender, RoutedEventArgs e)
        {
            dtEntryDate.Value = DateTime.Now.Date;
            txtLaneNo.Text = string.Empty;
            RefreshEMV_QRCODE();
        }

        private void cmdPaymentSearch_Click(object sender, RoutedEventArgs e)
        {
            RefreshEMV_QRCODE();
        }

        private void cmdUserSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchUser();
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = new Menu.MainMenu();
            PageContentManager.Instance.Current = page;
        }

        private void ResetDate()
        {
            dtEntryDate.DefaultValue = DateTime.Now;
            dtEntryDate.Value = DateTime.Now.Date;
        }

        private int? GetLaneFilter()
        {
            int? ret = new int?();
            if (string.IsNullOrEmpty(txtLaneNo.Text)) return ret;
            int num;
            if (int.TryParse(txtLaneNo.Text.Trim(), out num))
            {
                ret = new int?(num);
            }
            return ret;
        }

        private void RefreshEMV_QRCODE()
        {
            if (!dtEntryDate.Value.HasValue)
            {
                dtEntryDate.Focus();
                return;
            }

            DateTime dt1 = dtEntryDate.Value.Value.Date;
            DateTime dt2 = dt1.AddDays(1);

            if (rbEMV.IsChecked.Value)
            {
                // EMV
                RefreshEMV(dt1, dt2);
            }
            else
            {
                // QR Code
                RefreshQRCODE(dt1, dt2);
            }
        }

        private void RefreshEMV(DateTime dt1, DateTime dt2)
        {
            List<SCWEMVTransaction> results = new List<SCWEMVTransaction>();
            if (null != _selectUser && null != _tsb && null != _plazas)
            {
                int networkId = TODConfigManager.Instance.DMT.networkId;
                List<SCWEMVTransaction> items = new List<SCWEMVTransaction>();

                if (null != _plazas && _plazas.Count > 0)
                {
                    _plazas.ForEach(plaza =>
                    {
                        int pzId = plaza.SCWPlazaId;
                        SCWEMVTransactionList param = new SCWEMVTransactionList();
                        param.networkId = networkId;
                        param.plazaId = pzId;
                        param.staffId = _selectUser.UserId;
                        param.startDateTime = dt1;
                        param.endDateTime = dt2;
                        var emvList = scwOps.emvTransactionList(param);
                        if (null != emvList && null != emvList.list)
                        {
                            items.AddRange(emvList.list);
                        }
                    });

                    results = items.OrderBy(o => o.trxDateTime).Distinct().ToList();
                }
            }
        }

        private void RefreshQRCODE(DateTime dt1, DateTime dt2)
        {
            List<SCWQRCodeTransaction> results = new List<SCWQRCodeTransaction>();
            if (null != _selectUser && null != _tsb && null != _plazas)
            {
                int networkId = TODConfigManager.Instance.DMT.networkId;
                List<SCWQRCodeTransaction> items = new List<SCWQRCodeTransaction>();

                if (null != _plazas && _plazas.Count > 0)
                {
                    _plazas.ForEach(plaza =>
                    {
                        int pzId = plaza.SCWPlazaId;
                        SCWQRCodeTransactionList param = new SCWQRCodeTransactionList();
                        param.networkId = networkId;
                        param.plazaId = pzId;
                        param.staffId = _selectUser.UserId;
                        param.startDateTime = dt1;
                        param.endDateTime = dt2;
                        var emvList = scwOps.qrcodeTransactionList(param);
                        if (null != emvList && null != emvList.list)
                        {
                            items.AddRange(emvList.list);
                        }
                    });

                    results = items.OrderBy(o => o.trxDateTime).Distinct().ToList();
                }
            }
        }

        private void SearchUser()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="user">The User instance.</param>
        public void Setup(User user)
        {
            //_user = user;
            _tsb = TSB.GetCurrent().Value();
            ResetDate();
            if (null != _tsb)
            {
                _plazas = Plaza.GetTSBPlazas(_tsb).Value();
            }
        }

        #endregion
    }
}
