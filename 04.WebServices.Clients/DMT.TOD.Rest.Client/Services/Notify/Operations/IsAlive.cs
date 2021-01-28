#region Usings

using System.Collections.Generic;
using DMT.Services;

#endregion

namespace DMT.Services.Operations
{
    static partial class TOD
    {
        static partial class Notify
        {
            /// <summary>
            /// IsAlive.
            /// </summary>
            /// <returns>Returns NRestResult instance.</returns>
            public static NRestResult<Models.IsAliveResult> IsAlive()
            {
                var ret = Execute<Models.IsAliveResult>(RouteConsts.TOD.Notify.IsAlive.Url);
                return ret;
            }
        }
    }
}
