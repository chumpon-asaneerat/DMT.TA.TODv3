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
            [HttpPost]
            [ActionName(RouteConsts.TOD.Infrastructure.Plaza.Gets.Name)]
            //[AllowAnonymous]
            public NDbResult<List<Plaza>> Gets()
            {
                var ret = Plaza.GetPlazas();
                return ret;
            }
        }
    }
}
