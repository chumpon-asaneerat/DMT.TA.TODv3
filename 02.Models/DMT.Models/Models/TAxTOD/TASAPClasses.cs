#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

using Newtonsoft.Json;

#endregion

// SAPCustomer: Url: /api/account/sap/getcustomer
namespace DMT.Models
{
    // Server data result.
    /*
    {
        "CardCode": "111",
        "CardName": "Novatec Healthcare Co.,Ltd.",
        "UseFlag": 1
    }
    */
    /// <summary>The SAPCustomer class.</summary>
    public class SAPCustomer
    {
        /// <summary>Gets or sets CardCode.</summary>
        [PropertyMapName("CardCode")]
        public string CardCode { get; set; }
        /// <summary>Gets or sets CardName.</summary>
        [PropertyMapName("CardName")]
        public string CardName { get; set; }
        /// <summary>Gets or sets UseFlag.</summary>
        [PropertyMapName("UseFlag")]
        public int UseFlag { get; set; }
    }

    // Server data parameter.
    /*
    {
        "searchtext": null
    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {
                #region Customers

                /// <summary>
                /// Customers.
                /// </summary>
                public class Customers : NSearch<Customers>
                {
                    #region Public Properties

                    /// <summary>
                    /// Gets or sets searchtext.
                    /// </summary>
                    public string searchtext { get; set; }

                    #endregion

                    #region Static Method (Create)

                    /// <summary>
                    /// Create Search instance.
                    /// </summary>
                    /// <param name="filter">The filter.</param>
                    /// <returns>Returns Search instance.</returns>
                    public static Customers Create(
                        string filter = "")
                    {
                        var ret = new Customers();
                        ret.searchtext = filter;
                        return ret;
                    }

                    #endregion
                }

                #endregion
            }
        }
    }
}

// SAPTSB: Url: /api/account/sap/tsblist
namespace DMT.Models
{
    // Server data result
    /*
    {
      "TSBId": "09",
      "TSB_Th_Name": "อนุสรณ์สถาน",
      "TSB_Eng_name": "ANUSORN SATHAN",
      "SapWhsCode": "CAS",
      "TollwayID": 9
    }
    */
    // Server data parameter.
    /*
    {

    }
    */
    /// <summary>The SAPTSB class.</summary>
    public class SAPTSB
    {
        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }
        /// <summary>Gets or sets TSB_Th_Name.</summary>
        [PropertyMapName("TSB_Th_Name")]
        public string TSB_Th_Name { get; set; }
        /// <summary>Gets or sets TSB_Eng_name.</summary>
        [PropertyMapName("TSB_Eng_name")]
        public string TSB_Eng_name { get; set; }
        /// <summary>Gets or sets SapWhsCode.</summary>
        [PropertyMapName("SapWhsCode")]
        public string SapWhsCode { get; set; }
        /// <summary>Gets or sets TollwayID.</summary>
        [PropertyMapName("TollwayID")]
        public int TollwayID { get; set; }
    }
}

// SAPCouponSold: Url: /api/account/sap/couponsoldlist
namespace DMT.Models
{
    // Server data result.
    /*
    {
        "TollWayId": 9,
        "CouponType": "35",
        "SAPItemName": "Coupon 35 Baht",
        "SerialNo": "ข009863",
        "SAPSysSerial": 747977,
        "SoldDate": "2021-02-20T11:10:40.510Z",
        "SoldBy": "20001",
        "LaneId": "AN12"
    }
    */
    /// <summary>The SAPCouponSold class.</summary>
    public class SAPCouponSold
    {
        /// <summary>Gets or sets TollwayID.</summary>
        [PropertyMapName("TollwayID")]
        public int TollwayID { get; set; }
        /// <summary>Gets or sets CouponType.</summary>
        [PropertyMapName("CouponType")]
        public string CouponType { get; set; }
        /// <summary>Gets or sets SAPItemName.</summary>
        [PropertyMapName("SAPItemName")]
        public string SAPItemName { get; set; }
        /// <summary>Gets or sets SerialNo.</summary>
        [PropertyMapName("SerialNo")]
        public string SerialNo { get; set; }
        /// <summary>Gets or sets SAPSysSerial.</summary>
        [PropertyMapName("SAPSysSerial")]
        public string SAPSysSerial { get; set; }

        /// <summary>Gets or sets SoldDate.</summary>
        [PropertyMapName("SoldDate")]
        public DateTime? SoldDate { get; set; }
        /// <summary>Gets or sets SoldDateString.</summary>
        [JsonIgnore]
        public string SoldDateString
        {
            get
            {
                var ret = (!this.SoldDate.HasValue || this.SoldDate.Value == DateTime.MinValue) ?
                    "" : this.SoldDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }

        /// <summary>Gets or sets SoldBy.</summary>
        [PropertyMapName("SoldBy")]
        public string SoldBy { get; set; }
        /// <summary>Gets or sets LaneId.</summary>
        [PropertyMapName("LaneId")]
        public string LaneId { get; set; }
    }
    // Server data parameter.
    /*
    {
        "tollwayid": 9 ,
        "solddate": "2021-02-20"
    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {
                #region CouponSolds

                /// <summary>
                /// CouponSolds.
                /// </summary>
                public class CouponSolds : NSearch<CouponSolds>
                {
                    #region Public Properties

                    /// <summary>
                    /// Gets or sets tollwayid.
                    /// </summary>
                    public int tollwayid { get; set; }
                    /// <summary>
                    /// Gets or sets solddate.
                    /// </summary>
                    public DateTime? solddate { get; set; }

                    #endregion

                    #region Static Method (Create)

                    /// <summary>
                    /// Create Search instance.
                    /// </summary>
                    /// <param name="tollwayid">The tollwayid.</param>
                    /// <param name="solddate">The solddate.</param>
                    /// <returns>Returns Search instance.</returns>
                    public static CouponSolds Create(
                        DateTime? solddate = new DateTime?(), int tollwayid = 9)
                    {
                        var ret = new CouponSolds();
                        ret.tollwayid = tollwayid;
                        ret.solddate = solddate;
                        return ret;
                    }

                    #endregion
                }

                #endregion
            }
        }
    }
}

namespace DMT.Models
{
    // Url: /api/account/sap/save/arhead

    // Server data parameter.
    /*
    {

    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {

            }
        }
    }
}

namespace DMT.Models
{
    // Url: /api/account/sap/couponsoldlist

    // Server data parameter.
    /*
    {

    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {

            }
        }
    }
}

namespace DMT.Models
{
    // Url: /api/account/sap/get/arsumcoupon

    // Server data parameter.
    /*
    {

    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {

            }
        }
    }
}

namespace DMT.Models
{
    // Url: /api/account/sap/save/arline

    // Server data parameter.
    /*
    {

    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {

            }
        }
    }
}

namespace DMT.Models
{
    // Url: /api/account/sap/get/arcouponlist

    // Server data parameter.
    /*
    {

    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {

            }
        }
    }
}

namespace DMT.Models
{
    // Url: /api/account/sap/save/arserial

    // Server data parameter.
    /*
    {

    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {

            }
        }
    }
}
