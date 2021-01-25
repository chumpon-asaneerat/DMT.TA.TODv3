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

        #region Interna Variables

        private User _user = null;

        #endregion

        #region Button Handlers

        private void cmdRevenueSlipReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdRevenueSummaryReport_Click(object sender, RoutedEventArgs e)
        {

        }

        // TEST - PASSED.
        private void cmdEmptySlipReport_Click(object sender, RoutedEventArgs e)
        {
            // Revenue Slip (Empty).
            var page = TODApp.Pages.EmptyRevenueSlip;
            page.Setup(_user);
            PageContentManager.Instance.Current = page;
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
            var page = TODApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="user">The User Instance.</param>
        public void Setup(User user)
        {
            _user = user;
        }

        #endregion
    }
}
