#region Using

using System;
using System.Security.Principal;
using System.Windows;
using NLib.Services;

using DMT.Models;
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
            TANotifyService.Instance.TSBChanged += Instance_TSBChanged;
            TANotifyService.Instance.ShiftChanged += Instance_ShiftChanged;

            // Initial Page Content Manager
            PageContentManager.Instance.ContentChanged += new EventHandler(Instance_ContentChanged);
            PageContentManager.Instance.Start();
            // Init Sign In
            var page = new Pages.SignInPage();
            page.Setup(
                "ADMINS",
                "ACCOUNT",
                "CTC_MGR", "CTC", /*"TC",*/
                "MT_ADMIN", "MT_TECH",
                "FINANCE", "SV",
                "RAD_MGR", "RAD_SUP");
            PageContentManager.Instance.Current = page;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            // Release Page Content Manager
            PageContentManager.Instance.Shutdown();
            PageContentManager.Instance.ContentChanged -= new EventHandler(Instance_ContentChanged);

            TANotifyService.Instance.ShiftChanged -= Instance_ShiftChanged;
            TANotifyService.Instance.TSBChanged -= Instance_TSBChanged;
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
