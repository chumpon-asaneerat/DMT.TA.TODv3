#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
    {
        static partial class Shift
        {
            /// <summary>
            /// Gets all Shifts.
            /// </summary>
            /// <returns>Returns all Shifts.</returns>
            public static NRestResult<List<Models.Shift>> Gets()
            {
                var ret = Execute<List<Models.Shift>>(
                    RouteConsts.TA.Shift.Gets.Url);
                return ret;
            }
        }
    }
}
