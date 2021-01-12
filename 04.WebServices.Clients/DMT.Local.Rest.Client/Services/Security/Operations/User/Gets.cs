#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class Plaza
    {
        static partial class Security
        {
            static partial class User
            {
                /// <summary>
                /// Gets all Users.
                /// </summary>
                /// <returns>Returns all Users.</returns>
                public static NRestResult<List<Models.User>> Gets()
                {
                    var ret = Execute<List<Models.User>>(
                        RouteConsts.Security.User.Gets.Url);
                    return ret;
                }
            }
        }
    }
}
