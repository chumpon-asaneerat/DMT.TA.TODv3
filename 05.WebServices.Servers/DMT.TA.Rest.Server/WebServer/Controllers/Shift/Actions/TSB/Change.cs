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
            public NDbResult Change([FromBody] TSBShift value)
            {
                NDbResult result = new NDbResult();
                if (null == value)
                {
                    result.ParameterIsNull();
                    return result;
                }
                var dbRet = TSBShift.ChangeShift(value);
                if (null != dbRet && dbRet.Ok)
                {
                    // Raise Change Shift.
                    TANotifyService.Instance.RaiseTSBShiftChanged();
                    result.Success();
                }
                return result;
            }
        }
    }
}
