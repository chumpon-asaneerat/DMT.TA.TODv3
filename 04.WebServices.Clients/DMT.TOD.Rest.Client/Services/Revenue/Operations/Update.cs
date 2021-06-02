#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TOD
    {
        static partial class Revenue
        {
            /// <summary>
            /// Revenue Entry Update.
            /// </summary>
            /// <param name="value">The Revenue Entry instance</param>
            /// <returns>Returns NRestResult instance.</returns>
            public static NRestResult Update(Models.RevenueEntry value)
            {
                var ret = Execute(
                    RouteConsts.TOD.Revenue.Update.Url, value);
                return ret;
            }
        }
    }
}
