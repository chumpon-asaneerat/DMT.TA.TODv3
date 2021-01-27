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
            // Initial Page Content Manager
            PageContentManager.Instance.ContentChanged += new EventHandler(Instance_ContentChanged);
            PageContentManager.Instance.Start();
            // Init Sign In
            var page = TAApp.Pages.SignIn;
            page.Setup(TAApp.Permissions.CTC);
            PageContentManager.Instance.Current = page;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            // Release Page Content Manager
            PageContentManager.Instance.Shutdown();
            PageContentManager.Instance.ContentChanged -= new EventHandler(Instance_ContentChanged);
        }

        #endregion

        #region Page Content Manager Handlers

        void Instance_ContentChanged(object sender, EventArgs e)
        {
            this.container.Content = PageContentManager.Instance.Current;
        }

        #endregion
    }
}
