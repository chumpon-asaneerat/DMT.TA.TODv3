#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Reflection;

using DMT.Models;
using DMT.Services;

using NLib;
using NLib.Services;
using NLib.Reflection;
using System.Text.RegularExpressions;

#endregion

namespace DMT.TA.Pages.Reservation
{
    using ops = DMT.Services.Operations.TAxTOD.SAP2;

    /// <summary>
    /// Interaction logic for ReservationHistoryViewPage.xaml
    /// </summary>
    public partial class ReservationHistoryViewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ReservationHistoryViewPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        private CultureInfo culture = new CultureInfo("th-TH");
        private XmlLanguage language = XmlLanguage.GetLanguage("th-TH");

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup DateTime Picker
            dtCreateDate.CultureInfo = culture;
            dtCreateDate.Language = language;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Button Handlers

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenu();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        #endregion

        #region ListView Handlers

        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = grid.SelectedItem as ReserveDocument;
            if (null != item)
            {
                LoadItems(item.GOODS_RECIPIENT);
            }
            else LoadItems(null);
        }

        #endregion

        #region Private Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TAApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        private void Search()
        {
            string basedate = (dtCreateDate.Value.HasValue) ? 
                dtCreateDate.Value.Value.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo) : null;
            ReservationRequestStatus resStatus = cbReserveStatus.SelectedItem as ReservationRequestStatus;
            string sReqStatus = (null != resStatus) ? resStatus.Code : null;
            ReservationTransferStatus tranStatus = cbTransferStatus.SelectedItem as ReservationTransferStatus;
            string sTranStatus = (null != tranStatus) ? tranStatus.Code : null;

            grid.ItemsSource = null;
            var items = ops.SearchReservation(basedate, sReqStatus, sTranStatus).Value();
            grid.ItemsSource = items;
        }

        private void LoadItems(string goodsRecipient)
        {
            // update text
            txtGOODS_RECIPIENT.Text = goodsRecipient;

            grid2.ItemsSource = null;
            if (!string.IsNullOrEmpty(goodsRecipient))
            {
                var items = ops.GetReservationItems(goodsRecipient).Value();
                grid2.ItemsSource = items;
            }
        }

        private void LoadComboboxes()
        {
            cbReserveStatus.ItemsSource = ReservationRequestStatus.Gets();
            cbReserveStatus.SelectedIndex = 0;
            cbTransferStatus.ItemsSource = ReservationTransferStatus.Gets();
            cbTransferStatus.SelectedIndex = 0;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            // Focus on search textbox.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                dtCreateDate.Value = DateTime.Today;
                LoadComboboxes();
            }));
        }

        #endregion
    }
}
