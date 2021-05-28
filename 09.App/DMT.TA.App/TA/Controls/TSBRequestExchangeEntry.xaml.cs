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

namespace DMT.TA.Controls
{
    /// <summary>
    /// Interaction logic for TSBRequestExchangeEntry.xaml
    /// </summary>
    public partial class TSBRequestExchangeEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBRequestExchangeEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        /// <param name="manager">The TSB Request Exchange Manager.</param>
        public void Setup(TSBRequestCreditManager manager)
        {
            tranEntry.Setup(manager.Request);
            extEntry.Setup(manager.Request);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Period Begin.
        /// </summary>
        public DateTime? PeriodBegin 
        {
            get { return extEntry.PeriodBegin; }
            set { extEntry.PeriodBegin = value; }
        }
        /// <summary>
        /// Gets or sets Period End.
        /// </summary>
        public DateTime? PeriodEnd
        {
            get { return extEntry.PeriodEnd; }
            set { extEntry.PeriodEnd = value; }
        }

        #endregion
    }
}
