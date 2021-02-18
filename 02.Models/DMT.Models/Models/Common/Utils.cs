#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cypt = System.Security.Cryptography;

#endregion

namespace DMT.Utils
{
    #region MD5

    /// <summary>
    /// The MD5 Utils.
    /// </summary>
    public static class MD5
    {
        /// <summary>
        /// Encrypt original string to MD5 string (UTF8).
        /// </summary>
        /// <param name="value">The original string.</param>
        /// <returns>Returns encrypt string.</returns>
        public static string Encrypt(string value)
        {
            return Encrypt(value, Encoding.UTF8);
        }
        /// <summary>
        /// Encrypt original string to MD5 string.
        /// </summary>
        /// <param name="value">The original string.</param>
        /// <param name="encoding">The Encoding instance. Default is UTF8.</param>
        /// <returns>Returns encrypt string.</returns>
        public static string Encrypt(string value, Encoding encoding)
        {
            string ret = string.Empty;

            var encode = (null != encoding) ? encoding : Encoding.UTF8;
            using (var md5 = cypt.MD5.Create())
            {
                byte[] inputBytes = encode.GetBytes(value);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                ret = BitConverter.ToString(hashBytes)
                    .Replace("-", string.Empty).ToLower();
            }
            return ret;
        }
    }

    #endregion
}
