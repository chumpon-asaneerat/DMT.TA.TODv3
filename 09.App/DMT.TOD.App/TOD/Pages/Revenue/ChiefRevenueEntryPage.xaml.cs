#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using DMT.Configurations;
using DMT.Controls;
using DMT.Models;
using DMT.Services;

using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.TOD.Pages.Revenue
{
    using scwOps = Services.Operations.SCW.TOD;

    /// <summary>
    /// Interaction logic for ChiefRevenueEntryPage.xaml
    /// </summary>
    public partial class ChiefRevenueEntryPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChiefRevenueEntryPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        private CultureInfo culture = new CultureInfo("th-TH");

        private RevenueEntryManager manager = new RevenueEntryManager();

        /*
        private User _user = null; // Supervisor
        private User _selectUser = null; // Collector

        private TSB _tsb = null;
        private List<PlazaGroup> _plazaGroups = null;
        private List<Plaza> _plazas = null;
        private List<Models.Shift> _shifts = null;

        private UserShift _userShift = null;
        private UserShift _revenueShift = null;

        private List<LaneJob> _jobs = null;
        */
        #endregion

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup DateTime Picker.
            dtEntryDate.CultureInfo = culture;
            dtRevDate.CultureInfo = culture;
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;
        }

        #endregion

        #region Combobox Handlers

        private void cbPlazas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == plazaGroup) return; // No Selection.
            // Set Current Plaza Group.
            if (null != manager.Current) manager.Current.PlazaGroup = plazaGroup; 

            LoadTSBLanes();
        }

        private void cbShifts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var shift = cbShifts.SelectedItem as Models.Shift;
            if (null == shift) return; // No Selection.
            // Set Current Shift.
            if (null != manager.Current) manager.Current.Shift = shift; 

            LoadTSBLanes();
        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdGotoRevenueEntry_Click(object sender, RoutedEventArgs e)
        {
            GotoRevenueEntry();
        }

        private void cmdBack2_Click(object sender, RoutedEventArgs e)
        {
            GotoBack();
        }

        private void cmdGotoRevenueEntryPreview_Click(object sender, RoutedEventArgs e)
        {
            GotoPrintPreview();
        }

        private void cmdBack3_Click(object sender, RoutedEventArgs e)
        {
            GotoBack();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            // Printing.

            // Go Back to Main Menu.
            GotoMainMenu();
        }

        private void cmdUserSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchUser();
        }

        #endregion

        #region TextBox Handlers

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
                LoadTSBLanes();
                e.Handled = true;
            }
        }

        #endregion

        #region Private Methods

        #region Tab Navigate methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = new Menu.MainMenu();
            PageContentManager.Instance.Current = page;
        }

        private void GotoBack()
        {
            if (tabs.SelectedIndex == 0)
            {
                GotoMainMenu();
            }
            else if (tabs.SelectedIndex == 1)
            {
                tabs.SelectedIndex = 0;
            }
            else if (tabs.SelectedIndex == 2)
            {
                tabs.SelectedIndex = 1;
            }
        }

        private void GotoRevenueEntry()
        {
            tabs.SelectedIndex = 1;
        }

        private void GotoPrintPreview()
        {
            tabs.SelectedIndex = 2;
        }

        #endregion

        #region Load Masters

        private void LoadShifts()
        {
            cbShifts.ItemsSource = null;
            if (null != manager && null != manager.Current)
            {
                cbShifts.ItemsSource = manager.Current.Shifts;
                if (manager.Current.Shifts.Count > 0) cbShifts.SelectedIndex = 0;
            }
            LoadTSBLanes();
        }

        private void LoadPlazaGroups()
        {
            cbPlazas.ItemsSource = null;
            if (null != manager && null != manager.Current)
            {
                cbPlazas.ItemsSource = manager.Current.TSBPlazaGroups;
                if (manager.Current.TSBPlazaGroups.Count > 0) cbPlazas.SelectedIndex = 0;
            }
            LoadTSBLanes();
        }

        private void LoadTSBLanes()
        {
            /*
            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            var shift = cbShifts.SelectedItem as Models.Shift;
            if (null == plazaGroup || null == shift)
            {
                return;
            }

            grid.ItemsSource = null;

            int networkId = TODConfigManager.Instance.DMT.networkId;

            if (null == _jobs)
            {
                // Create new job list.
                _jobs = new List<LaneJob>();
            }
            _jobs.Clear();

            grid.ItemsSource = _jobs;
            */
        }

        #endregion

        private void Reset()
        {
            // Reset Plaza.
            cbPlazas.SelectedIndex = -1;
            LoadPlazaGroups();
            // Reset Shift.
            cbShifts.SelectedIndex = -1;
            LoadShifts();

            // Set Bindings On Tab - Date Selection.
            dtEntryDate.DataContext = manager;
            dtRevDate.DataContext = manager;
            txtUserId.DataContext = manager;
            txtUserName.DataContext = manager;
            // Set Bindings On Tab - Revenue Entry.
            txtPlazaName.DataContext = manager;
            txtShiftName.DataContext = manager;
            txtRevDate2.DataContext = manager;
            txtUserId2.DataContext = manager;
            txtUserName2.DataContext = manager;
        }

        private void ResetSelectUser()
        {
            /*
            _selectUser = null;
            txtSearchUserId.Text = string.Empty;
            txtUserId.Text = string.Empty;
            txtUserName.Text = string.Empty;
            */
        }

        private void SearchUser()
        {
            /*
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
                    LoadLanes();
                }
                else
                {
                    txtUserId.Text = string.Empty;
                    txtUserName.Text = string.Empty;
                    grid.ItemsSource = null; // setup null list.
                }
            }
            */
        }

        private void CheckUserShift()
        {
            /*
            _userShift = null;
            if (null != _selectUser)
            {
                _userShift = UserShift.GetUserShift(_selectUser.UserId).Value();
            }
            */
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="user"></param>
        public void Setup(User user)
        {
            tabs.SelectedIndex = 0;

            manager.User = user;
            Reset();

            /*
            _user = user;
            if (null != _user)
            {
                _shifts = Models.Shift.GetShifts().Value();

                _tsb = TSB.GetCurrent().Value();
                if (null != _tsb)
                {
                    _plazaGroups = PlazaGroup.GetTSBPlazaGroups(_tsb).Value();
                }
            }
            Reset();
            */
        }

        #endregion
    }
}
