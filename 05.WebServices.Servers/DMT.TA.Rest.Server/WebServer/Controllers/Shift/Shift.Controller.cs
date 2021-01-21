#region Using

using System;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// The Shift class.
    /// </summary>
    public partial class Shift
    {
        /// <summary>The TSB Controller class.</summary>
        [Authorize]
        public partial class CommonController : ApiController { }

        /// <summary>The TSB Controller class.</summary>
        [Authorize]
        public partial class TSBController : ApiController { }

        /// <summary>The User Controller class.</summary>
        [Authorize]
        public partial class UserController : ApiController { }
    }

    // Exports nested class to controller(s)
    /// <summary>
    /// The TSB Shift's Manage Controller class.
    /// </summary>
    public class ShiftController : Shift.CommonController { }
    /// <summary>
    /// The TSB Shift's Manage Controller class.
    /// </summary>
    public class TSBShiftManageController : Shift.TSBController { }
    /// <summary>
    /// The User Shift's Manage Controller class.
    /// </summary>
    public class UserShiftManageController : Shift.UserController { }
}
