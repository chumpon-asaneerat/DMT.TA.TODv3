#region Using

using System;
using System.Windows;
using System.Windows.Controls;

using System.Reflection;
using NLib;
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

        // TEST - PASSED.
        private void cmdRevenueSlipReport_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - Revenue Slip Report");

            var win = TODApp.Windows.RevenueSlipSearch;
            win.Setup(_user);
            if (win.ShowDialog() == false) return;
            if (win.SelectedEntry == null)
            {
                // write log
                med.Info("ไม่พบรายการ ในวันที่เลือก.");

                // No Selected Entry.
                var msg = TODApp.Windows.MessageBox;
                msg.Setup("ไม่พบรายการ ในวันที่เลือก", "DMT - Tour of Duty");
                msg.ShowDialog();
                return;
            }
            var page = TODApp.Pages.RevenueSlipPreview;
            page.Setup(_user, win.SelectedEntry);
            PageContentManager.Instance.Current = page;
        }

        // TEST - PASSED.
        private void cmdRevenueSummaryReport_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - Revenue Daily Summary Report");

            var win = TODApp.Windows.RevenueSummarySearch;
            win.Setup(_user);
            if (win.ShowDialog() == false) return;
            if (win.Revenues == null)
            {
                // write log
                med.Info("    - ไม่พบรายการ ในวันที่เลือก.");

                // No Result found.
                var msg = TODApp.Windows.MessageBox;
                msg.Setup("ไม่พบรายการ ในวันที่เลือก", "DMT - Tour of Duty");
                msg.ShowDialog();
                return;
            }
            var page = TODApp.Pages.DailyRevenueSummaryPreview;
            page.Setup(_user, win.Revenues);
            PageContentManager.Instance.Current = page;
        }

        // TEST - PASSED.
        private void cmdEmptySlipReport_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - Empty Revenue Slip Report");

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
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - MAIN MENU");

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
