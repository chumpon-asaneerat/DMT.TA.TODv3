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
    /// Interaction logic for ManageExchangePage.xaml
    /// </summary>
    public partial class ManageExchangePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ManageExchangePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private List<TSBExchangeTransaction> trans = new List<TSBExchangeTransaction>();

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdReturn_Click(object sender, RoutedEventArgs e)
        {
            var item = gridBorrow.SelectedItem as TSBExchangeTransaction;
            if (null == item)
            {
                return;
            }

            // Create received transaction and set as return
            var exchRet = new TSBExchangeTransaction();
            exchRet.TransactionDate = DateTime.Now;
            exchRet.TransactionType = TSBExchangeTransaction.TransactionTypes.Return;
            exchRet.Description = "คืนเงินยืม/ทอน";
            exchRet.GroupId = item.GroupId;
            exchRet.TSBId = item.TSBId;
            exchRet.TSBNameEN = item.TSBNameEN;
            exchRet.TSBNameTH = item.TSBNameTH;
            exchRet.UserId = TAApp.User.Current.UserId;
            exchRet.FullNameEN = TAApp.User.Current.FullNameEN;
            exchRet.FullNameTH = TAApp.User.Current.FullNameTH;
            exchRet.RequestDate = item.RequestDate;
            exchRet.RequestUserId = item.RequestUserId;
            exchRet.RequestFullNameEN = item.RequestFullNameEN;
            exchRet.RequestFullNameTH = item.RequestFullNameTH;
            exchRet.BorrowBHT = item.BorrowBHT; // set borrow amount.
            exchRet.HasRemark = true;

            var win = TAApp.Windows.ReturnExchange;
            win.Setup(exchRet);
            if (win.ShowDialog() == false)
            {
                return;
            }

            Refresh();
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
            LoadExchangeGroups();
            plazaSummary.Setup(); // Call for refresh.
        }
        private void LoadExchangeGroups()
        {
            gridBorrow.ItemsSource = null;

            trans = TSBExchangeTransaction.GetBorrowTransactions(TAAPI.TSB).Value();

            gridBorrow.ItemsSource = trans;
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
