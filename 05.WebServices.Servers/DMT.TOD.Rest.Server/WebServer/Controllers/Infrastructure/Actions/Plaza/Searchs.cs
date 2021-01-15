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
        partial class PlazaController
        {
            /// <summary>
            /// Get Plazas By TSB.
            /// </summary>
            /// <param name="value">The TSB.</param>
            /// <returns></returns>
            [HttpPost]
            [ActionName(RouteConsts.TOD.Infrastructure.Plaza.Search.ByTSB.Name)]
            //[AllowAnonymous]
            public NDbResult<List<Plaza>> ByTSB([FromBody] TSB value)
            {
                var ret = Plaza.GetTSBPlazas(value);
                return ret;
            }
            /// <summary>
            /// Gets Plazas By PlazaGroup.
            /// </summary>
            /// <param name="value">The PlazaGroup.</param>
            /// <returns></returns>
            [HttpPost]
            [ActionName(RouteConsts.TOD.Infrastructure.Plaza.Search.ByPlazaGroup.Name)]
            //[AllowAnonymous]
            public NDbResult<List<Plaza>> ByPlazaGroup([FromBody] PlazaGroup value)
            {
                var ret = Plaza.GetPlazaGroupPlazas(value);
                return ret;
            }
        }
    }
}
