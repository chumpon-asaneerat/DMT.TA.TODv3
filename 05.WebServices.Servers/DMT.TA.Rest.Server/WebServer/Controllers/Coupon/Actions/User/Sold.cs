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
            public NDbResult<UserCouponBalance> Sold([FromBody] Models.Search.Coupon.User.Sold value)
            {
                NDbResult<UserCouponBalance> ret;
                if (null == value)
                {
                    ret = new NDbResult<UserCouponBalance>();
                    ret.ParameterIsNull();
                    return ret;
                }
                ret = UserCouponBalance.GetCouponSoldBalance(value.PlazaGroup, value.User, value.Start, value.End);
                return ret;
            }
        }
    }
}
