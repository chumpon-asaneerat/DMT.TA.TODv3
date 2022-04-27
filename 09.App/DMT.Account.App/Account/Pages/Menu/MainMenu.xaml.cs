#region Using

using System;
using System.Windows;
using System.Windows.Controls;

using System.Reflection;
using NLib;
using NLib.Services;

#endregion

namespace DMT.Account.Pages.Menu
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainMenu()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void LogUser(MethodBase med, Models.User user)
        {
            // write signin user to log
            if (null != user)
                med.Info("    - Sign In UserId: '{0}', User Name: '{1}'.", user.UserId, user.FullNameTH);
            else med.Info("    - No Sign In user selected.");
        }

        #endregion

        #region Button Handlers

        private void cmdCreditAndCouponSummary_Click(object sender, RoutedEventArgs e)
        {
            // TSB Credit and Coupon Summary
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - TSB Credit and Coupon Summary");
            LogUser(med, AccountApp.User.Current); // write current user to log.

            var page = AccountApp.Pages.TSBCheckBalancePage;
            page.Setup(AccountApp.User.Current);
            PageContentManager.Instance.Current = page;
        }

        private void cndRequestExchangeManage_Click(object sender, RoutedEventArgs e)
        {
            // TSB Request Exchange Management
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - TSB Request Exchange Management");
            LogUser(med, AccountApp.User.Current); // write current user to log.

            var page = AccountApp.Pages.TSBRequestExchangeView;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void cndRequestExchangeHistory_Click(object sender, RoutedEventArgs e)
        {
            // TSB Request Exchange History
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - TSB Request Exchange History");
            LogUser(med, AccountApp.User.Current); // write current user to log.

            var page = AccountApp.Pages.TSBExchangeHistoryView;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void cmdTSBBalanceSummary_Click(object sender, RoutedEventArgs e)
        {
            // TSB Balance Summary
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - TSB Balance Summary");
            LogUser(med, AccountApp.User.Current); // write current user to log.
        }

        private void cndCouponSoldHistory_Click(object sender, RoutedEventArgs e)
        {
            // Coupon History
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - Coupon History");
            LogUser(med, AccountApp.User.Current); // write current user to log.

            var page = AccountApp.Pages.CouponHistoryView;
            page.Setup(AccountApp.User.Current);
            PageContentManager.Instance.Current = page;
        }


        private void cndSendDataToSAP_Click(object sender, RoutedEventArgs e)
        {
            // SAP Send Coupon Sold.
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - Coupon Sold (from SAP)");
            LogUser(med, AccountApp.User.Current); // write current user to log.

            var page = AccountApp.Pages.SAPSendCouponSoldView;
            page.Setup(AccountApp.User.Current);
            PageContentManager.Instance.Current = page;
        }

        private void cndExit_Click(object sender, RoutedEventArgs e)
        {
            // Exit
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - EXIT");
            LogUser(med, AccountApp.User.Current); // write current user to log.

            // When enter Sign In Screen reset current user.
            AccountApp.User.Current = null;

            var page = AccountApp.Pages.SignIn;
            page.Setup(AccountApp.Permissions.Account);
            PageContentManager.Instance.Current = page;
        }

        #endregion
    }
}
