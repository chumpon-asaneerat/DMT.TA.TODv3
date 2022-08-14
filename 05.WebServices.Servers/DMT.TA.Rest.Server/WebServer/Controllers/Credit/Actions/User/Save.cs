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
            [ActionName(RouteConsts.TA.Credit.User.Save.Name)]
            //[AllowAnonymous]
            public NDbResult<UserCreditBalance> SaveCurrent([FromBody] UserCreditBalance value)
            {
                NDbResult<UserCreditBalance> ret;
                if (null == value)
                {
                    ret = new NDbResult<UserCreditBalance>();
                    ret.ParameterIsNull();
                    return ret;
                }
                ret = UserCreditBalance.SaveUserCreditBalance(value);
                return ret;
            }
        }
    }
}
