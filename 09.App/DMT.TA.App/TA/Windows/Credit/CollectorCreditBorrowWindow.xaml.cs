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

        //private CurrentTSBManager manager = new CurrentTSBManager();
        private UserCreditBorrowManager manager = new UserCreditBorrowManager();

        #endregion

        #region Button Handlers

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            if (null != manager && null == manager.UserBalance)
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

            if (manager.HasNegative())
            {
                var win = TAApp.Windows.MessageBox;
                win.Setup(
                    "ไม่สามารถดำเนินการบันทึกข้อมูลได้ เนื่องจากระบบพบว่ามีการ ยอดการยืมเงินในบางรายการ เกินจำนวนที่่ด่านมีอยู่", 
                    "DMT - Toll Admin");
                win.ShowDialog();
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
                var plazaGroups = TAAPI.TSBPlazaGroups;
                cbPlzaGroups.ItemsSource = plazaGroups;
                if (plazaGroups.Count > 0) cbPlzaGroups.SelectedIndex = 0;
            }
        }

        private void ShowUserSearchPanel()
        {
            panelSearch1.Visibility = Visibility.Visible;
            panelSearch2.Visibility = Visibility.Visible;
        }

        private void HideUserSearchPanel()
        {
            panelSearch1.Visibility = Visibility.Collapsed;
            panelSearch2.Visibility = Visibility.Collapsed;
        }

        private void Reset(UserCreditBalance userBalance)
        {
            if (null == manager) manager = new UserCreditBorrowManager();

            cbPlzaGroups.SelectedIndex = -1;
            LoadPlazaGroups();

            manager.SetUser(null);
            // Set Bindings User Selection.
            txtUserId.DataContext = manager;
            txtUserName.DataContext = manager;

            manager.Setup(userBalance);

            if (manager.UserBalance.UserCreditId == 0)
            {
                ShowUserSearchPanel();
            }
            else HideUserSearchPanel();


            this.DataContext = manager.UserBalance;

            manager.UserBalance.Description = "ยอดยืมปัจจุบัน";
            manager.UserBalance.HasRemark = false;
            userBalanceEntry.DataContext = manager.UserBalance;

            manager.Transaction.Description = "ยืมเงิน";
            usrTransactinEntry.DataContext = manager.Transaction;

            manager.ResultBalance.Description = "ยอดด่านคงเหลือ";
            manager.ResultBalance.HasRemark = false;
            tsbBalanceEntry.DataContext = manager.ResultBalance;
        }

        private void ResetSelectUser()
        {
            if (null != manager) manager.SetUser(null);
            txtSearchUserId.Text = string.Empty;
        }

        private void SearchUser()
        {
            string userId = txtSearchUserId.Text.Trim();
            var result = TAAPI.SearchUser(userId, TAApp.Permissions.TC);
            if (!result.IsCanceled && null != manager)
            {
                manager.SetUser(result.User);
                if (null != manager.User)
                {
                    txtSearchUserId.Text = string.Empty;

                    Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        txtBagNo.SelectAll();
                        txtBagNo.Focus();
                    }));
                }
            }
        }

        private bool Save()
        {
            bool ret = false;
            if (string.IsNullOrEmpty(txtBagNo.Text) || string.IsNullOrEmpty(txtBeltNo.Text))
                return ret;

            if (null == manager)
                return ret;

            manager.PlazaGroup = cbPlzaGroups.SelectedItem as PlazaGroup;
            if (null == manager.PlazaGroup)
                return ret;

            ret = manager.Save();

            return ret;
        }

        #endregion

        #region Public Methods

        public void Setup(UserCreditBalance userBalance)
        {
            Reset(userBalance);

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtSearchUserId.Focus();
            }));
        }

        #endregion
    }
}
