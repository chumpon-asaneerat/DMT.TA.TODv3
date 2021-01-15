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
            /// <summary>
            /// Gets Lanes By TSB.
            /// </summary>
            /// <param name="value">The TSB.</param>
            /// <returns></returns>
            [HttpPost]
            [ActionName(RouteConsts.TOD.Infrastructure.Lane.Search.ByTSB.Name)]
            //[AllowAnonymous]
            public NDbResult<List<Lane>> ByTSB([FromBody] TSB value)
            {
                var ret = Lane.GetTSBLanes(value);
                return ret;
            }
            /// <summary>
            /// Gets Lanes By PlazaGroup.
            /// </summary>
            /// <param name="value">The PlazaGroup.</param>
            /// <returns></returns>
            [HttpPost]
            [ActionName(RouteConsts.TOD.Infrastructure.Lane.Search.ByPlazaGroup.Name)]
            //[AllowAnonymous]
            public NDbResult<List<Lane>> ByPlazaGroup([FromBody] PlazaGroup value)
            {
                var ret = Lane.GetPlazaGroupLanes(value);
                return ret;
            }
            /// <summary>
            /// Get Lanes By Plaza.
            /// </summary>
            /// <param name="value">The Plaza.</param>
            /// <returns></returns>
            [HttpPost]
            [ActionName(RouteConsts.TOD.Infrastructure.Lane.Search.ByPlaza.Name)]
            //[AllowAnonymous]
            public NDbResult<List<Lane>> ByTSB([FromBody] Plaza value)
            {
                var ret = Lane.GetPlazaLanes(value);
                return ret;
            }
        }
    }
}
