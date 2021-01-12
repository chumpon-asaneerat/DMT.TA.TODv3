#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    /// <summary>The TAServerCouponTransaction class.</summary>
    public class TAServerCouponTransaction
    {
        /// <summary>Gets or sets CouponPK.</summary>
        [PropertyMapName("CouponPK")]
        public int? CouponPK { get; set; }

        /// <summary>Gets or sets TransactionId.</summary>
        ////[PropertyMapName("TransactionId")]
        //public int? TransactionId { get; set; }

        /// <summary>Gets or sets TransactionDate.</summary>
        [PropertyMapName("TransactionDate")]
        public DateTime? TransactionDate { get; set; }

        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }

        /// <summary>Gets or sets CouponType.</summary>
        [PropertyMapName("CouponType")]
        public int? CouponType { get; set; }

        /// <summary>Gets or sets SerialNo.</summary>
        [PropertyMapName("SerialNo")]
        public string SerialNo { get; set; }

        /// <summary>Gets or sets Price.</summary>
        [PropertyMapName("Price")]
        public decimal? Price { get; set; }

        /// <summary>Gets or sets CouponStatus.</summary>
        [PropertyMapName("CouponStatus")]
        public int? CouponStatus { get; set; }

        /// <summary>Gets or sets LaneId.</summary>
        [PropertyMapName("LaneId")]
        public string LaneId { get; set; }

        /// <summary>Gets or sets UserId.</summary>
        [PropertyMapName("UserId")]
        public string UserId { get; set; }

        /// <summary>Gets or sets UserReceiveDate.</summary>
        [PropertyMapName("UserReceiveDate")]
        public DateTime? UserReceiveDate { get; set; }

        /// <summary>Gets or sets SoldDate.</summary>
        [PropertyMapName("SoldDate")]
        public DateTime? SoldDate { get; set; }

        /// <summary>Gets or sets SoldBy.</summary>
        [PropertyMapName("SoldBy")]
        public string SoldBy { get; set; }

        /// <summary>Gets or sets FinishFlag.</summary>
        [PropertyMapName("FinishFlag")]
        public int? FinishFlag { get; set; }

        /// <summary>Gets or sets SapChooseFlag.</summary>
        [PropertyMapName("SapChooseFlag")]
        public int? SapChooseFlag { get; set; }

        /// <summary>Gets or sets SapChooseDate.</summary>
        [PropertyMapName("SapChooseDate")]
        public DateTime? SapChooseDate { get; set; }
    }

    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class Coupon
            {
                #region Gets

                /// <summary>
                /// Gets.
                /// </summary>
                public class Gets : NSearch<Gets>
                {
                    #region Public Properties

                    /// <summary>
                    /// Gets or sets TSBId.
                    /// </summary>
                    public string TSBId { get; set; }
                    /// <summary>
                    /// Gets or sets User Id.
                    /// </summary>
                    public string UserId { get; set; }
                    /// <summary>
                    /// Gets or sets Transaction Type.
                    /// </summary>
                    public int? TransactionType { get; set; }
                    /// <summary>
                    /// Gets or sets Coupon Type.
                    /// </summary>
                    public int? Coupontype { get; set; }

                    #endregion

                    #region Static Method (Create)

                    /// <summary>
                    /// Create Search instance.
                    /// </summary>
                    /// <param name="tsbId">The TSB Id.</param>
                    /// <param name="userId">The User Id.</param>
                    /// <param name="transactionType">The Transaction Type.</param>
                    /// <param name="coupontype">The Coupon Type.</param>
                    /// <returns>Returns Search instance.</returns>
                    public static Gets Create(string tsbId, string userId = null, 
                        int? transactionType = null, int? coupontype = null)
                    {
                        var ret = new Gets();
                        ret.TSBId = tsbId;
                        ret.UserId = userId;
                        ret.TransactionType = transactionType;
                        ret.Coupontype = coupontype;
                        return ret;
                    }

                    #endregion
                }

                #endregion
            }
        }
    }
}
