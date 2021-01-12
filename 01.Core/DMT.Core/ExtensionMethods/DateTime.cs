#region Using

using System;
using System.Globalization;

#endregion

namespace DMT
{
    #region DateTime Extension Methods

    /// <summary>
    /// The DateTime Extension Methods.
    /// </summary>
    public static class DateTimeExtension
    {
        private static CultureInfo _fmt = null;
        /// <summary>
        /// Gets Thai CultureInfo.
        /// </summary>
        public static CultureInfo ThaiCultureInfo
        {
            get
            {
                if (null == _fmt)
                {
                    _fmt = new CultureInfo("th-TH");
                }
                return _fmt;
            }
        }

        /// <summary>
        /// To Date String (fixed format used ToDateTimeString for custom format).
        /// </summary>
        /// <param name="value">The DateTime instance.</param>
        /// <returns>Returns string that represents date part.</returns>
        public static string ToDateString(this DateTime value)
        {
            if (value == DateTime.MinValue || value == DateTime.MaxValue)
                return "";
            return value.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// To Time String (fixed format used ToDateTimeString for custom format).
        /// </summary>
        /// <param name="value">The DateTime instance.</param>
        /// <returns>Returns string that represents time part.</returns>
        public static string ToTimeString(this DateTime value)
        {
            if (value == DateTime.MinValue || value == DateTime.MaxValue)
                return "";
            return value.ToString("HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// To DateTime String.
        /// </summary>
        /// <param name="value">The DateTime instance.</param>
        /// <param name="format">The DateTime format.</param>
        /// <returns>Returns string that represents datetime part.</returns>
        public static string ToDateTimeString(this DateTime value,
            string format = "dd/MM/yyyy HH:mm:ss.fff")
        {
            if (value == DateTime.MinValue || value == DateTime.MaxValue)
                return "";
            return value.ToString(format, DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// To Thai Date String (fixed format used ToThaiDateTimeString for custom format).
        /// </summary>
        /// <param name="value">The DateTime instance.</param>
        /// <returns>Returns string that represents date part.</returns>
        public static string ToThaiDateString(this DateTime value)
        {
            if (value == DateTime.MinValue || value == DateTime.MaxValue)
                return "";
            return value.ToString("dd/MM/yyyy", DateTimeExtension.ThaiCultureInfo);
        }
        /// <summary>
        /// To Thai Time String (fixed format used ToDateTimeString for custom format).
        /// </summary>
        /// <param name="value">The DateTime instance.</param>
        /// <returns>Returns string that represents time part.</returns>
        public static string ToThaiTimeString(this DateTime value)
        {
            return value.ToTimeString();
        }
        /// <summary>
        /// To Thai DateTime String.
        /// </summary>
        /// <param name="value">The DateTime instance.</param>
        /// <param name="format">The DateTime format.</param>
        /// <returns>Returns string that represents datetime part.</returns>
        public static string ToThaiDateTimeString(this DateTime value,
            string format = "dd/MM/yyyy HH:mm:ss.fff")
        {
            if (value == DateTime.MinValue || value == DateTime.MaxValue)
                return "";
            return value.ToString(format, DateTimeExtension.ThaiCultureInfo);
        }
    }

    #endregion
}
