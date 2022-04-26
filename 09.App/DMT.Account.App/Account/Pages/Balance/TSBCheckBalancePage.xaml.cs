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

namespace DMT.Account.Pages.Balance
{
    using ops = DMT.Services.Operations.TAxTOD;

    /// <summary>
    /// Interaction logic for TSBCheckBalance.xaml
    /// </summary>
    public partial class TSBCheckBalancePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TSBCheckBalancePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _chief = null;

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadTSBBalances();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Show popup window
            if (null == sender || !(sender is Button)) return;
            var item = (sender as Button).DataContext as Models.TAATSBCreditResult;
            if (null != item)
            {
                var win = AccountApp.Windows.UserCheckBalance;
                win.Setup(item.TSBId);
                win.ShowDialog();

                LoadTSBBalances(); // refresh.
            }
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = AccountApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void LoadTSBBalances()
        {
            grid.ItemsSource = null;
            var items = ops.Credit.TSB.Gets().Value();
            grid.ItemsSource = items;
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

            LoadTSBBalances();

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                grid.Focus();
            }));
        }

        #endregion
    }
}
