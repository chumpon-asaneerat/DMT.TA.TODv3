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

            var userCredits = UserCreditBalance.GetCurrentBalances(TAAPI.TSB).Value();
            lstUsers.ItemsSource = userCredits;
        }

        private void AddUserTransaction()
        {
            BorrowCredit(null); // New Collector Borrow.
        }

        private void ReceiveBag(UserCreditBalance balance)
        {
            if (null == balance) 
                return;

            var win = TAApp.Windows.CollectorReceivedBag;
            win.Setup(balance);
            if (win.ShowDialog() == false)
            {
                return;
            }

            // Change state after received bag and update to database.
            balance.State = UserCreditBalance.StateTypes.Received;
            UserCreditBalance.SaveUserCreditBalance(balance);

            var usr = User.GetByUserId(balance.UserId).Value();
            // For Update User Bag Number and balance
            TAAccountUserCredit usrCdt = new TAAccountUserCredit();
            usrCdt.TSBId = balance.TSBId;
            usrCdt.UserId = balance.UserId;
            usrCdt.UserPrefix = (null != usr) ? usr.PrefixTH : string.Empty;
            usrCdt.UserFirstName = (null != usr) ? usr.FirstNameTH : balance.FullNameTH;
            usrCdt.UserLastName = (null != usr) ? usr.LastNameTH : string.Empty;
            //usrCdt.BagNo = (balance.State == UserCreditBalance.StateTypes.Initial) ? null : balance.BagNo;
            usrCdt.BagNo = balance.BagNo;
            usrCdt.CreditDate = balance.UserCreditDate;
            usrCdt.Credit = balance.BHTTotal;
            usrCdt.flag = 0; // if Received Bag or Balance Not zero @flag = 0

            TAxTODMQService.Instance.WriteQueue(usrCdt);

            // Find current TSB balance.
            var tsbBal = TSBCreditBalance.GetCurrent(TAAPI.TSB).Value();
            if (null != tsbBal)
            {
                // For Update TSB balance
                TAAccountTSBCredit tsbCdt = new TAAccountTSBCredit();
                tsbCdt.TSBId = tsbBal.TSBId;
                tsbCdt.Amnt1 = tsbBal.AmountBHT1;
                tsbCdt.Amnt2 = tsbBal.AmountBHT2;
                tsbCdt.Amnt5 = tsbBal.AmountBHT5;
                tsbCdt.Amnt10 = tsbBal.AmountBHT10;
                tsbCdt.Amnt20 = tsbBal.AmountBHT20;
                tsbCdt.Amnt50 = tsbBal.AmountBHT50;
                tsbCdt.Amnt100 = tsbBal.AmountBHT100;
                tsbCdt.Amnt500 = tsbBal.AmountBHT500;
                tsbCdt.Amnt1000 = tsbBal.AmountBHT1000;
                tsbCdt.Remark = null;
                tsbCdt.Updatedate = DateTime.Now;
                TAxTODMQService.Instance.WriteQueue(tsbCdt);
            }

            Refresh();
        }

        private void BorrowCredit(UserCreditBalance balance)
        {
            var win = TAApp.Windows.CollectorCreditBorrow;
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
