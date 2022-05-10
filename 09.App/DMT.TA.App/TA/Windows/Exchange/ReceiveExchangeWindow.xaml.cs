#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.TA.Windows.Exchange
{
    /// <summary>
    /// Interaction logic for ReceiveExchangeWindow.xaml
    /// </summary>
    public partial class ReceiveExchangeWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReceiveExchangeWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private TSBRequestCreditManager _manager = null;

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            // Close Window
            DialogResult = false;
        }

        private void cmdExchange_Click(object sender, RoutedEventArgs e)
        {
            // Accept Exchange and close Window
            if (SaveTransactions())
            {
                DialogResult = true;
            }
        }

        #endregion

        #region Private Methods

        private bool SaveTransactions()
        {
            bool success = false;

            return success;
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="manager">The TSB Request Exchange Manager.</param>
        public void Setup(TSBRequestCreditManager manager)
        {
            this._manager = manager;

            if (null != _manager && null != _manager.Request)
            {
                // Need notify error when some items is missing.
            }

            entry.Setup(manager);
        }

        #endregion
    }
}
