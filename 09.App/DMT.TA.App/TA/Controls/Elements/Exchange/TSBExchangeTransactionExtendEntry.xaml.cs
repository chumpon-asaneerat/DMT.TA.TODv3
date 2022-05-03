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

namespace DMT.TA.Controls.Elements
{
    /// <summary>
    /// Interaction logic for TSBExchangeTransactionExtendEntry.xaml
    /// </summary>
    public partial class TSBExchangeTransactionExtendEntry : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBExchangeTransactionExtendEntry()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        private CultureInfo culture = new CultureInfo("th-TH");
        private XmlLanguage language = XmlLanguage.GetLanguage("th-TH");

        #endregion

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup DateTime Picker.
            dtPreiodBegin.CultureInfo = culture;
            dtPreiodBegin.Language = language;
            dtPreiodEnd.CultureInfo = culture;
            dtPreiodEnd.Language = language;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The TSB Exchange Transaction.</param>
        public void Setup(TSBExchangeTransaction value)
        {
            dtPreiodBegin.Value = (null != value) ? value.PeriodBegin : new DateTime?();
            dtPreiodEnd.Value = (null != value) ? value.PeriodEnd : new DateTime?();
            this.DataContext = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Period Begin DateTime.
        /// </summary>
        public DateTime? PeriodBegin 
        {
            get { return dtPreiodBegin.Value; }
            set { dtPreiodBegin.Value = value; }
        }
        /// <summary>
        /// Gets or sets Period End DateTime.
        /// </summary>
        public DateTime? PeriodEnd
        {
            get { return dtPreiodEnd.Value; }
            set { dtPreiodEnd.Value = value; }
        }

        #endregion
    }
}
