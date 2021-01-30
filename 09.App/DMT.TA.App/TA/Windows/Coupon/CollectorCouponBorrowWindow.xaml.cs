#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.TA.Windows.Coupon
{
    /// <summary>
    /// Interaction logic for CollectorCouponBorrowWindow.xaml
    /// </summary>
    public partial class CollectorCouponBorrowWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectorCouponBorrowWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string last35Filter = string.Empty;
        private string last80Filter = string.Empty;

        private TSBCouponBorrowManager manager = null;

        #endregion

        #region Button Handlers

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
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

                    manager.Borrow(item);

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

                    manager.Borrow(item);

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

        private void cmd35StockToUser_Click(object sender, RoutedEventArgs e)
        {
            MoveToUser35();
        }

        private void cmd35UserToStock_Click(object sender, RoutedEventArgs e)
        {
            MoveToStock35();
        }

        private void cmd80StockToUser_Click(object sender, RoutedEventArgs e)
        {
            MoveToUser80();
        }

        private void cmd80UserToStock_Click(object sender, RoutedEventArgs e)
        {
            MoveToStock80();
        }

        #endregion

        #region ListView Handlers

        private void lv35Stock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToUser35();
        }

        private void lv35Stock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToUser35();
                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {

            }
        }

        private void lv35User_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToStock35();
        }

        private void lv35User_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToStock35();
                e.Handled = true;
            }
        }

        private void lv80Stock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToUser80();
        }

        private void lv80Stock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToUser80();
                e.Handled = true;
            }
        }

        private void lv80User_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToStock80();
        }

        private void lv80User_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToStock80();
                e.Handled = true;
            }
        }

        #endregion

        #region Private Methods

        private TSBCouponItem HasSingleItem(ListView lv)
        {
            if (null == lv) return null;
            var items = lv.ItemsSource as List<TSBCouponItem>;
            if (null != items && items.Count == 1)
                return items[0] as TSBCouponItem;
            return null; // no items or item is more than one.
        }

        private void MoveToUser35()
        {
            var items = lv35Stock.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Borrow(item);
            }
            
            UpadteC35ListViews();
        }

        private void MoveToStock35()
        {
            var items = lv35User.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Return(item);
            }

            UpadteC35ListViews();
        }

        private void MoveToUser80()
        {
            var items = lv80Stock.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Borrow(item);
            }

            UpadteC80ListViews();
        }

        private void MoveToStock80()
        {
            var items = lv80User.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Return(item);
            }

            UpadteC80ListViews();
        }

        private void UpadteC35ListViews()
        {
            lv35Stock.ItemsSource = null;
            lv35User.ItemsSource = null;

            manager.C35StockFilter = txtCoupon35Filter.Text.Trim();

            lv35Stock.ItemsSource = manager.C35Stocks;
            lv35User.ItemsSource = manager.C35Lanes;
        }

        private void UpadteC80ListViews()
        {
            lv80Stock.ItemsSource = null;
            lv80User.ItemsSource = null;

            manager.C80StockFilter = txtCoupon80Filter.Text.Trim();

            lv80Stock.ItemsSource = manager.C80Stocks;
            lv80User.ItemsSource = manager.C80Lanes;
        }

        private void UpadteListViews()
        {
            UpadteC35ListViews();
            UpadteC80ListViews();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="value">The TSB Coupon Borrow Manager.</param>
        public void Setup(TSBCouponBorrowManager value)
        {
            manager = value;
            if (null != manager && null != manager.User)
            {
                manager.Refresh();
                UpadteListViews();
            }

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
