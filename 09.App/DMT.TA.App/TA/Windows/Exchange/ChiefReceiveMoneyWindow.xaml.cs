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
                msg = string.Format("CHIEF RECEIVED MONEY - USER NOT FOUND. USERID: {0}", userId);
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
        }

        private void CheckUser(User user)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            string msg = string.Empty;

            if (null == user)
            {
                // write log.
                msg = "CHIEF RECEIVED MONEY - USER NOT FOUND.";
                med.Info(msg);
            }
            else
            {
                // write log.
                msg = string.Format("CHIEF RECEIVED MONEY - USER FOUND. USERID: {0}, USERNAME: {1}", user.UserId, user.FullNameTH);
                med.Info(msg);
            }

            if (null != user && !string.IsNullOrWhiteSpace(user.RoleId))
            {
                //var roles = new List<string>(TAApp.Permissions.CHIEFS);
                var roles = new List<string>(TAApp.Permissions.ACCOUNTS); // all money used accounts role.

                if (!roles.Contains(user.RoleId))
                {
                    txtMsg.Text = "พนักงานตามรหัสที่ระบุ ไม่มีสิทธิในการรับเงิน" + Environment.NewLine + "กรุณาป้อนรหัสพนักงานอื่น";

                    msg = string.Format("CHIEF RECEIVED MONEY - USERID: {0} HAS NO PERMISSION.", user.UserId);
                    med.Info(msg);

                    Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        txtUserId.SelectAll();
                        txtUserId.Focus();
                    }));

                    return;
                }
            }

            ShutdownService();
            this.DialogResult = true;
        }

        #endregion

        #endregion

        #region Public Methods

        public void Setup(decimal bhtTotal)
        {
            StartService();

            /*
            _userId = (null != balance) ? balance.UserId : string.Empty;
            */

            string msg2 = bhtTotal.ToString("n0");

            txtMsg.Text = string.Empty;
            txtAmount.Text = msg2;

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtUserId.Focus();
            }));
        }

        #endregion
    }
}
