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
                /// Gets Save User Credit Balance.
                /// </summary>
                /// <returns>Returns Current User Credit Balance instance.</returns>
                public static NRestResult<Models.UserCreditBalance> Save(
                    Models.UserCreditBalance value)
                {
                    var ret = Execute<Models.UserCreditBalance>(
                        RouteConsts.TA.Credit.User.Save.Url, value);
                    return ret;
                }
            }
        }
    }
}
