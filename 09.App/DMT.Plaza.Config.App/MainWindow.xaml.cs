#region Using

using System;
using System.Security.Principal;
using System.Windows;

using NLib.Services;

//using DMT.Models;
using DMT.Services;

using Fluent;

#endregion

namespace DMT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
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

        #region Loaded/Unloaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Initial Page Content Manager
            PageContentManager.Instance.ContentChanged += new EventHandler(Instance_ContentChanged);
            PageContentManager.Instance.Start();
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

        #region Button Handlers

        #region Local Uri/TSB Manager/User View

        private void cmdTSBManage_Click(object sender, RoutedEventArgs e)
        {
            PageContentManager.Instance.Current = new Config.Pages.TSBViewPage();
        }

        private void cmdUserView_Click(object sender, RoutedEventArgs e)
        {
            PageContentManager.Instance.Current = new Config.Pages.UserViewPage();
        }

        private void cmdShiftView_Click(object sender, RoutedEventArgs e)
        {
            PageContentManager.Instance.Current = new Config.Pages.ShiftViewPage();
        }

        #endregion

        #region Credit/Coupon

        private void cmdTSBCreditView_Click(object sender, RoutedEventArgs e)
        {
            //PageContentManager.Instance.Current = new Config.Pages.TSBCreditViewPage();
        }

        #endregion

        #endregion
    }
}
