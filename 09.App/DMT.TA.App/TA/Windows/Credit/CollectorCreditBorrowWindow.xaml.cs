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

        private User _user = null;

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

        private void cmdUserSearch_Click(object sender, RoutedEventArgs e)
        {

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

        private void SearchUser()
        {

        }

        private void ResetSelectUser()
        {

        }

        #endregion

        #region Public Methods

        public void Setup()
        {

        }

        #endregion
    }
}
