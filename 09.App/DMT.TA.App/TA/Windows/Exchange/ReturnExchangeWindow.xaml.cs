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
    /// Interaction logic for ReturnExchangeWindow.xaml
    /// </summary>
    public partial class ReturnExchangeWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReturnExchangeWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private TSBExchangeTransaction _item = null;

        #endregion

        #region Button Handlers

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            var confirm = TAApp.Windows.ConfirmAccountReceiveMoney;
            confirm.Setup((decimal)1200);
            if (confirm.ShowDialog() == false)
            {
                // failed to verify user
                MessageBox.Show("Cancel");
                return;
            }
            else
            {
                // OK Close Window
                DialogResult = true;
            }
        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            // Close Window
            DialogResult = false;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="item">The Exchange Item to return.</param>
        public void Setup(TSBExchangeTransaction item)
        {
            this._item = item;
            if (null != _item)
            {
                
            }
            entry.Setup(_item);
        }

        #endregion
    }
}
