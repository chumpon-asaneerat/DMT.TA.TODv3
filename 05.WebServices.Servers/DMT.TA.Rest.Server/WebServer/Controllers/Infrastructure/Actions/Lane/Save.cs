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
            [ActionName(RouteConsts.TA.Infrastructure.Lane.Save.Name)]
            //[AllowAnonymous]
            public NDbResult<Lane> Save([FromBody] Lane value)
            {
                var ret = Lane.SaveLane(value);
                return ret;
            }
        }
    }
}
