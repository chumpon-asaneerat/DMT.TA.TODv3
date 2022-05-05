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
            //tranEntry.Setup(manager.Request);
            //extEntry.Setup(manager.Request);
        }

        #endregion
    }
}
