#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class SCW
    {
        /// <summary>The Security Operations class.</summary>
        public static partial class Security
        {
            /// <summary>
            /// Execute GET version api.
            /// </summary>
            /// <returns>Returns version information in string.</returns>
            public static string GetVersion()
            {
                var ret = Get(RouteConsts.SCW.Version.Url);
                return (null != ret) ? ret.ToString() : string.Empty;
            }
        }
    }
}
