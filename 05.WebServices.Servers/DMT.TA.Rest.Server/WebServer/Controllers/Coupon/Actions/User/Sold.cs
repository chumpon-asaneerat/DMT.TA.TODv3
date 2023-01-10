#region Using

using System;
using System.Collections.Generic;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    partial class Coupon
    {
        partial class UserController
        {
            [HttpPost]
            [ActionName(RouteConsts.TA.Coupon.User.Sold.Name)]
            //[AllowAnonymous]
            public NDbResult<UserCouponSoldSummary> Sold([FromBody] Models.Search.Coupon.User.Sold value)
            {
                NDbResult<UserCouponSoldSummary> ret;
                if (null == value)
                {
                    ret = new NDbResult<UserCouponSoldSummary>();
                    ret.ParameterIsNull();
                    return ret;
                }
                ret = UserCouponSoldSummary.GetCouponSoldSummary(
                    //value.PlazaGroup, Ignore PlazaGroup
                    value.User, value.Start, value.End);
                return ret;
            }
        }
    }
}
