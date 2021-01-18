#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TOD
    {
        static partial class Shift
        {
            static partial class TSB
            {
                /// <summary>
                /// Change Shift
                /// </summary>
                /// <param name="value">The TSBShift instance.</param>
                /// <returns>Returns NRestResult instance.</returns>
                public static NRestResult Change(Models.TSBShift value)
                {
                    var ret = Execute(
                        RouteConsts.TOD.Shift.TSB.Change.Url, value);
                    return ret;
                }
            }
        }
    }
}
