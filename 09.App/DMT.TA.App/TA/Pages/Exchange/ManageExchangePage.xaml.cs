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

namespace DMT.TA.Pages.Exchange
{
    /// <summary>
    /// Interaction logic for ManageExchangePage.xaml
    /// </summary>
    public partial class ManageExchangePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ManageExchangePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdReturnExchange_Click(object sender, RoutedEventArgs e)
        {
            // FOR TEST ACCOUNT RECEIVE MONEY DIALOG
            var confirm = TAApp.Windows.ConfirmAccountReceiveMoney;
            confirm.Setup((decimal)1200);
            if (confirm.ShowDialog() == false)
            {
                // failed to verify user
                MessageBox.Show("Cancel");
            }
            else
            {
                // OK
                MessageBox.Show("OK");
            }
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TAApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void Refresh()
        {
            plazaSummary.Setup(); // Call for refresh.
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            Refresh();
        }

        #endregion
    }
}
