#region Using

using System;
using System.Collections.Generic;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    /*
    partial class Shift
    {
        partial class UserController
        {
            [HttpPost]
            [ActionName(RouteConsts.TOD.Shift.User.Current.Name)]
            //[AllowAnonymous]
            public NDbResult<UserShift> Current(User value)
            {
                NDbResult<UserShift> ret;
                if (null == value)
                {
                    ret = new NDbResult<UserShift>();
                    ret.ParameterIsNull();
                }
                else
                {
                    ret = UserShift.GetUserShift(value.UserId);
                }
                return ret;
            }
        }
    }
    */
}
