#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;
using System.Windows.Media;

#endregion

namespace DMT.Windows
{
    /// <summary>
    /// Interaction logic for MessageBoxYesNo3Window.xaml
    /// </summary>
    public partial class MessageBoxYesNo3Window : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MessageBoxYesNo3Window()
        {
            InitializeComponent();
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
        /// <param name="msg1"></param>
        /// <param name="msg2"></param>
        /// <param name="msg3"></param>
        /// <param name="msg4"></param>
        /// <param name="msg5"></param>
        /// <param name="msg6"></param>
        /// <param name="msg7"></param>
        /// <param name="msg8"></param>
        /// <param name="msg9"></param>
        /// <param name="title"></param>
        public void Setup(string msg1, string msg2, string msg3, string msg4, string msg5
            , string msg6, string msg7, string msg8, string msg9, string title)
        {
            this.Title = title;
            txtMsg1.Text = msg1;
            txtMsg2.Text = msg2;
            txtMsg3.Text = msg3;
            txtMsg4.Text = msg4;
            txtMsg5.Text = msg5;

            txtMsg6.Text = msg6;
            txtMsg7.Text = msg7;
            txtMsg8.Text = msg8;
            txtMsg9.Text = msg9;
        }
    }
}
