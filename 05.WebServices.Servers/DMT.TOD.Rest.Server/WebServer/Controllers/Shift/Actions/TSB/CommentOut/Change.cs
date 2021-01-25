#region Using

using System;
using System.Collections.Generic;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    /*
    using TAOps = Services.Operations.TA.Notify;

    partial class Shift
    {
        partial class TSBController
        {
            [HttpPost]
            [ActionName(RouteConsts.TOD.Shift.TSB.Change.Name)]
            //[AllowAnonymous]
            public NDbResult Change(TSBShift value)
            {
                var ret = TSBShift.ChangeShift(value);
                
                // Notify ShiftChanged to TODApp.
                TODNotifyService.Instance.RaiseShiftChanged();

                // Call TA App API to nofify Shift Changed.
                TAOps.ShiftChanged();

                return ret;
            }
        }
    }
    */
}
