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

namespace DMT.TA.Windows.Reports
{
    /// <summary>
    /// Interaction logic for CreditTransactionHistorySearchWindow.xaml
    /// </summary>
    public partial class CreditTransactionHistorySearchWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreditTransactionHistorySearchWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        private CultureInfo culture = new CultureInfo("th-TH");
        private XmlLanguage language = XmlLanguage.GetLanguage("th-TH");
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
            LoadExchangeHistory();
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

        }

        #endregion

        #region Private Methods

        private void LoadExchangeHistory()
        {
            CreditDate = new DateTime?();
            ExchangeHistories = null;
            if (!dtDate.Value.HasValue) return;
            var dt = dtDate.Value.Value.Date;
            CreditDate = dt;
            ExchangeHistories = UserCreditHistory.GetUserCreditHistories(dt).Value();
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
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Credit Date.
        /// </summary>
        public DateTime? CreditDate { get; private set; }
        /// <summary>
        /// Gets or sets Selected Exchange Histories.
        /// </summary>
        public List<Models.UserCreditHistory> ExchangeHistories { get; private set; }

        #endregion
    }
}
