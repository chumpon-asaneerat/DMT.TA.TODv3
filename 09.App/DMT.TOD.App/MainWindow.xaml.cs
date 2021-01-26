#region Using

using System;
using System.Windows;

using NLib.Services;

//using DMT.Models;
using DMT.Services;

#endregion

namespace DMT
{
    using taOps = Services.Operations.TA.Notify;

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
            TODNotifyService.Instance.TSBShiftChanged += Instance_TSBShiftChanged;
            TODNotifyService.Instance.UserShiftChanged += Instance_UserShiftChanged;

            // Initial Page Content Manager
            PageContentManager.Instance.ContentChanged += new EventHandler(Instance_ContentChanged);
            PageContentManager.Instance.Start();
            // Init Main Menu
            var page = TODApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;

            // Raise Event to notify TA App.
            taOps.TSBShiftChanged();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            // Release Page Content Manager
            PageContentManager.Instance.Shutdown();
            PageContentManager.Instance.ContentChanged -= new EventHandler(Instance_ContentChanged);

            TODNotifyService.Instance.UserShiftChanged -= Instance_UserShiftChanged;
            TODNotifyService.Instance.TSBShiftChanged -= Instance_TSBShiftChanged;
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

        private void Instance_TSBShiftChanged(object sender, EventArgs e)
        {
            RuntimeManager.Instance.RaiseUserShiftChanged();
        }

        private void Instance_UserShiftChanged(object sender, EventArgs e)
        {
            RuntimeManager.Instance.RaiseUserShiftChanged();
        }

        #endregion
    }
}
