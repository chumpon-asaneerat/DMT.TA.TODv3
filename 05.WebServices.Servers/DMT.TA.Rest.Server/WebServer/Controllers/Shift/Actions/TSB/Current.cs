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
            [ActionName(RouteConsts.TA.Shift.TSB.Current.Name)]
            //[AllowAnonymous]
            public NDbResult<TSBShift> Current()
            {
                var ret = TSBShift.GetTSBShift();
                return ret;
            }
        }
    }
}
