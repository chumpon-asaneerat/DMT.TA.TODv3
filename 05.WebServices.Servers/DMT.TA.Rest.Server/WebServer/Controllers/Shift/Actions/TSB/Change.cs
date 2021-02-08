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
            [ActionName(RouteConsts.TA.Shift.TSB.Change.Name)]
            //[AllowAnonymous]
            public NDbResult Change([FromBody] Models.TSBShift value)
            {
                var ret = Models.TSBShift.ChangeShift(value);
                if (null != ret && ret.Ok)
                {
                    Task.Run(() => 
                    { 
                        TANotifyService.Instance.RaiseTSBShiftChanged(); 
                    });
                }
                return ret;
            }
        }
    }
}
