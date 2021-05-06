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

        #endregion

        #region Window Handlers

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

        public void Setup(string filter)
        {
            var ret = ops.GetCustomers(Models.Search.TAxTOD.SAP.Customers.Create(filter));
            if (null != ret && ret.Ok)
            {
                var list = ret.Value();
                /*
                list.ForEach(item =>
                {
                    Console.WriteLine("Code: {0} - Name: {1}", item.CardCode, item.CardName);
                });
                */
            }

        }

        #endregion

        #region Public Properties

        #endregion
    }
}
