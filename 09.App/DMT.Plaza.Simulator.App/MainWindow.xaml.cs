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

        private void cmdLaneActivity_Click(object sender, RoutedEventArgs e)
        {
            var page = SimApp.Pages.LaneActivity;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void cmdUserView_Click(object sender, RoutedEventArgs e)
        {
            //PageContentManager.Instance.Current = new Simulator.Pages.UserViewPage();
        }

        private void cmdSupervisorView_Click(object sender, RoutedEventArgs e)
        {
            //PageContentManager.Instance.Current = new Simulator.Pages.SupervisorTaskPage();
        }

        private void cmdTSBCouponView_Click(object sender, RoutedEventArgs e)
        {
            //PageContentManager.Instance.Current = new Simulator.Pages.TSBCouponViewPage();
            var page = SimApp.Pages.BankNoteEntry;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void cmdTSBLaneSoldCoupon_Click(object sender, RoutedEventArgs e)
        {
            //PageContentManager.Instance.Current = new Simulator.Pages.TSBLaneSoldCouponPage();
            var page = SimApp.Pages.TAServerGetCoupon;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void cmdTSBPlazaSoldCoupon_Click(object sender, RoutedEventArgs e)
        {
            var page = SimApp.Pages.TAServerCouponSync;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void cmdSupervisorAdj_Click(object sender, RoutedEventArgs e)
        {
            var page = SimApp.Pages.SubAdjServer;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        #endregion
    }
}
