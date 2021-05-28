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
            NewRequest();
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

            manager.Save();

            Refresh();
        }

        private void EditRequest()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null == manager)
            {
                med.Info("TSBRequestCreditManager is null.");
            }

            int requestId = 0;
            manager.LoadRequest(requestId);

            var win = TAApp.Windows.RequestExchange;
            win.Setup(manager);
            if (win.ShowDialog() == false)
            {
                return;
            }

            manager.Save();

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
