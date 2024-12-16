#region Using

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
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
    /// Interaction logic for CouponTSBSalePage2.xaml
    /// </summary>
    public partial class CouponTSBSalePage2 : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CouponTSBSalePage2()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        private CultureInfo culture = new CultureInfo("th-TH");
        private XmlLanguage language = XmlLanguage.GetLanguage("th-TH");

        private User _chief = null;
        private string last40Filter = string.Empty;
        private string last90Filter = string.Empty;

        private TSBCouponSoldManager manager = null;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup DateTime Picker
            dtSoldDate.CultureInfo = culture;
            dtSoldDate.Language = language;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

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

        private void txtCoupon40Filter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                var item = HasSingleItem(lv40Stock);
                if (null != item)
                {
                    txtCoupon40Filter.Text = string.Empty;
                    last40Filter = string.Empty;

                    manager.Sold(item);

                    UpadteC40ListViews();
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                txtCoupon40Filter.Text = string.Empty;
                last40Filter = string.Empty;
                UpadteC40ListViews();
                e.Handled = true;
            }
            else if (e.Key == Key.Right)
            {
                e.Handled = true;

                // Focus on search textbox.
                txtCoupon90Filter.SelectAll();
                txtCoupon90Filter.Focus();
            }
            else
            {
                if (last40Filter != txtCoupon40Filter.Text)
                {
                    last40Filter = txtCoupon40Filter.Text;
                    UpadteC40ListViews();
                    e.Handled = true;
                }
            }
        }

        private void txtCoupon90Filter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                var item = HasSingleItem(lv90Stock);
                if (null != item)
                {
                    txtCoupon90Filter.Text = string.Empty;
                    last90Filter = string.Empty;

                    manager.Sold(item);

                    UpadteC90ListViews();
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                txtCoupon90Filter.Text = string.Empty;
                last90Filter = string.Empty;
                UpadteC90ListViews();
                e.Handled = true;
            }
            else if (e.Key == Key.Left)
            {
                e.Handled = true;

                // Focus on search textbox.
                txtCoupon40Filter.SelectAll();
                txtCoupon40Filter.Focus();
            }
            else
            {
                if (last90Filter != txtCoupon90Filter.Text)
                {
                    last90Filter = txtCoupon90Filter.Text;
                    UpadteC90ListViews();
                    e.Handled = true;
                }
            }
        }

        #endregion

        #region Button (Move) Handlers

        private void cmd40StockToSold_Click(object sender, RoutedEventArgs e)
        {
            MoveToSold40();
        }

        private void cmd40SoldToStock_Click(object sender, RoutedEventArgs e)
        {
            MoveToStock40();
        }

        private void cmd90StockToSold_Click(object sender, RoutedEventArgs e)
        {
            MoveToSold90();
        }

        private void cmd90SoldToStock_Click(object sender, RoutedEventArgs e)
        {
            MoveToStock90();
        }

        #endregion

        #region ListView Handlers

        private void lv40Stock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToSold40();
        }

        private void lv40Stock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToSold40();
                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {

            }
        }

        private void lv40Sold_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MoveToStock40();
        }

        private void lv40Sold_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToStock40();
                e.Handled = true;
            }
        }

        private void lv90Stock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToSold90();
        }

        private void lv90Stock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToSold90();
                e.Handled = true;
            }
        }

        private void lv90Sold_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToStock90();
        }

        private void lv90Sold_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToStock90();
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
            if (!dtSoldDate.Value.HasValue)
            {
                var win = TAApp.Windows.MessageBox;
                win.Setup("กรุณาเลือกวันที่ขายคูปอง", "Toll Admin");
                win.ShowDialog();
                return;
            }

            var sold40 = lv40Sold.ItemsSource as IList;
            var cnt40 = (null != sold40) ? sold40.Count : 0;
            var sold90 = lv90Sold.ItemsSource as IList;
            var cnt90 = (null != sold90) ? sold90.Count : 0;
            var cntTotal = cnt40 + cnt90;

            if (cnt40 <= 0 && cnt90 <= 0)
            {
                var win = TAApp.Windows.MessageBox;
                win.Setup("กรุณาเลือกคูปองที่ต้องการขาย", "Toll Admin");
                win.ShowDialog();
                return;
            }

            var win2 = TAApp.Windows.MessageBoxYesNo3;
            win2.Setup("ยืนยันการขายคูปอง จำนวน ", cntTotal.ToString("n0"), " เล่ม"
                , "คูปอง 40 บาท = ", cnt40.ToString("n0"), " เล่ม"
                , "คูปอง 90 บาท = ", cnt90.ToString("n0"), " เล่ม"
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

        private void MoveToSold40()
        {
            var items = lv40Stock.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Sold(item);
            }

            UpadteC40ListViews();
        }

        private void MoveToStock40()
        {
            var items = lv40Sold.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Unsold(item);
            }

            UpadteC40ListViews();
        }

        private void MoveToSold90()
        {
            var items = lv90Stock.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Sold(item);
            }

            UpadteC90ListViews();
        }

        private void MoveToStock90()
        {
            var items = lv90Sold.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Unsold(item);
            }

            UpadteC90ListViews();
        }

        private void UpadteC40ListViews()
        {
            lv40Stock.ItemsSource = null;
            lv40Sold.ItemsSource = null;

            txtC40StockCount.Text = "0";
            txtC40SoldCount.Text = "0";

            manager.C40StockFilter = txtCoupon40Filter.Text.Trim();

            var cStock = manager.C40Stocks;
            var cSold = manager.C40Solds;
            lv40Stock.ItemsSource = cStock;
            lv40Sold.ItemsSource = cSold;

            txtC40StockCount.Text = (null != cStock) ? cStock.Count.ToString("n0") : "0";
            txtC40SoldCount.Text = (null != cSold) ? cSold.Count.ToString("n0") : "0";
        }

        private void UpadteC90ListViews()
        {
            lv90Stock.ItemsSource = null;
            lv90Sold.ItemsSource = null;

            txtC90StockCount.Text = "0";
            txtC90SoldCount.Text = "0";

            manager.C90StockFilter = txtCoupon90Filter.Text.Trim();

            var cStock = manager.C90Stocks;
            var cSold = manager.C90Solds;
            lv90Stock.ItemsSource = cStock;
            lv90Sold.ItemsSource = cSold;

            txtC90StockCount.Text = (null != cStock) ? cStock.Count.ToString("n0") : "0";
            txtC90SoldCount.Text = (null != cSold) ? cSold.Count.ToString("n0") : "0";
        }

        private void UpadteListViews()
        {
            UpadteC40ListViews();
            UpadteC90ListViews();
        }

        #endregion

        #region Sold Date Methods

        public DateTime? GetSoldDate()
        {
            if (!dtSoldDate.Value.HasValue)
            {
                return new DateTime?();
            }
            else
            {
                var soldDate = dtSoldDate.Value.Value;
                var soldTime = DateTime.Now;
                return new DateTime?(new DateTime(soldDate.Year, soldDate.Month, soldDate.Day,
                    soldTime.Hour, soldTime.Minute, soldTime.Second, soldTime.Millisecond));
            }
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

            // Set Sold date
            if (null != manager)
            {
                manager.SoldDate = GetSoldDate();
            }

            var tsb = TSB.GetCurrent().Value();

            // load C40 items.
            List<TSBCouponTransaction> c40Items = new List<TSBCouponTransaction>();
            var c40coupons = (null != manager) ? manager.C40Solds : null;
            if (null != c40coupons)
            {
                c40coupons.ForEach(coupon => {
                    // update sold date to transaction
                    coupon.Transaction.SoldDate = GetSoldDate();
                    c40Items.Add(coupon.Transaction);
                });

            }
            // load C90 items.
            List<TSBCouponTransaction> c90Items = new List<TSBCouponTransaction>();
            var c90coupons = (null != manager) ? manager.C90Solds : null;
            if (null != c90coupons)
            {
                c90coupons.ForEach(coupon => {
                    // update sold date to transaction
                    coupon.Transaction.SoldDate = GetSoldDate();
                    c90Items.Add(coupon.Transaction);
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

            _summary.CountCouponBHT40 = c40Items.Count;
            _summary.CountCouponBHT90 = c90Items.Count;
            decimal a40 = decimal.Zero;
            c40Items.ForEach(c40 =>
            {
                a40 += c40.Price;
            });
            decimal a90 = decimal.Zero;
            c90Items.ForEach(c90 =>
            {
                a90 += c90.Price;
            });
            _summary.AmountCouponBHT40 = a40;
            _summary.AmountCouponBHT90 = a90;
            if (null != _summary) items.Add(_summary);

            // assign new data source (main for header)
            RdlcReportDataSource mainDS = new RdlcReportDataSource();
            mainDS.Name = "main"; // the datasource name in the rdlc report.
            mainDS.Items = items; // setup data source
            // Add to datasources
            inst.DataSources.Add(mainDS);

            // assign new data source (main for coupon35)
            RdlcReportDataSource c40DS = new RdlcReportDataSource();
            c40DS.Name = "C40"; // the datasource name in the rdlc report.
            c40DS.Items = c40Items; // setup data source
            // Add to datasources
            inst.DataSources.Add(c40DS);

            // assign new data source (main for coupon80)
            RdlcReportDataSource c90DS = new RdlcReportDataSource();
            c90DS.Name = "C90"; // the datasource name in the rdlc report.
            c90DS.Items = c90Items; // setup data source
            // Add to datasources
            inst.DataSources.Add(c90DS);

            // Add parameters (if required).

            // Coupon Sold Date.
            var soldDate = GetSoldDate();
            DateTime today = (soldDate.HasValue) ? soldDate.Value : DateTime.Now;

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

            // Set Sold Date to UI.
            dtSoldDate.Value = DateTime.Now;

            _chief = chief;
            if (null == manager) manager = new TSBCouponSoldManager();
            if (null != manager)
            {
                manager.SetUser(chief);
                manager.SoldDate = GetSoldDate(); // Set Sold Date to manager.
                manager.Refresh();
                UpadteListViews();
            }

            tabs.SelectedIndex = 0; // set start tab.

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtCoupon40Filter.SelectAll();
                txtCoupon40Filter.Focus();
            }));
        }

        #endregion
    }
}
