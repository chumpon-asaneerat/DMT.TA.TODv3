#region Using

using System;
using System.Windows;
using System.Windows.Controls;

using NLib.Services;

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

        private void cmdCollectorBOJ_Click(object sender, RoutedEventArgs e)
        {
            // เปิดกะ
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Owner = Application.Current.MainWindow;
            signinWin.Setup(TODApp.Permissions.TC);
            if (signinWin.ShowDialog() == false)
            {
                return;
            }
            var user = signinWin.User;

            // Begin of Shift Page
            var jobWindow = TODApp.Windows.BOS;
            jobWindow.Owner = Application.Current.MainWindow;
            //jobWindow.Setup(user);
            if (jobWindow.ShowDialog() == false)
            {
                return;
            }
        }

        private void cmdCollectorRevenueEntry_Click(object sender, RoutedEventArgs e)
        {
            // ป้อนรายได้
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Owner = Application.Current.MainWindow;
            signinWin.Setup(TODApp.Permissions.TC);
            if (signinWin.ShowDialog() == false)
            {
                return;
            }
            var user = signinWin.User;

            // Collector Revenue Entry Page
            var page = TODApp.Pages.CollectorRevenueEntry;
            //page.Setup(user);
            PageContentManager.Instance.Current = page;
        }

        private void cmdChiefRevenueEntry_Click(object sender, RoutedEventArgs e)
        {
            // ป้อนรายได้ย้อนหลัง
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Owner = Application.Current.MainWindow;
            signinWin.Setup(TODApp.Permissions.CTC);
            if (signinWin.ShowDialog() == false)
            {
                return;
            }
            var user = signinWin.User;

            // Chief Revenue Entry Page
            var page = TODApp.Pages.ChiefRevenueEntry;
            //page.Setup(user);
            PageContentManager.Instance.Current = page;
        }

        private void cmdChiefChangeShift_Click(object sender, RoutedEventArgs e)
        {
            // หัวหน้าเปลี่ยนกะ
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Owner = Application.Current.MainWindow;
            signinWin.Setup(TODApp.Permissions.CTC);
            if (signinWin.ShowDialog() == false)
            {
                return;
            }
            var user = signinWin.User;

            // Change Shift Page
            var page = TODApp.Pages.ChangeShift;
            page.Setup(user);
            PageContentManager.Instance.Current = page;
        }

        private void cmdReportMenu_Click(object sender, RoutedEventArgs e)
        {
            // รายงานต่าง ๆ
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Owner = Application.Current.MainWindow;
            signinWin.Setup(TODApp.Permissions.CTC);
            if (signinWin.ShowDialog() == false)
            {
                return;
            }
            var user = signinWin.User;

            // Report Main Menu
            var page = TODApp.Pages.ReportMenu;
            // setup
            page.Setup(user);
            PageContentManager.Instance.Current = page;
        }

        private void cmdEMVQRCode_Click(object sender, RoutedEventArgs e)
        {
            // EMV/QR Code
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Owner = Application.Current.MainWindow;
            signinWin.Setup(TODApp.Permissions.CTC);
            if (signinWin.ShowDialog() == false)
            {
                return;
            }
            var user = signinWin.User;

            // EMV/QRCode List Page
            var page = TODApp.Pages.EMVQRCode;
            //page.Setup(user);
            PageContentManager.Instance.Current = page;
        }

        private void cmdStaffJobs_Click(object sender, RoutedEventArgs e)
        {
            // รายชื่อพนักงานเข้ากะ
            var signinWin = TODApp.Windows.SignIn;
            signinWin.Owner = Application.Current.MainWindow;
            signinWin.Setup(TODApp.Permissions.CTC);
            if (signinWin.ShowDialog() == false)
            {
                return;
            }
            var user = signinWin.User;

            // Job List Page
            var page = TODApp.Pages.JobList;
            page.Setup(user);
            PageContentManager.Instance.Current = page;
        }

        #endregion
    }
}
