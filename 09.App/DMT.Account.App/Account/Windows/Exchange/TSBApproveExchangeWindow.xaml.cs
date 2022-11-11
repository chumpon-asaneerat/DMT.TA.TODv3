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
    /// Interaction logic for TSBApproveExchangeWindow.xaml
    /// </summary>
    public partial class TSBApproveExchangeWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBApproveExchangeWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            // Close Window
            DialogResult = false;
        }

        private void cmdReject_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            // Change buttons Visibility
            cmdReject.Visibility = Visibility.Collapsed;
            cmdEdit.Visibility = Visibility.Collapsed;
            cmdCancelEdit.Visibility = Visibility.Visible;
            cmdSaveEdit.Visibility = Visibility.Visible;
        }

        private void cmdCancelEdit_Click(object sender, RoutedEventArgs e)
        {
            // Restore buttons Visibility
            cmdReject.Visibility = Visibility.Visible;
            cmdEdit.Visibility = Visibility.Visible;
            cmdCancelEdit.Visibility = Visibility.Collapsed;
            cmdSaveEdit.Visibility = Visibility.Collapsed;
        }

        private void cmdSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            // Restore buttons Visibility
            cmdReject.Visibility = Visibility.Visible;
            cmdEdit.Visibility = Visibility.Visible;
            cmdCancelEdit.Visibility = Visibility.Collapsed;
            cmdSaveEdit.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Public Method

        public void Setup(Models.TSBExchangeTransaction approve)
        {
            if (null != approve)
            {
                this.Title = "รายละเอียดคำร้องการขอ/แลก เงินยืมทอนที่อนุมัติแล้ว - " + approve.TSBNameTH;
                entry.Setup(approve);
            }
            else
            {
                this.Title = "รายละเอียดคำร้องการขอ/แลก เงินยืมทอนที่อนุมัติแล้ว ";
            }
        }

        #endregion
    }
}
