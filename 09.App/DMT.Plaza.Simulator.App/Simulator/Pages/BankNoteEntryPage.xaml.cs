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
    using taaops = Services.Operations.TA.Infrastructure; // reference to static class.

    /// <summary>
    /// Interaction logic for BankNotEntryPage.xaml
    /// </summary>
    public partial class BankNoteEntryPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public BankNoteEntryPage()
        {
            InitializeComponent();
        }

        #endregion

        public void Setup()
        {
            var tran = new Models.TSBCreditTransaction();
            entry.DataContext = tran;

            var tran2 = new Models.TSBCreditTransaction();
            entry2.DataContext = tran2;

            var tran3 = new Models.TSBCreditTransaction();
            entry3.DataContext = tran3;
        }
    }
}
