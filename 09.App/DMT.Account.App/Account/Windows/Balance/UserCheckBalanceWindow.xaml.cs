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


namespace DMT.Windows
{
    using ops = DMT.Services.Operations.TAxTOD;

    /// <summary>
    /// Interaction logic for UserCheckBalanceWindow.xaml
    /// </summary>
    public partial class UserCheckBalanceWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public UserCheckBalanceWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Window Handlers

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                e.Handled = true;
            }
            else if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                DialogResult = true;
                e.Handled = true;
            }
        }

        private void LoadUserBalance(string tsbId)
        {
            grid.ItemsSource = null;
            var items = ops.Credit.User.Gets(tsbId).Value();
            if (null != items)
            {
                // calc sum at last item.
                var sum = new TAAUserCreditResult();
                sum.username = "รวม";
                sum.Credit = decimal.Zero;
                sum.C35 = 0;
                sum.C80 = 0;
                items.ForEach(item =>
                {
                    sum.Credit += (item.Credit.HasValue) ? item.Credit.Value : decimal.Zero;
                    sum.C35 += (item.C35.HasValue) ? item.C35.Value : 0;
                    sum.C80 += (item.C80.HasValue) ? item.C80.Value : 0;
                });
                items.Add(sum);
            }
            grid.ItemsSource = items;
        }

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region Public Method

        public void Setup(string tsbId)
        {
            LoadUserBalance(tsbId);
        }

        #endregion
    }
}
