#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

using DMT.Models;
using DMT.Services;
using NLib.Services;
using NLib;
using NLib.Reflection;

#endregion

namespace DMT.TA.Pages.Exchange
{
    /// <summary>
    /// Interaction logic for RequestExchangePage.xaml
    /// </summary>
    public partial class RequestExchangePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RequestExchangePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private TSBRequestCreditManager manager = null;

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdRequest_Click(object sender, RoutedEventArgs e)
        {
            // New Request
            NewRequest();
        }

        private void cmdRequestEdit_Click(object sender, RoutedEventArgs e)
        {
            // Edit Request
            if (!(sender is Button))
                return;
            var btn = (sender as Button);
            if (null == btn || null == btn.DataContext || !(btn.DataContext is Models.TSBExchangeGroup))
                return;
            var ctx = (btn.DataContext as Models.TSBExchangeGroup);
            if (null == ctx)
                return;
            EditRequest(ctx.PkId);
        }

        private void cmdReceivedDetail_Click(object sender, RoutedEventArgs e)
        {
            // Received.
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TAApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void Refresh()
        {
            gridRequest.ItemsSource = null;

            var items = Models.TSBExchangeGroup.GetRequestExchangeGroups(TAAPI.TSB).Value();

            gridRequest.ItemsSource = items;

            plazaSummary.Setup(); // Call for refresh.
        }

        private void NewRequest()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null == manager)
            {
                med.Info("TSBRequestCreditManager is null.");
                return;
            }

            manager.NewRequest();

            var win = TAApp.Windows.RequestExchange;
            win.Setup(manager);
            if (win.ShowDialog() == false)
            {
                return;
            }

            Refresh();
        }

        private void EditRequest(int requestId)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null == manager)
            {
                med.Info("TSBRequestCreditManager is null.");
            }

            manager.LoadRequest(requestId);

            var win = TAApp.Windows.RequestExchange;
            win.Setup(manager);
            if (win.ShowDialog() == false)
            {
                return;
            }

            Refresh();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            if (null == manager)
            {
                manager = new TSBRequestCreditManager();
            }
            Refresh();
        }

        #endregion
    }
}
