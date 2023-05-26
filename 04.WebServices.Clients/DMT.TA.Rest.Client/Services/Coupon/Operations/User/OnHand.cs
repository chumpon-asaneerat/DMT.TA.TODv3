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
                /// Gets User Coupon OnHand Balance.
                /// </summary>
                /// <param name="value">The Search parameter.</param>
                /// <returns>Returns User Coupon OnHand Balance instance.</returns>
                public static NRestResult<Models.UserCouponOnHandSummary> OnHand(
                    Models.Search.Coupon.User.OnHand value)
                {
                    var ret = Execute<Models.UserCouponOnHandSummary>(
                        RouteConsts.TA.Coupon.User.OnHand.Url, value);
                    return ret;
                }
            }
        }
    }
}
