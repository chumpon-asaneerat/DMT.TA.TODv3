#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using NLib.Services;

using DMT.Models;
using DMT.Services;
using DMT.Controls;
using System.Windows.Threading;

#endregion

namespace DMT.Pages
{
    /// <summary>
    /// Interaction logic for SignInPage.xaml
    /// </summary>
    public partial class SignInPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SignInPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private List<string> _roles = new List<string>();
        private User _user = null;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SmartcardManager.Instance.UserChanged += Instance_UserChanged;

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtUserId.Focus();
            }));
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            SmartcardManager.Instance.UserChanged -= Instance_UserChanged;
            SmartcardManager.Instance.Shutdown();
        }

        #endregion

        #region Smartcard Handler(s)

        private void Instance_UserChanged(object sender, EventArgs e)
        {
            if (null != SmartcardManager.Instance.User)
            {
                _user = SmartcardManager.Instance.User;
                if (tabs.SelectedIndex == 0)
                {
                    if (null != _user)
                    {
                        UserAccess.Success(_user.UserId).Value(); // Update success.
                        LogInLog(true);
                    }
                    VerifyUser();
                }
                else if (tabs.SelectedIndex == 1)
                {
                    txtUserId2.Text = _user.UserId;
                    txtPassword2.SelectAll();
                    txtPassword2.Focus();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(SmartcardManager.Instance.CardId))
                    return;
                ShowError("ไม่พบข้อมูลบัตรพนักงานในระบบ");
            }
        }

        #endregion

        #region Button Handler(s)

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (tabs.SelectedIndex != 0) return;
            CheckSignInInput();
        }

        private void cmdChangePwd_Click(object sender, RoutedEventArgs e)
        {
            GotoChangePasswordTab();
        }

        private void cmdOK2_Click(object sender, RoutedEventArgs e)
        {
            CheckChangePassword();
        }

        private void cmdCancel2_Click(object sender, RoutedEventArgs e)
        {
            GotoSignInTab();
        }

        private void cmdOK3_Click(object sender, RoutedEventArgs e)
        {
            SmartcardManager.Instance.Shutdown();
            GotoMainMenu();
        }

        private void cmdChangePwd3_Click(object sender, RoutedEventArgs e)
        {
            GotoChangePasswordTab();
        }

        private void cmdChangePwd4_Click(object sender, RoutedEventArgs e)
        {
            GotoChangePasswordTab();
        }

        private void cmdOK5_Click(object sender, RoutedEventArgs e)
        {
            GotoSignInTab();
        }

        private void cmdOK6_Click(object sender, RoutedEventArgs e)
        {
            GotoSignInTab();
        }

        #endregion

        #region TextBox Keydown

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (tabs.SelectedIndex != 0) return;
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                CheckSignInInput();
                e.Handled = true;
            }
        }

        private void txtConfirmPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (tabs.SelectedIndex != 1) return;
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                CheckChangePassword();
                e.Handled = true;
            }
        }

        #endregion

        #region Private Methods

        private void ShowError(string message)
        {
            if (tabs.SelectedIndex == 0)
            {
                txtMsg.Text = message;
            }
            else if (tabs.SelectedIndex == 1)
            {
                txtMsg2.Text = message;
            }
        }

        private void ClearInputs()
        {
            txtUserId.Text = string.Empty;
            txtPassword.Password = string.Empty;
            txtMsg.Text = string.Empty;

            txtUserId2.Text = string.Empty;
            txtPassword2.Password = string.Empty;
            txtNewPassword.Password = string.Empty;
            txtConfirmPassword.Password = string.Empty;
            txtMsg2.Text = string.Empty;
        }

        private void GotoMainMenu()
        {
            // Set Current User.
            TAApp.User.Current = _user;
            // Init Main Menu
            PageContentManager.Instance.Current = TAApp.Pages.MainMenu;
        }

        private void GotoSignInTab()
        {
            tabs.SelectedIndex = 0;

            ClearInputs();

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtUserId.Focus();
            }));
        }

        private void GotoChangePasswordTab()
        {
            tabs.SelectedIndex = 1;

            ClearInputs();

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtUserId2.Focus();
            }));
        }

        private void GotoNotifyPasswordNearExpiredTab(int remainDays)
        {
            txtExpiredInDays.Text = remainDays.ToString();
            tabs.SelectedIndex = 2;
        }

        private void GotoNotifyPasswordExpiredTab()
        {
            tabs.SelectedIndex = 3;
        }

        private void GotoChangePasswordSuccessTab()
        {
            tabs.SelectedIndex = 4;
        }

        private void GotoLockAccountTab(int lockHours)
        {
            //txtLockHours.Text = lockHours.ToString();
            tabs.SelectedIndex = 5;
        }

        private void CheckSignInInput()
        {
            txtMsg.Text = string.Empty;

            string userId = txtUserId.Text.Trim();
            string pwd = txtPassword.Password.Trim();
            if (string.IsNullOrWhiteSpace(userId))
            {
                ShowError("กรุณาป้อนรหัสพนักงาน");
                txtUserId.SelectAll();
                txtUserId.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(pwd))
            {
                ShowError("กรุณาป้อนรหัสผ่าน");
                txtPassword.SelectAll();
                txtPassword.Focus();
                return;
            }

            var md5 = Utils.MD5.Encrypt(pwd);
            _user = User.GetByLogIn(userId, md5).Value();

            // Check if enter mismatch password.
            if (IsUserExists(userId))
            {
                if (IsAccountLock(userId))
                {
                    return;
                }
                if (null == _user)
                {
                    UserAccess.Failed(userId).Value(); // Update failed.
                    LogInLog(false);
                }
                else
                {
                    UserAccess.Success(userId).Value(); // Update success.
                    LogInLog(true);
                }
            }

            VerifyUser();
        }

        private bool IsUserExists(string userId)
        {
            var usr = User.GetByUserId(userId).Value();
            return (null != usr);
        }

        private bool IsPasswordNeearExpireOrExpired()
        {
            bool ret = false;

            if (null != _user)
            {
                if (_user.PasswordDate.HasValue)
                {
                    DateTime expire = _user.PasswordDate.Value.AddDays(User.DefaultExpiredDays);
                    TimeSpan ts = expire.Date - DateTime.Now.Date;
                    if (ts.TotalDays <= 0)
                    {
                        // Expired.
                        GotoNotifyPasswordExpiredTab();
                        ret = true;
                    }
                    else if (ts.TotalDays <= User.DefaultNotifyDays)
                    {
                        // Update last notify date.
                        UserAccess.Nofity(_user.UserId);
                        // Near Expired.
                        GotoNotifyPasswordNearExpiredTab(Convert.ToInt32(ts.TotalDays));
                        ret = true;
                    }
                }
            }

            return ret;
        }

        private bool IsAccountLock(string userId)
        {
            bool ret = false;

            var access = UserAccess.GetUserAccess(userId).Value();
            if (null != access && access.LastLockDate.HasValue)
            {
                DateTime lockDate = access.LastLockDate.Value.AddHours(UserAccess.DefaultLockHours);
                TimeSpan ts = lockDate - DateTime.Now;
                if (ts.TotalHours > 0)
                {
                    GotoLockAccountTab(Convert.ToInt32(ts.TotalHours));
                    ret = true;
                }
                else
                {
                    // Reset lock.
                    UserAccess.Success(userId);
                    LogInLog(true);
                }
            }

            return ret;
        }

        private void VerifyUser()
        {
            if (null == _user)
            {
                ShowError("ไม่พบข้อมูลพนักงานตามรหัสพนักงาน และรหัสผ่านที่ระบุ" + Environment.NewLine + "กรุณาป้อนรหัสใหม่");
                txtUserId.SelectAll();
                txtUserId.Focus();
                return;
            }

            if (null != _user && _roles.IndexOf(_user.RoleId) == -1)
            {
                ShowError("พนักงานตามรหัสที่ระบุ ไม่มีสิทธิในการเข้าใช้งาน" + Environment.NewLine + "กรุณาป้อนรหัสพนักงานอื่น");
                txtUserId.SelectAll();
                txtUserId.Focus();
                return;
            }

            if (null != _user)
            {
                var access = UserAccess.GetUserAccess(_user.UserId).Value();
                if (null == access)
                {
                    ShowError("ไม่พบข้อมูลการเข้าใช้งานของพนักงาน" + Environment.NewLine + "กรุณาป้อนรหัสพนักงานอื่น");
                    return;
                }
            }

            if (IsPasswordNeearExpireOrExpired())
            {
                return;
            }

            SmartcardManager.Instance.Shutdown();
            GotoMainMenu();
        }

        private void CheckChangePassword()
        {
            // Call change password.
            if (ChangePassword())
            {
                GotoChangePasswordSuccessTab();
            }
        }

        private bool ChangePassword()
        {
            bool ret = false;

            var userId = txtUserId2.Text.Trim();
            var md5 = Utils.MD5.Encrypt(txtPassword2.Password);
            _user = User.GetByLogIn(userId, md5).Value();

            if (null == _user)
            {
                ShowError("ไม่พบข้อมูลพนักงานตามรหัสพนักงาน และรหัสผ่านที่ระบุ" + Environment.NewLine + "กรุณาป้อนรหัสใหม่");
                txtUserId2.SelectAll();
                txtUserId2.Focus();
                return ret;
            }
            var oldPwd = Utils.MD5.Encrypt(txtPassword2.Password);
            if (_user.Password != oldPwd)
            {
                ShowError("รหัสผ่านเก่าไม่ถูกต้อง" + Environment.NewLine + "กรุณาป้อนรหัสผ่านใหม่");
                txtPassword2.SelectAll();
                txtPassword2.Focus();
                return ret;
            }

            var newPwd = Utils.MD5.Encrypt(txtNewPassword.Password);
            var confPwd = Utils.MD5.Encrypt(txtConfirmPassword.Password);
            if (newPwd != confPwd)
            {
                ShowError("ข้อมูลยืนยันรหัสผ่านใหม่ไม่ถูกต้อง" + Environment.NewLine + "กรุณาป้อนรหัสผ่านใหม่");
                txtConfirmPassword.SelectAll();
                txtConfirmPassword.Focus();
                return ret;
            }

            _user.Password = newPwd; // change password.
            _user.PasswordDate = DateTime.Now;

            var saveRet = User.SaveUser(_user);
            if (!saveRet.Ok)
            {
                ShowError("บันทึกข้อมูลไม่สำเร็จ" + Environment.NewLine + "กรุณาลองทำการบันทึกข้อมูลใหม่");
                txtUserId2.SelectAll();
                txtUserId2.Focus();
                return ret;
            }
            ret = true;

            // Write to SCW Message Queue
            var inst = new SCWChangePassword();
            inst.staffId = _user.UserId;
            inst.password = oldPwd;
            inst.newPassword = newPwd;
            inst.confirmNewPassword = confPwd;

            SCWMQService.Instance.WriteQueue(inst);

            return ret;
        }

        private void LogInLog(bool success)
        {
            /*
            var inst = new SCWLogInAudit();
            inst.networkId = (null != Configurations.AccountConfigManager.Instance.DMT) ? 
                Configurations.AccountConfigManager.Instance.DMT.networkId : 31;
            inst.plazaId = ??;
            inst.description = (success) ? "LogIn Success" : "Invalid Password.";
            inst.staffId = _user.UserId;
            inst.status = (success) ? "success" : "fail";

            SCWMQService.Instance.WriteQueue(inst);
            */
        }

        #endregion

        #region Public Methods and Properties

        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="roles">The Role list.</param>
        public void Setup(params string[] roles)
        {
            // Clear Inputs.
            ClearInputs();
            tabs.SelectedIndex = 0;

            _roles.Clear();
            _roles.AddRange(roles);

            SmartcardManager.Instance.Start();

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtUserId.Focus();
            }));
        }
        /// <summary>
        /// Gets current User.
        /// </summary>
        public User User { get { return _user; } }

        #endregion
    }
}
