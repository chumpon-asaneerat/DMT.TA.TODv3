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

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
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

        private void Refresh()
        {

        }

        public void Setup(User user)
        {

        }
    }
}
