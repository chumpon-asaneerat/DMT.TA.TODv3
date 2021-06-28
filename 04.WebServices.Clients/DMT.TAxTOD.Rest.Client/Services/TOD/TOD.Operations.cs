#region Usings

using System;
using System.Collections.Generic;
using DMT.Models;

#endregion

namespace DMT.Services.Operations
{
    partial class TAxTOD
    {
        /// <summary>The TOD Operations class.</summary>
        public static partial class TOD
        {
            /// <summary>TSBShift class.</summary>
            public static partial class TSBShift
            {
                /// <summary>
                /// Execute Save (TSBShift) api.
                /// </summary>
                /// <param name="value">The api parameter.</param>
                /// <returns>Returns instance of NRestResult.</returns>
                public static NRestResult Save(Models.TSBShift value)
                {
                    var ret = Execute(
                        RouteConsts.TAxTOD.TOD.TSBShift.Save.Url, value);
                    return ret;
                }
            }
            /// <summary>RevenueEntry class.</summary>
            public static partial class UserShift
            {
                /// <summary>
                /// Execute Save (UserShift) api.
                /// </summary>
                /// <param name="value">The api parameter.</param>
                /// <returns>Returns instance of NRestResult.</returns>
                public static NRestResult Save(Models.UserShift value)
                {
                    var ret = Execute(
                        RouteConsts.TAxTOD.TOD.UserShift.Save.Url, value);
                    return ret;
                }
            }
            /// <summary>RevenueEntry class.</summary>
            public static partial class RevenueEntry
            {
                /// <summary>
                /// Execute Save (RevenueEntry) api.
                /// </summary>
                /// <param name="value">The api parameter.</param>
                /// <returns>Returns instance of NRestResult.</returns>
                public static NRestResult Save(Models.RevenueEntry value)
                {
                    var ret = Execute(
                        RouteConsts.TAxTOD.TOD.RevenueEntry.Save.Url, value);
                    return ret;
                }
            }
        }
    }
}
