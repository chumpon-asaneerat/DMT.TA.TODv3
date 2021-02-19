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
                /// Gets User Coupon Sold Balance.
                /// </summary>
                /// <param name="value">The Search parameter.</param>
                /// <returns>Returns User Coupon Sold Balance instance.</returns>
                public static NRestResult<Models.UserCouponSoldSummary> Sold(
                    Models.Search.Coupon.User.Sold value)
                {
                    var ret = Execute<Models.UserCouponSoldSummary>(
                        RouteConsts.TA.Coupon.User.Sold.Url, value);
                    return ret;
                }
            }
        }
    }
}
