#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using DMT.Models;
using DMT.Configurations;
using DMT.Services;
using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.Simulator.Pages
{
    using taServerops = Services.Operations.TAxTOD; // reference to static class.

    /// <summary>
    /// Interaction logic for TAServerGetCoupon.xaml
    /// </summary>
    public partial class TAServerGetCoupon : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TAServerGetCoupon()
        {
            InitializeComponent();
        }

        #endregion

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        #region Public Methods

        private void Refresh()
        {
            grid.ItemsSource = null;
            var para = (pGrid1.SelectedObject as Search.TAxTOD.Coupon.Gets);
            if (null != para)
            {
                var ret = taServerops.Coupon.Gets(para);
                pGrid2.SelectedObject = ret.Output;
                grid.ItemsSource = ret.Value();
            }
        }

        public void Setup()
        {
            NRestOut ret = new NRestOut();

            pGrid1.SelectedObject = Search.TAxTOD.Coupon.Gets.Create("319", null, 1, 35, 1, 20);
        }

        #endregion
    }
}
