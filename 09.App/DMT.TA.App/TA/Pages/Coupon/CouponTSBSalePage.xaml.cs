#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Reflection;

using DMT.Models;
using DMT.Services;

using NLib;
using NLib.Services;
using NLib.Reports.Rdlc;
using NLib.Reflection;
using System.Collections;

#endregion

namespace DMT.TA.Pages.Coupon
{
    /// <summary>
    /// Interaction logic for CouponTSBSalePage.xaml
    /// </summary>
    public partial class CouponTSBSalePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CouponTSBSalePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _chief = null;
        private string last35Filter = string.Empty;
        private string last80Filter = string.Empty;

        private TSBCouponSoldManager manager = null;

        #endregion

        #region Button Handlers

        private void cmdRefresh_Click(object sender, RoutedEventArgs e)
        {
            Resync();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            GotoPrintPreview();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            GotoEditScreen();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintReceipt();
        }

        #endregion

        #region TextBox Handlers

        private void txtCoupon35Filter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                var item = HasSingleItem(lv35Stock);
                if (null != item)
                {
                    txtCoupon35Filter.Text = string.Empty;
                    last35Filter = string.Empty;

                    manager.Sold(item);

                    UpadteC35ListViews();
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                txtCoupon35Filter.Text = string.Empty;
                last35Filter = string.Empty;
                UpadteC35ListViews();
                e.Handled = true;
            }
            else if (e.Key == Key.Right)
            {
                e.Handled = true;

                // Focus on search textbox.
                txtCoupon80Filter.SelectAll();
                txtCoupon80Filter.Focus();
            }
            else
            {
                if (last35Filter != txtCoupon35Filter.Text)
                {
                    last35Filter = txtCoupon35Filter.Text;
                    UpadteC35ListViews();
                    e.Handled = true;
                }
            }
        }

        private void txtCoupon80Filter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                var item = HasSingleItem(lv80Stock);
                if (null != item)
                {
                    txtCoupon80Filter.Text = string.Empty;
                    last80Filter = string.Empty;

                    manager.Sold(item);

                    UpadteC80ListViews();
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                txtCoupon80Filter.Text = string.Empty;
                last80Filter = string.Empty;
                UpadteC80ListViews();
                e.Handled = true;
            }
            else if (e.Key == Key.Left)
            {
                e.Handled = true;

                // Focus on search textbox.
                txtCoupon35Filter.SelectAll();
                txtCoupon35Filter.Focus();
            }
            else
            {
                if (last80Filter != txtCoupon80Filter.Text)
                {
                    last80Filter = txtCoupon80Filter.Text;
                    UpadteC80ListViews();
                    e.Handled = true;
                }
            }
        }

        #endregion

        #region Button (Move) Handlers

        private void cmd35StockToSold_Click(object sender, RoutedEventArgs e)
        {
            MoveToSold35();
        }

        private void cmd35SoldToStock_Click(object sender, RoutedEventArgs e)
        {
            MoveToStock35();
        }

        private void cmd80StockToSold_Click(object sender, RoutedEventArgs e)
        {
            MoveToSold80();
        }

        private void cmd80SoldToStock_Click(object sender, RoutedEventArgs e)
        {
            MoveToStock80();
        }

        #endregion

        #region ListView Handlers

        private void lv35Stock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToSold35();
        }

        private void lv35Stock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToSold35();
                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {

            }
        }

        private void lv35Sold_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MoveToStock35();
        }

        private void lv35Sold_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToStock35();
                e.Handled = true;
            }
        }

        private void lv80Stock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToSold80();
        }

        private void lv80Stock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToSold80();
                e.Handled = true;
            }
        }

        private void lv80Sold_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToStock80();
        }

        private void lv80Sold_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToStock80();
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

        private void GotoPrintPreview()
        {
            var sold35 = lv35Sold.ItemsSource as IList;
            var cnt35 = (null != sold35) ? sold35.Count : 0;
            var sold80 = lv80Sold.ItemsSource as IList;
            var cnt80 = (null != sold80) ? sold80.Count : 0;
            var cntTotal = cnt35 + cnt80;

            if (cnt35 <= 0 && cnt80 <= 0)
            {
                var win = TAApp.Windows.MessageBox;
                win.Setup("กรุณาเลือกคูปองที่ต้องการขาย", "Toll Admin");
                win.ShowDialog();
                return;
            }

            var win2 = TAApp.Windows.MessageBoxYesNo3;
            win2.Setup("ยืนยันการขายคูปอง จำนวน ", cntTotal.ToString("n0"), " เล่ม"
                , "คูปอง 35 บาท = ", cnt35.ToString("n0"), " เล่ม"
                , "คูปอง 80 บาท = ", cnt80.ToString("n0"), " เล่ม"
                , "Toll Admin");
            if (win2.ShowDialog() == false) return;

            tabs.SelectedIndex = 1;

            PreparePreview();
        }

        private void GotoEditScreen()
        {
            tabs.SelectedIndex = 0;
        }

        private void PrintReceipt()
        {
            Print(ReportDisplayName);
        }

        #endregion

        #region Coupon Manage methods

        private TSBCouponItem HasSingleItem(ListView lv)
        {
            if (null == lv) return null;
            var items = lv.ItemsSource as List<TSBCouponItem>;
            if (null != items && items.Count == 1)
                return items[0] as TSBCouponItem;
            return null; // no items or item is more than one.
        }

        private void MoveToSold35()
        {
            var items = lv35Stock.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Sold(item);
            }

            UpadteC35ListViews();
        }

        private void MoveToStock35()
        {
            var items = lv35Sold.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Unsold(item);
            }

            UpadteC35ListViews();
        }

        private void MoveToSold80()
        {
            var items = lv80Stock.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Sold(item);
            }

            UpadteC80ListViews();
        }

        private void MoveToStock80()
        {
            var items = lv80Sold.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Unsold(item);
            }

            UpadteC80ListViews();
        }

        private void UpadteC35ListViews()
        {
            lv35Stock.ItemsSource = null;
            lv35Sold.ItemsSource = null;

            txtC35StockCount.Text = "0";
            txtC35SoldCount.Text = "0";

            manager.C35StockFilter = txtCoupon35Filter.Text.Trim();

            var cStock = manager.C35Stocks;
            var cSold = manager.C35Solds;
            lv35Stock.ItemsSource = cStock;
            lv35Sold.ItemsSource = cSold;

            txtC35StockCount.Text = (null != cStock) ? cStock.Count.ToString("n0") : "0";
            txtC35SoldCount.Text = (null != cSold) ? cSold.Count.ToString("n0") : "0";
        }

        private void UpadteC80ListViews()
        {
            lv80Stock.ItemsSource = null;
            lv80Sold.ItemsSource = null;

            txtC80StockCount.Text = "0";
            txtC80SoldCount.Text = "0";

            manager.C80StockFilter = txtCoupon80Filter.Text.Trim();

            var cStock = manager.C80Stocks;
            var cSold = manager.C80Solds;
            lv80Stock.ItemsSource = cStock;
            lv80Sold.ItemsSource = cSold;

            txtC80StockCount.Text = (null != cStock) ? cStock.Count.ToString("n0") : "0";
            txtC80SoldCount.Text = (null != cSold) ? cSold.Count.ToString("n0") : "0";
        }

        private void UpadteListViews()
        {
            UpadteC35ListViews();
            UpadteC80ListViews();
        }

        #endregion

        #region Print Methods

        private void PreparePreview()
        {
            var model = GetReportModel();
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
            get { return "coupon.receipt." + DateTime.Now.ToThaiDateTimeString("ddMMyyyyHHmmssfff"); }
        }

        private RdlcReportModel GetReportModel()
        {
            Assembly assembly = this.GetType().Assembly;
            RdlcReportModel inst = new RdlcReportModel();
            inst.Definition.EmbededReportName = "DMT.TA.Reports.CouponSalesReceiptRep.rdlc";
            inst.Definition.RdlcInstance = RdlcReportUtils.GetEmbededReport(assembly,
                inst.Definition.EmbededReportName);
            // clear reprot datasource.
            inst.DataSources.Clear();

            var tsb = TSB.GetCurrent().Value();

            // load C35 items.
            List<TSBCouponTransaction> c35Items = new List<TSBCouponTransaction>();
            var c35coupons = (null != manager) ? manager.C35Solds : null;
            if (null != c35coupons)
            {
                c35coupons.ForEach(coupon => {
                    c35Items.Add(coupon.Transaction);
                });

            }
            // load C80 items.
            List<TSBCouponTransaction> c80Items = new List<TSBCouponTransaction>();
            var c80coupons = (null != manager) ? manager.C80Solds : null;
            if (null != c80coupons)
            {
                c80coupons.ForEach(coupon => {
                    c80Items.Add(coupon.Transaction);
                });
            }

            // create and calculate main summary list.
            List<TSBCouponSummary> items = new List<TSBCouponSummary>();

            var _summary = new TSBCouponSummary();
            _summary.UserId = manager.User.UserId;
            _summary.FullNameEN = manager.User.FullNameEN;
            _summary.FullNameTH = manager.User.FullNameTH;
            _summary.TSBId = tsb.TSBId;
            _summary.TSBNameEN = tsb.TSBNameEN;
            _summary.TSBNameTH = tsb.TSBNameTH;

            _summary.CountCouponBHT35 = c35Items.Count;
            _summary.CountCouponBHT80 = c80Items.Count;
            decimal a35 = decimal.Zero;
            c35Items.ForEach(c35 =>
            {
                a35 += c35.Price;
            });
            decimal a80 = decimal.Zero;
            c80Items.ForEach(c80 =>
            {
                a80 += c80.Price;
            });
            _summary.AmountCouponBHT35 = a35;
            _summary.AmountCouponBHT80 = a80;
            if (null != _summary) items.Add(_summary);

            // assign new data source (main for header)
            RdlcReportDataSource mainDS = new RdlcReportDataSource();
            mainDS.Name = "main"; // the datasource name in the rdlc report.
            mainDS.Items = items; // setup data source
            // Add to datasources
            inst.DataSources.Add(mainDS);

            // assign new data source (main for coupon35)
            RdlcReportDataSource c35DS = new RdlcReportDataSource();
            c35DS.Name = "C35"; // the datasource name in the rdlc report.
            c35DS.Items = c35Items; // setup data source
            // Add to datasources
            inst.DataSources.Add(c35DS);

            // assign new data source (main for coupon80)
            RdlcReportDataSource c80DS = new RdlcReportDataSource();
            c80DS.Name = "C80"; // the datasource name in the rdlc report.
            c80DS.Items = c80Items; // setup data source
            // Add to datasources
            inst.DataSources.Add(c80DS);

            // Add parameters (if required).

            // Coupon Received Date.
            DateTime today = DateTime.Today;
            //string couponDate = today.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
            string couponDate = today.ToThaiDateTimeString("dd/MM/yyyy");
            inst.Parameters.Add(RdlcReportParameter.Create("couponDate", couponDate));
            // Supervisor (Current User)
            string supervisorFullName = TAApp.User.Current.FullNameTH;
            inst.Parameters.Add(RdlcReportParameter.Create("supervisorFullName", supervisorFullName));

            // get TSB Coupon Sale invoice running number
            string _runningNumber = manager.InvoiceId;
            inst.Parameters.Add(RdlcReportParameter.Create("runningNumber", _runningNumber));

            return inst;
        }

        private void Print(string documentName)
        {
            this.IsEnabled = false;

            rptViewer.Visibility = Visibility.Hidden;
            waitPanel.Visibility = Visibility.Visible;

            if (null != manager)
            {
                manager.Save(); // Save all.
            }

            // print reports.
            this.rptViewer.Print(documentName);

            // After print.
            Setup(_chief);

            waitPanel.Visibility = Visibility.Hidden;
            rptViewer.Visibility = Visibility.Visible;
            this.IsEnabled = true;
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
        /// Setup
        /// </summary>
        /// <param name="chief">The Current User.</param>
        public void Setup(User chief)
        {
            waitPanel.Visibility = Visibility.Hidden;
            rptViewer.Visibility = Visibility.Visible;

            _chief = chief;
            if (null == manager) manager = new TSBCouponSoldManager();
            if (null != manager)
            {
                manager.SetUser(chief);
                manager.Refresh();
                UpadteListViews();
            }

            tabs.SelectedIndex = 0; // set start tab.

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtCoupon35Filter.SelectAll();
                txtCoupon35Filter.Focus();
            }));
        }

        #endregion
    }
}
