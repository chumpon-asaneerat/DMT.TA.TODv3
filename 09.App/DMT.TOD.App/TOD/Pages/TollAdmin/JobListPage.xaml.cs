#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using DMT.Configurations;
using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.TOD.Pages.TollAdmin
{
    using scwOps = Services.Operations.SCW.TOD;

    /// <summary>
    /// Interaction logic for JobListPage.xaml
    /// </summary>
    public partial class JobListPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public JobListPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _user = null;
        private List<UserShift> _usershifts = null;
        private TSB _tsb = null;
        private List<Plaza> _plazas = null;
        private List<Lane> _lanes = null;
        private List<LaneJob> _jobs = null;

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshUserShifts();
        }

        #endregion

        #region ListView Handlers

        private void lstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = lstUsers.SelectedItem as UserShift;
            RefreshJobList(item);
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = new Menu.MainMenu();
            PageContentManager.Instance.Current = page;
        }

        private void RefreshUserShifts()
        {
            lstUsers.ItemsSource = null;

            // Gets User Shifts that not closed.
            _usershifts = UserShift.GetUnCloseUserShifts().Value();

            lstUsers.ItemsSource = _usershifts;

            lstUsers.SelectedIndex = -1; // Set SelectedIndex will refresh job list.
        }

        private void RefreshJobList(UserShift userShift)
        {
            lstLaneJobs.ItemsSource = null;
            if (null == userShift) return; // no selection.

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
                param.staffId = userShift.UserId;

                var ret = scwOps.jobList(param);
                if (null != ret && null != ret.list && ret.list.Count > 0)
                {
                    ret.list.ForEach(job =>
                    {
                        // Maps Lanes to get access more info for binding.
                        // Note: SCW return only laneId so its cannot display more information so we need to map on 
                        // local lane data.
                        var matchLane = _lanes.Find(lane =>
                        {
                            return job.plazaId == lane.SCWPlazaId && job.laneId == lane.LaneNo;
                        });
                        if (null != matchLane)
                        {
                            alljobs.Add(new LaneJob(job, matchLane, userShift));
                        }
                    });
                }
            });

            // sort and assigned to jobs list.
            _jobs.AddRange(alljobs.OrderBy(x => x.Begin).ToArray());

            lstLaneJobs.ItemsSource = _jobs;
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
                // Get Current TSB and related plazas, lanes.
                _tsb = TSB.GetCurrent().Value();
                if (null == _tsb) return;
                _plazas = Plaza.GetTSBPlazas(_tsb.TSBId).Value();
                _lanes = Lane.GetTSBLanes(_tsb.TSBId).Value();

                // Load User Shifts.
                RefreshUserShifts();
            }
        }

        #endregion
    }
}
