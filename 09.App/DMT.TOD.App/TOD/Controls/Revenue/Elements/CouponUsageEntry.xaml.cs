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
    /// Interaction logic for CouponUsageEntry.xaml
    /// </summary>
    public partial class CouponUsageEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CouponUsageEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private RevenueEntryManager manager = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The RevenueEntryManager instance.</param>
        public void Setup(RevenueEntryManager value)
        {
            // reset data context
            this.DataContext = null;

            manager = value;
            this.DataContext = (null != manager) ? manager.Entry : null;
        }

        #endregion
    }
}
