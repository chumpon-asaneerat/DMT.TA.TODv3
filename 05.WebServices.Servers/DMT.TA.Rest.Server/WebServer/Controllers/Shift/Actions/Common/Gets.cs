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
        partial class CommonController
        {
            [HttpPost]
            [ActionName(RouteConsts.TA.Shift.Gets.Name)]
            //[AllowAnonymous]
            public NDbResult<List<Models.Shift>> Gets()
            {
                var ret = Models.Shift.GetShifts();
                return ret;
            }
        }
    }
}
