#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using DMT.Configurations;
using DMT.Controls;
using DMT.Models;
using DMT.Services;

#endregion

namespace DMT.TOD.Controls.Revenue.Elements
{
    using scwOps = Services.Operations.SCW.TOD;

    /// <summary>
    /// Interaction logic for EMVEntry.xaml
    /// </summary>
    public partial class EMVEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public EMVEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private RevenueEntryManager manager = null;

        private int rowCnt = 0;
        private decimal amtVal = 0;

        #endregion

        #region DataContext Change Handler

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (null != manager && null != manager.Entry)
            {
                if (manager.Entry.IsHistorical)
                {
                    UpdateList();
                }
                else
                {
                    UpdateList();
                }
            }
            else
            {
                UpdateSummary();
            }
        }

        #endregion

        #region Private Methods

        private void UpdateList()
        {
            grid.ItemsSource = null;

            List<LaneEMV> results = new List<LaneEMV>();

            if (null != manager && null != manager.Payments)
            {
                results = manager.Payments.EMVItems;
            }

            // Calculate Summary.
            if (null != results && results.Count > 0)
            {
                rowCnt = results.Count;
                amtVal = decimal.Zero;
                results.ForEach(item =>
                {
                    amtVal += item.Amount;
                });
            }
            else
            {
                rowCnt = 0;
                amtVal = decimal.Zero;
            }

            UpdateSummary();

            grid.ItemsSource = results;
        }

        private void UpdateSummary()
        {
            txtQty.Text = rowCnt.ToString("n0");
            txtTotal.Text = amtVal.ToString("n0");
        }

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
            UpdateList();
        }

        #endregion
    }
}
