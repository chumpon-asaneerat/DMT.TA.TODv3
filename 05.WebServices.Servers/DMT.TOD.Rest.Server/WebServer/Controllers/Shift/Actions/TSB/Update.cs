﻿#region Using

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
                var ret = Models.TSBShift.ChangeShift(value);
                if (null != ret && ret.Ok)
                {
                    TODNotifyService.Instance.RaiseTSBShiftChanged();
                }
                return ret;
            }
        }
    }
}
