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

namespace DMT.TA.Pages.Exchange
{
    /// <summary>
    /// Interaction logic for RequestExchangePage.xaml
    /// </summary>
    public partial class RequestExchangePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RequestExchangePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdRequest_Click(object sender, RoutedEventArgs e)
        {
            NewRequest();
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TAApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void Refresh()
        {
            plazaSummary.Setup(); // Call for refresh.
        }

        private void NewRequest()
        {
            TSBExchangeGroup group = new TSBExchangeGroup();
            TSBExchangeTransaction transaction = new TSBExchangeTransaction();
            transaction.Description = "แลกเปลี่ยนเงินยืม/ทอน";
            transaction.TransactionType = TSBExchangeTransaction.TransactionTypes.Request;
            EditRequest(group, transaction, true);
        }

        private void EditRequest(TSBExchangeGroup group, TSBExchangeTransaction transaction, bool isNew)
        {
            var win = TAApp.Windows.RequestExchange;
            win.Setup(group, transaction);
            if (win.ShowDialog() == false)
            {
                return;
            }

            Refresh();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            Refresh();
        }

        #endregion
    }
}
