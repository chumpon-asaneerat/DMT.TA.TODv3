#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using DMT.Configurations;
using DMT.Models;
using DMT.Services;

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
        private CultureInfo culture = new CultureInfo("th-TH");

        private User _user = null;
        private TSB _tsb = null;
        private List<PlazaGroup> _plazaGroups = null;
        private List<Plaza> _plazas = null;

        private DateTime? _entryDate = DateTime.Now;
        private DateTime? _revDate = DateTime.Now;

        private UserShift _userShift = null;
        private UserShift _revenueShift = null;

        private List<LaneJob> _jobs = null;

        #endregion

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Combobox Handlers

        private void cbPlazas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == plazaGroup) return;
            _plazas = Plaza.GetPlazaGroupPlazas(plazaGroup).Value();
            LoadLanes();
        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdGotoRevenueEntry_Click(object sender, RoutedEventArgs e)
        {
            tabs.SelectedIndex = 1; // goto next tab.
        }

        private void cmdBack2_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
            // Used below code if need to go back to select date.
            //GotoBack();
        }

        private void cmdGotoRevenueEntryPreview_Click(object sender, RoutedEventArgs e)
        {
            tabs.SelectedIndex = 2; // goto next tab.
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

        private void LoadLanes()
        {
            var plazaGroup = cbPlazas.SelectedItem as PlazaGroup;
            if (null == plazaGroup || null == _userShift || !_userShift.Begin.HasValue)
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

            var alljobs = new List<LaneJob>();
            // Gets jobs from each plaza on selected UserShift.
            _plazas.ForEach(plaza => 
            {
                // Load job for each user.
                var param = new SCWJobList();
                param.networkId = networkId;
                param.plazaId = plaza.SCWPlazaId;
                param.staffId = _userShift.UserId;

                var ret = scwOps.jobList(param);
                if (null != ret && null != ret.list && ret.list.Count > 0)
                {
                    DateTime currTime = DateTime.Now;
                    ret.list.ForEach(job =>
                    {
                        if (job.bojDateTime.HasValue &&
                            _userShift.Begin.Value <= job.bojDateTime.Value)
                        {
                            alljobs.Add(new LaneJob(job, _userShift));
                        }
                    });

                    // sort and assigned to jobs list.
                    _jobs.AddRange(alljobs.OrderBy(x => x.Begin).ToArray());
                }
            });

            grid.ItemsSource = _jobs;
        }

        private void CheckUserShift()
        {
            _userShift = null;
            if (null != _user)
            {
                _userShift = UserShift.GetUserShift(_user.UserId).Value();
                if (null != _userShift)
                {
                    _revDate = (_userShift.Begin.HasValue) ? _userShift.Begin.Value.Date : new DateTime?();
                    txtRevDate.Text = (_revDate.HasValue) ? _revDate.Value.ToThaiDateTimeString("dd/MM/yyyy") : string.Empty;
                }
                else
                {
                    // Show Message User Shift not found.
                    var msg = TODApp.Windows.MessageBox;
                    msg.Setup("ไม่พบข้อมูลกะของพนักงาน", "DMT - Tour of Duty");
                    msg.ShowDialog();
                }
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
                }
            }
            Reset();
            CheckUserShift();
            LoadLanes();
        }

        #endregion
    }
}
