#region Using

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

using DMT.Configurations;
using DMT.Controls;
using DMT.Models;
using DMT.Services;

#endregion

namespace DMT.Account.Controls.Elements
{
    /// <summary>
    /// Interaction logic for TSBRequestTransactionEntry.xaml
    /// </summary>
    public partial class TSBRequestTransactionEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBRequestTransactionEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The TSB Exchange Transaction.</param>
        public void Setup(TSBExchangeTransaction value)
        {
            this.DataContext = value;
        }

        #endregion
    }
}
