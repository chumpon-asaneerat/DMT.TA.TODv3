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

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
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

        #endregion
    }
}
