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
                static partial class Search
                {
                    /// <summary>
                    /// Gets User By User Id.
                    /// </summary>
                    /// <param name="value">The Search.User.ById instance.</param>
                    /// <returns>Returns User instance.</returns>
                    public static NRestResult<Models.User> ById(Models.Search.User.ById value)
                    {
                        var ret = Execute<Models.User>(
                            RouteConsts.Security.User.Search.ById.Url, value);
                        return ret;
                    }
                    /// <summary>
                    /// Gets User By CardId.
                    /// </summary>
                    /// <param name="value">The Search.User.ByCardId instance.</param>
                    /// <returns>Returns User instance.</returns>
                    public static NRestResult<Models.User> ByCardId(Models.Search.User.ByCardId value)
                    {
                        var ret = Execute<Models.User>(
                            RouteConsts.Security.User.Search.ByCardId.Url, value);
                        return ret;
                    }
                    /// <summary>
                    /// Gets User By LogIn.
                    /// </summary>
                    /// <param name="value">The Search.User.ByLogIn instance.</param>
                    /// <returns>Returns User instance.</returns>
                    public static NRestResult<Models.User> ByLogIn(Models.Search.User.ByLogIn value)
                    {
                        var ret = Execute<Models.User>(
                            RouteConsts.Security.User.Search.ByLogIn.Url, value);
                        return ret;
                    }

                    /// <summary>
                    /// Gets Users By RoleId.
                    /// </summary>
                    /// <param name="value">The Search.User.ByRoleId instance.</param>
                    /// <returns>Returns Users list.</returns>
                    public static NRestResult<List<Models.User>> ByRoleId(Models.Search.User.ByRoleId value)
                    {
                        var ret = Execute<List<Models.User>>(
                            RouteConsts.Security.User.Search.ByRoleId.Url, value);
                        return ret;
                    }
                    /// <summary>
                    /// Gets Users By GroupId.
                    /// </summary>
                    /// <param name="value">The Search.User.ByGroupId instance.</param>
                    /// <returns>Returns Users list.</returns>
                    public static NRestResult<List<Models.User>> ByGroupId(Models.Search.User.ByGroupId value)
                    {
                        var ret = Execute<List<Models.User>>(
                            RouteConsts.Security.User.Search.ByGroupId.Url, value);
                        return ret;
                    }
                    /// <summary>
                    /// Gets Users By Filter.
                    /// </summary>
                    /// <param name="value">The Search.User.ByFilter instance.</param>
                    /// <returns>Returns Users list.</returns>
                    public static NRestResult<List<Models.User>> ByFilter(Models.Search.User.ByFilter value)
                    {
                        var ret = Execute<List<Models.User>>(
                            RouteConsts.Security.User.Search.ByFilter.Url, value);
                        return ret;
                    }
                }
            }
        }
    }
}
