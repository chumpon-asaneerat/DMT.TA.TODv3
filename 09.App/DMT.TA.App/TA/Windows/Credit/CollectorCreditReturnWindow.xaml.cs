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

namespace DMT.TA.Windows.Credit
{
    /// <summary>
    /// Interaction logic for CollectorCreditReturnWindow.xaml
    /// </summary>
    public partial class CollectorCreditReturnWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectorCreditReturnWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private UserCreditReturnManager manager = null;

        #endregion

        #region Button Handlers

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            if (manager.HasNegative())
            {
                var win = TAApp.Windows.MessageBox;
                win.Setup(
                    "ไม่สามารถดำเนินการบันทึกข้อมูลได้ เนื่องจากระบบพบว่ามีการ คืนเงิน เกินจำนวนที่่ได้ยืมไป",
                    "DMT - Toll Admin");
                win.ShowDialog();
                return;
            }
            if (null != manager.UserBalance && null != manager.Transaction)
            {
                string msg1 = "ยืนยันการคืนเงิน ยืมทอน";
                string usr = manager.UserBalance.FullNameTH;
                string amt = manager.Transaction.BHTTotal.ToString("n0");

                var win = TAApp.Windows.MessageBoxYesNo1;
                win.Setup(msg1, usr, amt, "DMT - Toll Admin");
                if (win.ShowDialog() == true)
                {
                    if (manager.Save())
                    {
                        this.DialogResult = true;
                    }
                }
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region Private Methods

        private void Reset(UserCreditBalance userBalance)
        {
            if (null == manager) manager = new UserCreditReturnManager();

            manager.Setup(userBalance);

            this.DataContext = manager.UserBalance;

            manager.UserBalance.Description = "ยอดยืมปัจจุบัน";
            manager.UserBalance.HasRemark = false;
            userBalanceEntry.DataContext = manager.UserBalance;

            manager.Transaction.Description = "คืนเงิน";
            usrTransactinEntry.DataContext = manager.Transaction;
        }

        #endregion

        #region Public Methods

        public void Setup(UserCreditBalance balance)
        {
            Reset(balance);
        }

        #endregion
    }
}
