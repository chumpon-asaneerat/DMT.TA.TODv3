#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
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
    /// Interaction logic for TSBApproveBalanceViewPage.xaml
    /// </summary>
    public partial class TSBApproveBalanceViewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBApproveBalanceViewPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        #endregion

        #region ListView Handlers

        private void gridBalance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridTransactions.ItemsSource = null;
            var item = (null != gridBalance.SelectedItem && gridBalance.SelectedItem is TAATSBApproveCredit) ?
                (gridBalance.SelectedItem as TAATSBApproveCredit) : null;
            LoadTransactions(item);
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = AccountApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void LoadTransactions(TAATSBApproveCredit item)
        {
            gridTransactions.ItemsSource = null;
            if (null == item) return;
            var items = ops.Exchange.GetTSBApproveCreditTransactions(item.TSBId).Value();
            gridTransactions.ItemsSource = items;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            var tsbBalances = ops.Exchange.GetTSBApproveCredits().Value();
            gridBalance.ItemsSource = tsbBalances;
        }

        #endregion
    }
}
