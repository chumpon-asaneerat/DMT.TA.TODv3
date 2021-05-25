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

namespace DMT.TA.Windows.Exchange
{
    /// <summary>
    /// Interaction logic for RequestExchangeWindow.xaml
    /// </summary>
    public partial class RequestExchangeWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RequestExchangeWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            // Cancel Request and close window
            DialogResult = true;
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            // Save or Update Request and close window
            DialogResult = true;
        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            // Close Window
            DialogResult = false;
        }

        #endregion

        #region Public Method

        public void Setup()
        {

        }

        #endregion
    }
}
