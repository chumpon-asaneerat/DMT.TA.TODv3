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

namespace DMT.TA.Controls
{
    /// <summary>
    /// Interaction logic for UserCreditTransactionEntry.xaml
    /// </summary>
    public partial class UserCreditTransactionEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserCreditTransactionEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The User Credit Transaction.</param>
        public void Setup(UserCreditTransaction value)
        {

        }

        #endregion
    }
}
