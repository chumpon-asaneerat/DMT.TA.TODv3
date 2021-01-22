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
using DMT.Controls;

using NLib.Services;
using NLib.Reflection;
using System.Threading;
using System.Windows.Threading;

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

        private User _user = null;
        private string _laneFilter = string.Empty;

        private PaymentManager paymentMgr = new PaymentManager(new CurrentTSBManager());

        #endregion

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup DateTime Picker.
            dtEntryDate.CultureInfo = culture;
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;
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

        #region TextBox Handlers

        private void txtLaneNo_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var currFilter = txtLaneNo.Text.Trim();
            if (e.Key == System.Windows.Input.Key.Enter ||
                e.Key == System.Windows.Input.Key.Return)
            {
                if (_laneFilter != currFilter)
                {
                    _laneFilter = currFilter;
                    RefreshEMV_QRCODE();
                    e.Handled = true;
                }
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                txtLaneNo.Text = string.Empty;
                e.Handled = true;
            }
        }

        private void txtLaneNo_GotFocus(object sender, RoutedEventArgs e)
        {
            _laneFilter = txtLaneNo.Text.Trim();
        }

        private void txtLaneNo_LostFocus(object sender, RoutedEventArgs e)
        {
            var currFilter = txtLaneNo.Text.Trim();
            if (_laneFilter != currFilter)
            {
                _laneFilter = currFilter;
                RefreshEMV_QRCODE();
            }
        }

        private void txtSearchUserId_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter ||
                e.Key == System.Windows.Input.Key.Return)
            {
                SearchUser();
                e.Handled = true;
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                ResetSelectUser();
                RefreshEMV_QRCODE();
                e.Handled = true;
            }
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
            var page = TODApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void Reset()
        {
            paymentMgr.User = null;
            paymentMgr.EnableLaneFilter = true;

            dtEntryDate.DefaultValue = DateTime.Now;
            dtEntryDate.Value = DateTime.Now.Date;
            // Set Bindings User Selection.
            txtUserId.DataContext = paymentMgr;
            txtUserName.DataContext = paymentMgr;
        }

        private void ResetSelectUser()
        {
            paymentMgr.User = null;
            txtSearchUserId.Text = string.Empty;
        }

        private void SearchUser()
        {
            string userId = txtSearchUserId.Text.Trim();
            var result = TODAPI.SearchUser(userId, TODApp.Permissions.TC);
            if (!result.IsCanceled && null != paymentMgr)
            {
                paymentMgr.User = result.User;
                if (null != paymentMgr.User)
                {
                    txtSearchUserId.Text = string.Empty;
                }
                RefreshEMV_QRCODE();
            }
        }

        private void RefreshEMV_QRCODE()
        {
            grid.ItemsSource = null;

            if (!dtEntryDate.Value.HasValue)
            {
                dtEntryDate.Focus();
                return;
            }

            DateTime dt1 = dtEntryDate.Value.Value.Date;
            DateTime dt2 = dt1.AddDays(1);

            if (null == paymentMgr) return;

            paymentMgr.PaymentType = (rbEMV.IsChecked.Value) ? PaymentTypes.EMV : PaymentTypes.QRCode;
            paymentMgr.Begin = dt1;
            paymentMgr.End = dt2;
            paymentMgr.Filter = _laneFilter;
            paymentMgr.Refresh();

            if (rbEMV.IsChecked.Value)
            {
                grid.ItemsSource = paymentMgr.EMVItems;
            }
            else
            {
                grid.ItemsSource = paymentMgr.QRCodeItems;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="user">The User instance.</param>
        public void Setup(User user)
        {
            _user = user;
            if (null != _user)
            {

            }

            Reset();
            ResetSelectUser();
            txtLaneNo.Text = string.Empty;

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtSearchUserId.SelectAll();
                txtSearchUserId.Focus();
            }));
        }

        #endregion
    }
}
