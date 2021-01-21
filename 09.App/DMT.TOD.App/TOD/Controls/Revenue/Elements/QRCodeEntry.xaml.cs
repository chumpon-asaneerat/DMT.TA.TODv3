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

        private TSB _tsb = null;
        private Models.RevenueEntry entry = null;

        #endregion

        #region DataContext Change Handler

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (null != entry)
            {
                if (entry.IsHistorical)
                {
                    LoadItems();
                }
                else
                {
                    LoadItems();
                }
            }
            else
            {
                UpdateSummary();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load Items.
        /// </summary>
        public void LoadItems()
        {
            if (null == entry) return;
            this.grid.ItemsSource = null;

            if (null == _tsb) _tsb = TSB.GetCurrent().Value();

            this.grid.ItemsSource = null;
        }

        private void UpdateSummary()
        {

        }

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
