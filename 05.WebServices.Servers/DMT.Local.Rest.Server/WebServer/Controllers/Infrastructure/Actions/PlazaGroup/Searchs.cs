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
        partial class PlazaGroupController
        {
            /// <summary>
            /// Gets PlazaGroups By TSB.
            /// </summary>
            /// <param name="value">The TSB.</param>
            /// <returns></returns>
            [HttpPost]
            [ActionName(RouteConsts.Infrastructure.PlazaGroup.Search.ByTSB.Name)]
            //[AllowAnonymous]
            public NDbResult<List<PlazaGroup>> ByTSB([FromBody] TSB value)
            {
                var ret = PlazaGroup.GetTSBPlazaGroups(value);
                return ret;
            }
        }
    }
}
