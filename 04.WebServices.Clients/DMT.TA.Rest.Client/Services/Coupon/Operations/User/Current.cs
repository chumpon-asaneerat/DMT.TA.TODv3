#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
    {
        static partial class Coupon
        {
            static partial class User
            {
                /// <summary>
                /// Gets Current User Balance.
                /// </summary>
                /// <param name="value">The Search parameter.</param>
                /// <returns>Returns Current User Coupon Balance instance.</returns>
                public static NRestResult<Models.UserCouponBalance> Current(
                    Models.Search.Coupon.User.Current value)
                {
                    var ret = Execute<Models.UserCouponBalance>(
                        RouteConsts.TA.Coupon.User.Current.Url, value);
                    return ret;
                }
            }
        }
    }
}
