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
            [ActionName(RouteConsts.TA.Infrastructure.TSB.Current.Name)]
            //[AllowAnonymous]
            public NDbResult<TSB> Current()
            {
                var ret = TSB.GetCurrent();
                return ret;
            }
        }
    }
}
