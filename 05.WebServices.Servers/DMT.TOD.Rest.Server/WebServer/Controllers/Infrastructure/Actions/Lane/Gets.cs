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
        partial class LaneController
        {
            [HttpPost]
            [ActionName(RouteConsts.TOD.Infrastructure.Lane.Gets.Name)]
            //[AllowAnonymous]
            public NDbResult<List<Lane>> Gets()
            {
                var ret = Lane.GetLanes();
                return ret;
            }
        }
    }
}
