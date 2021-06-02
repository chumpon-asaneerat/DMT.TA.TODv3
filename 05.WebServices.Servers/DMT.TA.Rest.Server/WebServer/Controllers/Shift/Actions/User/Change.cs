#region Using

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    partial class Shift
    {
        partial class UserController
        {
            [HttpPost]
            [ActionName(RouteConsts.TA.Shift.User.Change.Name)]
            //[AllowAnonymous]
            public NDbResult Change([FromBody] Models.UserShift value)
            {
                var ret = Models.UserShift.UpdateUserShift(value);
                if (null != ret && ret.Ok)
                {
                    /*
                    Task.Run(() => 
                    { 
                        TANotifyService.Instance.RaiseTSBShiftChanged(); 
                    });
                    */
                }
                return ret;
            }
        }
    }
}
