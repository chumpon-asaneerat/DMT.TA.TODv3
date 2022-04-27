#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;
using System.Windows.Media;
using System.Windows.Threading;

#endregion

namespace DMT.Windows
{
    /// <summary>
    /// Interaction logic for ConfirmApproveWindow.xaml
    /// </summary>
    public partial class ConfirmApproveWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConfirmApproveWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Window Handlers

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                DialogResult = false;
            }
        }

        #endregion

        #region Buton Handlers

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="msg"></param>
        public void Setup(string msg = "")
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                txtMsg.Text = msg;
            }
            // Focus on Ok button.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                cmdOk.Focus();
            }));
        }
    }
}
