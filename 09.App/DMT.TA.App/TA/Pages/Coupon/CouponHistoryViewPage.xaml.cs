#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Reflection;

using DMT.Models;
using DMT.Services;

using NLib;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.TA.Pages.Coupon
{
    /// <summary>
    /// Interaction logic for CouponHistoryViewPage.xaml
    /// </summary>
    public partial class CouponHistoryViewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CouponHistoryViewPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _chief = null;

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

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TAApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void Resync()
        {
            CouponSyncService.Instance.ReSyncAll();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="chief">The Current User.</param>
        public void Setup(User chief)
        {
            _chief = chief;

            if (null != _chief)
            {

            }

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtSAPItemCode.SelectAll();
                txtSAPItemCode.Focus();
            }));
        }

        #endregion
    }
}
