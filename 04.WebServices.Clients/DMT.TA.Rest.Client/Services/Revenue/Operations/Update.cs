#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
    {
        static partial class Revenue
        {
            /// <summary>
            /// Revenue Entry Update.
            /// </summary>
            /// <param name="value">The RevenueEntry instance</param>
            /// <returns>Returns NRestResult instance.</returns>
            public static NRestResult Update(Models.RevenueEntry value)
            {
                var ret = Execute(
                    RouteConsts.TA.Revenue.Update.Url, value);
                return ret;
            }
        }
    }
}
