#region Using

using System;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// The Coupon class.
    /// </summary>
    public partial class Coupon
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
    /// The TSB Coupon's Manage Controller class.
    /// </summary>
    public class TSBCouponManageController : Coupon.TSBController { }
    /// <summary>
    /// The User Coupon's Manage Controller class.
    /// </summary>
    public class UserCouponManageController : Coupon.UserController { }
}
