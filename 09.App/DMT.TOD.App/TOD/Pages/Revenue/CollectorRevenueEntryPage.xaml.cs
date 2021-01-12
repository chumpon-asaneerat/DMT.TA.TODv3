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

namespace DMT.TOD.Pages.Revenue
{
    /// <summary>
    /// Interaction logic for CollectorRevenueEntryPage.xaml
    /// </summary>
    public partial class CollectorRevenueEntryPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectorRevenueEntryPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdGotoRevenueEntry_Click(object sender, RoutedEventArgs e)
        {
            tabs.SelectedIndex = 1; // goto next tab.
        }

        private void cmdBack2_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
            // Used below code if need to go back to select date.
            //GotoBack();
        }

        private void cmdGotoRevenueEntryPreview_Click(object sender, RoutedEventArgs e)
        {
            tabs.SelectedIndex = 2; // goto next tab.
        }

        private void cmdBack3_Click(object sender, RoutedEventArgs e)
        {
            GotoBack();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            // Printing.

            // Go Back to Main Menu.
            GotoMainMenu();
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = new Menu.MainMenu();
            PageContentManager.Instance.Current = page;
        }

        private void GotoBack()
        {
            if (tabs.SelectedIndex == 0)
            {
                GotoMainMenu();
            }
            else if (tabs.SelectedIndex == 1)
            {
                tabs.SelectedIndex = 0;
            }
            else if (tabs.SelectedIndex == 2)
            {
                tabs.SelectedIndex = 1;
            }
        }

        #endregion
    }
}
