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
                /// Gets Change TSB Shift.
                /// </summary>
                /// <param name="value">The TSB Shift instance</param>
                /// <returns>Returns NRestResult instance.</returns>
                public static NRestResult Change(Models.TSBShift value)
                {
                    var ret = Execute(
                        RouteConsts.TA.Shift.TSB.Change.Url, value);
                    return ret;
                }
            }
        }
    }
}
