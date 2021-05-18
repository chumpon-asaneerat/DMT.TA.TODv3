#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using DMT.Models;
using DMT.Services;
using DMT.Controls;
using System.Windows.Threading;
using NLib;
using System.Reflection;

#endregion

namespace DMT.Windows
{
    /// <summary>
    /// Interaction logic for UserSearchWindow.xaml
    /// </summary>
    public partial class UserSearchWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserSearchWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private List<User> _users = null;

        #endregion

        #region Window Handlers

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                e.Handled = true;
            }
        }

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        #endregion

        #region ListView Handlers

        private void lvUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CheckSelection();
        }

        private void lvUsers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                CheckSelection();
                e.Handled = true;
            }
        }

        #endregion

        #region Private Methods

        private void CheckSelection()
        {
            var usr = lvUsers.SelectedItem as User;
            if (null != usr)
            {
                DialogResult = true;
            }
        }

        #endregion

        #region Public Methods and Properties

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="users">The User List.</param>
        public void Setup(List<User> users)
        {
            lvUsers.ItemsSource = null;

            _users = users;

            lvUsers.ItemsSource = _users;

            // Focus on ListView.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                lvUsers.Focus();
            }));
        }
        /// <summary>
        /// Gets Selected User.
        /// </summary>
        public User SelectedUser
        {
            get { return lvUsers.SelectedItem as User; }
        }

        #endregion
    }
}
