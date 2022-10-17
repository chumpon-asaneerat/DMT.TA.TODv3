#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
using NLib.Reflection;

#endregion

namespace DMT.TA.Pages.Coupon
{
    /// <summary>
    /// Interaction logic for EditSoldCouponPage.xaml
    /// </summary>
    public partial class EditSoldCouponPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public EditSoldCouponPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        private CultureInfo culture = new CultureInfo("th-TH");
        private XmlLanguage language = XmlLanguage.GetLanguage("th-TH");

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

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void cmdC35Swap_Click(object sender, RoutedEventArgs e)
        {
            var cmd = sender as Button;
            var ctx = null != cmd ? cmd.DataContext : null;
            var tran = ctx as TSBCouponTransaction;
            SwapC35(tran);
        }

        private void cmdC35Void_Click(object sender, RoutedEventArgs e)
        {
            var cmd = sender as Button;
            var ctx = null != cmd ? cmd.DataContext : null;
            var tran = ctx as TSBCouponTransaction;
            VoidC35(tran);
        }

        private void cmdC80Swap_Click(object sender, RoutedEventArgs e)
        {
            var cmd = sender as Button;
            var ctx = null != cmd ? cmd.DataContext : null;
            var tran = ctx as TSBCouponTransaction;
            SwapC80(tran);
        }

        private void cmdC80Void_Click(object sender, RoutedEventArgs e)
        {
            var cmd = sender as Button;
            var ctx = null != cmd ? cmd.DataContext : null;
            var tran = ctx as TSBCouponTransaction;
            VoidC80(tran);
        }


        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TAApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void ClearInputs()
        {
            dtSoldDate.Value = DateTime.Today.Date;
            grid35.ItemsSource = null; // clear grid.
            grid80.ItemsSource = null; // clear grid.
        }

        private void Search()
        {
            grid35.ItemsSource = null; // clear grid.
            grid80.ItemsSource = null; // clear grid.

            if (!dtSoldDate.Value.HasValue)
            {
                var win = TAApp.Windows.MessageBox;
                win.Setup("กรุณาเลือกวันที่ขายคูปอง", "Toll Admin");
                win.ShowDialog();
                return;
            }

            RefreshListviews();
        }

        private void SwapC35(TSBCouponTransaction tran)
        {
            if (null == tran) return;

            var coupons = TSBCouponEditManager.GetStockCoupon35s();
            var win = TAApp.Windows.ChooseCouponInStock;
            win.Setup(coupons);
            if (win.ShowDialog() == false)
            {
                return;
            }
            var src = tran;
            var dst = win.SelectedCoupon;
            if (null != src && null != dst)
            {

                RefreshListviews();
            }
        }

        private void SwapC80(TSBCouponTransaction tran)
        {
            if (null == tran) return;

            var coupons = TSBCouponEditManager.GetStockCoupon80s();
            var win = TAApp.Windows.ChooseCouponInStock;
            win.Setup(coupons);
            if (win.ShowDialog() == false)
            {
                return;
            }
            var src = tran;
            var dst = win.SelectedCoupon;
            if (null != src && null != dst)
            {

                RefreshListviews();
            }
        }

        private void VoidC35(TSBCouponTransaction tran)
        {
            if (null == tran) return;

            var win = TAApp.Windows.MessageBoxYesNo;
            string msg = string.Format("ต้องการยกเลิกการขายคูปอง เลขที่: {0} ?", tran.CouponId);
            win.Setup(msg, "Toll Admin");
            if (win.ShowDialog() == false)
            {
                return;
            }
            // Void
            TSBCouponEditManager.VoidCoupon(tran);

            Search(); // refresh
        }

        private void VoidC80(TSBCouponTransaction tran)
        {
            if (null == tran) return;

            var win = TAApp.Windows.MessageBoxYesNo;
            string msg = string.Format("ต้องการยกเลิกการขายคูปอง เลขที่: {0} ?", tran.CouponId);
            win.Setup(msg, "Toll Admin");
            if (win.ShowDialog() == false)
            {
                return;
            }
            // Void
            TSBCouponEditManager.VoidCoupon(tran);

            Search(); // refresh
        }

        private void RefreshListviews()
        {

            grid35.ItemsSource = TSBCouponEditManager.GetSoldCoupon35s(dtSoldDate.Value.Value); // set items to grid.
            grid80.ItemsSource = TSBCouponEditManager.GetSoldCoupon80s(dtSoldDate.Value.Value); // set items to grid.
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            ClearInputs();
            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                dtSoldDate.SelectAll();
                dtSoldDate.Focus();
            }));
        }

        #endregion
    }
}
