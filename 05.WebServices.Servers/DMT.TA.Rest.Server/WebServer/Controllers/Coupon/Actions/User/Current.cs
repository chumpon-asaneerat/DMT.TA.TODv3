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
            [ActionName(RouteConsts.TA.Coupon.User.Current.Name)]
            //[AllowAnonymous]
            public NDbResult<UserCouponBalance> Current([FromBody] Models.Search.Coupon.User.Current value)
            {
                NDbResult<UserCouponBalance> ret;
                if (null == value)
                {
                    ret = new NDbResult<UserCouponBalance>();
                    ret.ParameterIsNull();
                    return ret;
                }
                ret = UserCouponBalance.GetUserBalance(value.User);
                return ret;
            }
        }
    }
}
