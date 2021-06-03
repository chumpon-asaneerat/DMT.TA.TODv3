#region Using

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Web.Http;
using NLib;
using DMT.Models;

#endregion

namespace DMT.Services
{
    partial class Shift
    {
        partial class UserController
        {
            [HttpPost]
            [ActionName(RouteConsts.TOD.Shift.User.Update.Name)]
            //[AllowAnonymous]
            public NDbResult Update([FromBody] Models.UserShift value)
            {
                MethodBase med = MethodBase.GetCurrentMethod();

                var ret = Models.UserShift.UpdateUserShift(value);
                if (null != ret && ret.Ok)
                {
                    med.Info("Sync UserShift from TA - success.");
                }
                else
                {
                    med.Info("Sync UserShift from TA - failed.");
                }

                return ret;
            }
        }
    }
}
