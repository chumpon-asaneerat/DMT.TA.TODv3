#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.Simulator.Windows
{
    using localOps = Services.Operations.Plaza; // reference to static class.

    /// <summary>
    /// Interaction logic for UserListWindow.xaml
    /// </summary>
    public partial class UserListWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserListWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private List<User> _users = null;
        private List<User> _filterUsers = null;
        private string _lastFilter = string.Empty;
        private bool _onFiltering = false;
        private DispatcherTimer timer = null;

        #endregion

        #region Loaded/Unloaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(.25);
            timer.Start();

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtUserId.Focus();
            }));
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (null != timer)
            {
                timer.Tick -= Timer_Tick;
                timer.Stop();
            }
            timer = null;
        }

        #endregion

        #region PreviewKeyDown

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (!string.IsNullOrEmpty(txtUserId.Text))
                {
                    txtUserId.Text = string.Empty;
                    txtUserId.Focus();
                }
                else
                {
                    DialogResult = false;
                }
            }
        }

        #endregion

        #region Button Handlers

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region ListBox Handlers

        private void lstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = lstUsers.SelectedIndex;
            if (idx == -1) return;
            User = _filterUsers[idx];
        }

        private void lstUsers_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int idx = lstUsers.SelectedIndex;
            if (idx == -1) return;
            User = _filterUsers[idx];
            if (null == User) return;
            DialogResult = true;
        }

        #endregion

        #region TextBox Handlers

        private void txtUserId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckUserSelection();
            }
            else if (e.Key == Key.Escape)
            {
                txtUserId.Text = string.Empty;
            }
        }

        #endregion

        #region Timer Handler

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_onFiltering) return;
            _onFiltering = true;

            if (_lastFilter != txtUserId.Text.Trim())
            {
                _lastFilter = txtUserId.Text.Trim();
                ApplyFilter();
            }

            _onFiltering = false;
        }

        #endregion

        #region Private Methods

        private void ApplyFilter()
        {
            lstUsers.ItemsSource = null;

            _filterUsers = _users.FindAll(user =>
            {
                return user.UserId.Contains(_lastFilter);
            });

            lstUsers.ItemsSource = _filterUsers;
        }

        private void CheckUserSelection()
        {
            if (null != _filterUsers && _filterUsers.Count == 1)
            {
                // Auto choose user because has only one in filter list.
                User = _filterUsers[0];
                DialogResult = true;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="users">The exclude user id list.</param>
        public void Setup(params string[] users)
        {
            lstUsers.ItemsSource = null;
            // Load Users.
            var allusers = localOps.Security.User.Gets().Value();
            
            if (null != users && users.Length > 0)
            {
                var excludes = new List<string>(users);
                // filter out all user on lanes.
                _users = allusers.FindAll(usr =>
                {
                    return !excludes.Contains(usr.UserId);
                });
            }
            else
            {
                _users = allusers;
            }

            _lastFilter = string.Empty;
            txtUserId.Text = string.Empty;
            _filterUsers = _users;
            lstUsers.ItemsSource = _filterUsers;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets selected user.
        /// </summary>
        public User User { get; private set; }

        #endregion
    }
}
