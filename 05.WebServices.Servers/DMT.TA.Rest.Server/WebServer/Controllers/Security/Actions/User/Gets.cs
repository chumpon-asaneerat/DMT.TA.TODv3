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
        partial class UserController
        {
            [HttpPost]
            [ActionName(RouteConsts.TA.Security.User.Gets.Name)]
            //[AllowAnonymous]
            public NDbResult<List<Models.User>> Gets()
            {
                var ret = Models.User.GetUsers();
                return ret;
            }
        }
    }
}
