#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Controls;
using DMT.Services;

using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.TA.Windows.Credit
{
    /// <summary>
    /// Interaction logic for CollectorReceivedBagWindow.xaml
    /// </summary>
    public partial class CollectorReceivedBagWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectorReceivedBagWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string _userId = string.Empty;

        #endregion

        #region Button Handlers

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            ShutdownService();
            DialogResult = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            ShutdownService();
            DialogResult = false;
        }

        #endregion

        #region Private Methods

        private void StartService()
        {
            SmartcardManager.Instance.UserChanged += Instance_UserChanged;
            SmartcardManager.Instance.Start();
        }

        private void ShutdownService()
        {
            SmartcardManager.Instance.Shutdown();
            SmartcardManager.Instance.UserChanged -= Instance_UserChanged;
        }

        #region Smartcard Handler(s)

        private void Instance_UserChanged(object sender, EventArgs e)
        {
            var user = SmartcardManager.Instance.User;
            if (null == user) return;
            CheckUser(user);
        }

        #endregion

        #region Check User

        private void CheckUser(User user)
        {

            if (null == user || (null != user && user.UserId != _userId))
            {
                txtMsg.Text = "รหัสพนักงานไม่ตรงกับ พนักงานที่รับถุงเงิน";
                txtUserId.SelectAll();
                txtUserId.Focus();
                return;
            }

            ShutdownService();
            this.DialogResult = true;
        }

        #endregion

        #endregion

        #region Public Methods

        public void Setup(UserCreditBalance balance)
        {
            StartService();

            _userId = (null != balance) ? balance.UserId : string.Empty;
            string msg1 = (null != balance) ? balance.BagNo : string.Empty;
            string msg2 = (null != balance) ? balance.BHTTotal.ToString("n0") : "0";

            txtMsg.Text = string.Empty;
            txtBagID.Text = msg1;
            txtAmount.Text = msg2;
        }

        #endregion
    }
}
