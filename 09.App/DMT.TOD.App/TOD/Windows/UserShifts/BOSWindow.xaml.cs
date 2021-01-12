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

namespace DMT.TOD.Windows.UserShifts
{
    /// <summary>
    /// Interaction logic for BOSWindow.xaml
    /// </summary>
    public partial class BOSWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public BOSWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        #endregion
    }
}
