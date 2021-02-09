#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Markup;

using DMT.Configurations;
using DMT.Models;
using DMT.Services;
using DMT.Controls;

using NLib.Services;
using NLib.Reflection;
using System.Threading;
using System.Windows.Threading;

#endregion

namespace DMT.TOD.Windows.Reports
{
    /// <summary>
    /// Interaction logic for RevenueSlipSearchWindow.xaml
    /// </summary>
    public partial class RevenueSlipSearchWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RevenueSlipSearchWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        private CultureInfo culture = new CultureInfo("th-TH");
        private XmlLanguage language = XmlLanguage.GetLanguage("TH");
        private User _user = null;

        #endregion

        #region Loaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup DateTime Picker.
            dtDate.CultureInfo = culture;
            dtDate.Language = language;
        }

        #endregion

        #region Button Handlers

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            SelectedEntry = grid.SelectedItem as RevenueEntry;
            DialogResult = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region Date Handlers

        private void dtDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            LoadRevenues();
        }

        #endregion

        #region ListView Handlers

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedEntry = grid.SelectedItem as RevenueEntry;
            if (null != SelectedEntry)
            {
                DialogResult = true;
            }
        }

        #endregion

        #region Private Methods

        private void LoadRevenues()
        {
            SelectedEntry = null;
            grid.ItemsSource = null;

            if (!dtDate.Value.HasValue) return;
            var dt = dtDate.Value.Value.Date;

            var revenues = RevenueEntry.FindByRevnueDate(dt).Value();
            if (null != revenues)
            {
                grid.ItemsSource = revenues;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="user">The User.</param>
        public void Setup(User user)
        {
            _user = user;
            dtDate.DefaultValue = DateTime.Now;
            dtDate.Value = DateTime.Now.Date;
            LoadRevenues();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Selected Revenue Entry.
        /// </summary>
        public Models.RevenueEntry SelectedEntry { get; private set; }

        #endregion
    }
}
