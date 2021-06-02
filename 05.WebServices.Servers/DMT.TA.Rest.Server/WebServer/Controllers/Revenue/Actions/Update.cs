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
        partial class TAARevenueController
        {
            [HttpPost]
            [ActionName(RouteConsts.TA.Revenue.Update.Name)]
            //[AllowAnonymous]
            public NDbResult Update([FromBody] Models.UserShift value)
            {
                // Write to Queue for send to all TOD clients.
                TODClientManager.Instance.SendToTOD(value);

                var ret = new NDbResult();
                ret.Success();

                if (null != ret && ret.Ok)
                {
                }

                return ret;
            }
        }
    }
}
