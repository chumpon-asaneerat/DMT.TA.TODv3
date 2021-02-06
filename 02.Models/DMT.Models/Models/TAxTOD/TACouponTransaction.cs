#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    // Server data result.
    /*
    {
      "RowNo": "1",
      "CouponPK": 8,
      "TransactionDate": "2020-11-16T10:16:18.893Z",
      "TSBId": "09",
      "CouponType": "35",
      "SerialNo": "ข003954",
      "Price": 665,
      "UserId": "",
      "UserReceiveDate": null,
      "CouponStatus": "1",
      "SoldDate": null,
      "SoldBy": null,
      "LaneId": null,
      "FinishFlag": "1",
      "SapChooseFlag": "1",
      "SapChooseDate": null,
      "SAPSysSerial": 742068,
      "SAPWhsCode": "CAS",
      "TollWayId": 9,
      "SAPItemName": "Coupon 35 Baht",
      "sendtaflag": "0"
    }
    */

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

        /// <summary>Gets or sets UserId.</summary>
        [PropertyMapName("UserId")]
        public string UserId { get; set; }

        /// <summary>Gets or sets UserReceiveDate.</summary>
        [PropertyMapName("UserReceiveDate")]
        public DateTime? UserReceiveDate { get; set; }

        /// <summary>Gets or sets CouponStatus.</summary>
        [PropertyMapName("CouponStatus")]
        public int? CouponStatus { get; set; }

        /// <summary>Gets or sets SoldDate.</summary>
        [PropertyMapName("SoldDate")]
        public DateTime? SoldDate { get; set; }

        /// <summary>Gets or sets SoldBy.</summary>
        [PropertyMapName("SoldBy")]
        public string SoldBy { get; set; }

        /// <summary>Gets or sets LaneId.</summary>
        [PropertyMapName("LaneId")]
        public string LaneId { get; set; }

        /// <summary>Gets or sets FinishFlag.</summary>
        [PropertyMapName("FinishFlag")]
        public int? FinishFlag { get; set; }

        /// <summary>Gets or sets SapChooseFlag.</summary>
        [PropertyMapName("SapChooseFlag")]
        public int? SapChooseFlag { get; set; }

        /// <summary>Gets or sets SapChooseDate.</summary>
        [PropertyMapName("SapChooseDate")]
        public DateTime? SapChooseDate { get; set; }

        /// <summary>Gets or sets SAPSysSerial.</summary>
        [PropertyMapName("SAPSysSerial")]
        public string SAPSysSerial { get; set; }

        /// <summary>Gets or sets SAPWhsCode.</summary>
        [PropertyMapName("SAPWhsCode")]
        public string SAPWhsCode { get; set; }

        /// <summary>Gets or sets TollWayId.</summary>
        [PropertyMapName("TollWayId")]
        public int? TollWayId { get; set; }

        /// <summary>Gets or sets SAPItemName.</summary>
        [PropertyMapName("SAPItemName")]
        public string SAPItemName { get; set; }

        /// <summary>Gets or sets sendtaflag.</summary>
        [PropertyMapName("sendtaflag")]
        public int? sendtaflag { get; set; }
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

                    /// <summary>
                    /// Gets or sets PageNum.
                    /// </summary>
                    public int? PageNum { get; set; }
                    /// <summary>
                    /// Gets or sets RowsPerPage.
                    /// </summary>
                    public int? RowsPerPage { get; set; }
                    /// <summary>
                    /// Gets or sets flag (null for get all, 0 for get only new).
                    /// </summary>
                    public int? flag { get; set; }

                    #endregion

                    #region Static Method (Create)

                    /// <summary>
                    /// Create Search instance.
                    /// </summary>
                    /// <param name="tsbId">The TSB Id.</param>
                    /// <param name="userId">The User Id.</param>
                    /// <param name="transactionType">The Transaction Type.</param>
                    /// <param name="coupontype">The Coupon Type.</param>
                    /// <param name="flag">flag (null for get all, 0 for get only new).</param>
                    /// <param name="pageNum">The Page No.</param>
                    /// <param name="rowsPerPage">The Rows Per Page.</param>
                    /// <returns>Returns Search instance.</returns>
                    public static Gets Create(string tsbId, string userId = null, 
                        int? transactionType = null, int? coupontype = null,
                        int? flag = 0,
                        int? pageNum = 1, int? rowsPerPage = 20)
                    {
                        var ret = new Gets();
                        ret.TSBId = tsbId;
                        ret.UserId = userId;
                        ret.TransactionType = transactionType;
                        ret.Coupontype = coupontype;
                        ret.flag = flag;
                        ret.PageNum = pageNum;
                        ret.RowsPerPage = rowsPerPage;
                        return ret;
                    }

                    #endregion
                }

                #endregion
            }
        }
    }
}
