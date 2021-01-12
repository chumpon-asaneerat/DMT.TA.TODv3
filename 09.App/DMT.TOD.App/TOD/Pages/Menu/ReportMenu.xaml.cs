#region Using

using System;
using System.Windows;
using System.Windows.Controls;

using NLib.Services;

using DMT.Models;
//using DMT.Windows;

#endregion

namespace DMT.TOD.Pages.Menu
{
    /// <summary>
    /// Interaction logic for ReportMenu.xaml
    /// </summary>
    public partial class ReportMenu : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportMenu()
        {
            InitializeComponent();
        }

        #endregion

        private User _user = null;

        #region Button Handlers

        private void cmdRevenueSlipReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdRevenueSummaryReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdEmptySlipReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
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

        public void Setup(User user)
        {
            _user = user;
        }
    }
}
