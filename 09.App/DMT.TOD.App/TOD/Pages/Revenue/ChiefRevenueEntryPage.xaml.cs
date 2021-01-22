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
            var page = TODApp.Pages.MainMenu;
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

            #region Check Date

            if (!manager.RevenueDate.HasValue)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("กรุณาเลือกวันที่ของรายได้", "DMT - Tour of Duty");
                win.ShowDialog();
                dtRevDate.Focus();
                return;
            }

            if (manager.RevenueDate.HasValue && manager.RevenueDate.Value.Date > DateTime.Now.Date)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("ไม่อณุญาตให้ป้อนรายได้ล่วงหน้า กรุณาเลือกวันที่ของรายได้ใหม่", "DMT - Tour of Duty");
                win.ShowDialog();
                dtRevDate.Focus();
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

            bool isContinuous = true;
            if (null != manager.Jobs && null != manager.Jobs.PlazaGroupJobs && manager.Jobs.PlazaGroupJobs.Count > 0)
            {
                isContinuous = manager.Jobs.IsContinuous;
            }

            if (!isContinuous)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("การเลือกรายการ ต้องเป็นรายการต่อเเนื่องกันเท่านั้น", "DMT - Tour of Duty");
                win.ShowDialog();
                cbPlazas.Focus();
                return;
            }

            #endregion

            #region Check Has Revenue Shift (for collector)

            manager.CheckRevenueShift();

            if (null != manager.RevenueShift)
            {
                if (manager.RevenueShift.RevenueDate.HasValue &&
                    manager.RevenueShift.RevenueDate.Value != DateTime.MinValue)
                {
                    var win = TODApp.Windows.MessageBox;
                    win.Setup("กะของพนักงานนี้ ถูกป้อนรายได้แล้ว", "DMT - Tour of Duty");
                    win.ShowDialog();
                    return;
                }
            }
            else
            {
                if (manager.IsNewRevenueShift)
                {
                    var win = TODApp.Windows.MessageBox;
                    win.Setup("ไม่สามารถนำส่งรายได้ เนื่องจากไม่พบข้อมูลการทำงาน", "DMT - Tour of Duty");
                    win.ShowDialog();
                    return;
                }
                else
                {
                    var win = TODApp.Windows.MessageBox;
                    win.Setup("กะนี้ถูกจัดเก็บรายได้แล้ว.", "DMT - Tour of Duty");
                    win.ShowDialog();
                    return;
                }
            }

            #endregion

            if (!manager.NewRevenueEntry())
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("ไม่พบข้อมูลกะรายได้ของพนักงาน หรือไม่พบข้อมูลที่เกี่ยวข้อง", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            entry.Setup(manager); // Reset Context.

            // All check condition OK.
            tabs.SelectedIndex = 1;
        }

        private void GotoPrintPreview()
        {
            #region Check Has BagNo/BeltNo

            if (!entry.HasBagNo)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("กรุณาระบุ หมายเลขถุงเงิน", "DMT - Tour of Duty");
                win.ShowDialog();
                entry.FocusBagNo();
                return;
            }
            if (!entry.HasBeltNo)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("กรุณาระบุ หมายเลขเข็มขัดนิรภัย", "DMT - Tour of Duty");
                win.ShowDialog();
                entry.FocusBeltNo();
                return;
            }

            #endregion

            // Slip Preview
            if (!PrepareReport())
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("พบปัญหาในการเตรียมข้อมูล สำหรับจัดพิมพ์", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }
            // All OK so goto next tab.
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

        private bool PrepareReport()
        {
            if (null == manager || !manager.CanBuildRevenueSlipReport) return false;
            var model = manager.GetRevenueSlipReportModel();

            if (null == model ||
                null == model.DataSources || model.DataSources.Count <= 0 ||
                null == model.DataSources[0] || null == model.DataSources[0].Items)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("No result found.", "DMT - Tour of Duty");
                win.ShowDialog();
                if (win.ShowDialog() == true)
                {
                    this.rptViewer.ClearReport();
                }
            }
            else
            {
                this.rptViewer.LoadReport(model);
            }

            return true;
        }

        private void PrintReport()
        {
            if (null == manager || null == manager.Entry)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("Revenue Entry is not found.", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            if (null == manager.Entry ||
                !manager.Entry.RevenueDate.HasValue ||
                manager.Entry.RevenueDate.Value == DateTime.MinValue ||
                !manager.Entry.EntryDate.HasValue ||
                manager.Entry.EntryDate.Value == DateTime.MinValue)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("Entry Date or Revenue Date is not set.", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            bool hasActivitied = manager.SaveRevenueEntry();

            if (manager.Entry.RevenueDate.HasValue && manager.Entry.RevenueDate.Value != DateTime.MinValue &&
                manager.Entry.EntryDate.HasValue && manager.Entry.EntryDate.Value != DateTime.MinValue)
            {
                // print reports only date exists.
                this.rptViewer.Print();
            }

            if (!hasActivitied || null == manager.User)
            {
                GotoMainMenu();
            }
            else
            {
                // Still has more jobs on another Plaza Group.
                var win = TODApp.Windows.MessageBoxYesNo;
                win.Setup("กะปัจจุบันยังป้อนรายได้ไม่ครบ ต้องการป้อนรายได้ต่อหรือไม่ ?", "DMT - Tour of Duty");
                if (win.ShowDialog() == true)
                {
                    Setup(manager.User); // Goback to first page.
                }
                else
                {
                    GotoMainMenu();
                }
            }
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
            if (null != _chief)
            {

            }
            if (null != manager)
            {
                manager.User = null; // Reset.
                manager.ByChief = true;
                if (null != manager.UserShifts) manager.UserShifts.IsCustom = true;
            }
            Reset();
        }

        #endregion
    }
}
