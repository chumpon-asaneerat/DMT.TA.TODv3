#region Using

using System;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// The Credit class.
    /// </summary>
    public partial class Credit
    {
        /// <summary>The User Controller class.</summary>
        [Authorize]
        public partial class UserController : ApiController { }
    }

    /// <summary>
    /// The User Credit's Manage Controller class.
    /// </summary>
    public class TAAUserCreditManageController : Credit.UserController { }
}
