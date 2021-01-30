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

        private void txtCoupon35Filter_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtCoupon80Filter_KeyDown(object sender, KeyEventArgs e)
        {

        }

        #endregion

        #region Button (Move) Handlers

        private void cmd35StockToUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmd35UserToStock_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmd80StockToUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmd80UserToStock_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region ListView Handlers

        private void lv35Stock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void lv35Stock_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void lv35User_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void lv35User_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void lv80Stock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void lv80Stock_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void lv80User_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void lv80User_KeyDown(object sender, KeyEventArgs e)
        {

        }

        #endregion

        #region Private Methods

        private void LoadUserCoupons()
        {

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
                LoadUserCoupons();
            }
        }

        #endregion
    }
}
