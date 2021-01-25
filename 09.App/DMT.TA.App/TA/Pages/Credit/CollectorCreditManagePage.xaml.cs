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

namespace DMT.TA.Pages.Credit
{
    /// <summary>
    /// Interaction logic for CollectorCreditManagePage.xaml
    /// </summary>
    public partial class CollectorCreditManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectorCreditManagePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdAddCollector_Click(object sender, RoutedEventArgs e)
        {
            AddUserTransaction();
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

        private void AddUserTransaction()
        {
            var win = TAApp.Windows.CollectorCreditBorrow;
            win.Owner = Application.Current.MainWindow;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }
            Refresh();
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
