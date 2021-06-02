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
                //var ret = Models.UserShift.UpdateUserShift(value);
                var ret = new NDbResult();
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
