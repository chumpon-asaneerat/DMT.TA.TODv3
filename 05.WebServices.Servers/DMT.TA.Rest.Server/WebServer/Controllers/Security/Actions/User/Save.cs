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
            [ActionName(RouteConsts.TA.Security.User.Save.Name)]
            //[AllowAnonymous]
            public NDbResult<Models.User> Save([FromBody] Models.User value)
            {
                var ret = Models.User.SaveUser(value);
                return ret;
            }
        }
    }
}
