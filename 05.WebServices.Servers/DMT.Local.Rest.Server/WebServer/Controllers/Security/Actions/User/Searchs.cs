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
            #region ById

            /// <summary>
            /// Gets User By UserId.
            /// </summary>
            /// <param name="value">The Search.User.ById class instance.</param>
            /// <returns></returns>
            [HttpPost]
            [ActionName(RouteConsts.Security.User.Search.ById.Name)]
            //[AllowAnonymous]
            public NDbResult<Models.User> ById([FromBody] Models.Search.User.ById value)
            {
                NDbResult<Models.User> ret;
                if (null == value)
                {
                    ret = new NDbResult<User>();
                    ret.ParameterIsNull();
                }
                else
                {
                    ret = Models.User.GetByUserId(value.UserId);
                }
                return ret;
            }

            #endregion

            #region ByCardId

            /// <summary>
            /// Get User By CardId.
            /// </summary>
            /// <param name="value">The Search.User.ByCardId class instance.</param>
            /// <returns></returns>
            [HttpPost]
            [ActionName(RouteConsts.Security.User.Search.ByCardId.Name)]
            //[AllowAnonymous]
            public NDbResult<Models.User> ByCardId([FromBody] Models.Search.User.ByCardId value)
            {
                NDbResult<Models.User> ret;
                if (null == value)
                {
                    ret = new NDbResult<User>();
                    ret.ParameterIsNull();
                }
                else
                {
                    ret = Models.User.GetByCardId(value.CardId);
                }
                return ret;
            }

            #endregion

            #region ByLogIn

            /// <summary>
            /// Get User By LogIn.
            /// </summary>
            /// <param name="value">The Search.User.ByLogIn class instance.</param>
            /// <returns></returns>
            [HttpPost]
            [ActionName(RouteConsts.Security.User.Search.ByLogIn.Name)]
            //[AllowAnonymous]
            public NDbResult<Models.User> ByLogIn([FromBody] Models.Search.User.ByLogIn value)
            {
                NDbResult<Models.User> ret;
                if (null == value)
                {
                    ret = new NDbResult<User>();
                    ret.ParameterIsNull();
                }
                else
                {
                    ret = Models.User.GetByLogIn(value.UserId, value.Password);
                }
                return ret;
            }

            #endregion

            #region ByRoleId

            /// <summary>
            /// Get Users By RoleId.
            /// </summary>
            /// <param name="value">The Search.User.ByRoleId class instance.</param>
            /// <returns></returns>
            [HttpPost]
            [ActionName(RouteConsts.Security.User.Search.ByRoleId.Name)]
            //[AllowAnonymous]
            public NDbResult<List<Models.User>> ByRoleId([FromBody] Models.Search.User.ByRoleId value)
            {
                NDbResult<List<Models.User>> ret;
                if (null == value)
                {
                    ret = new NDbResult<List<Models.User>>();
                    ret.ParameterIsNull();
                }
                else
                {
                    ret = Models.User.FilterByRoleId(value.RoleId);
                }
                return ret;
            }

            #endregion

            #region ByGroupId

            /// <summary>
            /// Get Users By GroupId.
            /// </summary>
            /// <param name="value">The Search.User.ByGroupId class instance.</param>
            /// <returns></returns>
            [HttpPost]
            [ActionName(RouteConsts.Security.User.Search.ByGroupId.Name)]
            //[AllowAnonymous]
            public NDbResult<List<Models.User>> ByGroupId([FromBody] Models.Search.User.ByGroupId value)
            {
                NDbResult<List<Models.User>> ret;
                if (null == value)
                {
                    ret = new NDbResult<List<Models.User>>();
                    ret.ParameterIsNull();
                }
                else
                {
                    ret = Models.User.FilterByGroupId(value.GroupId);
                }
                return ret;
            }

            #endregion

            #region ByFilter

            /// <summary>
            /// Get Users filter by UserId (increment search).
            /// </summary>
            /// <param name="value">The Search.User.FilterById class instance.</param>
            /// <returns></returns>
            [HttpPost]
            [ActionName(RouteConsts.Security.User.Search.ByFilter.Name)]
            //[AllowAnonymous]
            public NDbResult<List<Models.User>> ByFilter([FromBody] Models.Search.User.ByFilter value)
            {
                NDbResult<List<Models.User>> ret;
                if (null == value)
                {
                    ret = new NDbResult<List<Models.User>>();
                    ret.ParameterIsNull();
                }
                else
                {
                    ret = Models.User.FilterByUserId(value.UserId, value.Roles);
                }
                return ret;
            }

            #endregion
        }
    }
}
