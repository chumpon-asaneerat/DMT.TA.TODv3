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

        private User _chief = null; // Supervisor
        /*
        private UserShift _userShift = null;
        private UserShift _revenueShift = null;
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

        private void cbShifts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var shift = cbShifts.SelectedItem as Models.Shift;
            if (null == shift) return; // No Selection.
            // Set Current Shift.
            if (null != manager.Current) manager.Current.Shift = shift;

            LoadTSBLanes();
        }

        private void cbPlazas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == plazaGroup) return; // No Selection.
            // Set Current Plaza Group.
            if (null != manager) manager.PlazaGroup = plazaGroup;

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
            GotoPrevious();
        }

        private void cmdGotoRevenueEntryPreview_Click(object sender, RoutedEventArgs e)
        {
            GotoPrintPreview();
        }

        private void cmdBack3_Click(object sender, RoutedEventArgs e)
        {
            GotoPrevious();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            // Printing.
            PrintReport();
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

        private void GotoPrevious()
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
            #region Check RevenueEntryManager

            if (null == manager)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("Program Error: RevenueEntryManager is null.", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            #endregion

            #region Check Select Shift

            if (null == manager.Current.Shift)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("กรุณาเลือกกะของรายได้", "DMT - Tour of Duty");
                win.ShowDialog();
                cbShifts.Focus();
                return;
            }

            #endregion

            #region Check Select PlazaGroup

            if (null == manager.PlazaGroup)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("กรุณาเลือกด่านของรายได้", "DMT - Tour of Duty");
                win.ShowDialog();
                cbPlazas.Focus();
                return;
            }

            #endregion

            #region Check Select User

            if (null == manager.User)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("กรุณาเลือกพนักงาน", "DMT - Tour of Duty");
                win.ShowDialog();
                txtUserId.Focus();
                return;
            }

            #endregion

            #region Check is Continuous selection

            /*
            bool isContinuous = true;
            if (null != _manager.Attendances && _manager.Attendances.Count > 0)
            {
                // Create indexes list.
                int idx = 0;
                List<int> indexs = new List<int>();
                foreach (var att in _manager.Attendances)
                {
                    if (att.Selected) indexs.Add(idx);
                    idx++;
                }
                // Check Continuous
                if (null != indexs && indexs.Count > 0)
                {
                    // 3, 4, 5, 7
                    int currIndex = indexs[0] - 1; // set init value to first minus 1 for check in loop.
                    foreach (int val in indexs)
                    {
                        if (val - 1 > currIndex)
                        {
                            isContinuous = false;
                            break;
                        }
                        currIndex = val; // update new current index.
                    }
                }
            }

            if (!isContinuous)
            {
                DMT.Windows.MessageBoxWindow msg = new DMT.Windows.MessageBoxWindow();
                msg.Owner = Application.Current.MainWindow;
                msg.Setup("การเลือกรายการ ต้องเป็นรายการต่อเเนื่องกันเท่านั้น", "DMT - Tour of Duty");
                if (msg.ShowDialog() == true)
                {
                    cbPlazas.Focus();
                }
                return;
            }
            */

            #endregion

            // All check condition OK.
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
            grid.ItemsSource = null;

            if (null == manager || null == manager.Jobs || null == manager.UserShifts) return;

            manager.Jobs.OnlyJobInShift = false; // Show all jobs
            manager.Jobs.UserShift = manager.UserShifts.Create();
            manager.Jobs.PlazaGroup = manager.PlazaGroup;
            manager.Jobs.Refresh();
            grid.ItemsSource = manager.Jobs.PlazaGroupJobs;
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
            if (null != manager) manager.User = null;
            /*
            txtSearchUserId.Text = string.Empty;
            txtUserId.Text = string.Empty;
            txtUserName.Text = string.Empty;
            */
        }

        private void SearchUser()
        {
            string userId = txtSearchUserId.Text.Trim();
            var result = TODAPI.SearchUser(userId, TODApp.Permissions.TC);
            if (!result.IsCanceled && null != manager)
            {
                manager.User = result.User;
                if (null != manager.User)
                {
                    txtSearchUserId.Text = string.Empty;
                }
                LoadTSBLanes();
            }
        }

        private void CreateUserShift()
        {
            if (null != manager)
            {
                manager.UserShifts.IsCustom = true;
            }
        }

        private void PrintReport()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="chief">The chief user.</param>
        public void Setup(User chief)
        {
            tabs.SelectedIndex = 0;

            _chief = chief;
            if (null != manager)
            {
                manager.ByChief = true;
                if (null != manager.UserShifts) manager.UserShifts.IsCustom = true;
            }
            Reset();
        }

        #endregion
    }
}
