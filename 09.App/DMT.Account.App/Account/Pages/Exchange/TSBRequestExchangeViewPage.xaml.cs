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
    using ops = DMT.Services.Operations.TAxTOD;

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

        private void cmdRequestDetail_Click(object sender, RoutedEventArgs e)
        {
            // Show Detail windows
            if (null == sender || !(sender is Button)) return;
            var item = (sender as Button).DataContext as Models.TAAExchangeSummary;
            if (null != item)
            {
                var req = new TSBExchangeTransaction();
                req.TransactionDate = (item.RequestDate.HasValue) ? item.RequestDate.Value : DateTime.MinValue;
                req.TransactionType = TSBExchangeTransaction.TransactionTypes.Request;
                req.Remark = item.RequestRemark;

                var win = AccountApp.Windows.ResuestExchangeWindow;
                win.Setup(req);
                win.ShowDialog();

                LoadAll(); // refresh.
            }
        }

        private void cmdApproveDetail_Click(object sender, RoutedEventArgs e)
        {
            // Show Detail windows
            if (null == sender || !(sender is Button)) return;
            var item = (sender as Button).DataContext as Models.TAAExchangeSummary;
            if (null != item)
            {
                var apprv = new TSBExchangeTransaction();
                apprv.TransactionDate = (item.RequestDate.HasValue) ? item.RequestDate.Value : DateTime.MinValue;
                apprv.TransactionType = TSBExchangeTransaction.TransactionTypes.Approve;
                apprv.Remark = item.RequestRemark;

                var win = AccountApp.Windows.ResuestExchangeWindow;
                win.Setup(apprv);
                win.ShowDialog();

                LoadAll(); // refresh.
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

        private void LoadRequestList()
        {
            gridRequest.ItemsSource = null;
            // get by (R)equest status.
            var list = ops.Exchange.Gets("R").Value();
            gridRequest.ItemsSource = list;
        }

        private void LoadApproveList()
        {
            gridApprove.ItemsSource = null;
            // get by (A)pprove status.
            var list = ops.Exchange.Gets("A").Value();
            gridApprove.ItemsSource = list;
        }

        private void LoadAll()
        {
            LoadRequestList();
            LoadApproveList();
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

            LoadAll();
        }

        #endregion
    }
}
