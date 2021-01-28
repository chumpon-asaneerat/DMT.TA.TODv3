#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using NLib;
using NLib.Design;
using NLib.Reflection;

using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
// required for JsonIgnore attribute.
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

#endregion

namespace DMT.Models
{
    #region CouponType

    /// <summary>
    /// The Coupon Type enum.
    /// </summary>
    public enum CouponType : int
    {
        /// <summary>
        /// Coupon 35 BHT
        /// </summary>
        BHT35 = 35,
        /// <summary>
        /// Coupon 80 BHT
        /// </summary>
        BHT80 = 80
    }

    #endregion

    #region TSB Coupon Transaction Types

    /// <summary>
    /// The TSB Transaction Types Enum.
    /// </summary>
    public enum TSBCouponTransactionTypes : int
    {
        /// <summary>
        /// Cancel or Remove
        /// </summary>
        CancelOrRemove = 0,
        /// <summary>
        /// TSB Stock
        /// </summary>
        Stock = 1,
        /// <summary>
        /// Borrow By User on Lane
        /// </summary>
        Lane = 2,
        /// <summary>
        /// Sold By User on Lane
        /// </summary>
        SoldByLane = 3,
        /// <summary>
        /// Sold By Supervisor on TSB
        /// </summary>
        SoldByTSB = 4
    }

    #endregion

    #region TSB Coupon Finished Flags

    /// <summary>
    /// The TSB Coupon Finished Flags Enum.
    /// </summary>
    public enum TSBCouponFinishedFlags : int
    {
        /// <summary>
        /// Completed
        /// </summary>
        Completed = 0,
        /// <summary>
        /// Avaliable
        /// </summary>
        Avaliable = 1
    }

    #endregion
}
