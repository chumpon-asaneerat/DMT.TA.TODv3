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

        private TSBCouponReturnManager manager = null;

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

        private void txtCoupon35Filter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                UpadteC35ListViews();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                txtCoupon35Filter.Text = string.Empty;
                UpadteC35ListViews();
                e.Handled = true;
            }
        }

        private void txtCoupon80Filter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                UpadteListViews();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                txtCoupon80Filter.Text = string.Empty;
                UpadteC80ListViews();
                e.Handled = true;
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

        private void MoveToOnHand35()
        {
            var items = lv35SoldByLane.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            UpadteC35ListViews();
        }

        private void MoveToSoldByLane35()
        {
            var items = lv35OnLane.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            UpadteC35ListViews();
        }

        private void MoveToOnHand80()
        {
            var items = lv80SoldByLane.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            UpadteC80ListViews();
        }

        private void MoveToSoldByLane80()
        {
            var items = lv80OnLane.SelectedItems;
            if (null == items || items.Count <= 0) return;
            if (null == manager) return;

            UpadteC80ListViews();
        }

        private void UpadteC35ListViews()
        {
            lv35SoldByLane.ItemsSource = null;
            lv35OnLane.ItemsSource = null;

            manager.C35SoldByLaneFilter = txtCoupon35Filter.Text.Trim();

            lv35SoldByLane.ItemsSource = manager.C35Stocks;
            lv35OnLane.ItemsSource = manager.C35Lanes;
        }

        private void UpadteC80ListViews()
        {
            lv80SoldByLane.ItemsSource = null;
            lv80OnLane.ItemsSource = null;

            manager.C80SoldByLaneFilter = txtCoupon80Filter.Text.Trim();

            lv80SoldByLane.ItemsSource = manager.C80Stocks;
            lv80OnLane.ItemsSource = manager.C80Lanes;
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
            if (null != manager && null != manager.User)
            {
                manager.Refresh();
                UpadteListViews();
            }
        }

        #endregion
    }
}
