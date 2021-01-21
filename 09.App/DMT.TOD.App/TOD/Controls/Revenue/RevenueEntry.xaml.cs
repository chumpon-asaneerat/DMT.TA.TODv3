#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
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

        private Models.RevenueEntry entry = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Load Payments.
        /// </summary>
        public void LoadPayments()
        {
            this.emvEntry.LoadItems();
            this.qrcodeEntry.LoadItems();
        }
        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The Revenue Entry.</param>
        public void Setup(Models.RevenueEntry value)
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

            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtBagNo.SelectAll();
                txtBagNo.Focus();
            }));
        }
        /// <summary>
        /// Set Bag No Focus.
        /// </summary>
        public void FocusBagNo()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtBagNo.Focus();
                txtBagNo.SelectAll();
            }));
        }
        /// <summary>
        /// Set Belt No Focus.
        /// </summary>
        public void FocusBeltNo()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                txtBeltNo.Focus();
                txtBeltNo.SelectAll();
            }));
        }
        /// <summary>
        /// Checks Has BagNo.
        /// </summary>
        public bool HasBagNo { get { return !string.IsNullOrWhiteSpace(txtBagNo.Text); } }
        /// <summary>
        /// Checks Has Belt.
        /// </summary>
        public bool HasBeltNo { get { return !string.IsNullOrWhiteSpace(txtBeltNo.Text); } }

        #endregion
    }
}
