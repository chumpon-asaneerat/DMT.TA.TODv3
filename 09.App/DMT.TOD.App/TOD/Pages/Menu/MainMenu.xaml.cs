#region Using

using System;
using System.Windows;
using System.Windows.Controls;

using System.Reflection;
using NLib;
using NLib.Services;

using DMT.Models;
using DMT.Models.ExtensionMethods;
using DMT.Services;
using DMT.Windows;

#endregion

namespace DMT.TOD.Pages.Menu
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

        // TEST - PASSED.
        private void cmdCollectorBOJ_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - User Change Shift");

            // เปิดกะ
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Setup(TODApp.Permissions.TC);
            if (signinWin.ShowDialog() == false) return;

            var user = signinWin.User;

            if (null != user)
            {
                // Check User shift.
                var usrshf = UserShift.GetUserShift(TODAPI.TSB.TSBId, user.UserId).Value();
                if (null != usrshf)
                {
                    // write log
                    med.Info("พนักงานเปิดกะแล้ว กรุณาป้อนรายได้ให้เสร็จสิ้นก่อน.");

                    var win = TODApp.Windows.MessageBox;
                    win.Setup("พนักงานเปิดกะแล้ว กรุณาป้อนรายได้ให้เสร็จสิ้นก่อน", "DMT - Tour of Duty");
                    win.ShowDialog();
                    return;
                }
            }

            // Begin of Shift Page
            var jobWindow = TODApp.Windows.BOS;
            jobWindow.Setup(user);
            if (jobWindow.ShowDialog() == false)
            {
                return;
            }
        }

        // TEST - PASSED.
        private void cmdCollectorRevenueEntry_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - Revenue Entry (User)");

            // ป้อนรายได้
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Setup(TODApp.Permissions.TC);
            if (signinWin.ShowDialog() == false) return;

            var user = signinWin.User;

            if (null != user)
            {
                // Check User shift.
                var usrshf = UserShift.GetUserShift(TODAPI.TSB.TSBId, user.UserId).Value();
                if (null == usrshf)
                {
                    // write log
                    med.Info("ไม่พบกะพนักงาน กรุณาเปิดกะก่อนเข้าทำงานที่ช่องทาง.");

                    var win = TODApp.Windows.MessageBox;
                    win.Setup("ไม่พบกะพนักงาน กรุณาเปิดกะก่อนเข้าทำงานที่ช่องทาง", "DMT - Tour of Duty");
                    win.ShowDialog();
                    return;
                }
            }

            // Collector Revenue Entry Page
            var page = TODApp.Pages.CollectorRevenueEntry;
            page.Setup(user);
            PageContentManager.Instance.Current = page;
        }

        // TEST - PASSED.
        private void cmdChiefRevenueEntry_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - Revenue Entry (Chief)");

            // ป้อนรายได้ย้อนหลัง
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Setup(TODApp.Permissions.CTC);
            if (signinWin.ShowDialog() == false) return;

            var user = signinWin.User;

            // Chief Revenue Entry Page
            var page = TODApp.Pages.ChiefRevenueEntry;
            page.Setup(user, true);
            PageContentManager.Instance.Current = page;
        }

        // TEST - PASSED.
        private void cmdChiefChangeShift_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - Chief Change Shift");

            // หัวหน้าเปลี่ยนกะ
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Setup(TODApp.Permissions.CTC);
            if (signinWin.ShowDialog() == false) return;

            var user = signinWin.User;

            // Change Shift Page
            var page = TODApp.Pages.ChangeShift;
            page.Setup(user);
            PageContentManager.Instance.Current = page;
        }

        // TEST - PASSED.
        private void cmdReportMenu_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - Report menu");

            // รายงานต่าง ๆ
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Setup(TODApp.Permissions.CTC);
            if (signinWin.ShowDialog() == false) return;

            var user = signinWin.User;

            // Report Main Menu
            var page = TODApp.Pages.ReportMenu;
            // setup
            page.Setup(user);
            PageContentManager.Instance.Current = page;
        }

        // TEST - PASSED.
        private void cmdEMVQRCode_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - EMV/QR Code");

            // EMV/QR Code
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Setup(TODApp.Permissions.CTC);
            if (signinWin.ShowDialog() == false) return;

            var user = signinWin.User;

            // EMV/QRCode List Page
            var page = TODApp.Pages.EMVQRCode;
            page.Setup(user);
            PageContentManager.Instance.Current = page;
        }

        // TEST - PASSED.
        private void cmdStaffJobs_Click(object sender, RoutedEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            med.Info("==> MENU - Job List.");

            // รายชื่อพนักงานเข้ากะ
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Setup(TODApp.Permissions.CTC);
            if (signinWin.ShowDialog() == false) return;

            var user = signinWin.User;

            // Job List Page
            var page = TODApp.Pages.JobList;
            page.Setup(user);
            PageContentManager.Instance.Current = page;
        }

        #endregion
    }
}
