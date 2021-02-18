#region Using

using System;
using System.Collections.Generic;
using System.Linq;

using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;

// required for JsonIgnore.
using Newtonsoft.Json;
using NLib;
using NLib.Reflection;
using System.Security.Permissions;

#endregion

namespace DMT.Models
{
    #region Search Main class (empty method).

    /// <summary>
    /// The Search class.
    /// </summary>
    public partial class Search { }

    #endregion

    #region Revenues
    /*
    partial class Search
    {
        public static class Revenues
        {
            public class PlazaShift : NSearch<PlazaShift>
            {
                public UserShift Shift { get; set; }
                public PlazaGroup PlazaGroup { get; set; }

                public static PlazaShift Create(UserShift shift, PlazaGroup plazaGroup)
                {
                    var ret = new PlazaShift();
                    ret.Shift = shift;
                    ret.PlazaGroup = plazaGroup;
                    return ret;
                }
            }
            public class SaveRevenueShift : NSearch<SaveRevenueShift>
            {
                public UserShiftRevenue RevenueShift { get; set; }
                public string RevenueId { get; set; }
                public DateTime RevenueDate { get; set; }

                public static SaveRevenueShift Create(UserShiftRevenue revenueShift,
                    string revenueId, DateTime revenueDate)
                {
                    var ret = new SaveRevenueShift();
                    ret.RevenueShift = revenueShift;
                    ret.RevenueId = revenueId;
                    ret.RevenueDate = revenueDate;
                    return ret;
                }
            }
        }
    }
    */
    #endregion

    #region Search (Credit)

    static partial class Search
    {
        /// <summary>Credit Searchs.</summary>
        public static partial class Credit
        {
            /// <summary>TSB Credit Searchs.</summary>
            public static partial class TSB
            {

            }
            /// <summary>User Credit Searchs.</summary>
            public static partial class User
            {
                #region Current

                /// <summary>
                /// Gets Current User Credit (By PlazaGroup). 
                /// State is not complted and has No RevenueId.
                /// </summary>
                public class Current : NSearch<Current>
                {
                    #region Public Properties

                    /// <summary>
                    /// Gets or sets User.
                    /// </summary>
                    public Models.User User { get; set; }
                    /// <summary>
                    /// Gets or sets Plaza Group.
                    /// </summary>
                    public Models.PlazaGroup PlazaGroup { get; set; }

                    #endregion

                    #region Static Method (Create)

                    /// <summary>
                    /// Create Search instance.
                    /// </summary>
                    /// <param name="user">The User.</param>
                    /// <param name="plazaGroup">The Plaza Group.</param>
                    /// <returns>Returns Search instance.</returns>
                    public static Current Create(Models.User user,
                        Models.PlazaGroup plazaGroup)
                    {
                        var ret = new Current();
                        ret.User = user;
                        ret.PlazaGroup = plazaGroup;
                        return ret;
                    }

                    #endregion
                }

                #endregion

                #region Completed

                /// <summary>
                /// Gets Completed User Credit (By PlazaGroup). 
                /// State is complted and has No RevenueId.
                /// </summary>
                public class Completed : NSearch<Completed>
                {
                    #region Public Properties

                    /// <summary>
                    /// Gets or sets User.
                    /// </summary>
                    public Models.User User { get; set; }
                    /// <summary>
                    /// Gets or sets Plaza Group.
                    /// </summary>
                    public Models.PlazaGroup PlazaGroup { get; set; }

                    #endregion

                    #region Static Method (Create)

                    /// <summary>
                    /// Create Search instance.
                    /// </summary>
                    /// <param name="user">The User.</param>
                    /// <param name="plazaGroup">The Plaza Group.</param>
                    /// <returns>Returns Search instance.</returns>
                    public static Completed Create(Models.User user,
                        Models.PlazaGroup plazaGroup)
                    {
                        var ret = new Completed();
                        ret.User = user;
                        ret.PlazaGroup = plazaGroup;
                        return ret;
                    }

                    #endregion
                }

                #endregion

                #region Comment out

                /*
                public class GetActiveById : NSearch<GetActive>
                {
                    public string UserId { get; set; }
                    public string PlazaGroupId { get; set; }

                    public static GetActiveById Create(string userId, string plazGroupId)
                    {
                        var ret = new GetActiveById();
                        ret.UserId = userId;
                        ret.PlazaGroupId = plazGroupId;
                        return ret;
                    }
                }
                */

                #endregion
            }
        }
    }

    #endregion

    #region Search (Coupon)

    static partial class Search
    {
        /// <summary>Coupon Searchs.</summary>
        public static partial class Coupon
        {
            /// <summary>TSB Coupon Searchs.</summary>
            public static partial class TSB
            {

            }
            /// <summary>User Coupon Searchs.</summary>
            public static partial class User
            {
                #region Current

                /// <summary>
                /// Gets User Coupon Sold Balance. 
                /// </summary>
                public class Sold : NSearch<Sold>
                {
                    #region Public Properties

                    /// <summary>
                    /// Gets or sets PlazaGroup.
                    /// </summary>
                    public Models.PlazaGroup PlazaGroup { get; set; }
                    /// <summary>
                    /// Gets or sets User.
                    /// </summary>
                    public Models.User User { get; set; }
                    /// <summary>
                    /// Gets or sets Start Time (to check SoldDate).
                    /// </summary>
                    public DateTime? Start { get; set; }
                    /// <summary>
                    /// Gets or sets End Time (to check SoldDate).
                    /// </summary>
                    public DateTime? End { get; set; }

                    #endregion

                    #region Static Method (Create)

                    /// <summary>
                    /// Create Search instance.
                    /// </summary>
                    /// <param name="usplazaGrouper">The PlazaGroup.</param>
                    /// <param name="user">The User.</param>
                    /// <param name="start">The Start Time (to check SoldDate).</param>
                    /// <param name="end">The End Time (to check SoldDate).</param>
                    /// <returns>Returns Search instance.</returns>
                    public static Sold Create(Models.PlazaGroup plazaGroup, Models.User user, DateTime? start, DateTime? end)
                    {
                        var ret = new Sold();
                        ret.PlazaGroup = plazaGroup;
                        ret.User = user;
                        ret.Start = start;
                        ret.End = end;
                        return ret;
                    }

                    #endregion
                }

                #endregion
            }
        }
    }


    #endregion

    #region UserCoupon

    /*
    partial class Search
    {
        public class TSBCoupons
        {
            public class ByUser : NSearch<ByUser>
            {
                public TSB TSB { get; set; }
                public User User { get; set; }

                public static ByUser Create(TSB tsb, User user)
                {
                    var ret = new ByUser();
                    ret.User = user;
                    ret.TSB = tsb;
                    return ret;
                }
            }
        }

        public class UserCoupons
        {
            public class BorrowCoupons : NSearch<BorrowCoupons>
            {
                public User User { get; set; }
                public List<TSBCouponTransaction> Coupons { get; set; }

                public static BorrowCoupons Create(User user, List<TSBCouponTransaction> coupons)
                {
                    var ret = new BorrowCoupons();
                    ret.User = user;
                    ret.Coupons = coupons;
                    return ret;
                }
            }

            public class ReturnCoupons : NSearch<ReturnCoupons>
            {
                public User User { get; set; }
                public List<TSBCouponTransaction> Coupons { get; set; }

                public static ReturnCoupons Create(User user, List<TSBCouponTransaction> coupons)
                {
                    var ret = new ReturnCoupons();
                    ret.User = user;
                    ret.Coupons = coupons;
                    return ret;
                }
            }

            public class ByUser : NSearch<ByUser>
            {
                public TSB TSB { get; set; }
                public User User { get; set; }

                public static ByUser Create(TSB tsb, User user)
                {
                    var ret = new ByUser();
                    ret.User = user;
                    ret.TSB = tsb;
                    return ret;
                }
            }

            public class ToTSBCoupons : NSearch<ToTSBCoupons>
            {
                public TSB TSB { get; set; }
                public List<UserCouponTransaction> Coupons { get; set; }

                public static ToTSBCoupons Create(TSB tsb, List<UserCouponTransaction> coupons)
                {
                    var ret = new ToTSBCoupons();
                    ret.TSB = tsb;
                    ret.Coupons = coupons;
                    return ret;
                }
            }
        }
    }
    */
    #endregion
}
