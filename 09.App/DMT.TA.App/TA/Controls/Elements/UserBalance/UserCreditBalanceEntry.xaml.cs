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

namespace DMT.TA.Controls.Elements.UserBalance
{
    /// <summary>
    /// Interaction logic for UserCreditBalanceEntry.xaml
    /// </summary>
    public partial class UserCreditBalanceEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserCreditBalanceEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void Refresh()
        {

        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            Refresh();
        }

        #endregion
    }
}
