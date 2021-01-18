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
                /// Change Shift
                /// </summary>
                /// <param name="value">The UserShift instance.</param>
                /// <returns>Returns NRestResult instance.</returns>
                public static NRestResult Change(Models.UserShift value)
                {
                    var ret = Execute(
                        RouteConsts.TOD.Shift.User.Change.Url, value);
                    return ret;
                }
            }
        }
    }
}
