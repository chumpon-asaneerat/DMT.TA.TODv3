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

namespace DMT.Windows
{
    /// <summary>
    /// Interaction logic for TSBRequestExchangeWindow.xaml
    /// </summary>
    public partial class TSBRequestExchangeWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBRequestExchangeWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            // Approve and close Window
            DialogResult = true;
        }

        private void cmdReject_Click(object sender, RoutedEventArgs e)
        {
            // Reject and close Window
            DialogResult = true;
        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            // Close Window
            DialogResult = false;
        }

        #endregion

        #region Public Method

        public void Setup(Models.TSBExchangeTransaction request)
        {
            //this.Title = "รายละเอียดคำร้องการขอ/แลก เงินยืมทอน";
            if (null != request)
            {
                entry.Setup(request, new TSBExchangeTransaction());
            }
        }

        #endregion
    }
}
