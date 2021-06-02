#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
    {
        static partial class Shift
        {
            static partial class User
            {
                /// <summary>
                /// User Shift Update.
                /// </summary>
                /// <param name="value">The User Shift instance</param>
                /// <returns>Returns NRestResult instance.</returns>
                public static NRestResult Update(Models.UserShift value)
                {
                    var ret = Execute(
                        RouteConsts.TA.Shift.User.Update.Url, value);
                    return ret;
                }
            }
        }
    }
}
