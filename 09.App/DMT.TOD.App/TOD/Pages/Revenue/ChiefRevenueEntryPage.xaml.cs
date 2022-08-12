#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

using DMT.Configurations;
using DMT.Controls;
using DMT.Models;
using DMT.Services;

using NLib;
using NLib.Services;
using NLib.Reflection;
using System.Windows.Threading;
using System.Reflection;
using System.Threading.Tasks;

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
        private XmlLanguage language = XmlLanguage.GetLanguage("th-TH");

        private RevenueEntryManager manager = null;

        private User _chief = null; // Supervisor

        #endregion

        #region Loaded/Unload

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup DateTime Picker.
            dtEntryDate.CultureInfo = culture;
            dtEntryDate.Language = language;
            dtRevDate.CultureInfo = culture;
            dtRevDate.Language = language;
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
            Setup(_chief, false); // Reload.
        }

        #endregion

        #region Combobox Handlers

        private void cbShifts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var shift = cbShifts.SelectedItem as Models.Shift;
            if (null == shift) return; // No Selection.
            // Set Current Shift.
            if (null != manager.Current)
            {
                manager.Current.Shift = shift;
                // Need to call for re create user shift based on selected shift.
                manager.UserShifts.UpdateUserShift();
            }

            RefreshJobList();
        }

        private void cbPlazas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == plazaGroup) return; // No Selection.
            // Set Current Plaza Group.
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
                RefreshJobList();
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

            if (!dtRevDate.Value.HasValue)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("กรุณาเลือกวันที่ของรายได้", "DMT - Tour of Duty");
                win.ShowDialog();
                dtRevDate.Focus();
                return;
            }

            if (dtRevDate.Value.HasValue && dtRevDate.Value.Value.Date > DateTime.Now.Date)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("ไม่อณุญาตให้ป้อนรายได้ล่วงหน้า กรุณาเลือกวันที่ของรายได้ใหม่", "DMT - Tour of Duty");
                win.ShowDialog();
                dtRevDate.Focus();
                return;
            }
            // Setup Revenue Date.
            manager.RevenueDate = dtRevDate.Value;

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
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("[<<<<   START PRINT PREVIEW   >>>>]");
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

            #region Check Valid and Duplicatate BagNo, BeltNo

            if (!entry.IsValidBagNo)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("หมายเลขถุงเงิน ต้องเป็นตัวเลขเท่านั้น กรุณาตรวจสอบข้อมูล", "DMT - Tour of Duty");
                win.ShowDialog();

                entry.FocusBagNo();

                return;
            }
            if (!entry.IsValidBeltNo)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("หมายเลขเข็มขัดนิรภัย ต้องเป็นตัวเลขเท่านั้น กรุณาตรวจสอบข้อมูล", "DMT - Tour of Duty");
                win.ShowDialog();

                entry.FocusBeltNo();

                return;
            }

            if (entry.HasDuplicateBagNo)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("ไม่สามารถใช้ หมายเลขถุงเงินซ้ำ ภายในวันเดียวกัน กรุณาเปลี่ยนเลขใหม่", "DMT - Tour of Duty");
                win.ShowDialog();

                entry.FocusBagNo();

                return;
            }
            if (entry.HasDuplicateBeltNo)
            {
                var win = TODApp.Windows.MessageBox;
                var oldHt = win.Height;
                win.Height += 50;
                win.Setup("ไม่สามารถใช้ หมายเลขเข็มขัดนิรภัยซ้ำ ภายในวันเดียวกัน กรุณาเปลี่ยนเลขใหม่", "DMT - Tour of Duty");
                win.ShowDialog();
                win.Height = oldHt;

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

            med.Info("[<<<<   END PRINT PREVIEW   >>>>]");

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
            RefreshJobList();
        }

        private void LoadPlazaGroups()
        {
            cbPlazas.ItemsSource = null;
            if (null != manager && null != manager.Current)
            {
                cbPlazas.ItemsSource = manager.Current.TODPlazaGroups;
                if (null != manager.Current.TODPlazaGroups && manager.Current.TODPlazaGroups.Count > 0)
                {
                    cbPlazas.SelectedIndex = 0;
                }
            }
            RefreshJobList();
        }

        private void RefreshJobList()
        {
            grid.ItemsSource = null;

            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;

            if (null == manager || null == manager.Jobs || null == manager.UserShifts || null == plazaGroup) return;

            // Refresh jobs.
            manager.Jobs.ViewMode = ViewModes.TOD; // Show by PlazaId in TOD config Only
            manager.Jobs.OnlyJobInShift = false; // Show all jobs
            manager.Jobs.UserShift = manager.UserShifts.Create();
            manager.Jobs.PlazaGroup = plazaGroup; // assign selected plaza group.
            manager.Jobs.Refresh();

            grid.ItemsSource = manager.Jobs.PlazaGroupJobs;
        }

        #endregion

        #region Reset

        private void Reset(bool isNew)
        {
            // Reset Bindings On Tab - Date Selection.
            dtEntryDate.DataContext = null;
            txtUserId.DataContext = null;
            txtUserName.DataContext = null;
            // Reset Bindings On Tab - Revenue Entry.
            txtPlazaName.DataContext = null;
            txtShiftName.DataContext = null;
            txtRevDate2.DataContext = null;
            txtUserId2.DataContext = null;
            txtUserName2.DataContext = null;

            // Reset Plaza.
            cbPlazas.SelectedIndex = -1;
            LoadPlazaGroups();
            // Reset Shift.
            cbShifts.SelectedIndex = -1;
            LoadShifts();

            // Setup entry date.
            manager.EntryDate = DateTime.Now;
            if (isNew)
            {
                manager.RevenueDate = manager.EntryDate;
            }
            else
            {
                if (!manager.RevenueDate.HasValue) manager.RevenueDate = manager.EntryDate;
            }

            // Set Bindings On Tab - Date Selection.
            dtEntryDate.DataContext = manager;
            //dtRevDate.DataContext = manager;
            dtRevDate.Value = DateTime.Now.Date;

            txtUserId.DataContext = manager;
            txtUserName.DataContext = manager;
            // Set Bindings On Tab - Revenue Entry.
            txtPlazaName.DataContext = manager;
            txtShiftName.DataContext = manager;
            txtRevDate2.DataContext = manager;
            txtUserId2.DataContext = manager;
            txtUserName2.DataContext = manager;

            TODApp.Chief.Current = null; // clear chief.
            if (null != _chief)
            {
                TODApp.Chief.Current = _chief; // assign current chief.
            }
        }

        #endregion

        #region User related methods

        private void ResetSelectUser()
        {
            if (null != manager) manager.User = null;
            txtSearchUserId.Text = string.Empty;
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
                RefreshJobList();
            }
        }

        #endregion

        #region Report methods

        private string ReportDisplayName
        {
            get { return "revenue." + DateTime.Now.ToThaiDateTimeString("ddMMyyyyHHmmssfff"); }
        }

        private bool PrepareReport()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            if (null == manager || !manager.CanBuildRevenueSlipReport) return false;

            med.Info("<<<<<<<<<  Generate Revenue Slip Report Model  >>>>>>>>>");
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

                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        this.rptViewer.LoadReport(model);
                    }, DispatcherPriority.Background);
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
                //win.Setup("กะปัจจุบันยังป้อนรายได้ไม่ครบ ต้องการป้อนรายได้ต่อหรือไม่ ?", "DMT - Tour of Duty");
                win.Setup("พบ Job การทำงาน ของพนักงาน ที่ยังไม่ได้นำส่งรายได้\r\nต้องการนำส่งรายได้ ต่อ หรือ ไม่", "DMT - Tour of Duty");
                if (win.ShowDialog() == true)
                {
                    string userId = (null != manager && null != manager.User) ? manager.User.UserId : string.Empty;
                    Setup(_chief, false); // Goback to first page.
                    // Assign User
                    txtSearchUserId.Text = userId;
                    SearchUser();
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
        /// <param name="chief">The chief user.</param>
        /// <param name="isNew">Is New Entry. If true Revenue Date will reset otherwise the user select date will used.</param>
        public void Setup(User chief, bool isNew)
        {
            if (null == manager)
            {
                manager = new RevenueEntryManager();
            }

            tabs.SelectedIndex = 0;
            cmdOk.Visibility = Visibility.Visible;

            _chief = chief;

            if (null != manager)
            {
                manager.User = null; // Reset.
                manager.ByChief = true;
                // Refresh.
                manager.Refresh();

                if (null != manager.UserShifts) manager.UserShifts.IsCustom = true;
            }
            Reset(isNew);

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
