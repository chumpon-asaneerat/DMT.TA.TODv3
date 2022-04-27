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
    /// Interaction logic for TSBRequestExchangeEntry.xaml
    /// </summary>
    public partial class TSBRequestExchangeEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBRequestExchangeEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        public void Setup(TSBExchangeTransaction req, TSBExchangeTransaction approve)
        {
            req.HasRemark = true;
            requestEntry.Setup(req);

            approve.HasRemark = true;
            approveEntry.Setup(approve);
        }

        #endregion
    }
}
