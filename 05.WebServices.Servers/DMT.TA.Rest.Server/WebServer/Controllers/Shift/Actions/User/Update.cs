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
            [ActionName(RouteConsts.TA.Shift.User.Update.Name)]
            //[AllowAnonymous]
            public NDbResult Update([FromBody] Models.UserShift value)
            {
                MethodBase med = MethodBase.GetCurrentMethod();

                var ret = new NDbResult();
                ret.Success();

                if (null != ret && ret.Ok)
                {
                    med.Info("Generate UserShift files (prepare for sent to all TODs).");
                    // Write to Queue for send to all TOD clients.
                    //TODClientManager.Instance.SendToTOD(value);
                }
                else
                {
                    med.Info("No UserShift files generated.");
                }

                return ret;
            }
        }
    }
}
