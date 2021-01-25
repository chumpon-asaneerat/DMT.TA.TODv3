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
    /// Interaction logic for TSBPlazaCreditSummaryEntry.xaml
    /// </summary>
    public partial class TSBPlazaCreditSummaryEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBPlazaCreditSummaryEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private TSBCreditBalance _balance = null;

        #endregion

        #region Private Methods

        private void Refresh()
        {
            _balance = TSBCreditBalance.GetCurrent().Value();
            if (null != _balance)
            {
                this.DataContext = _balance;
                this.balanceEntry.DataContext = _balance;
                this.sumEntry.DataContext = _balance;
            }
            else
            {
                this.DataContext = null;
                this.balanceEntry.DataContext = null;
                this.sumEntry.DataContext = null;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            Refresh();
        }

        #endregion
    }
}
