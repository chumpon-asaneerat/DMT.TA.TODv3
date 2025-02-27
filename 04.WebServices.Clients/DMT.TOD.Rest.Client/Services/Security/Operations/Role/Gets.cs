﻿#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TOD
    {
        static partial class Security
        {
            static partial class Role
            {
                /// <summary>
                /// Gets all Roles.
                /// </summary>
                /// <returns>Returns all Roles.</returns>
                public static NRestResult<List<Models.Role>> Gets()
                {
                    var ret = Execute<List<Models.Role>>(
                        RouteConsts.TOD.Security.Role.Gets.Url, new { });
                    return ret;
                }
            }
        }
    }
}
