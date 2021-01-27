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
using System.Windows.Threading;
using System.Windows.Media;

#endregion

namespace DMT.Windows
{
    /// <summary>
    /// Interaction logic for MessageBoxYesNo1Window.xaml
    /// </summary>
    public partial class MessageBoxYesNo1Window : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MessageBoxYesNo1Window()
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

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="msg1"></param>
        /// <param name="userName"></param>
        /// <param name="amount"></param>
        /// <param name="head"></param>
        public void Setup(string msg1, string userName, string amount, string head)
        {
            this.Title = head;
            txtMsg1.Text = msg1;
            txtUserName.Text = userName;
            txtAmount.Text = amount;

            // Focus on Ok button.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                cmdOk.Focus();
            }));
        }

        #endregion
    }
}
