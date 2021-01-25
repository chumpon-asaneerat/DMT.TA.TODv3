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
    /// Interaction logic for TSBCreditBalanceEntry.xaml
    /// </summary>
    public partial class TSBCreditBalanceEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBCreditBalanceEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The TSB Credit Balance.</param>
        public void Setup(TSBCreditBalance value)
        {

        }

        #endregion
    }
}
