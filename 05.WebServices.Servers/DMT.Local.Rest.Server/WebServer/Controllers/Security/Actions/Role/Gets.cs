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
            [ActionName(RouteConsts.Security.Role.Gets.Name)]
            //[AllowAnonymous]
            public NDbResult<List<Role>> Gets()
            {
                var ret = Role.GetRoles();
                return ret;
            }
        }
    }
}
