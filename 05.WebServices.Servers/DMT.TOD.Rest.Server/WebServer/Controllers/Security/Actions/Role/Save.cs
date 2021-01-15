#region Using

using System;
using System.Collections.Generic;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    partial class Security
    {
        partial class RoleController
        {
            [HttpPost]
            [ActionName(RouteConsts.TOD.Security.Role.Save.Name)]
            //[AllowAnonymous]
            public NDbResult<Models.Role> Save([FromBody] Models.Role value)
            {
                var ret = Models.Role.SaveRole(value);
                return ret;
            }
        }
    }
}
