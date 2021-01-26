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
            /// Notify TSBShiftChanged.
            /// </summary>
            /// <returns>Returns NRestResult instance.</returns>
            public static NRestResult TSBShiftChanged()
            {
                var ret = Execute(RouteConsts.TOD.Notify.TSBShiftChanged.Url);
                return ret;
            }
        }
    }
}
