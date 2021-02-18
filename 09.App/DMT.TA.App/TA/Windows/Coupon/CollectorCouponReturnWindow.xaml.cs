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
    /// Interaction logic for CollectorCouponReturnWindow.xaml
    /// </summary>
    public partial class CollectorCouponReturnWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectorCouponReturnWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string last35Filter = string.Empty;
        private string last80Filter = string.Empty;

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
                this.IsEnabled = false;
                manager.MarkCompleted();
                manager.ReturnToStock();
                manager.Save();
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

        private void txtCoupon35Filter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                var item = HasSingleItem(lv35SoldByLane);
                if (null != item)
                {
                    txtCoupon35Filter.Text = string.Empty;
                    last35Filter = string.Empty;

                    manager.UnsoldByLane(item);

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
                var item = HasSingleItem(lv80SoldByLane);
                if (null != item)
                {
                    txtCoupon80Filter.Text = string.Empty;
                    last80Filter = string.Empty;

                    manager.UnsoldByLane(item);

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

        private void cmd35SoldToOnHand_Click(object sender, RoutedEventArgs e)
        {
            MoveToOnHand35();
        }

        private void cmd35OnHandToSold_Click(object sender, RoutedEventArgs e)
        {
            MoveToSoldByLane35();
        }

        private void cmd80SoldToOnHand_Click(object sender, RoutedEventArgs e)
        {
            MoveToOnHand80();
        }

        private void cmd80OnHandToSold_Click(object sender, RoutedEventArgs e)
        {
            MoveToSoldByLane80();
        }

        #endregion

        #region ListView Handlers

        private void lv35SoldByLane_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToOnHand35();
        }

        private void lv35SoldByLane_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToOnHand35();
                e.Handled = true;
            }
        }

        private void lv35OnLane_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToSoldByLane35();
        }

        private void lv35OnLane_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToSoldByLane35();
                e.Handled = true;
            }
        }

        private void lv80SoldByLane_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToOnHand80();
        }

        private void lv80SoldByLane_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToOnHand80();
                e.Handled = true;
            }
        }

        private void lv80OnLane_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveToSoldByLane80();
        }

        private void lv80OnLane_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                MoveToSoldByLane80();
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

        private void MoveToOnHand35()
        {
            var items = lv35SoldByLane.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.UnsoldByLane(item);
            }

            UpadteC35ListViews();
        }

        private void MoveToSoldByLane35()
        {
            var items = lv35OnLane.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.SoldByLane(item);
            }

            UpadteC35ListViews();
        }

        private void MoveToOnHand80()
        {
            var items = lv80SoldByLane.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.UnsoldByLane(item);
            }

            UpadteC80ListViews();
        }

        private void MoveToSoldByLane80()
        {
            var items = lv80OnLane.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            foreach (TSBCouponItem item in items)
            {
                if (null == item) continue;
                manager.SoldByLane(item);
            }

            UpadteC80ListViews();
        }

        private void UpadteC35ListViews()
        {
            lv35SoldByLane.ItemsSource = null;
            lv35OnLane.ItemsSource = null;

            manager.C35SoldByLaneFilter = txtCoupon35Filter.Text.Trim();

            lv35SoldByLane.ItemsSource = manager.C35UserSolds;
            lv35OnLane.ItemsSource = manager.C35UserOnHands;
        }

        private void UpadteC80ListViews()
        {
            lv80SoldByLane.ItemsSource = null;
            lv80OnLane.ItemsSource = null;

            manager.C80SoldByLaneFilter = txtCoupon80Filter.Text.Trim();

            lv80SoldByLane.ItemsSource = manager.C80UserSolds;
            lv80OnLane.ItemsSource = manager.C80UserOnHands;
        }

        private void UpadteListViews()
        {
            UpadteC35ListViews();
            UpadteC80ListViews();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The TSB Coupon Return Manager.</param>
        public void Setup(TSBCouponReturnManager value)
        {
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
                txtCoupon35Filter.SelectAll();
                txtCoupon35Filter.Focus();
            }));
        }

        #endregion
    }
}
