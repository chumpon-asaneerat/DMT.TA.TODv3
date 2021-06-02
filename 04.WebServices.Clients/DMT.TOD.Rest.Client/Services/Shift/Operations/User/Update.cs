#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TOD
    {
        static partial class Shift
        {
            static partial class User
            {
                /// <summary>
                /// User Shift Update.
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
