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

        private CurrentTSBManager _manager = null;

        #endregion

        #region Private Methods

        private void Refresh()
        {
            if (null == _manager) _manager = new CurrentTSBManager();
            _manager.Refresh();

            var _balance = _manager.Credit.TSBBalance;
            if (null == _balance)
            {
                _balance = new TSBCreditBalance(); // Create Empty Balance.
            }
            _balance.Description = "ยอดที่สามารถยืมได้";
            this.DataContext = _balance;
            this.balanceEntry.Setup(_balance);
            this.sumEntry.Setup(_balance);
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
