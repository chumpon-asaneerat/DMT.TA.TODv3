#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
    {
        static partial class Shift
        {
            static partial class TSB
            {
                /// <summary>
                /// Gets Current TSB Shift.
                /// </summary>
                /// <returns>Returns Current TSB Shift instance.</returns>
                public static NRestResult<Models.TSBShift> Current()
                {
                    var ret = Execute<Models.TSBShift>(
                        RouteConsts.TA.Shift.TSB.Current.Url, new { });
                    return ret;
                }
            }
        }
    }
}
