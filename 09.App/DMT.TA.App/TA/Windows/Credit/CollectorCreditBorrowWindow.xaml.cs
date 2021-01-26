#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;
using System.Windows.Threading;

#endregion

namespace DMT.TA.Windows.Credit
{
    /// <summary>
    /// Interaction logic for CollectorCreditBorrowWindow.xaml
    /// </summary>
    public partial class CollectorCreditBorrowWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectorCreditBorrowWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private CurrentTSBManager manager = new CurrentTSBManager();

        #endregion

        #region Button Handlers

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            if (null != manager && null == manager.User)
            {
                var win = TAApp.Windows.MessageBox;
                win.Setup("โปรดระบุ พนักงาน", "DMT - Toll Admin");
                win.ShowDialog();

                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    txtSearchUserId.SelectAll();
                    txtSearchUserId.Focus();
                }));
                return;
            }
            if (cbPlzaGroups.SelectedIndex == -1)
            {
                var win = TAApp.Windows.MessageBox;
                win.Setup("โปรดระบุ ด่าน", "DMT - Toll Admin");
                win.ShowDialog();

                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    cbPlzaGroups.Focus();
                }));
                return;
            }

            if (string.IsNullOrEmpty(txtBagNo.Text))
            {
                var win = TAApp.Windows.MessageBox;
                win.Setup("โปรดระบุ หมายเลขถุงเงิน", "DMT - Toll Admin");
                win.ShowDialog();

                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    txtBagNo.SelectAll();
                    txtBagNo.Focus();
                }));
                return;
            }
            else if (string.IsNullOrEmpty(txtBeltNo.Text))
            {
                var win = TAApp.Windows.MessageBox;
                win.Setup("โปรดระบุ หมายเลขเข็มขัดนิรภัย", "DMT - Toll Admin");
                win.ShowDialog();

                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    txtBeltNo.SelectAll();
                    txtBeltNo.Focus();
                }));
                return;
            }

            if (Save())
            {
                DialogResult = true;
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void cmdUserSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchUser();
        }

        #endregion

        #region TextBox Handlers

        private void txtSearchUserId_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter ||
                e.Key == System.Windows.Input.Key.Return)
            {
                SearchUser();
                e.Handled = true;
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                ResetSelectUser();
                e.Handled = true;
            }
        }

        #endregion

        #region Private Methods

        private void LoadPlazaGroups()
        {
            cbPlzaGroups.ItemsSource = null;
            if (null != manager && null != manager)
            {
                cbPlzaGroups.ItemsSource = manager.TSBPlazaGroups;
                if (manager.TSBPlazaGroups.Count > 0) cbPlzaGroups.SelectedIndex = 0;
            }
        }

        private void Reset()
        {
            cbPlzaGroups.SelectedIndex = -1;
            LoadPlazaGroups();

            manager.User = null;
            // Set Bindings User Selection.
            txtUserId.DataContext = manager;
            txtUserName.DataContext = manager;

            manager.Refresh();

            // User Balance (Overall)
            var usrBalance = new UserCreditBalance();
            usrBalance.Description = "ยอดยืมปัจจุบัน";
            usrBalance.HasRemark = false;
            userBalanceEntry.DataContext = usrBalance;

            // User Transaction
            var usrTran = new UserCreditTransaction();
            usrTran.Description = "ยืมเงิน";
            usrTran.HasRemark = false;
            usrTransactinEntry.DataContext = usrTran;

            // TSB Balance
            var tsbBalance = (null != manager.Credit) ? manager.Credit.TSBBalance : new TSBCreditBalance();
            tsbBalance.Description = "ยอดด่านคงเหลือ";
            tsbBalance.HasRemark = false;
            tsbBalanceEntry.DataContext = tsbBalance;
        }

        private void ResetSelectUser()
        {
            manager.User = null;
            txtSearchUserId.Text = string.Empty;
        }

        private void SearchUser()
        {
            string userId = txtSearchUserId.Text.Trim();
            var result = TAAPI.SearchUser(userId, TAApp.Permissions.TC);
            if (!result.IsCanceled && null != manager)
            {
                manager.User = result.User;
                if (null != manager.User)
                {
                    txtSearchUserId.Text = string.Empty;
                }
            }
        }

        private bool Save()
        {
            bool ret = false;
            if (string.IsNullOrEmpty(txtBagNo.Text) || string.IsNullOrEmpty(txtBeltNo.Text))
                return ret;

            ret = true;

            return ret;
        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            Reset();
        }

        #endregion
    }
}
