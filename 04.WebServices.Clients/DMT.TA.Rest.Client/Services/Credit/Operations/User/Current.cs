﻿#region Usings

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
                /// Gets Current User Credit Balance.
                /// </summary>
                /// <returns>Returns Current User Credit Balance instance.</returns>
                public static NRestResult<Models.UserCreditBalance> Current(
                    Models.Search.Credit.User.Current value)
                {
                    var ret = Execute<Models.UserCreditBalance>(
                        RouteConsts.TA.Credit.User.Current.Url, value);
                    return ret;
                }
            }
        }
    }
}
