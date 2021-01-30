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
