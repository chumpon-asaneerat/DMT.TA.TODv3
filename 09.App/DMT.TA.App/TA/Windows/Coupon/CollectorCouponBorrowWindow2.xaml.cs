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
    /// Interaction logic for CollectorCouponBorrowWindow2.xaml
    /// </summary>
    public partial class CollectorCouponBorrowWindow2 : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectorCouponBorrowWindow2()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string last40Filter = string.Empty;
        private string last90Filter = string.Empty;

        private TSBCouponBorrowManager manager = null;

        #endregion

        #region Window Handlers

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            Utils.WPFUtils.DisableClose(this); // Disable close button and system menu.
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // if not enable always set cancel to true that mean not allow to close until process finished.
            e.Cancel = (IsEnabled) ? false : true;
        }

        #endregion

        #region Button Handlers

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            if (null != manager)
            {
                waitPanel.Visibility = Visibility.Visible;
                this.IsEnabled = false;

                manager.Save();

                this.IsEnabled = true;
                waitPanel.Visibility = Visibility.Hidden;
            }

            DialogResult = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
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

                    manager.Borrow(item);

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

                    manager.Borrow(item);

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

        private void cmd40StockToUser_Click(object sender, RoutedEventArgs e)
        {
            MoveToUser40();
        }

        private void cmd40UserToStock_Click(object sender, RoutedEventArgs e)
        {
            MoveToStock40();
        }

        private void cmd90StockToUser_Click(object sender, RoutedEventArgs e)
        {
            MoveToUser90();
        }

        private void cmd90UserToStock_Click(object sender, RoutedEventArgs e)
        {
            MoveToStock90();
        }

        #endregion

        #region ListView Handlers

        private void lv40Stock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToUser40();
        }

        private void lv40Stock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToUser40();
                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {

            }
        }

        private void lv40User_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToStock40();
        }

        private void lv40User_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToStock40();
                e.Handled = true;
            }
        }

        private void lv90Stock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToUser90();
        }

        private void lv90Stock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToUser90();
                e.Handled = true;
            }
        }

        private void lv90User_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToStock90();
        }

        private void lv90User_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToStock90();
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

        private void MoveToUser40()
        {
            var items = lv40Stock.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Borrow(item);
            }

            UpadteC40ListViews();
        }

        private void MoveToStock40()
        {
            var items = lv40User.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Return(item);
            }

            UpadteC40ListViews();
        }

        private void MoveToUser90()
        {
            var items = lv90Stock.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Borrow(item);
            }

            UpadteC90ListViews();
        }

        private void MoveToStock90()
        {
            var items = lv90User.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.Return(item);
            }

            UpadteC90ListViews();
        }

        private void UpadteC40ListViews()
        {
            lv40Stock.ItemsSource = null;
            lv40User.ItemsSource = null;

            manager.C40StockFilter = txtCoupon40Filter.Text.Trim();

            lv40Stock.ItemsSource = manager.C40Stocks;
            lv40User.ItemsSource = manager.C40Lanes;
        }

        private void UpadteC90ListViews()
        {
            lv90Stock.ItemsSource = null;
            lv90User.ItemsSource = null;

            manager.C90StockFilter = txtCoupon90Filter.Text.Trim();

            lv90Stock.ItemsSource = manager.C90Stocks;
            lv90User.ItemsSource = manager.C90Lanes;
        }

        private void UpadteListViews()
        {
            UpadteC40ListViews();
            UpadteC90ListViews();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="value">The TSB Coupon Borrow Manager.</param>
        public void Setup(TSBCouponBorrowManager value)
        {
            waitPanel.Visibility = Visibility.Hidden;

            manager = value;
            txtCurrentUserId.DataContext = null;
            txtCurrentUserName.DataContext = null;
            if (null != manager && null != manager.User)
            {
                txtCurrentUserId.DataContext = manager.User;
                txtCurrentUserName.DataContext = manager.User;
                manager.Refresh();
                UpadteListViews();
            }

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
