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
                /// Gets Completed User Credit Balance.
                /// </summary>
                /// <returns>Returns Current User Credit Balance instance.</returns>
                public static NRestResult<Models.UserCreditBalance> Completed(
                    Models.Search.Credit.User.Completed value)
                {
                    var ret = Execute<Models.UserCreditBalance>(
                        RouteConsts.TA.Credit.User.Completed.Url, value);
                    return ret;
                }
            }
        }
    }
}
