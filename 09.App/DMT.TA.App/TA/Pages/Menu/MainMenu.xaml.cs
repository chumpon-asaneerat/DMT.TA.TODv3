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
            var page = TAApp.Pages.RequestExchange;
            //page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void cmdReturnExchange_Click(object sender, RoutedEventArgs e)
        {
            // คืนเงินยืมทอนฝ่ายบัญชี
            var page = TAApp.Pages.ManageExchange;
            //page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void cmdInHouseExchange_Click(object sender, RoutedEventArgs e)
        {
            // แลกเงินหมุนเวียนในด่าน
            var page = TAApp.Pages.InternalExchange;
            //page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void cmdCouponSoldByPlaza_Click(object sender, RoutedEventArgs e)
        {
            // หัวหน่าขายคูปอง
            var page = TAApp.Pages.CouponTSBSale;
            //page.Setup(DMT.Controls.TAApp.User.Current);
            PageContentManager.Instance.Current = page;
        }

        private void cmdCouponSoldHistory_Click(object sender, RoutedEventArgs e)
        {
            // ประวัติการขายคูปอง
            var page = TAApp.Pages.CouponHistoryView;
            //page.Setup();
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
            var page = TAApp.Pages.CollectorCreditManage;
            //page.RefreshPlazaInfo();
            PageContentManager.Instance.Current = page;
        }

        private void cmdUserBorrowCoupon_Click(object sender, RoutedEventArgs e)
        {
            // รับคูปอง (collector)
            var page = TAApp.Pages.ReceiveCoupon;
            //page.Setup(DMT.Controls.TAApp.User.Current);
            PageContentManager.Instance.Current = page;
        }

        private void cmdUserReturnCoupon_Click(object sender, RoutedEventArgs e)
        {
            // คืนคูปอง (collector)
            var page = TAApp.Pages.ReturnCoupon;
            //page.Setup(DMT.Controls.TAApp.User.Current);
            PageContentManager.Instance.Current = page;
        }

        private void cmdUserCreditHistory_Click(object sender, RoutedEventArgs e)
        {
            // ประวัติการแลกเงินยืมทอน (collector)
            var page = TAApp.Pages.CreditHistoryView;
            //page.Setup(DMT.Controls.TAApp.User.Current);
            PageContentManager.Instance.Current = page;
        }

        private void cmdCheckBalance_Click(object sender, RoutedEventArgs e)
        {
            // เช็คยอดด่าน
            var win = TAApp.Windows.PlazaBalanceSummary;
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
