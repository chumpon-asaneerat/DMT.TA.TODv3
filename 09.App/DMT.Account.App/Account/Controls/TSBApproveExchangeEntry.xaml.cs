#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Configurations;
using DMT.Controls;
using DMT.Models;
using DMT.Services;

#endregion

namespace DMT.Account.Controls
{
    /// <summary>
    /// Interaction logic for TSBApproveExchangeEntry.xaml
    /// </summary>
    public partial class TSBApproveExchangeEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBApproveExchangeEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        public void Setup(TSBExchangeTransaction approve)
        {
            approve.HasRemark = true;
            approveEntry.Setup(approve);
        }

        #endregion
    }
}
