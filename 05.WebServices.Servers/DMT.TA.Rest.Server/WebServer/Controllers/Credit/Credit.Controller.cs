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
        /// <summary>The TSB Controller class.</summary>
        [Authorize]
        public partial class TSBController : ApiController { }

        /// <summary>The User Controller class.</summary>
        [Authorize]
        public partial class UserController : ApiController { }
    }

    // Exports nested class to controller(s)
    /// <summary>
    /// The TSB Credit's Manage Controller class.
    /// </summary>
    public class TSBCreditManageController : Credit.TSBController { }
    /// <summary>
    /// The User Credit's Manage Controller class.
    /// </summary>
    public class UserCreditManageController : Credit.UserController { }
}
