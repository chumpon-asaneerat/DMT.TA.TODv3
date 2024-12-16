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
    /// Interaction logic for CollectorCouponReturnWindow2.xaml
    /// </summary>
    public partial class CollectorCouponReturnWindow2 : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectorCouponReturnWindow2()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string last40Filter = string.Empty;
        private string last90Filter = string.Empty;

        private TSBCouponReturnManager manager = null;

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

                manager.MarkCompleted();
                manager.ReturnToStock();
                manager.Save();

                waitPanel.Visibility = Visibility.Hidden;
                this.IsEnabled = true;
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
                var item = HasSingleItem(lv40SoldByLane);
                if (null != item)
                {
                    txtCoupon40Filter.Text = string.Empty;
                    last40Filter = string.Empty;

                    manager.UnsoldByLane(item);

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
                var item = HasSingleItem(lv90SoldByLane);
                if (null != item)
                {
                    txtCoupon90Filter.Text = string.Empty;
                    last90Filter = string.Empty;

                    manager.UnsoldByLane(item);

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

        private void cmd40SoldToOnHand_Click(object sender, RoutedEventArgs e)
        {
            MoveToOnHand40();
        }

        private void cmd40OnHandToSold_Click(object sender, RoutedEventArgs e)
        {
            MoveToSoldByLane40();
        }

        private void cmd90SoldToOnHand_Click(object sender, RoutedEventArgs e)
        {
            MoveToOnHand90();
        }

        private void cmd90OnHandToSold_Click(object sender, RoutedEventArgs e)
        {
            MoveToSoldByLane90();
        }

        #endregion

        #region ListView Handlers

        private void lv40SoldByLane_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToOnHand40();
        }

        private void lv40SoldByLane_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToOnHand40();
                e.Handled = true;
            }
        }

        private void lv40OnLane_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToSoldByLane40();
        }

        private void lv40OnLane_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToSoldByLane40();
                e.Handled = true;
            }
        }

        private void lv90SoldByLane_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToOnHand90();
        }

        private void lv90SoldByLane_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToOnHand90();
                e.Handled = true;
            }
        }

        private void lv90OnLane_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToSoldByLane90();
        }

        private void lv90OnLane_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToSoldByLane90();
                e.Handled = true;
            }
        }

        #endregion

        #region Private Methods

        private TSBCouponItem HasSingleItem(ListView lv)
        {
            if (null == lv) return null;
            var items = lv.SelectedItems;
            if (null != items && items.Count == 1)
                return items[0] as TSBCouponItem;
            return null; // no items or item is more than one.
        }

        private void MoveToOnHand40()
        {
            var items = lv40SoldByLane.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.UnsoldByLane(item);
            }

            UpadteC40ListViews();
        }

        private void MoveToSoldByLane40()
        {
            var items = lv40OnLane.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.SoldByLane(item);
            }

            UpadteC40ListViews();
        }

        private void MoveToOnHand90()
        {
            var items = lv90SoldByLane.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.UnsoldByLane(item);
            }

            UpadteC90ListViews();
        }

        private void MoveToSoldByLane90()
        {
            var items = lv90OnLane.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.SoldByLane(item);
            }

            UpadteC90ListViews();
        }

        private void UpadteC40ListViews()
        {
            lv40SoldByLane.ItemsSource = null;
            lv40OnLane.ItemsSource = null;

            manager.C40SoldByLaneFilter = txtCoupon40Filter.Text.Trim();

            lv40SoldByLane.ItemsSource = manager.C40UserSolds;
            lv40OnLane.ItemsSource = manager.C40UserOnHands;
        }

        private void UpadteC90ListViews()
        {
            lv90SoldByLane.ItemsSource = null;
            lv90OnLane.ItemsSource = null;

            manager.C90SoldByLaneFilter = txtCoupon90Filter.Text.Trim();

            lv90SoldByLane.ItemsSource = manager.C90UserSolds;
            lv90OnLane.ItemsSource = manager.C90UserOnHands;
        }

        private void UpadteListViews()
        {
            UpadteC40ListViews();
            UpadteC90ListViews();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The TSB Coupon Return Manager.</param>
        public void Setup(TSBCouponReturnManager value)
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
