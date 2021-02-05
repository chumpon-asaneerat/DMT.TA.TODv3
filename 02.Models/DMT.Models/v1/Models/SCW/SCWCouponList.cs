#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    #region Parameter classes

    /// <summary>The SCWCouponList class.</summary>
    public class SCWCouponList
    {
        /// <summary>Gets or sets networkId.</summary>
        [PropertyMapName("networkId")]
        public int? networkId { get; set; }
    }

    #endregion

    #region Result related classes

    /// <summary>The SCWCoupon class.</summary>
    public class SCWCoupon
    {
        /// <summary>Gets or sets couponId.</summary>
        [PropertyMapName("couponId")]
        public int couponId { get; set; }

        /// <summary>Gets or sets couponValue.</summary>
        [PropertyMapName("couponValue")]
        public decimal couponValue { get; set; }

        /// <summary>Gets or sets abbreviation.</summary>
        [PropertyMapName("abbreviation")]
        public string abbreviation { get; set; }

        /// <summary>Gets or sets description.</summary>
        [PropertyMapName("description")]
        public string description { get; set; }
    }

    /// <summary>The SCWCouponListResult class.</summary>
    public class SCWCouponListResult
    {
        /// <summary>Gets or sets list.</summary>
        //[PropertyMapName("list")]
        public List<SCWCoupon> list { get; set; }

        /// <summary>Gets or sets status.</summary>
        [PropertyMapName("status")]
        public SCWStatus status { get; set; }
    }

    #endregion
}
