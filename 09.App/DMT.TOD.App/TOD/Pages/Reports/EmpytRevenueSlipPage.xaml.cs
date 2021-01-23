#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;

using DMT.Configurations;
using DMT.Models;
using DMT.Services;
using DMT.Controls;

using NLib.Services;
using NLib.Reflection;
using System.Threading;
using System.Windows.Threading;

#endregion

namespace DMT.TOD.Pages.Reports
{
    /// <summary>
    /// Interaction logic for EmpytRevenueSlipPage.xaml
    /// </summary>
    public partial class EmpytRevenueSlipPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public EmpytRevenueSlipPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private User _user = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="user">The user instance.</param>
        public void Setup(User user)
        {
            _user = user;
            if (null != _user) 
            { 
            }
        }

        #endregion
    }
}
