#region Using

using System;
using System.Windows;
using System.Windows.Controls;

using NLib.Services;

#endregion

namespace DMT.TA.Pages.Menu
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

        private void cmdRequestExchange_Click(object sender, RoutedEventArgs e)
        {
            // ยืม/แลก เงินยืมทอนฝ่ายบัญชี
            var page = new Exchange.RequestExchangePage();
            //page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void cmdReturnExchange_Click(object sender, RoutedEventArgs e)
        {
            // คืนเงินยืมทอนฝ่ายบัญชี
            var page = new Exchange.ManageExchangePage();
            //page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void cmdInHouseExchange_Click(object sender, RoutedEventArgs e)
        {
            // แลกเงินหมุนเวียนในด่าน
            var page = new Exchange.InternalExchangePage();
            //page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void cmdCouponSoldByPlaza_Click(object sender, RoutedEventArgs e)
        {
            // หัวหน่าขายคูปอง
            var page = new Coupon.CouponTSBSalePage();
            //page.Setup(DMT.Controls.TAApp.User.Current);
            PageContentManager.Instance.Current = page;
        }

        private void cmdCouponSoldHistory_Click(object sender, RoutedEventArgs e)
        {
            // ประวัติการขายคูปอง
            var page = new Coupon.CouponHistoryViewPage();
            PageContentManager.Instance.Current = page;
        }

        private void cmdCreditTransactionSummaryReport_Click(object sender, RoutedEventArgs e)
        {
            // รายงานสรุปการยืมเงินทอน
            /*
            var page = new Reports.CollectorFundSummaryReportPage();
            page.Setup(DMT.Controls.TAApp.User.Current);
            PageContentManager.Instance.Current = page;
            */
        }

        private void cmdUserCreditManage_Click(object sender, RoutedEventArgs e)
        {
            // เงินยืมทอน (collector)
            var page = new Credit.CollectorCreditManagePage();
            //page.RefreshPlazaInfo();
            PageContentManager.Instance.Current = page;
        }

        private void cmdUserBorrowCoupon_Click(object sender, RoutedEventArgs e)
        {
            // รับคูปอง (collector)
            var page = new Coupon.ReceiveCouponPage();
            PageContentManager.Instance.Current = page;
        }

        private void cmdUserReturnCoupon_Click(object sender, RoutedEventArgs e)
        {
            // คืนคูปอง (collector)
            var page = new Coupon.ReturnCouponPage();
            PageContentManager.Instance.Current = page;
        }

        private void cmdUserCreditHistory_Click(object sender, RoutedEventArgs e)
        {
            // ประวัติการแลกเงินยืมทอน (collector)
            var page = new Credit.CreditHistoryViewPage();
            //page.Setup(DMT.Controls.TAApp.User.Current);
            PageContentManager.Instance.Current = page;
        }

        private void cmdCheckBalance_Click(object sender, RoutedEventArgs e)
        {
            // เช็คยอดด่าน
            var win = new Windows.Plaza.PlazaBalanceSummaryWindow();
            win.Owner = Application.Current.MainWindow;
            //win.RefreshPlazaInfo();
            if (win.ShowDialog() == false)
            {
                return;
            }
        }

        private void cmdExit_Click(object sender, RoutedEventArgs e)
        {
            // ออกจากระบบ
            // When enter Sign In Screen reset current user.
            Controls.TAApp.User.Current = null;

            var page = new DMT.Pages.SignInPage();
            page.Setup(
                "ADMINS",
                "ACCOUNT",
                "CTC_MGR", "CTC", /*"TC",*/
                "MT_ADMIN", "MT_TECH",
                "FINANCE", "SV",
                "RAD_MGR", "RAD_SUP");

            PageContentManager.Instance.Current = page;
        }

        private void cmdSetting_Click(object sender, RoutedEventArgs e)
        {
            // ตั้งค่า
        }

        #endregion
    }
}
