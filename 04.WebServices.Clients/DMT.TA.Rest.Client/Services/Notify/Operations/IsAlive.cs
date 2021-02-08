#region Usings

using System.Collections.Generic;
using DMT.Configurations;
using DMT.Services;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
    {
        static partial class Notify
        {
            /// <summary>
            /// IsAlive.
            /// </summary>
            /// <returns>Returns NRestResult instance.</returns>
            public static NRestResult<Models.IsAliveResult> IsAlive()
            {
                var ret = Execute<Models.IsAliveResult>(RouteConsts.TA.Notify.IsAlive.Url, new { });
                return ret;
            }
        }
    }
}
