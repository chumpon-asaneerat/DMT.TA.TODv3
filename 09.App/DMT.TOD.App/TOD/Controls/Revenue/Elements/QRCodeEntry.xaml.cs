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

namespace DMT.TOD.Controls.Revenue.Elements
{
    /// <summary>
    /// Interaction logic for QRCodeEntry.xaml
    /// </summary>
    public partial class QRCodeEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public QRCodeEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private RevenueEntry entry = null;

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
        }

        #endregion
    }
}
