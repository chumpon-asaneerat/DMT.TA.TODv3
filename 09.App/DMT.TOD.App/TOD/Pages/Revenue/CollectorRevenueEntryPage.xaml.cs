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
using DMT.Services;

using NLib;
using NLib.Services;
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
        private TSB _tsb = null;
        private List<PlazaGroup> _plazaGroups = null;
        private List<Plaza> _TSBPlazas = null;
        private List<Plaza> _plazas = null;

        private DateTime? _entryDate = DateTime.Now;
        private DateTime? _revDate = DateTime.Now;

        private UserShift _userShift = null;
        private UserShiftRevenue _revenueShift = null;
        private bool _issNewRevenueShift = false;

        private bool _SCWOnline = false;
        private List<LaneJob> _allJobs = null;
        private List<LaneJob> _currJobs = null;

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
            if (null == plazaGroup) return;
            LoadTSBJobs();
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
            //GotoBack();
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

        #endregion

        #region Private Methods

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
            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == plazaGroup)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("กรุณาเลือกด่านของรายได้", "DMT - Tour of Duty");
                win.ShowDialog();
                cbPlazas.Focus();
                return;
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

            // TODO: Need TA.
            /*
            if (!_manager.IsReturnBag)
            {
                var win = TODApp.Windows.MessageBox;
                win.Setup("ระบบตรวจพบว่ายังไม่มีการคืนถุงเงิน กรุณาคืนถุงเงินก่อนป้อนรายได้.", "DMT - Tour of Duty");
                win.ShowDialog();
                return;
            }
            */

            // All check condition OK.
            tabs.SelectedIndex = 1; // goto next tab.
        }

        private void GotoPrintPreview()
        {
            tabs.SelectedIndex = 2;
        }

        private void Reset()
        {
            // Reset Plaza.
            cbPlazas.SelectedIndex = -1;
            LoadPlazaGroups();
            // Update entry date and revenue date.
            _entryDate = DateTime.Now;
            _revDate = DateTime.Now;
            txtEntryDate.Text = (_entryDate.HasValue) ? _entryDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss") : string.Empty;
            txtRevDate.Text = (_revDate.HasValue) ? _revDate.Value.ToThaiDateTimeString("dd/MM/yyyy") : string.Empty;

            _userShift = null;
        }

        private void LoadPlazaGroups()
        {
            cbPlazas.ItemsSource = null;
            if (null != _plazaGroups)
            {
                cbPlazas.ItemsSource = _plazaGroups;
                if (_plazaGroups.Count > 0) cbPlazas.SelectedIndex = 0;
            }
        }

        private void LoadTSBJobs()
        {
            if (null == _userShift || !_userShift.Begin.HasValue)
            {
                return;
            }

            int networkId = TODConfigManager.Instance.DMT.networkId;

            // Create new job list.
            if (null == _allJobs) _allJobs = new List<LaneJob>();
            _allJobs.Clear();

            // Gets jobs from each plaza on selected UserShift.
            if (null != _TSBPlazas)
            {
                _TSBPlazas.ForEach(plaza =>
                {
                    // Load job for each user.
                    var param = new SCWJobList();
                    param.networkId = networkId;
                    param.plazaId = plaza.SCWPlazaId;
                    param.staffId = _userShift.UserId;

                    var ret = scwOps.jobList(param);
                    // Checks execute status.
                    _SCWOnline = (null != ret && null != ret.status && ret.status.code == "S200");

                    if (_SCWOnline && null != ret.list && ret.list.Count > 0)
                    {
                        var jobs = new List<LaneJob>();

                        ret.list.ForEach(job =>
                        {
                            if (job.bojDateTime.HasValue &&
                                _userShift.Begin.Value <= job.bojDateTime.Value &&
                                job.plazaId.Value == plaza.SCWPlazaId)
                            {
                                jobs.Add(new LaneJob(job, _userShift));
                            }
                        });

                        // sort and assigned to jobs list.
                        _allJobs.AddRange(jobs.OrderBy(x => x.Begin).ToArray());
                    }
                });

                LoadPlazaGroupJobs();
            }
        }

        private void LoadPlazaGroupJobs()
        {
            grid.ItemsSource = null;

            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            _plazas = (null != plazaGroup) ? Plaza.GetPlazaGroupPlazas(plazaGroup).Value() : null;

            if (null == _currJobs) _currJobs = new List<LaneJob>();
            _currJobs.Clear();

            if (null == _plazas || null == _allJobs || _allJobs.Count <= 0) return;

            if (null != _plazas)
            {
                _plazas.ForEach(plaza => 
                {
                    _allJobs.ForEach(job =>
                    {
                        if (job.PlazaGroupId == plaza.PlazaGroupId)
                        {
                            // Match Selected Plaza Group Id.
                            _currJobs.Add(job);
                        }
                    });
                });
            }

            grid.ItemsSource = _currJobs;
        }

        private void CheckUserShift()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            _userShift = null;
            if (null != _user)
            {
                _userShift = UserShift.GetUserShift(_user.UserId).Value();
                if (null != _userShift)
                {
                    string dt1 = (_userShift.Begin.HasValue) ? _userShift.Begin.Value.ToDateTimeString() : string.Empty;
                    string dt2 = (_userShift.End.HasValue) ? _userShift.End.Value.ToDateTimeString() : string.Empty;

                    string msg = string.Format("User Shift found. Begin: {0}, End {1}", dt1, dt2);
                    med.Info(msg);

                    // Update Revenue Date to UI.
                    _revDate = (_userShift.Begin.HasValue) ? _userShift.Begin.Value.Date : new DateTime?();
                    txtRevDate.Text = (_revDate.HasValue) ? _revDate.Value.ToThaiDateTimeString("dd/MM/yyyy") : string.Empty;
                }
                else
                {
                    string msg = "User Shift not found.";
                    med.Info(msg);

                    // Show Message User Shift not found.
                    var win = TODApp.Windows.MessageBox;
                    win.Setup("ไม่พบข้อมูลกะของพนักงาน", "DMT - Tour of Duty");
                    win.ShowDialog();
                }
            }
        }

        public void CheckRevenueShift()
        {
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
            _user = user;
            if (null != _user)
            {
                _tsb = TSB.GetCurrent().Value();
                if (null != _tsb)
                {
                    _plazaGroups = PlazaGroup.GetTSBPlazaGroups(_tsb).Value();
                    _TSBPlazas = Plaza.GetTSBPlazas(_tsb).Value();
                }
            }
            Reset();
            CheckUserShift();
            LoadTSBJobs();
        }

        #endregion
    }
}
