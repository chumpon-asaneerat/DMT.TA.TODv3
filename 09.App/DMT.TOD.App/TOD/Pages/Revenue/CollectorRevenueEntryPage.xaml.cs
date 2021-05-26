#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

using DMT.Configurations;
using DMT.Models;
using DMT.Models.ExtensionMethods;
using DMT.Services;

using NLib;
using NLib.Services;
using NLib.Reports.Rdlc;
using NLib.Reflection;

#endregion

namespace DMT.TOD.Pages.Revenue
{
    using scwOps = Services.Operations.SCW.TOD;

    /// <summary>
    /// Interaction logic for CollectorRevenueEntryPage.xaml
    /// </summary>
    public partial class CollectorRevenueEntryPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectorRevenueEntryPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        //private CultureInfo culture = new CultureInfo("th-TH");

        private User _user = null;
        private RevenueEntryManager manager = null;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;
            TODConfigManager.Instance.ConfigChanged += Instance_ConfigChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            TODConfigManager.Instance.ConfigChanged -= Instance_ConfigChanged;
        }

        #endregion

        #region Config Changed Handler

        private void Instance_ConfigChanged(object sender, EventArgs e)
        {
            // Re assign User because when config change the CurrentTSBManager will reset all data
            // before this handler.
            if (null != manager) manager.User = _user;
            Setup(_user); // Reload.
        }

        #endregion

        #region Combobox Handlers

        private void cbPlazas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null != manager && null == plazaGroup) return;
            if (null != manager) manager.PlazaGroup = plazaGroup;
            RefreshJobList();
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
            GotoMainMenu();
            // Used below code if need to go back to select date.
            //GotoPrevious();
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

            #region Check Jobs if online

            if (null != manager.Jobs && 
                manager.Jobs.SCWOnline && 
                (manager.Jobs.PlazaGroupJobs == null || manager.Jobs.PlazaGroupJobs.Count <= 0))
            {
                // Online but no jobs on current plaza group.
                var win = TODApp.Windows.MessageBox;
                win.Setup("ไม่พบข้อมูลเลนที่ยังไม่ถูกป้อนรายได้", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            #endregion

            #region Check Return Bag

            if (!manager.IsReturnBag())
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("ระบบตรวจพบว่ายังไม่มีการคืนถุงเงิน กรุณาคืนถุงเงินก่อนป้อนรายได้.", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
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
            tabs.SelectedIndex = 1; // goto next tab.
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

        private void LoadPlazaGroups()
        {
            cbPlazas.ItemsSource = null;
            if (null != manager && null != manager.Current)
            {
                cbPlazas.ItemsSource = manager.Current.TODPlazaGroups;
                if (manager.Current.TODPlazaGroups.Count > 0) cbPlazas.SelectedIndex = 0;
            }
        }

        private void RefreshJobList()
        {
            grid.ItemsSource = null;

            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == manager || null == manager.UserShift || null == plazaGroup) return;

            // Refresh jobs.
            manager.Jobs.ViewMode = ViewModes.TOD; // Show by PlazaId in TOD config Only
            manager.Jobs.OnlyJobInShift = true;
            manager.Jobs.UserShift = manager.UserShift; // assign current user shift.
            manager.Jobs.PlazaGroup = plazaGroup; // assign selected plaza group.
            manager.Jobs.Refresh();
            // Bind to ListView
            grid.ItemsSource = manager.Jobs.PlazaGroupJobs;
        }

        #endregion

        #region Reset

        private void Reset()
        {
            // Reset Plaza.
            cbPlazas.SelectedIndex = -1;
            LoadPlazaGroups();

            // Setup entry date.
            manager.EntryDate = DateTime.Now;

            // Load Job.
            RefreshJobList();

            // Set Bindings On Tab - Date Selection.
            txtRevDate.DataContext = manager;
            txtEntryDate.DataContext = manager;
            // Set Bindings On Tab - Revenue Entry.
            txtPlazaName.DataContext = manager;
            txtShiftName.DataContext = manager;
            txtRevDate2.DataContext = manager;
            txtUserId2.DataContext = manager;
            txtUserName2.DataContext = manager;
        }

        #endregion

        #region Report methods

        private string ReportDisplayName
        {
            get { return "revenue." + DateTime.Now.ToThaiDateTimeString("ddMMyyyyHHmmssfff"); }
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
                win.Setup("ไม่พบข้อมูลในการจัดพิมพ์รายงาน.", "DMT - Tour of Duty");
                win.ShowDialog();
                this.rptViewer.ClearReport();
            }
            else
            {
                // Set Display Name (default file name).
                model.DisplayName = ReportDisplayName;

                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    this.rptViewer.LoadReport(model);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
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

            MethodBase med = MethodBase.GetCurrentMethod();

            cmdOk.Visibility = Visibility.Collapsed; // Hide button.

            bool hasActivitied = manager.SaveRevenueEntry();

            if (manager.Entry.RevenueDate.HasValue && manager.Entry.RevenueDate.Value != DateTime.MinValue &&
                manager.Entry.EntryDate.HasValue && manager.Entry.EntryDate.Value != DateTime.MinValue)
            {
                try
                {
                    // print reports only date exists.
                    this.rptViewer.Print(ReportDisplayName);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            }

            cmdOk.Visibility = Visibility.Visible; // Show button

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
                    var user = manager.User;
                    Setup(user); // Goback to first page.
                }
                else
                {
                    GotoMainMenu();
                }
            }
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="user"></param>
        public void Setup(User user)
        {
            if (null == manager)
            {
                manager = new RevenueEntryManager();
            }

            tabs.SelectedIndex = 0;
            cmdOk.Visibility = Visibility.Visible;

            _user = user;
            if (null != _user)
            {

            }

            if (null != manager)
            {
                manager.User = null; // Reset User
                manager.ByChief = false;
                manager.User = user;
            }

            Reset();
        }

        #endregion
    }
}
