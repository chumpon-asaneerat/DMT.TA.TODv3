#region Using

using System;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// The Infrastructure class.
    /// </summary>
    public partial class Infrastructure
    {
        /// <summary>The TSB Controller class.</summary>
        [Authorize]
        public partial class TSBController : ApiController { }

        /// <summary>The PlazaGroup Controller class.</summary>
        [Authorize]
        public partial class PlazaGroupController : ApiController { }

        /// <summary>The Plaza Controller class.</summary>
        [Authorize]
        public partial class PlazaController : ApiController { }

        /// <summary>The Lane Controller class.</summary>
        [Authorize]
        public partial class LaneController : ApiController { }
    }

    // Exports nested class to controller(s)
    /// <summary>
    /// The Infrastructure's TSB Manage Controller class.
    /// </summary>
    public class TSBManageController : Infrastructure.TSBController { }
    /// <summary>
    /// The Infrastructure's PlazaGroup Manage Controller class.
    /// </summary>
    public class PlazaGroupManageController : Infrastructure.PlazaGroupController { }
    /// <summary>
    /// The Infrastructure's Plaza Manage Controller class.
    /// </summary>
    public class PlazaManageController : Infrastructure.PlazaController { }
    /// <summary>
    /// The Infrastructure's Lane Manage Controller class.
    /// </summary>
    public class LaneManageController : Infrastructure.LaneController { }
}
