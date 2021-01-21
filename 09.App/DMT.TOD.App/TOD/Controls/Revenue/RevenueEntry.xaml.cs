#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using DMT.Configurations;
using DMT.Controls;
using DMT.Models;
using DMT.Services;

#endregion

namespace DMT.TOD.Controls.Revenue
{
    /// <summary>
    /// Interaction logic for RevenueEntry.xaml
    /// </summary>
    public partial class RevenueEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RevenueEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private RevenueEntry entry = null;

        #endregion

        #region TextBox Handlers

        private void txtBagNo_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtBeltNo_KeyDown(object sender, KeyEventArgs e)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The Revenue Entry.</param>
        public void Setup(RevenueEntry value)
        {
            entry = value;
            this.DataContext = entry;

            this.trafficRevenue.Setup(entry);
            this.otherRevenue.Setup(entry);
            this.freePass.Setup(entry);
            this.couponSold.Setup(entry);
            this.couponUsage.Setup(entry);
            this.emvEntry.Setup(entry);
            this.qrcodeEntry.Setup(entry);
        }

        #endregion
    }
}
