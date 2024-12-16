#region Using

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using DMT.Models;
using DMT.Services;

using NLib;
using NLib.Services;
using NLib.Reports.Rdlc;
using NLib.Reflection;

#endregion

namespace DMT.TA.Pages.Coupon
{
    /// <summary>
    /// Interaction logic for ReceiveCouponPage2.xaml
    /// </summary>
    public partial class ReceiveCouponPage2 : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReceiveCouponPage2()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _chief = null;
        private TSBCouponBorrowManager manager = null;

        #endregion

        #region Button Handlers

        private void cmdRefresh_Click(object sender, RoutedEventArgs e)
        {
            Resync();
        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdUserSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchUser();
        }

        private void cmdAppend_Click(object sender, RoutedEventArgs e)
        {
            AppendUser();
        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);
            var tran = (null != button) ? button.DataContext as TSBCouponSummary : null;
            if (null == manager || null == tran) return;
            var user = User.GetByUserId(tran.UserId).Value();
            if (null == user) return;
            manager.SetUser(user);
            BorrownCoupon();
        }

        private void cmdPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);
            var item = (null != button) ? button.DataContext as TSBCouponSummary : null;
            if (null == item) return;

            GotoPrintPreview(item);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelPreview();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Print(ReportDisplayName);
        }

        #endregion

        #region TextBox Handlers

        private void txtSearchUserId_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter ||
                e.Key == System.Windows.Input.Key.Return)
            {
                SearchUser();
                e.Handled = true;
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                ResetSelectUser();
                RefreshCoupons();
                e.Handled = true;
            }
        }

        #endregion

        #region Private Methods

        #region Navigate methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TAApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void GotoPrintPreview(TSBCouponSummary item)
        {
            if (null == item) return;

            PreparePreview(item);
        }

        private void CancelPreview()
        {
            Setup(_chief);
        }

        #endregion

        #region Reset/User methods

        private void Reset()
        {
            if (null == manager) manager = new TSBCouponBorrowManager();
            manager.SetUser(null);
            if (null != manager)
            {

            }

            txtToday.Text = DateTime.Now.ToThaiDateTimeString("yyyy/MM/dd HH:mm");
            // Set Bindings User Selection.
            txtUserId.DataContext = manager;
            txtUserName.DataContext = manager;
        }

        private void ResetSelectUser()
        {
            if (null != manager) manager.SetUser(null);
            txtSearchUserId.Text = string.Empty;
            cmdAppend.IsEnabled = false;
        }

        private void SearchUser()
        {
            string userId = txtSearchUserId.Text.Trim();
            var result = TAAPI.SearchUser(userId, TAApp.Permissions.TC);
            if (!result.IsCanceled && null != manager)
            {
                manager.SetUser(result.User);
                if (null != manager.User)
                {
                    txtSearchUserId.Text = string.Empty;
                }
                RefreshCoupons();
            }

            bool hasUser = (null != manager && null != manager.User);
            cmdAppend.IsEnabled = hasUser;
        }

        private void AppendUser()
        {
            if (null == manager || null == manager.User) return;
            var win = TAApp.Windows.CollectorCouponBorrow2;
            win.Setup(manager);
            win.ShowDialog();
            Reset();
            ResetSelectUser();
            RefreshCoupons();
        }

        #endregion

        #region Coupon Manage methods

        private void BorrownCoupon()
        {
            if (null == manager || null == manager.User) return;
            var win = TAApp.Windows.CollectorCouponBorrow2;
            win.Setup(manager);
            win.ShowDialog();
            Reset();
            ResetSelectUser();
            RefreshCoupons();
        }

        private void RefreshCoupons()
        {
            grid.ItemsSource = null;
            var summaries = TSBCouponSummary.GetTSBCouponSummaries(TAAPI.TSB).Value();
            if (null != summaries)
            {
                grid.ItemsSource = summaries;
            }
        }

        #endregion

        #region Print Methods

        private void PreparePreview(TSBCouponSummary item)
        {
            if (null == item) return; // No Item Selected

            tabs.SelectedIndex = 1;
            var model = GetReportModel(item);
            if (null == model ||
                null == model.DataSources || model.DataSources.Count <= 0 ||
                null == model.DataSources[0] || null == model.DataSources[0].Items)
            {
                var win = TAApp.Windows.MessageBox;
                win.Setup("ไม่พบข้อมูลในการจัดพิมพ์รายงาน.", "DMT - Toll Admin");
                win.ShowDialog();

                this.rptViewer.ClearReport();
            }
            else
            {
                this.rptViewer.LoadReport(model);
            }
        }

        private string ReportDisplayName
        {
            get { return "coupon." + DateTime.Now.ToThaiDateTimeString("ddMMyyyyHHmmssfff"); }
        }

        private RdlcReportModel GetReportModel(TSBCouponSummary summary)
        {
            Assembly assembly = this.GetType().Assembly;
            RdlcReportModel inst = new RdlcReportModel();

            // Set Display Name (default file name).
            inst.DisplayName = ReportDisplayName;

            inst.Definition.EmbededReportName = "DMT.TA.Reports.CollectorCouponReceived.rdlc";
            inst.Definition.RdlcInstance = RdlcReportUtils.GetEmbededReport(assembly,
                inst.Definition.EmbededReportName);
            // clear reprot datasource.
            inst.DataSources.Clear();

            List<TSBCouponSummary> items = new List<TSBCouponSummary>();
            if (null != summary) items.Add(summary);

            // gets coupon list by type.
            var user = User.GetByUserId(summary.UserId).Value();

            TSBCouponBorrowManager _manager = new TSBCouponBorrowManager();
            if (null != user)
            {
                _manager.SetUser(user);
                _manager.Refresh(); // reload data.
            }

            // load C40 items.
            List<TSBCouponTransaction> c40Items = new List<TSBCouponTransaction>();
            var c40coupons = _manager.C40Lanes;
            if (null != c40coupons)
            {
                c40coupons.ForEach(coupon => {
                    c40Items.Add(coupon.Transaction);
                });
            }

            // load C90 items.
            List<TSBCouponTransaction> c90Items = new List<TSBCouponTransaction>();
            var c90coupons = _manager.C90Lanes;
            if (null != c90coupons)
            {
                c90coupons.ForEach(coupon => {
                    c90Items.Add(coupon.Transaction);
                });
            }

            // assign new data source (main for header)
            RdlcReportDataSource mainDS = new RdlcReportDataSource();
            mainDS.Name = "main"; // the datasource name in the rdlc report.
            mainDS.Items = items; // setup data source
            // Add to datasources
            inst.DataSources.Add(mainDS);

            // assign new data source (main for coupon35)
            RdlcReportDataSource c40DS = new RdlcReportDataSource();
            c40DS.Name = "C40"; // the datasource name in the rdlc report.
            c40DS.Items = c90Items; // setup data source
            // Add to datasources
            inst.DataSources.Add(c40DS);

            // assign new data source (main for coupon80)
            RdlcReportDataSource c90DS = new RdlcReportDataSource();
            c90DS.Name = "C90"; // the datasource name in the rdlc report.
            c90DS.Items = c90Items; // setup data source
            // Add to datasources
            inst.DataSources.Add(c90DS);

            // Add parameters (if required).
            // Coupon Received Date.
            DateTime today = DateTime.Today;
            //string couponDate = today.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
            string couponDate = today.ToThaiDateTimeString("dd/MM/yyyy");
            inst.Parameters.Add(RdlcReportParameter.Create("couponDate", couponDate));
            // Supervisor (Current User)
            string supervisorFullName = TAApp.User.Current.FullNameTH;
            inst.Parameters.Add(RdlcReportParameter.Create("supervisorFullName", supervisorFullName));

            return inst;
        }

        private void Print(string documentName)
        {
            // print reports.
            this.rptViewer.Print(documentName);
            Setup(_chief); // Go back to tab 1
        }

        #endregion

        #region Resync

        private void Resync()
        {
            CouponSyncService.Instance.ReSyncAll();
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup/
        /// </summary>
        /// <param name="chief">The current user.</param>
        public void Setup(User chief)
        {
            _chief = chief;
            if (null != _chief)
            {

            }

            tabs.SelectedIndex = 0;

            Reset();
            ResetSelectUser();
            RefreshCoupons();

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtSearchUserId.SelectAll();
                txtSearchUserId.Focus();
            }));
        }

        #endregion
    }
}
