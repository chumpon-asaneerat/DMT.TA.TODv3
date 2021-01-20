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
            //Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
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
            var page = new Menu.MainMenu();
            PageContentManager.Instance.Current = page;
        }

        private void ResetDate()
        {
            dtEntryDate.DefaultValue = DateTime.Now;
            dtEntryDate.Value = DateTime.Now.Date;
        }

        private void ResetSelectUser()
        {
            _selectUser = null;
            txtSearchUserId.Text = string.Empty;
            txtUserId.Text = string.Empty;
            txtUserName.Text = string.Empty;
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
            grid.ItemsSource = null;

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
            List<LaneEMV> results = new List<LaneEMV>();
            List<LaneEMV> items = new List<LaneEMV>();
            List<LaneEMV> sortList = new List<LaneEMV>();

            if (null != _selectUser && null != _tsb && null != _plazas)
            {
                int networkId = TODConfigManager.Instance.DMT.networkId;

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
                            emvList.list.ForEach(item =>
                            {
                                items.Add(new LaneEMV(item));
                            });
                        }
                    });

                    sortList = items.OrderBy(o => o.TrxDateTime).Distinct().ToList();
                }
                // Filter By Lane
                var filter = GetLaneFilter();
                if (filter.HasValue)
                {
                    // Filter only specificed lane no.
                    results = sortList.Where(o => o.LaneNo == filter.Value).ToList();
                }
                else
                {
                    results.AddRange(sortList.ToArray());
                }
            }

            grid.ItemsSource = results;
        }

        private void RefreshQRCODE(DateTime dt1, DateTime dt2)
        {
            List<LaneQRCode> results = new List<LaneQRCode>();
            List<LaneQRCode> items = new List<LaneQRCode>();
            List<LaneQRCode> sortList = new List<LaneQRCode>();

            if (null != _selectUser && null != _tsb && null != _plazas)
            {
                int networkId = TODConfigManager.Instance.DMT.networkId;

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
                            emvList.list.ForEach(item =>
                            {
                                items.Add(new LaneQRCode(item));
                            });
                        }
                    });

                    sortList = items.OrderBy(o => o.TrxDateTime).Distinct().ToList();
                }
                // Filter By Lane
                var filter = GetLaneFilter();
                if (filter.HasValue)
                {
                    // Filter only specificed lane no.
                    results = sortList.Where(o => o.LaneNo == filter.Value).ToList();
                }
                else
                {
                    results.AddRange(sortList.ToArray());
                }
            }

            grid.ItemsSource = results;
        }

        private void SearchUser()
        {
            if (!string.IsNullOrEmpty(txtSearchUserId.Text))
            {
                string userId = txtSearchUserId.Text;
                if (string.IsNullOrEmpty(userId)) return;

                UserSearchManager.Instance.Title = "กรุณาเลือกพนักงานเก็บเงิน";
                _selectUser = UserSearchManager.Instance.SelectUser(userId, TODApp.Permissions.TC);
                if (null != _selectUser)
                {
                    txtUserId.Text = _selectUser.UserId;
                    txtUserName.Text = _selectUser.FullNameTH;
                    txtSearchUserId.Text = string.Empty;
                    RefreshEMV_QRCODE();
                }
                else
                {
                    txtUserId.Text = string.Empty;
                    txtUserName.Text = string.Empty;
                    grid.ItemsSource = null; // setup null list.
                }
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
            //_user = user;
            _tsb = TSB.GetCurrent().Value();

            ResetDate();
            ResetSelectUser();
            txtLaneNo.Text = string.Empty;

            if (null != _tsb)
            {
                _plazas = Plaza.GetTSBPlazas(_tsb).Value();
                grid.ItemsSource = null;
            }


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
