#region Using

using System;
using System.Windows;
using System.Windows.Controls;

using System.Reflection;
using NLib;
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

        // OK - ยืม/แลก เงินยืมทอนฝ่ายบัญชี
        private void cmdRequestExchange_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - ยืม/แลก เงินยืมทอนฝ่ายบัญชี");
            LogUser(med, TAApp.User.Current); // write current user to log.

            // ยืม/แลก เงินยืมทอนฝ่ายบัญชี
            var page = TAApp.Pages.RequestExchange;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        // OK - คืนเงินยืมทอนฝ่ายบัญชี
        private void cmdReturnExchange_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - คืนเงินยืมทอนฝ่ายบัญชี");
            LogUser(med, TAApp.User.Current); // write current user to log.

            // คืนเงินยืมทอนฝ่ายบัญชี
            var page = TAApp.Pages.ManageExchange;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        // OK - แลกเงินหมุนเวียนในด่าน
        private void cmdInHouseExchange_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - แลกเงินหมุนเวียนในด่าน");
            LogUser(med, TAApp.User.Current); // write current user to log.

            // แลกเงินหมุนเวียนในด่าน
            var page = TAApp.Pages.InternalExchange;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        // OK - หัวหน่าขายคูปอง
        private void cmdCouponSoldByPlaza_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - หัวหน่าขายคูปอง");
            LogUser(med, TAApp.User.Current); // write current user to log.

            // หัวหน่าขายคูปอง
            var page = TAApp.Pages.CouponTSBSale;
            page.Setup(TAApp.User.Current);
            PageContentManager.Instance.Current = page;
        }

        private void cmdCouponSoldHistory_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - ประวัติการขายคูปอง");
            LogUser(med, TAApp.User.Current); // write current user to log.

            // ประวัติการขายคูปอง
            var page = TAApp.Pages.CouponHistoryView;
            page.Setup(TAApp.User.Current);
            PageContentManager.Instance.Current = page;
        }

        private void cmdCreditTransactionSummaryReport_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - รายงานสรุปการยืมเงินทอน");
            LogUser(med, TAApp.User.Current); // write current user to log.

            // รายงานสรุปการยืมเงินทอน
            /*
            var page = new Reports.CollectorFundSummaryReportPage();
            page.Setup(TAApp.User.Current);
            PageContentManager.Instance.Current = page;
            */
        }

        // OK - เงินยืมทอน (collector)
        private void cmdUserCreditManage_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - เงินยืมทอน (collector)");
            LogUser(med, TAApp.User.Current); // write current user to log.

            // เงินยืมทอน (collector)
            var page = TAApp.Pages.CollectorCreditManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        // OK - รับคูปอง (collector)
        private void cmdUserBorrowCoupon_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - รับคูปอง (collector)");
            LogUser(med, TAApp.User.Current); // write current user to log.

            // รับคูปอง (collector)
            var page = TAApp.Pages.ReceiveCoupon;
            page.Setup(TAApp.User.Current);
            PageContentManager.Instance.Current = page;
        }

        // OK - คืนคูปอง (collector)
        private void cmdUserReturnCoupon_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - คืนคูปอง (collector)");
            LogUser(med, TAApp.User.Current); // write current user to log.

            // คืนคูปอง (collector)
            var page = TAApp.Pages.ReturnCoupon;
            page.Setup(TAApp.User.Current);
            PageContentManager.Instance.Current = page;
        }

        private void cmdUserCreditHistory_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - ประวัติการแลกเงินยืมทอน (collector)");
            LogUser(med, TAApp.User.Current); // write current user to log.

            // ประวัติการแลกเงินยืมทอน (collector)
            var page = TAApp.Pages.CreditHistoryView;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        // OK - เช็คยอดด่าน.
        private void cmdCheckBalance_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - เช็คยอดด่าน");
            LogUser(med, TAApp.User.Current); // write current user to log.

            // เช็คยอดด่าน
            var win = TAApp.Windows.PlazaBalanceSummary;
            win.Setup();
            win.ShowDialog();
        }

        // OK - ออกจากระบบ.
        private void cmdExit_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - Exit");
            LogUser(med, TAApp.User.Current); // write current user to log.

            // ออกจากระบบ
            TAApp.User.Current = null; // When enter Sign In Screen reset current user.
            var page = TAApp.Pages.SignIn;
            page.Setup(TAApp.Permissions.CTC);
            PageContentManager.Instance.Current = page;
        }

        private void cmdSetting_Click(object sender, RoutedEventArgs e)
        {
            // ตั้งค่า
        }

        #endregion
    }
}
