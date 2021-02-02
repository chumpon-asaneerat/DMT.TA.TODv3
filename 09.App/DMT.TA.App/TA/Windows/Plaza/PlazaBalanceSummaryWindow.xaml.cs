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

namespace DMT.TA.Windows.Plaza
{
    /// <summary>
    /// Interaction logic for PlazaBalanceSummaryWindow.xaml
    /// </summary>
    public partial class PlazaBalanceSummaryWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlazaBalanceSummaryWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private CurrentTSBManager _manager = null;

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Close Window and Goto Main Menu
            DialogResult = false;
        }

        private void Refresh()
        {
            if (null == _manager) _manager = new CurrentTSBManager();
            _manager.Refresh();

            var _balance = _manager.Credit.TSBBalance;
            if (null == _balance)
            {
                _balance = new TSBCreditBalance(); // Create Empty Balance.
            }

            _balance.Description = "เงินยืมทอนหมุนเวียนด่าน";
            this.DataContext = _balance;
            this.creditBalanceEntry.Setup(_balance);
            
            this.couponBalanceEntry.Setup(_manager.TSBCouponBalance);

            this.creditSummaryEntry.Setup(_balance);
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
