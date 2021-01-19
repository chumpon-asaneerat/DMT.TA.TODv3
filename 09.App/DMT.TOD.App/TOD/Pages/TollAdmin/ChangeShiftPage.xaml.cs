#region Using

using System;
using System.Collections.Generic;
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
    /// Interaction logic for ChangeShiftPage.xaml
    /// </summary>
    public partial class ChangeShiftPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChangeShiftPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _user = null;
        private List<User> _users = null;
        private TSB _tsb = null;
        private List<Plaza> _plazas = null;
        private List<LaneJob> _jobs = null;

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
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

        private void RefreshLanes()
        {
            lvJobs.ItemsSource = null;
            if (null == _tsb || null == _plazas || _plazas.Count <= 0 || null == _user) return;

            // Read all scw jobs.
            int networkId = TODConfigManager.Instance.DMT.networkId;
            
            if (null == _jobs)
            {
                // Create new job list.
                _jobs = new List<LaneJob>();
            }
            _jobs.Clear();

            // Gets jobs from each plaza.
            _plazas.ForEach(plaza => 
            {
                if (null != _users && _users.Count > 0)
                {
                    _users.ForEach(usr => 
                    {
                        var param = new SCWJobList();
                        param.networkId = networkId;
                        param.plazaId = plaza.SCWPlazaId;
                        param.staffId = usr.UserId;

                        var ret = scwOps.jobList(param);
                        if (null != ret && null != ret.list && ret.list.Count > 0)
                        {
                            ret.list.ForEach(job =>
                            {
                                _jobs.Add(new LaneJob(job, usr));
                            });
                        }
                    });
                }
            });

            lvJobs.ItemsSource = _jobs;
        }

        #endregion

        #region Public Methods

        public void Setup(User user)
        {
            _user = user;
            if (null != _user)
            {
                DateTime dt = DateTime.Now;
                var shifts = Models.Shift.GetShifts().Value();
                cbShifts.ItemsSource = shifts;

                _tsb = TSB.GetCurrent().Value();
                if (null == _tsb) return;
                _plazas = Plaza.GetTSBPlazas(_tsb.TSBId).Value();

                _users = new List<User>();
                var userShifts = UserShift.GetUnCloseUserShifts().Value();
                if (null != userShifts && userShifts.Count > 0)
                {
                    UserCache cache = new UserCache();
                    userShifts.ForEach(userShift => 
                    {
                        if (!cache.Contains(userShift.UserId))
                        {
                            var usr = cache[userShift.UserId];
                            if (null != usr) _users.Add(usr);
                        }
                    });
                }

                // Load related lane data.
                RefreshLanes();
            }
        }

        #endregion
    }
}
