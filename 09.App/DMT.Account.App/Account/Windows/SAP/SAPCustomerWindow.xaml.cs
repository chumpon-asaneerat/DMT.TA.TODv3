#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;

#endregion

using ops = DMT.Services.Operations.TAxTOD.SAP;

namespace DMT.Windows
{
    /// <summary>
    /// Interaction logic for SAPCustomerWindow.xaml
    /// </summary>
    public partial class SAPCustomerWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SAPCustomerWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private List<SAPCustomer> _customers = null;

        #endregion

        #region Button Handlers

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="filter">The filter text.</param>
        public void Setup(string filter)
        {
            _customers = null;

            var ret = ops.GetCustomers(Models.Search.TAxTOD.SAP.Customers.Create(filter));
            if (null != ret && ret.Ok)
            {
                _customers = ret.Value();
            }

            grid.ItemsSource = _customers;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets selected customer.
        /// </summary>
        public SAPCustomer SelectedCustomer
        {
            get 
            {
                if (null == _customers || grid.SelectedIndex < 0) return null;
                if (grid.SelectedIndex >= _customers.Count) return null;
                return _customers[grid.SelectedIndex];
            }
        }

        #endregion
    }
}
