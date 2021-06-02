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
        partial class TSBController
        {
            [HttpPost]
            [ActionName(RouteConsts.TA.Shift.TSB.Update.Name)]
            //[AllowAnonymous]
            public NDbResult Update([FromBody] Models.TSBShift value)
            {
                // Write to Queue for send to all TOD clients.
                TODClientManager.Instance.SendToTOD(value);

                var ret = Models.TSBShift.ChangeShift(value);
                if (null != ret && ret.Ok)
                {
                    TANotifyService.Instance.RaiseTSBShiftChanged();
                }

                return ret;
            }
        }
    }
}
