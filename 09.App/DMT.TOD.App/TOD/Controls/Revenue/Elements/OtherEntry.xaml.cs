﻿#region Using

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
    /// Interaction logic for OtherEntry.xaml
    /// </summary>
    public partial class OtherEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public OtherEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private Models.RevenueEntry entry = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The Revenue Entry.</param>
        public void Setup(Models.RevenueEntry value)
        {
            entry = value;
            this.DataContext = entry;
        }

        #endregion
    }
}