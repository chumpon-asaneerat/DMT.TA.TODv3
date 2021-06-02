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
                /// TSB Shift Update.
                /// </summary>
                /// <param name="value">The TSB Shift instance</param>
                /// <returns>Returns NRestResult instance.</returns>
                public static NRestResult Update(Models.TSBShift value)
                {
                    var ret = Execute(
                        RouteConsts.TA.Shift.TSB.Update.Url, value);
                    return ret;
                }
            }
        }
    }
}
