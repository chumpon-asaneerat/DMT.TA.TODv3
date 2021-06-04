#region Using

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace DMT.Controls.Utils
{
    /// <summary>
    /// The String To Number Converter class.
    /// </summary>
    public class StringToNumberConverter : IValueConverter
    {
        /// <summary>
        /// Convert.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
        /// <summary>
        /// ConvertBack.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                try
                {
                    var str = (string)value;
                    if (string.IsNullOrWhiteSpace(str)) return 0;
                    if (targetType == typeof(decimal))
                    {
                        decimal ret;
                        if (!decimal.TryParse(str.Trim(), out ret))
                        {
                            return decimal.Zero;
                        }
                    }
                    else if (targetType == typeof(int))
                    {
                        int ret;
                        if (!int.TryParse(str.Trim(), out ret))
                        {
                            return 0;
                        }
                    }
                }
                catch (Exception) 
                {
                    return 0;
                }
            }
            return value;
        }
    }
}
