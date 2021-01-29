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
    /// <summary>
    /// Interaction logic for TAServerCouponSyncPage.xaml
    /// </summary>
    public partial class TAServerCouponSyncPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TAServerCouponSyncPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmdStart.IsEnabled = true;
            cmdShutdown.IsEnabled = !cmdStart.IsEnabled;

            progress.Value = 0;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Button Handlers

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            cmdStart.IsEnabled = false;
            cmdShutdown.IsEnabled = !cmdStart.IsEnabled;
            Start();
        }

        private void cmdShutdown_Click(object sender, RoutedEventArgs e)
        {
            cmdStart.IsEnabled = true;
            cmdShutdown.IsEnabled = !cmdStart.IsEnabled;
            Shutdown();
        }

        #endregion

        #region Private Methods

        private void Start()
        {

        }

        private void Shutdown()
        {

        }

        #endregion

        #region Public Methods

        public void Setup()
        {

        }

        #endregion
    }
}
