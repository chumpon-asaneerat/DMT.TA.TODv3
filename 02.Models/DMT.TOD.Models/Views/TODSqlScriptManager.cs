#region Using

using System;
using System.IO;
using System.Reflection;
using NLib;

#endregion

namespace DMT.Views
{
    #region TODSqliteScriptManager

    /// <summary>
    /// The TOD Sqlite Script Manager class.
    /// </summary>
    public class TODSqliteScriptManager
    {
        private static Assembly Current { get { return typeof(TODSqliteScriptManager).Assembly; } }
        /// <summary>
        /// Gets View SQL Script (from embedded resource).
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>Returns load view sql script.</returns>
        public static string GetScript(string resourceName)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            string ret = string.Empty;
            if (!string.IsNullOrWhiteSpace(resourceName))
            {
                try
                {
                    using (Stream stream = Current.GetManifestResourceStream(resourceName))
                    {
                        if (null != stream)
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                if (null != reader)
                                {
                                    ret = reader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    ret = string.Empty;
                }
            }
            return ret;
        }
    }

    #endregion
}
