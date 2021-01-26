#region Using

using System;
using System.Collections.Generic;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    partial class Credit
    {
        partial class UserController
        {
            [HttpPost]
            [ActionName(RouteConsts.TA.Credit.User.Current.Name)]
            //[AllowAnonymous]
            public NDbResult<UserCreditBalance> Current([FromBody] Models.Search.Credit.User.Current value)
            {
                NDbResult<UserCreditBalance> ret;
                if (null == value)
                {
                    ret = new NDbResult<UserCreditBalance>();
                    ret.ParameterIsNull();
                    return ret;
                }
                ret = UserCreditBalance.GetActiveUserCreditBalance(value.User, value.PlazaGroup);
                return ret;
            }
        }
    }
}
