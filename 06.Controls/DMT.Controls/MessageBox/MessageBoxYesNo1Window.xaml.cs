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
        /// <param name="msg2"></param>
        /// <param name="red"></param>
        /// <param name="head"></param>
        public void Setup(string msg1, string msg2, bool red, string head)
        {
            this.Title = head;
            txtMsg1.Text = msg1;
            txtMsg2.Text = msg2;

            if (red)
            {
                txtMsg1.Foreground = new SolidColorBrush(Colors.Red);
                txtMsg2.Foreground = new SolidColorBrush(Colors.Red);
            }

            // Focus on Ok button.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                cmdOk.Focus();
            }));
        }

        #endregion
    }
}
