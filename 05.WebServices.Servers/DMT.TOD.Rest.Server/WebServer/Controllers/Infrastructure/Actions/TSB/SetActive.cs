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

    partial class Infrastructure
    {
        partial class TSBController
        {
            [HttpPost]
            [ActionName(RouteConsts.TOD.Infrastructure.TSB.SetActive.Name)]
            //[AllowAnonymous]
            public NDbResult SetActive([FromBody] TSB value)
            {
                var ret = TSB.SetActive(value.TSBId);
                if (null != ret && ret.Ok)
                {
                    // Notify TSBChanged to (TAApp and TODApp).
                    TAOps.TSBChanged();
                    TODOps.TSBChanged();
                }
                return ret;
            }
        }
    }
}
