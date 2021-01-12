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
            /// Notify ShiftChanged.
            /// </summary>
            /// <returns>Returns NRestResult instance.</returns>
            public static NRestResult ShiftChanged()
            {
                var ret = Execute(RouteConsts.TA.Notify.ShiftChanged.Url);
                return ret;
            }
        }
    }
}
