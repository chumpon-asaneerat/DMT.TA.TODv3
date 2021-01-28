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
}
