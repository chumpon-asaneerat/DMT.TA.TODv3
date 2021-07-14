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

        #region Private Methods

        private void CalculateTimeFrame()
        {
            if (null == manager) return;
            // Load EMV/QR Code
            DateTime dt1 = DateTime.MinValue;
            DateTime dt2 = DateTime.MinValue;
            if (manager.ByChief)
            {
                if (null != manager.Jobs && null !=
                    manager.Jobs.PlazaGroupJobs && manager.Jobs.PlazaGroupJobs.Count > 0)
                {
                    #region

                    int idx;
                    int iCnt = manager.Jobs.PlazaGroupJobs.Count;

                    // Find begin date.
                    idx = 0;
                    while (dt1 == DateTime.MinValue && idx < iCnt)
                    {
                        var job = manager.Jobs.PlazaGroupJobs[idx];
                        if (null == job || !job.Selected)
                        {
                            idx++;
                            continue;
                        }
                        if (job.Begin.HasValue) dt1 = job.Begin.Value;
                        else if (job.End.HasValue) dt1 = job.End.Value;

                        if (dt1 != DateTime.MinValue) break;

                        idx++;
                    }
                    // Find end date.
                    idx = iCnt - 1;
                    while (dt2 == DateTime.MinValue && idx >= 0)
                    {
                        var job = manager.Jobs.PlazaGroupJobs[idx];
                        if (null == job || !job.Selected)
                        {
                            // Skip unselected job item.
                            idx--;
                            continue;
                        }
                        if (job.End.HasValue)
                        {
                            // End Time is not null.
                            dt2 = job.End.Value;
                        }
                        else dt2 = DateTime.Now; // End Time is null.

                        if (dt2 != DateTime.MinValue) break;

                        idx--;
                    }

                    #endregion
                }
                else
                {
                    dt1 = manager.Entry.ShiftBegin.Value;
                    dt2 = manager.Entry.ShiftEnd.Value;
                }
            }
            else
            {
                dt1 = manager.Entry.ShiftBegin.Value;
                dt2 = manager.Entry.ShiftEnd.Value;
            }

            manager.Payments.EnableLaneFilter = false;
            manager.Payments.PlazaGroup = manager.PlazaGroup;
            manager.Payments.PaymentType = PaymentTypes.Both;
            manager.Payments.Begin = dt1;
            manager.Payments.End = dt2;
            manager.Payments.Refresh();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The RevenueEntryManager instance.</param>
        public void Setup(RevenueEntryManager value)
        {
            manager = value;

            // Reset data context
            this.DataContext = null;

            if (null != manager)
            {
                this.DataContext = manager.Entry;

                CalculateTimeFrame();
            }

            this.trafficRevenue.Setup(manager);
            this.otherRevenue.Setup(manager);
            this.nonRevenue.Setup(manager);
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
            if (tabs.SelectedIndex != 0) tabs.SelectedIndex = 0;

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
            if (tabs.SelectedIndex != 0) tabs.SelectedIndex = 0;

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
