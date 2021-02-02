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
    /// Interaction logic for TSBCouponBalanceEntry.xaml
    /// </summary>
    public partial class TSBCouponBalanceEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBCouponBalanceEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The Current TSB Coupon Balance.</param>
        public void Setup(TSBCouponBalance value)
        {
            this.DataContext = value;
        }

        #endregion
    }
}
