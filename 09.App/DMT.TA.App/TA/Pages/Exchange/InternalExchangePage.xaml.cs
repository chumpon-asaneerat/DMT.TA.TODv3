#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

using DMT.Configurations;
using DMT.Models;
using DMT.Services;
using DMT.Controls;

using NLib.Services;
using NLib.Reflection;

#endregion

namespace DMT.TA.Pages.Exchange
{
    /// <summary>
    /// Interaction logic for InternalExchangePage.xaml
    /// </summary>
    public partial class InternalExchangePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public InternalExchangePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        private CultureInfo culture = new CultureInfo("th-TH");
        private XmlLanguage language = XmlLanguage.GetLanguage("th-TH");

        private InternalExchangeManager manager = null;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup DateTime Picker.
            dtEntryDate.CultureInfo = culture;
            dtEntryDate.Language = language;
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;
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
            Refresh();
        }

        private void cmdExchange_Click(object sender, RoutedEventArgs e)
        {
            AddExchange();
        }

        #endregion

        #region DateTime Picker Handlers

        private void dtEntryDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Refresh();
        }

        #endregion

        #region Private Methods

        #region Navigation Methods

        private void GotoMainMenu()
        {
            // Main Menu Page
            var page = TAApp.Pages.MainMenu;
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region Reset

        private void Reset()
        {
            // Date Picker
            dtEntryDate.DefaultValue = DateTime.Now;
            dtEntryDate.Value = DateTime.Now.Date;
        }

        #endregion

        #region Exchhange methods

        private void RefreshInternalExchanges()
        {
            grid.ItemsSource = null;
        }

        private void AddExchange()
        {
            var win = TAApp.Windows.InternalExchange;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }
            Refresh();
        }

        #endregion

        #region Refresh

        private void Refresh()
        {
            plazaSummary.Setup(); // Call for refresh.
            RefreshInternalExchanges();
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            if (null == manager)
            {
                manager = new InternalExchangeManager();
            }
            Reset();
            Refresh();
        }

        #endregion
    }
}
