#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;
using System.Windows.Threading;

#endregion

namespace DMT.TA.Pages.Coupon
{
    /// <summary>
    /// Interaction logic for ReceiveCouponPage.xaml
    /// </summary>
    public partial class ReceiveCouponPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReceiveCouponPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _chief = null;
        private TSBCouponBorrowManager manager = null;

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdUserSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchUser();
        }

        private void cmdAppend_Click(object sender, RoutedEventArgs e)
        {
            AppendUser();
        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region TextBox Handlers

        private void txtSearchUserId_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter ||
                e.Key == System.Windows.Input.Key.Return)
            {
                SearchUser();
                e.Handled = true;
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                ResetSelectUser();
                RefreshCoupons();
                e.Handled = true;
            }
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TAApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void Reset()
        {
            if (null == manager) manager = new TSBCouponBorrowManager();
            manager.SetUser(null);
            if (null != manager)
            {

            }

            txtToday.Text = DateTime.Now.Date.ToThaiDateTimeString("yyyy/MM/dd HH:mm");
            // Set Bindings User Selection.
            txtUserId.DataContext = manager;
            txtUserName.DataContext = manager;
        }

        private void ResetSelectUser()
        {
            if (null != manager) manager.SetUser(null);
            txtSearchUserId.Text = string.Empty;
            cmdAppend.IsEnabled = false;
        }

        private void SearchUser()
        {
            string userId = txtSearchUserId.Text.Trim();
            var result = TAAPI.SearchUser(userId, TAApp.Permissions.TC);
            if (!result.IsCanceled && null != manager)
            {
                manager.SetUser(result.User);
                if (null != manager.User)
                {
                    txtSearchUserId.Text = string.Empty;
                }
                RefreshCoupons();
            }

            bool hasUser = (null != manager && null != manager.User);
            cmdAppend.IsEnabled = hasUser;
        }

        private void AppendUser()
        {

        }

        private void RefreshCoupons()
        {

        }

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
            ResetSelectUser();

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtSearchUserId.SelectAll();
                txtSearchUserId.Focus();
            }));
        }

        #endregion
    }
}
