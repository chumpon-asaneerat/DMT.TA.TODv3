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

        private RevenueEntryManager manager = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The RevenueEntryManager instance.</param>
        public void Setup(RevenueEntryManager value)
        {
            manager = value;

            if (null != manager)
            {
                this.DataContext = manager.Entry;

                // Load EMV/QR Code
                DateTime dt1 = manager.Entry.ShiftBegin.Value;
                DateTime dt2 = manager.Entry.ShiftEnd.Value;

                manager.Payments.EnableLaneFilter = false;
                manager.Payments.PlazaGroup = manager.PlazaGroup;
                manager.Payments.PaymentType = PaymentTypes.Both;
                manager.Payments.Begin = dt1;
                manager.Payments.End = dt2;
                manager.Payments.Refresh();
            }

            this.trafficRevenue.Setup(manager);
            this.otherRevenue.Setup(manager);
            this.freePass.Setup(manager);
            this.couponSold.Setup(manager);
            this.couponUsage.Setup(manager);
            this.emvEntry.Setup(manager);
            this.qrcodeEntry.Setup(manager);

            tabs.SelectedIndex = 0; // Reset Tab index.

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
