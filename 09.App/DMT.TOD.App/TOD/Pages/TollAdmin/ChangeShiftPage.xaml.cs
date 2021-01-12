#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.TOD.Pages.TollAdmin
{
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

        #endregion

        #region Public Methods

        public void Setup(User user)
        {
            List<SCWJob> jobs = new List<SCWJob>();
            jobs.Add(new SCWJob() { staffId = "00111", laneId = 1 });
            jobs.Add(new SCWJob() { staffId = "00111", laneId = 2 });
            jobs.Add(new SCWJob() { staffId = "00111", laneId = 3 });
            lvJobs.ItemsSource = jobs;
        }

        #endregion
    }
}
