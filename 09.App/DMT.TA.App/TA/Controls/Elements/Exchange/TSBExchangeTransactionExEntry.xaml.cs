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

namespace DMT.TA.Controls.Elements
{
    /// <summary>
    /// Interaction logic for TSBExchangeTransactionExEntry.xaml
    /// </summary>
    public partial class TSBExchangeTransactionExEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBExchangeTransactionExEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The TSB Exchange Transaction.</param>
        public void Setup(TSBExchangeTransaction value)
        {
            this.DataContext = value;
        }

        #endregion
    }
}
