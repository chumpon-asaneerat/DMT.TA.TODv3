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
            [HttpPost]
            [ActionName(RouteConsts.TOD.Infrastructure.PlazaGroup.Gets.Name)]
            //[AllowAnonymous]
            public NDbResult<List<PlazaGroup>> Gets()
            {
                var ret = PlazaGroup.GetPlazaGroups();
                return ret;
            }
        }
    }
}
