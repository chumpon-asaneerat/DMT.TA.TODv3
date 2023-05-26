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
            [ActionName(RouteConsts.TA.Coupon.User.OnHand.Name)]
            //[AllowAnonymous]
            public NDbResult<UserCouponOnHandSummary> OnHand([FromBody] Models.Search.Coupon.User.OnHand value)
            {
                NDbResult<UserCouponOnHandSummary> ret;
                if (null == value)
                {
                    ret = new NDbResult<UserCouponOnHandSummary>();
                    ret.ParameterIsNull();
                    return ret;
                }
                ret = UserCouponOnHandSummary.GetCouponOnHandSummary(
                    //value.PlazaGroup, Ignore PlazaGroup
                    value.User);
                return ret;
            }
        }
    }
}
