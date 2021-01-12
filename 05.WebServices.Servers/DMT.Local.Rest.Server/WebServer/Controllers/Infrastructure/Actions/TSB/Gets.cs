#region Using

using System;
using System.Collections.Generic;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    partial class Infrastructure
    {
        partial class TSBController
        {
            [HttpPost]
            [ActionName(RouteConsts.Infrastructure.TSB.Gets.Name)]
            //[AllowAnonymous]
            public NDbResult<List<TSB>> Gets()
            {
                var ret = TSB.GetTSBs();
                return ret;
            }
        }
    }
}
