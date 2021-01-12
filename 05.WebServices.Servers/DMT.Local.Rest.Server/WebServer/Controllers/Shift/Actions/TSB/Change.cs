#region Using

using System;
using System.Collections.Generic;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    using TAOps = Operations.TA.Notify;
    using TODOps = Operations.TOD.Notify;

    partial class Shift
    {
        partial class TSBController
        {
            [HttpPost]
            [ActionName(RouteConsts.Shift.TSB.Change.Name)]
            //[AllowAnonymous]
            public NDbResult Change([FromBody] TSBShift value)
            {
                var ret = TSBShift.ChangeShift(value);
                if (null != ret && ret.Ok)
                {
                    // Notify ShiftChanged to (TAApp and TODApp).
                    TAOps.ShiftChanged();
                    TODOps.ShiftChanged();
                }
                return ret;
            }
        }
    }
}
