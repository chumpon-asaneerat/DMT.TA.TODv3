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
            /// Notify TSBChanged.
            /// </summary>
            /// <returns>Returns NRestResult instance.</returns>
            public static NRestResult TSBChanged()
            {
                var ret = Execute(RouteConsts.TOD.Notify.TSBChanged.Url);
                return ret;
            }
        }
    }
}
