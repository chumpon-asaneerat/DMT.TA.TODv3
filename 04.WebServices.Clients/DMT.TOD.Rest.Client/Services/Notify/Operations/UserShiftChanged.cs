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
            /// Notify UserShiftChanged.
            /// </summary>
            /// <returns>Returns NRestResult instance.</returns>
            public static NRestResult UserShiftChanged()
            {
                var ret = Execute(RouteConsts.TOD.Notify.UserShiftChanged.Url);
                return ret;
            }
        }
    }
}
