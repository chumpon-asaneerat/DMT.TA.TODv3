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

namespace DMT.Account.Pages.Exchange
{
    /// <summary>
    /// Interaction logic for TSBRequestExchangeViewPage.xaml
    /// </summary>
    public partial class TSBRequestExchangeViewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBRequestExchangeViewPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _chief = null;

        #endregion

        #region TabControl Handler

        private void tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabs.SelectedIndex == 0)
            {
                cmdApprove.Visibility = Visibility.Visible;
                cmdNotApprove.Visibility = Visibility.Visible;
            }
            else
            {
                cmdApprove.Visibility = Visibility.Collapsed;
                cmdNotApprove.Visibility = Visibility.Collapsed;
            }

        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdNotApprove_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = AccountApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region Public Method
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
        }

        #endregion
    }
}
