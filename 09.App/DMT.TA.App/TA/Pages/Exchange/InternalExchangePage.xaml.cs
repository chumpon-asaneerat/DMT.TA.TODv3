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
    /// Interaction logic for InternalExchangePage.xaml
    /// </summary>
    public partial class InternalExchangePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public InternalExchangePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdExchange_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = new Menu.MainMenu();
            PageContentManager.Instance.Current = page;
        }

        #endregion
    }
}
