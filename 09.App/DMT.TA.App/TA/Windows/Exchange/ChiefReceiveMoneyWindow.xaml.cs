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
using System.Windows.Threading;

using NLib;
using System.Reflection;

#endregion

namespace DMT.TA.Windows.Exchange
{
    /// <summary>
    /// Interaction logic for ChiefReceiveMoneyWindow.xaml
    /// </summary>
    public partial class ChiefReceiveMoneyWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChiefReceiveMoneyWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string _userId = string.Empty;

        #endregion

        #region TextBox Handlers

        private void txtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter || e.Key == System.Windows.Input.Key.Return)
            {
                CheckUser();
                e.Handled = true;
            }
        }

        #endregion

        #region Button Handlers

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            ShutdownService();
            CheckUser();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            ShutdownService();
            DialogResult = false;
        }

        #endregion

        #region Private Methods

        #region Start/Shutdown Smartcard service

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

        #endregion

        #region Smartcard Handler(s)

        private void Instance_UserChanged(object sender, EventArgs e)
        {
            var user = SmartcardManager.Instance.User;
            if (null == user) return;
            CheckUser(user);
        }

        #endregion

        #region Check User

        private void CheckUser()
        {
            /*
            string userId = txtUserId.Text.Trim();
            string pwd = txtPassword.Password.Trim();
            if (string.IsNullOrWhiteSpace(userId))
            {
                txtMsg.Text = "กรุณาป้อนรหัสพนักงาน";

                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    txtUserId.SelectAll();
                    txtUserId.Focus();
                }));
                return;
            }
            if (string.IsNullOrWhiteSpace(pwd))
            {
                txtMsg.Text = "กรุณาป้อนรหัสผ่าน";

                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    txtPassword.SelectAll();
                    txtPassword.Focus();
                }));
                return;
            }

            var md5 = Utils.MD5.Encrypt(pwd);
            var user = User.GetByLogIn(userId, md5).Value();

            MethodBase med = MethodBase.GetCurrentMethod();
            string msg = string.Empty;

            if (null == user)
            {
                // write log.
                msg = string.Format("RECEIVED BAG - USER NOT FOUND. USERID: {0}", userId);
                med.Info(msg);

                txtMsg.Text = "ไม่พบข้อมูลพนักงาน ตามรหัสที่ระบุ กรุณาใส่รหัสพนักงานใหม่";

                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    txtUserId.SelectAll();
                    txtUserId.Focus();
                }));
                return;
            }

            CheckUser(user);
            */
        }

        private void CheckUser(User user)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            string msg = string.Empty;
            /*
            if (null == user)
            {
                // write log.
                msg = "RECEIVED BAG - USER NOT FOUND.";
                med.Info(msg);
            }
            else
            {
                // write log.
                msg = string.Format("RECEIVED BAG - USER FOUND. USERID: {0}, USERNAME: {1}", user.UserId, user.FullNameTH);
                med.Info(msg);
            }

            if (string.IsNullOrEmpty(_userId) || null == user || (null != user && user.UserId != _userId))
            {
                txtMsg.Text = "รหัสพนักงานไม่ตรงกับ พนักงานที่รับถุงเงิน";

                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    txtUserId.SelectAll();
                    txtUserId.Focus();
                }));

                return;
            }
            */
            ShutdownService();
            this.DialogResult = true;
        }

        #endregion

        #endregion

        #region Public Methods

        public void Setup(/* UserCreditBalance balance */)
        {
            StartService();

            /*
            _userId = (null != balance) ? balance.UserId : string.Empty;
            string msg1 = (null != balance) ? balance.BagNo : string.Empty;
            string msg2 = (null != balance) ? balance.BHTTotal.ToString("n0") : "0";

            txtMsg.Text = string.Empty;
            txtBagID.Text = msg1;
            txtAmount.Text = msg2;

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtUserId.Focus();
            }));
            */
        }

        #endregion
    }
}
