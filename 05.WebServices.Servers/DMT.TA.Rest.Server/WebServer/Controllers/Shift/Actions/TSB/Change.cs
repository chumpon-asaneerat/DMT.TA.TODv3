#region Using

using System;
using System.Collections.Generic;
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
            [ActionName(RouteConsts.TA.Shift.TSB.Change.Name)]
            //[AllowAnonymous]
            public NDbResult Change(TSBShift value)
            {
                var ret = TSBShift.ChangeShift(value);
                if (ret.Ok)
                {
                    // Raise Change Shift.
                    TANotifyService.Instance.RaiseTSBShiftChanged();
                }
                return ret;
            }
        }
    }
}
