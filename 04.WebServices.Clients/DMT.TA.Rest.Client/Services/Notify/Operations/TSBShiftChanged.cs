#region Usings

using System.Collections.Generic;
using DMT.Services;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
    {
        static partial class Notify
        {
            /// <summary>
            /// Notify TSBShiftChanged.
            /// </summary>
            /// <returns>Returns NRestResult instance.</returns>
            public static NRestResult TSBShiftChanged()
            {
                var ret = Execute(RouteConsts.TA.Notify.TSBShiftChanged.Url);
                return ret;
            }
        }
    }
}
