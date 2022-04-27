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
