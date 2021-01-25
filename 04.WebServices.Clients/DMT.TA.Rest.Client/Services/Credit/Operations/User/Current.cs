#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
    {
        static partial class Credit
        {
            static partial class User
            {
                /// <summary>
                /// Gets Current TSB Shift.
                /// </summary>
                /// <returns>Returns Current User Credit Balance instance.</returns>
                public static NRestResult<Models.TSBShift> Current()
                {
                    var ret = Execute<Models.TSBShift>(
                        RouteConsts.TA.Credit.User.Current.Url);
                    return ret;
                }
            }
        }
    }
}
