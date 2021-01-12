#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class Plaza
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
                        RouteConsts.Security.Role.Gets.Url);
                    return ret;
                }
            }
        }
    }
}
