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
        private JobManager jobMgr = new JobManager(new CurrentTSBManager());

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
            var page = TODApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void RefreshUserShifts()
        {
            lstUsers.ItemsSource = null; // Reset.
            lstUsers.ItemsSource = TODAPI.UnCloseUserShifts;
            lstUsers.SelectedIndex = -1; // Set SelectedIndex will refresh job list.
        }

        private void RefreshJobList(UserShift value)
        {
            lstLaneJobs.ItemsSource = null;
            if (null == value || !value.Begin.HasValue) return; // no selection.
            // Refresh jobs.
            jobMgr.OnlyJobInShift = true;
            jobMgr.UserShift = value;// assign selected user shift.
            jobMgr.Refresh();
            // Bind to ListView
            lstLaneJobs.ItemsSource = jobMgr.AllJobs;
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
                // Load User Shifts.
                RefreshUserShifts();
            }
        }

        #endregion
    }
}
