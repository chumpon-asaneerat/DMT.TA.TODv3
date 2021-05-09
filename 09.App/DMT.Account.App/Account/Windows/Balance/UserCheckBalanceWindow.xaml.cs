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


namespace DMT.Windows
{
    /// <summary>
    /// Interaction logic for UserCheckBalanceWindow.xaml
    /// </summary>
    public partial class UserCheckBalanceWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public UserCheckBalanceWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Window Handlers

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                e.Handled = true;
            }
            else if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                DialogResult = true;
                e.Handled = true;
            }
        }

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region Public Method

        public void Setup(string tsbId)
        {

        }

        #endregion
    }
}
