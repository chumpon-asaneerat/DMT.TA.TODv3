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

        /// <summary>The TOD TSB Shift Controller class.</summary>
        [Authorize]
        public partial class TSBController : ApiController { }

        /// <summary>The TOD User Shift Controller class.</summary>
        [Authorize]
        public partial class UserController : ApiController { }
    }

    // Exports nested class to controller(s)
    /// <summary>
    /// The TOD Shift's Manage Controller class.
    /// </summary>
    public class TODShiftController : Shift.CommonController { }
    // Exports nested class to controller(s)
    /// <summary>
    /// The TOD TSB Shift's Manage Controller class.
    /// </summary>
    public class TODTSBShiftController : Shift.CommonController { }
    // Exports nested class to controller(s)
    /// <summary>
    /// The TOD User Shift's Manage Controller class.
    /// </summary>
    public class TODUserShiftController : Shift.CommonController { }
}
