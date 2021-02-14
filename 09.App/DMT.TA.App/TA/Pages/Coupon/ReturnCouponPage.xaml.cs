#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.TA.Pages.Coupon
{
    /// <summary>
    /// Interaction logic for ReturnCouponPage.xaml
    /// </summary>
    public partial class ReturnCouponPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReturnCouponPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _chief = null;
        private TSBCouponReturnManager manager = null;

        #endregion

        #region Button Handlers

        private void cmdRefresh_Click(object sender, RoutedEventArgs e)
        {
            Resync();
        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdReturn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);
            var tran = (null != button) ? button.DataContext as TSBCouponSummary : null;
            if (null == manager || null == tran) return;
            var user = User.GetByUserId(tran.UserId).Value();
            if (null == user) return;
            manager.SetUser(user);
            ReturnCoupon();
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

        #endregion

        #region Reset

        private void Reset()
        {
            if (null == manager) manager = new TSBCouponReturnManager();
            manager.SetUser(null);
            if (null != manager)
            {

            }
        }

        #endregion

        #region Coupon Manage methods

        private void ReturnCoupon()
        {
            if (null == manager || null == manager.User) return;
            var win = TAApp.Windows.CollectorCouponReturn;
            win.Setup(manager);
            win.ShowDialog();
            Reset();
            RefreshCoupons();
        }

        private void RefreshCoupons()
        {
            grid.ItemsSource = null;
            var summaries = TSBCouponSummary.GetTSBCouponSummaries(TAAPI.TSB).Value();
            if (null != summaries)
            {
                grid.ItemsSource = summaries;
            }
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
        /// Setup/
        /// </summary>
        /// <param name="chief">The current user.</param>
        public void Setup(User chief)
        {
            _chief = chief;
            if (null != _chief)
            {

            }

            Reset();
            RefreshCoupons();
        }

        #endregion
    }
}
