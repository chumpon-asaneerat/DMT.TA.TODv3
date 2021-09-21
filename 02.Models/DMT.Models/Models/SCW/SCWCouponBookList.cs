#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    #region Parameter classes

    /// <summary>The SCWCouponBookList class.</summary>
    public class SCWCouponBookList
    {
        /// <summary>Gets or sets networkId.</summary>
        [PropertyMapName("networkId")]
        public int? networkId { get; set; }
    }

    #endregion

    #region Result related classes

    /// <summary>The SCWCouponBook class.</summary>
    public class SCWCouponBook
    {
        /// <summary>Gets or sets couponBookId.</summary>
        [PropertyMapName("couponBookId")]
        public int couponBookId { get; set; }

        /// <summary>Gets or sets couponBookValue.</summary>
        [PropertyMapName("couponBookValue")]
        public decimal couponBookValue { get; set; }

        /// <summary>Gets or sets abbreviation.</summary>
        [PropertyMapName("abbreviation")]
        public string abbreviation { get; set; }

        /// <summary>Gets or sets description.</summary>
        [PropertyMapName("description")]
        public string description { get; set; }
    }

    /// <summary>The SCWCouponBookListResult class.</summary>
    public class SCWCouponBookListResult : SCWResult
    {
        /// <summary>Gets or sets list.</summary>
        //[PropertyMapName("list")]
        public List<SCWCouponBook> list { get; set; }
    }

    #endregion
}
