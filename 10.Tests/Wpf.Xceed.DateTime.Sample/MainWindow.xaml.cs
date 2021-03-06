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
using System.Windows.Data;

#endregion

namespace Toolkit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private CultureInfo culture = CultureInfo.CreateSpecificCulture("th-TH");
        private ThaiBuddhistCalendar calendar = new ThaiBuddhistCalendar();
        private XmlLanguage language = XmlLanguage.GetLanguage("th-TH");

        public MainWindow()
        {
            //CultureInfo culture = (CultureInfo.CurrentCulture.Clone() as CultureInfo);
            // Setup culture
            culture.DateTimeFormat.Calendar = calendar;
            CultureInfo.CurrentCulture = culture;

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            culture.DateTimeFormat.YearMonthPattern = @"yyyy/MM";
            Thread.CurrentThread.CurrentCulture = culture;


            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup DateTime Picker.
            dtEntryDate.CultureInfo = culture;
            //culture.DateTimeFormat.YearMonthPattern = "";
            dtEntryDate.Language = language;
        }
    }
}


namespace Toolkit
{
    public class ThaiYearConverter : IValueConverter
    {
        private static ThaiBuddhistCalendar thaicalendar = new ThaiBuddhistCalendar();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (null != culture) culture.DateTimeFormat.Calendar = thaicalendar;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (null != culture) culture.DateTimeFormat.Calendar = thaicalendar;
            throw new NotImplementedException();
        }
    }
}