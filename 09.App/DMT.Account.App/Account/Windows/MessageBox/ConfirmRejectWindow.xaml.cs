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
    /// Interaction logic for ConfirmRejectWindow.xaml
    /// </summary>
    public partial class ConfirmRejectWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConfirmRejectWindow()
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
            if (string.IsNullOrWhiteSpace(txtReason.Text) && paReasonBox.Visibility == Visibility.Visible)
            {
                var win = AccountApp.Windows.MessageBox;
                win.Setup("กรุณาระบุเหตุผลการไม่อนุมัติ.", "DMT - TA (Account)");
                win.ShowDialog();
                // Focus on Ok button.
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    txtReason.Focus();
                }));
                return;
            }
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
        /// <param name="hideReasonBox"></param>
        /// <param name="msg"></param>
        public void Setup(bool hideReasonBox, string msg = "")
        {
            if (hideReasonBox)
                paReasonBox.Visibility = Visibility.Collapsed;
            else paReasonBox.Visibility = Visibility.Visible;

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
        /// <summary>
        /// Gets reason.
        /// </summary>
        public string Reason
        {
            get { return txtReason.Text; }
        }
    }
}
