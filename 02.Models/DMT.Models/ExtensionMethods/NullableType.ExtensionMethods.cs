#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using DMT.Models;
using NLib.Reflection;
using NLib.Utils;

#endregion

namespace DMT.Models.ExtensionMethods
{
    #region Nullable Type Extenstion Methods

    /// <summary>
    /// The NullableType ExtensionMethods class.
    /// </summary>
    public static class NullableTypeExtensionMethods
    {
        #region DateTime

        /// <summary>
        /// Nullable to DateTime value converter.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>Returns convert value.</returns>
        public static DateTime Value(this DateTime? value)
        {
            return value ?? DateTime.MinValue;
        }
        /// <summary>
        /// DateTime to Nullable value converter.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>Returns convert value.</returns>
        public static DateTime? Value(this DateTime value)
        {
            return (value == DateTime.MinValue) ? new DateTime?() : value;
        }

        #endregion

        #region Int

        /// <summary>
        /// Nullable to int value converter.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>Returns convert value.</returns>
        public static int Value(this int? value)
        {
            return value ?? 0;
        }
        /// <summary>
        /// Int to Nullable value converter.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>Returns convert value.</returns>
        public static int? Value(this int value)
        {
            return (value == int.MinValue) ? new int?() : value;
        }

        #endregion

        #region Decimal

        /// <summary>
        /// Nullable to decimal value converter.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>Returns convert value.</returns>
        public static decimal Value(this decimal? value)
        {
            return value ?? decimal.MinValue;
        }
        /// <summary>
        /// Decimal to Nullable value converter.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>Returns convert value.</returns>
        public static decimal? Value(this decimal value)
        {
            return (value == decimal.MinValue) ? new decimal?() : value;
        }

        #endregion
    }

    #endregion
}
