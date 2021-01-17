#region Using

using System;
using System.Windows;
using System.Windows.Controls;

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

        #region Button Handlers

        private void cmdCreditAndCouponSummary_Click(object sender, RoutedEventArgs e)
        {
            // TSB Credit and Coupon Summary
        }

        private void cndRequestExchangeHistory_Click(object sender, RoutedEventArgs e)
        {
            // TSB Request Exchange History
        }

        private void cmdTSBBalanceSummary_Click(object sender, RoutedEventArgs e)
        {
            // TSB Balance Summary
        }

        private void cndRequestExchangeManage_Click(object sender, RoutedEventArgs e)
        {
            // TSB Request Exchange Management
        }

        private void cndCouponSoldHistory_Click(object sender, RoutedEventArgs e)
        {
            // Coupon Sold History
        }

        private void cndExit_Click(object sender, RoutedEventArgs e)
        {
            // Exit
            // When enter Sign In Screen reset current user.
            AccountApp.User.Current = null;

            var page = new DMT.Pages.SignInPage();
            page.Setup(
                "ADMINS",
                "ACCOUNT",
                /*"CTC_MGR", "CTC", "TC",*/
                "MT_ADMIN", "MT_TECH",
                "FINANCE", "SV",
                "RAD_MGR", "RAD_SUP");

            PageContentManager.Instance.Current = page;
        }

        #endregion
    }
}
