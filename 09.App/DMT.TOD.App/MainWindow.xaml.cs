﻿#region Using

using System;
using System.Windows;

using NLib.Services;

//using DMT.Models;
using DMT.Services;

#endregion

namespace DMT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Load/Unload

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TODNotifyService.Instance.TSBChanged += Instance_TSBChanged;
            TODNotifyService.Instance.ShiftChanged += Instance_ShiftChanged;

            // Initial Page Content Manager
            PageContentManager.Instance.ContentChanged += new EventHandler(Instance_ContentChanged);
            PageContentManager.Instance.Start();
            // Init Main Menu
            PageContentManager.Instance.Current = new TOD.Pages.Menu.MainMenu();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            // Release Page Content Manager
            PageContentManager.Instance.Shutdown();
            PageContentManager.Instance.ContentChanged -= new EventHandler(Instance_ContentChanged);

            TODNotifyService.Instance.ShiftChanged -= Instance_ShiftChanged;
            TODNotifyService.Instance.TSBChanged -= Instance_TSBChanged;
        }

        #endregion

        #region Page Content Manager Handlers

        void Instance_ContentChanged(object sender, EventArgs e)
        {
            this.container.Content = PageContentManager.Instance.Current;
        }

        #endregion

        #region Notify Service Handlers

        private void Instance_TSBChanged(object sender, EventArgs e)
        {
            RuntimeManager.Instance.RaiseTSBChanged();
        }

        private void Instance_ShiftChanged(object sender, EventArgs e)
        {
            RuntimeManager.Instance.RaiseShiftChanged();
        }

        #endregion
    }
}
