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

        private RevenueEntryManager manager = new RevenueEntryManager();

        /*
        private UserShift _userShift = null;
        private UserShiftRevenue _revenueShift = null;
        private bool _issNewRevenueShift = false;
        private UserCreditBalance _userCredit = null;
        private Models.RevenueEntry _revenueEntry = null;
        */

        #endregion

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;
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

            /*
            entry.Setup(null, null, null); // Reset Context.

            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == plazaGroup)
            {
            }

            CheckRevenueShift(); // Check Revenue Shift.

            if (null != _revenueShift)
            {
                if (_revenueShift.RevenueDate.HasValue)
                {
                    var win = TODApp.Windows.MessageBox;
                    win.Setup("กะของพนักงานนี้ ถูกป้อนรายได้แล้ว", "DMT - Tour of Duty");
                    win.ShowDialog();
                    return;
                }
                if (_SCWOnline && (_currJobs == null || _currJobs.Count <= 0))
                {
                    // Online but no jobs on current plaza group.
                    var win = TODApp.Windows.MessageBox;
                    win.Setup("ไม่พบข้อมูลเลนที่ยังไม่ถูกป้อนรายได้", "DMT - Tour of Duty");
                    win.ShowDialog();
                    return;
                }
            }
            else
            {
                if (_issNewRevenueShift)
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

            if (!IsReturnBag())
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("ระบบตรวจพบว่ายังไม่มีการคืนถุงเงิน กรุณาคืนถุงเงินก่อนป้อนรายได้.", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            if (!PrepareRevenueEntry())
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("ไม่พบข้อมูลกะรายได้ของพนักงาน หรือไม่พบข้อมูลที่เกี่ยวข้อง", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }
            */
            // All check condition OK.
            tabs.SelectedIndex = 1; // goto next tab.
        }

        private void GotoPrintPreview()
        {
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

            // Slip Preview
            if (!PrepareReport())
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("พบปัญหาในการเตรียมข้อมูลสำหรับจัดพิมพ์", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }
            tabs.SelectedIndex = 2;
        }

        #endregion

        #region Load Masters

        private void LoadPlazaGroups()
        {
            cbPlazas.ItemsSource = null;
            if (null != manager && null != manager.Current)
            {
                cbPlazas.ItemsSource = manager.Current.TSBPlazaGroups;
                if (manager.Current.TSBPlazaGroups.Count > 0) cbPlazas.SelectedIndex = 0;
            }
        }

        private void RefreshJobList()
        {
            grid.ItemsSource = null;

            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == manager || null == manager.UserShift || null == plazaGroup) return;
            // Refresh jobs.
            manager.Jobs.OnlyJobInShift = true;
            manager.Jobs.UserShift = manager.UserShift; // assign current user shift.
            manager.Jobs.PlazaGroup = plazaGroup; // assign selected plaza group.
            manager.Jobs.Refresh();
            // Bind to ListView
            grid.ItemsSource = manager.Jobs.PlazaGroupJobs;
        }

        #endregion

        private void Reset()
        {
            // Reset Plaza.
            cbPlazas.SelectedIndex = -1;
            LoadPlazaGroups();
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






        public void CheckRevenueShift()
        {
            /*
            MethodBase med = MethodBase.GetCurrentMethod();

            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == _userShift || null == plazaGroup) return;
            _issNewRevenueShift = false;
            _revenueShift = UserShiftRevenue.GetPlazaRevenue(_userShift, plazaGroup).Value();
            if (null == _revenueShift)
            {
                string msg = "User Revenue Shift not found. Create New!!.";
                med.Info(msg);

                // Create new if not found.
                _revenueShift = UserShiftRevenue.CreatePlazaRevenue(_userShift, plazaGroup).Value();
                _issNewRevenueShift = true;
            }
            else
            {
                string msg = "User Revenue Shift found.";
                med.Info(msg);
            }
            */
        }

        private string CreateLaneList()
        {
            return (null != manager && null != manager.Jobs) ? manager.Jobs.GetLaneString(false) : string.Empty;
        }

        private bool IsReturnBag()
        {
            bool ret = true;

            // TODO: Need TA.
            /*
            var usrSearch = Search.UserCredits.GetActiveById.Create(
                this.UserShift.UserId, this.PlazaGroup.PlazaGroupId);
            var userCredit = ops.Credits.GetNoRevenueEntryUserCreditBalanceById(usrSearch).Value();
            if (null != userCredit && userCredit.State == UserCreditBalance.StateTypes.Completed)
            {
                ret = true;
            }
            ret = false;
            */
            return ret;
        }

        private bool PrepareRevenueEntry()
        {
            /*
            MethodBase med = MethodBase.GetCurrentMethod();

            txtShiftName.Text = (null != _userShift) ? _userShift.ShiftNameTH : string.Empty;

            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            txtPlazaName.Text = (null != plazaGroup) ? plazaGroup.PlazaGroupNameTH : string.Empty;

            txtUserId2.Text = (null != _userShift) ? _userShift.UserId : string.Empty;
            txtUserName2.Text = (null != _userShift) ? _userShift.FullNameTH : string.Empty;

            if (null == _user || null == _supervisor) return false;
            if (null == _userShift || null == plazaGroup) return false;

            // Create new Revenue Entry.
            _revenueEntry = new Models.RevenueEntry();

            // Is historical
            _revenueEntry.IsHistorical = false;
            // assigned plaza group.
            _revenueEntry.PlazaGroupId = plazaGroup.PlazaGroupId;
            // update object properties.
            plazaGroup.AssignTo(_revenueEntry); // assigned plaza group name (EN/TH)
            _userShift.AssignTo(_revenueEntry); // assigned user shift

            // assigned date after sync object(s) to RevenueEntry.
            _revenueEntry.EntryDate = _entryDate; // assigned Entry date.
            var dtNow = DateTime.Now;
            _revenueEntry.RevenueDate = new DateTime(
                _revDate.Value.Year, _revDate.Value.Month, _revDate.Value.Day,
                dtNow.Hour, dtNow.Minute, dtNow.Second, dtNow.Millisecond);

            // Create Lane list (comma seperate string).
            _revenueEntry.Lanes = CreateLaneList().Trim();

            // Find begin/end of revenue.
            DateTime begin = _userShift.Begin.Value; // Begin time used start of shift.
            DateTime end = DateTime.Now; // End time used printed date

            if (!_revenueEntry.ShiftBegin.HasValue || _revenueEntry.ShiftBegin.Value == DateTime.MinValue)
            {
                _revenueEntry.ShiftBegin = begin;
            }
            if (!_revenueEntry.ShiftEnd.HasValue || _revenueEntry.ShiftEnd == DateTime.MinValue)
            {
                _revenueEntry.ShiftEnd = end;
            }

            // Update Colllector data,
            _revenueEntry.CollectorNameEN = _user.FullNameEN;
            _revenueEntry.CollectorNameTH = _user.FullNameTH;
            // Update Chief data,
            _revenueEntry.SupervisorId = _supervisor.UserId;
            _revenueEntry.SupervisorNameEN = _supervisor.FullNameEN;
            _revenueEntry.SupervisorNameTH = _supervisor.FullNameTH;

            // TODO: Need TA
            // Check User Credit to get BagNo and BeltNo.
            //_userCredit = ops.Credits.GetNoRevenueEntryUserCreditBalanceById(search).Value();
            if (null != _userCredit)
            {
                string msg = string.Format("User Credit found. BagNo: {0}, BeltNo: {1}",
                    _userCredit.BagNo, _userCredit.BeltNo);
                med.Info(msg);

                _revenueEntry.BagNo = _userCredit.BagNo;
                _revenueEntry.BeltNo = _userCredit.BeltNo;
            }
            else
            {
                string msg = "User Credit not found.";
                med.Info(msg);

                _revenueEntry.BagNo = string.Empty;
                _revenueEntry.BeltNo = string.Empty;
            }

            entry.Setup(_revenueEntry, _tsb, _plazas);
            */
            return true;
        }

        private RdlcReportModel GetReportModel()
        {
            Assembly assembly = this.GetType().Assembly;
            RdlcReportModel inst = new RdlcReportModel();
            inst.Definition.EmbededReportName = "DMT.TOD.Reports.RevenueSlip.rdlc";
            inst.Definition.RdlcInstance = RdlcReportUtils.GetEmbededReport(assembly,
                inst.Definition.EmbededReportName);
            // clear reprot datasource.
            inst.DataSources.Clear();
            /*
            List<RevenueEntry> items = new List<RevenueEntry>();
            if (null != _revenueEntry)
            {
                items.Add(_revenueEntry);
            }

            // assign new data source
            RdlcReportDataSource mainDS = new RdlcReportDataSource();
            mainDS.Name = "main"; // the datasource name in the rdlc report.
            mainDS.Items = items; // setup data source
            // Add to datasources
            inst.DataSources.Add(mainDS);

            // Add parameters (if required).
            DateTime today = DateTime.Now;
            string printDate = today.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
            inst.Parameters.Add(RdlcReportParameter.Create("PrintDate", printDate));
            string histText = (null != _revenueEntry && _revenueEntry.IsHistorical) ?
                "(นำส่งย้อนหลัง)" : "";
            inst.Parameters.Add(RdlcReportParameter.Create("HistoryText", histText));
            */
            return inst;
        }
        /// <summary>
        /// Checks all information to build report is loaded.
        /// </summary>
        private bool CanBuildReport
        {
            get
            {
                return false;
                /*
                var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
                return (null != _userShift &&
                    null != plazaGroup &&
                    null != _revenueShift &&
                    null != _currJobs &&
                    null != _revenueEntry);
                */
            }
        }

        private bool PrepareReport()
        {
            if (!CanBuildReport) return false;
            /*
            var model = GetReportModel();
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
            */
            return true;
        }

        private bool SaveRevenueEntry()
        {
            /*
            if (null == _revenueEntry ||
                !_revenueEntry.RevenueDate.HasValue ||
                _revenueEntry.RevenueDate.Value == DateTime.MinValue ||
                !_revenueEntry.EntryDate.HasValue ||
                _revenueEntry.EntryDate.Value == DateTime.MinValue)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("Entry Date or Revenue Date is not set.", "DMT - Tour of Duty");
                win.ShowDialog();
                return false;
            }

            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == plazaGroup || null == _userShift) return false;

            // Save information.
            MethodBase med = MethodBase.GetCurrentMethod();

            if (_revenueEntry.RevenueId == string.Empty)
            {
                // Set Unique ID.
                var unique = UniqueCode.GetUniqueId("RevenueEntry").Value();
                if (string.IsNullOrWhiteSpace(_revenueEntry.RevenueId))
                {
                    string yr = DateTime.Now.ToThaiDateTimeString("yy");
                    string autoId = (null != unique) ? yr + unique.LastNumber.ToString("D5") : string.Empty; // auto generate.
                    _revenueEntry.RevenueId = autoId;
                    UniqueCode.IncreaseUniqueId("RevenueEntry");
                }
            }
            */

            // TODO: Need TA
            /*
            // Set UserCredits's Revenue Id
            var usrSearch = Search.UserCredits.GetActiveById.Create(
                _userShift.UserId, plazaGroup.PlazaGroupId);
            UserCreditBalance userCredit = null;
            userCredit = ops.Credits.GetNoRevenueEntryUserCreditBalanceById(usrSearch).Value();
            userCredit.RevenueId = this.RevenueEntry.RevenueId;
            ops.Credits.SaveUserCreditBalance(userCredit);
            */

            // Save Revenue Entry.
            /*
            var revInst = Models.RevenueEntry.Save(_revenueEntry).Value();
            string revId = (null != revInst) ? revInst.RevenueId : string.Empty;
            if (null != _revenueShift)
            {
                // save revenue shift (for plaza)
                UserShiftRevenue.SavePlazaRevenue(_revenueShift, _revenueEntry.RevenueDate.Value, revId);
            }

            // get all lanes information.
            bool bCloseUserShift = (
                (null == _allJobs && null == _currJobs) ||
                (null != _allJobs && null != _currJobs && _allJobs.Count == _currJobs.Count));

            if (bCloseUserShift)
            {
                // no lane activitie in user shift.
                UserShift.EndUserShift(_userShift);
            }

            // Generte Revenue (declare) File and mark sync status.
            GenerateRevnueFile();

            return !bCloseUserShift;
            */
            return false;
        }

        private void PrintReport()
        {
            /*
            if (null == _revenueEntry)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("Revenue Entry is not found.", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }

            bool hasActivitied = SaveRevenueEntry();

            if (_revenueEntry.RevenueDate.HasValue && _revenueEntry.RevenueDate.Value != DateTime.MinValue &&
                _revenueEntry.EntryDate.HasValue && _revenueEntry.EntryDate.Value != DateTime.MinValue)
            {
                // print reports only date exists.
                this.rptViewer.Print();
            }

            if (!hasActivitied || null == _user)
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
                    Setup(_user); // Goback to first page.
                }
                else
                {
                    GotoMainMenu();
                }
            }
            */
        }

        private void GenerateRevnueFile()
        {
            /*
            if (null == _revenueEntry) return;

            // Generate File.
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                if (null == _revenueEntry) return;

                int networkId = TODConfigManager.Instance.DMT.networkId;

                // Need to sync currency and coupon master!!
                var currencies = MCurrency.GetCurrencies().Value();
                var coupons = MCoupon.GetCoupons().Value();
                var cardAllows = MCardAllow.GetCardAllows().Value();

                var emv = new List<SCWEMVTransaction>();
                if (null != entry.EMVItems)
                {
                    entry.EMVItems.ForEach(item => 
                    {
                        if (null == item.Transaction) return;
                        emv.Add(item.Transaction);
                    });
                }
                var qrCode = new List<SCWQRCodeTransaction>();//RevenueEntryManager.GetQRCodeList(tsb, value, value.RevenueEntry);
                if (null != entry.QRCodeItems)
                {
                    entry.QRCodeItems.ForEach(item =>
                    {
                        if (null == item.Transaction) return;
                        qrCode.Add(item.Transaction);
                    });
                }

                // find lane attendances.
                var jobs = new List<SCWJob>(); //ops.Lanes.GetAttendancesByRevenue(entry).Value();
                _currJobs.ForEach(job => 
                {
                    if (null == job.Job) return;
                    jobs.Add(job.Job);
                });

                int plazaId = (null != _plazas && _plazas.Count > 0) ? _plazas[0].SCWPlazaId : -1;

                if (plazaId == -1)
                {
                    med.Info("declare error: Cannot search plaza id.");
                    return;
                }

                // Create declare json file.
                // send to server
                SCWDeclare declare = _revenueEntry.ToServer(networkId, currencies, coupons, cardAllows,
                    jobs, emv, qrCode, plazaId);
                // send.
                SCWMQService.Instance.WriteQueue(declare);

                // Update local database status.
                _revenueEntry.Status = 1; // generated json file OK.
                _revenueEntry.LastUpdate = DateTime.Now;
                Models.RevenueEntry.Save(_revenueEntry);
            }
            catch (Exception ex)
            {
                med.Err(ex);
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

            if (null != manager)
            {
                manager.ByChief = false;
                manager.User = user;
            }

            Reset();
        }

        #endregion
    }
}
