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

namespace DMT.TA.Controls.Elements.TSBBalance
{
    /// <summary>
    /// Interaction logic for TSBCreditSummaryEntry.xaml
    /// </summary>
    public partial class TSBCreditSummaryEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBCreditSummaryEntry()
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
