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
