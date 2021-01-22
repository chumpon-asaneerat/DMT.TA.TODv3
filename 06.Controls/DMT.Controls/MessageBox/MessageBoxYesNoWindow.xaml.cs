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

#endregion

namespace DMT.Windows
{
    /// <summary>
    /// Interaction logic for MessageBoxYesNoWindow.xaml
    /// </summary>
    public partial class MessageBoxYesNoWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MessageBoxYesNoWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Window Handlers

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                e.Handled = true;
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
        /// <param name="msg"></param>
        /// <param name="head"></param>
        public void Setup(string msg, string head)
        {
            this.Title = head;
            txtMsg.Text = msg;

            // Focus on Ok button.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                cmdOk.Focus();
            }));
        }

        #endregion
    }
}
