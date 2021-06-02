#region Using

using System;
using System.Web.Http;
using DMT.Models;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// The Revenue Controller class.
    /// </summary>
    [Authorize] // Authorize Attribute can set here or set in each method(s).
    public partial class RevenueController : ApiController { }

    // Exports nested class to controller(s)
    /// <summary>
    /// The TA Revenue's Controller class.
    /// </summary>
    public class TAARevenueController : RevenueController { }
}
