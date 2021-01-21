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

namespace DMT.TOD.Controls.Revenue.Elements
{
    /// <summary>
    /// Interaction logic for CouponRevenueEntry.xaml
    /// </summary>
    public partial class CouponRevenueEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CouponRevenueEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private Models.RevenueEntry entry = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The Revenue Entry.</param>
        public void Setup(Models.RevenueEntry value)
        {
            entry = value;
            this.DataContext = entry;
        }

        #endregion
    }
}
