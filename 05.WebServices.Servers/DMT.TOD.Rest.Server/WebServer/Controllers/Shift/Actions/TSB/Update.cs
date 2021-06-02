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
            [ActionName(RouteConsts.TOD.Shift.TSB.Update.Name)]
            //[AllowAnonymous]
            public NDbResult Update([FromBody] Models.TSBShift value)
            {
                //var ret = Models.TSBShift.ChangeShift(value);

                var ret = new NDbResult();
                ret.Error(new Exception("Not implements."));
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
