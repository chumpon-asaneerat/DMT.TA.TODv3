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

        private TSBCreditBalance _creditBalance = null;
        //TODO: Required Coupon Models.
        //private TSBCouponBalance _couponBalance = null;

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
            _creditBalance = TSBCreditBalance.GetCurrent().Value();
            if (null == _creditBalance)
            {
                _creditBalance = new TSBCreditBalance(); // Create Empty Balance.
            }

            //if (null == _couponBalance)
            //{
            //    _couponBalance = new TSBCouponBalance();
            //}

            _creditBalance.Description = "เงินยืมทอนหมุนเวียนด่าน";
            this.DataContext = _creditBalance;
            this.creditBalanceEntry.Setup(_creditBalance);
            //this.couponEntryEntry.Setup(_couponBalance);
            this.creditSummaryEntry.Setup(_creditBalance);
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
