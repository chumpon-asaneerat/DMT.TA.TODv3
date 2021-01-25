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

namespace DMT.TA.Controls
{
    /// <summary>
    /// Interaction logic for TSBCreditTransactionEntry.xaml
    /// </summary>
    public partial class TSBCreditTransactionEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBCreditTransactionEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The TSB Credit Transaction.</param>
        public void Setup(TSBCreditTransaction value)
        {

        }

        #endregion
    }
}
