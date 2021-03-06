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

        private void dtEntryDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            dtOut.SelectedDate = dtEntryDate.Value;
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
            string[] values = (null != value) ? value.ToString().Split(' ') : null;
            string ret = value.ToString();
            if (null != values && values.Length >= 2)
            {
                int yr = int.Parse(values[1]);
                if (yr < 2500) yr += 543;
                ret = values[0] + " " + yr.ToString();
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (null != culture) culture.DateTimeFormat.Calendar = thaicalendar;
            throw new NotImplementedException();
        }
    }
}