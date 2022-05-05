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
    /// Interaction logic for TSBReceivedExchangeEntry.xaml
    /// </summary>
    public partial class TSBReceivedExchangeEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBReceivedExchangeEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        /// <param name="manager">The TSB Request Exchange Manager.</param>
        public void Setup(TSBRequestCreditManager manager)
        {
            // Request
            requestEntry.Setup(manager.Request);
            requestEntry.IsEnabled = false; // always read only
            // Approve
            approveEntry.Setup(manager.Approve);
            approveEntry.IsEnabled = false; // always read only
            // Receive
            receivedEntry.Setup(manager.Receive);
            // Exchange (to account dept.)
            exchangeOutEntry.Setup(manager.ExchangeOut);
        }

        #endregion
    }
}
