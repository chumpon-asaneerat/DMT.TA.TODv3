#region Using

using System;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// The Security class.
    /// </summary>
    public partial class Security
    {
        /// <summary>The Role Controller class.</summary>
        [Authorize]
        public partial class RoleController : ApiController { }

        /// <summary>The User Controller class.</summary>
        [Authorize]
        public partial class UserController : ApiController { }
    }

    // Exports nested class to controller(s)
    /// <summary>
    /// The Security's Role Manage Controller class.
    /// </summary>
    public class TODRoleManageController : Security.RoleController { }
    /// <summary>
    /// The Security's User Manage Controller class.
    /// </summary>
    public class TODUserManageController : Security.UserController { }
}
