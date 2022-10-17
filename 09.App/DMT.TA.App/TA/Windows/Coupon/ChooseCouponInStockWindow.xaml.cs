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
    /// Interaction logic for ChooseCouponInStockWindow.xaml
    /// </summary>
    public partial class ChooseCouponInStockWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChooseCouponInStockWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string lastFilter = string.Empty;
        private List<TSBCouponTransaction> _coupons = null;

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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region TextBox Handlers

        private void txtCouponFilter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                var item = HasSingleItem(grid);
                if (null != item)
                {
                    txtCouponFilter.Text = string.Empty;
                    lastFilter = string.Empty;

                    SelectedCoupon = item; // auto select.

                    DialogResult = true; // Close Window.
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                txtCouponFilter.Text = string.Empty;
                lastFilter = string.Empty;
                UpadteListViews();
                e.Handled = true;
            }
            else if (e.Key == Key.Right)
            {
                e.Handled = true;
                // Focus on search textbox.
                txtCouponFilter.SelectAll();
                txtCouponFilter.Focus();
            }
            else
            {
                if (lastFilter != txtCouponFilter.Text)
                {
                    lastFilter = txtCouponFilter.Text;
                    UpadteListViews();
                    e.Handled = true;
                }
            }
        }

        #endregion

        #region Private Methods

        private TSBCouponTransaction HasSingleItem(ListView lv)
        {
            if (null == lv) return null;
            var items = lv.SelectedItems;
            if (null != items && items.Count == 1)
                return items[0] as TSBCouponTransaction;
            return null; // no items or item is more than one.
        }

        private void UpadteListViews()
        {
            grid.ItemsSource = null;

            if (null != _coupons)
            {
                string filter = txtCouponFilter.Text.Trim();
                List<TSBCouponTransaction> items = null;

                if (string.IsNullOrEmpty(filter))
                {
                    items = _coupons;
                }
                else
                {
                    items = _coupons.FindAll(item =>
                    {
                        return item.CouponId.Contains(filter);
                    });
                }


                grid.ItemsSource = items;
            }
        }

        #endregion

        #region Public Methods

        public void Setup(List<TSBCouponTransaction> coupons)
        {
            SelectedCoupon = null;
            _coupons = coupons;
            UpadteListViews();
        }

        #endregion

        #region Public Properties

        public TSBCouponTransaction SelectedCoupon { get; set; }

        #endregion
    }
}
