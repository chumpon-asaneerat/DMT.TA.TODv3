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

namespace DMT.TA.Pages.Credit
{
    /// <summary>
    /// Interaction logic for CollectorCreditManagePage.xaml
    /// </summary>
    public partial class CollectorCreditManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectorCreditManagePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdAddCollector_Click(object sender, RoutedEventArgs e)
        {
            AddUserTransaction();
        }

        private void cmdReceivedBag_Click(object sender, RoutedEventArgs e)
        {
            var elem = (sender as FrameworkElement);
            var context = (null != elem) ? elem.DataContext : null;
            var userCredit = (null != context) ? (context as UserCreditBalance) : null;
            if (null == userCredit) return;

            ReceiveBag(userCredit);
        }

        private void cmdBorrow_Click(object sender, RoutedEventArgs e)
        {
            var elem = (sender as FrameworkElement);
            var context = (null != elem) ? elem.DataContext : null;
            var userCredit = (null != context) ? (context as UserCreditBalance) : null;
            if (null == userCredit) return;

            BorrowCredit(userCredit);
        }

        private void cmdReturn_Click(object sender, RoutedEventArgs e)
        {
            var elem = (sender as FrameworkElement);
            var context = (null != elem) ? elem.DataContext : null;
            var userCredit = (null != context) ? (context as UserCreditBalance) : null;
            if (null == userCredit) return;

            ReturnCredit(userCredit);
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

            var userCredits = UserCreditBalance.GetActiveUserCreditBalances(TAAPI.TSB).Value();
            lstUsers.ItemsSource = userCredits;
        }

        private void AddUserTransaction()
        {
            BorrowCredit(null); // New Collector Borrow.
        }

        private void ReceiveBag(UserCreditBalance balance)
        {
            var win = TAApp.Windows.CollectorReceivedBag;
            win.Owner = Application.Current.MainWindow;
            win.Setup(balance);
            if (win.ShowDialog() == false)
            {
                return;
            }
            Refresh();
        }

        private void BorrowCredit(UserCreditBalance balance)
        {
            var win = TAApp.Windows.CollectorCreditBorrow;
            win.Owner = Application.Current.MainWindow;
            win.Setup(balance);
            if (win.ShowDialog() == false)
            {
                return;
            }
            Refresh();
        }

        private void ReturnCredit(UserCreditBalance balance)
        {
            var win = TAApp.Windows.CollectorCreditReturn;
            win.Owner = Application.Current.MainWindow;
            win.Setup(balance);
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
