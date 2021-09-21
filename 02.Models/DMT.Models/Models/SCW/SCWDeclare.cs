#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    #region Parameter related classes

    /// <summary>The SCWDeclareCash class.</summary>
    public class SCWDeclareCash
    {
        /// <summary>Gets or sets currencyId.</summary>
        [PropertyMapName("currencyId")]
        public int currencyId { get; set; }

        /// <summary>Gets or sets currencyDenomId.</summary>
        [PropertyMapName("currencyDenomId")]
        public int currencyDenomId { get; set; }

        /// <summary>Gets or sets denomValue.</summary>
        [PropertyMapName("denomValue")]
        public decimal denomValue { get; set; }

        /// <summary>Gets or sets number.</summary>
        [PropertyMapName("number")]
        public int number { get; set; }

        /// <summary>Gets or sets total.</summary>
        [PropertyMapName("total")]
        public decimal total { get; set; }
    }

    /// <summary>The SCWDeclareCoupon class.</summary>
    public class SCWDeclareCoupon
    {
        /// <summary>Gets or sets couponId.</summary>
        [PropertyMapName("couponId")]
        public int couponId { get; set; }

        /// <summary>Gets or sets couponValue.</summary>
        [PropertyMapName("couponValue")]
        public decimal couponValue { get; set; }

        /// <summary>Gets or sets number.</summary>
        [PropertyMapName("number")]
        public int number { get; set; }

        /// <summary>Gets or sets total.</summary>
        [PropertyMapName("currentotalcyId")]
        public decimal total { get; set; }
    }

    /// <summary>The SCWDeclareCouponBook class.</summary>
    public class SCWDeclareCouponBook
    {
        /// <summary>Gets or sets couponBookId.</summary>
        [PropertyMapName("couponBookId")]
        public int couponBookId { get; set; }

        /// <summary>Gets or sets couponBookValue.</summary>
        [PropertyMapName("couponBookValue")]
        public decimal couponBookValue { get; set; }

        /// <summary>Gets or sets number.</summary>
        [PropertyMapName("number")]
        public int number { get; set; }

        /// <summary>Gets or sets total.</summary>
        [PropertyMapName("total")]
        public decimal total { get; set; }
    }

    /// <summary>The SCWDeclareFreePass class.</summary>
    public class SCWDeclareFreePass
    {
        /// <summary>Gets or sets cardAllowId.</summary>
        [PropertyMapName("cardAllowId")]
        public int cardAllowId { get; set; }

        /// <summary>Gets or sets number.</summary>
        [PropertyMapName("number")]
        public int number { get; set; }
    }

    /// <summary>The SCWDeclareQRCode class.</summary>
    public class SCWDeclareQRCode
    {
        /// <summary>Gets or sets approvalCode.</summary>
        [PropertyMapName("approvalCode")]
        public string approvalCode { get; set; }

        /// <summary>Gets or sets trxDate.</summary>
        [PropertyMapName("trxDate")]
        public DateTime trxDate { get; set; }

        /// <summary>Gets or sets amount.</summary>
        [PropertyMapName("amount")]
        public decimal amount { get; set; }
    }

    /// <summary>The SCWDeclareEMV class.</summary>
    public class SCWDeclareEMV
    {
        /// <summary>Gets or sets approvalCode.</summary>
        [PropertyMapName("approvalCode")]
        public string approvalCode { get; set; }

        /// <summary>Gets or sets trxDate.</summary>
        [PropertyMapName("trxDate")]
        public DateTime trxDate { get; set; }

        /// <summary>Gets or sets amount.</summary>
        [PropertyMapName("amount")]
        public decimal amount { get; set; }
    }

    /// <summary>The SCWDeclare class.</summary>
    public class SCWDeclare
    {
        /// <summary>Gets or sets networkId.</summary>
        [PropertyMapName("networkId")]
        public int? networkId { get; set; }

        /// <summary>Gets or sets plazaId.</summary>
        [PropertyMapName("plazaId")]
        public int? plazaId { get; set; }

        /// <summary>Gets or sets staffId.</summary>
        [PropertyMapName("staffId")]
        public string staffId { get; set; }

        /// <summary>Gets or sets bagNumber.</summary>
        [PropertyMapName("bagNumber")]
        public string bagNumber { get; set; }

        /// <summary>Gets or sets safetyBeltNumber.</summary>
        [PropertyMapName("safetyBeltNumber")]
        public string safetyBeltNumber { get; set; }

        /// <summary>Gets or sets shiftTypeId.</summary>
        [PropertyMapName("shiftTypeId")]
        public int? shiftTypeId { get; set; } // 1 = shift 1, 2 = shift 2, 3 = shift 3

        /// <summary>Gets or sets declareDateTime.</summary>
        [PropertyMapName("declareDateTime")]
        public DateTime? declareDateTime { get; set; }

        /// <summary>Gets or sets attendanceDateTime.</summary>
        [PropertyMapName("attendanceDateTime")]
        public DateTime? attendanceDateTime { get; set; }

        /// <summary>Gets or sets departureDateTime.</summary>
        [PropertyMapName("departureDateTime")]
        public DateTime? departureDateTime { get; set; }

        /// <summary>Gets or sets operationDate.</summary>
        [PropertyMapName("operationDate")]
        public DateTime? operationDate { get; set; }

        /// <summary>Gets or sets declareById.</summary>
        [PropertyMapName("declareById")]
        public string declareById { get; set; }

        /// <summary>Gets or sets declareByName.</summary>
        [PropertyMapName("declareByName")]
        public string declareByName { get; set; }

        /// <summary>Gets or sets cashTotalAmount.</summary>
        [PropertyMapName("cashTotalAmount")]
        public decimal? cashTotalAmount { get; set; }

        /// <summary>Gets or sets couponTotalAmount.</summary>
        [PropertyMapName("couponTotalAmount")]
        public decimal? couponTotalAmount { get; set; }

        /// <summary>Gets or sets couponBookTotalAmount.</summary>
        [PropertyMapName("couponBookTotalAmount")]
        public decimal? couponBookTotalAmount { get; set; }

        /// <summary>Gets or sets cardAllowTotalAmount.</summary>
        [PropertyMapName("cardAllowTotalAmount")]
        public decimal? cardAllowTotalAmount { get; set; }

        /// <summary>Gets or sets qrcodeTotalAmount.</summary>
        [PropertyMapName("qrcodeTotalAmount")]
        public decimal? qrcodeTotalAmount { get; set; }

        /// <summary>Gets or sets emvTotalAmount.</summary>
        [PropertyMapName("emvTotalAmount")]
        public decimal? emvTotalAmount { get; set; }

        /// <summary>Gets or sets otherTotalAmount.</summary>
        [PropertyMapName("otherTotalAmount")]
        public decimal? otherTotalAmount { get; set; }

        /// <summary>Gets or sets nonRevenue. NEW!.</summary>
        [PropertyMapName("nonRevenue")]
        public decimal? nonRevenue { get; set; }

        /// <summary>Gets or sets cashRemark.</summary>
        [PropertyMapName("cashRemark")]
        public string cashRemark { get; set; }

        /// <summary>Gets or sets otherRemark.</summary>
        [PropertyMapName("otherRemark")]
        public string otherRemark { get; set; }

        /// <summary>Gets or sets nonRevenueRemark. NEW!.</summary>
        [PropertyMapName("nonRevenueRemark")]
        public string nonRevenueRemark { get; set; }


        /// <summary>Gets or sets chiefId.</summary>
        [PropertyMapName("chiefId")]
        public string chiefId { get; set; }

        /// <summary>Gets or sets chiefName.</summary>
        [PropertyMapName("chiefName")]
        public string chiefName { get; set; }

        /// <summary>Gets or sets jobList.</summary>
        //[PropertyMapName("jobList")]
        public List<SCWJob> jobList { get; set; }

        /// <summary>Gets or sets cashList.</summary>
        //[PropertyMapName("cashList")]
        public List<SCWDeclareCash> cashList { get; set; }

        /// <summary>Gets or sets couponList.</summary>
        //[PropertyMapName("couponList")]
        public List<SCWDeclareCoupon> couponList { get; set; }

        /// <summary>Gets or sets couponBookList.</summary>
        //[PropertyMapName("couponBookList")]
        public List<SCWDeclareCouponBook> couponBookList { get; set; }

        /// <summary>Gets or sets cardAllowList.</summary>
        //[PropertyMapName("cardAllowList")]
        public List<SCWDeclareFreePass> cardAllowList { get; set; }

        /// <summary>Gets or sets qrcodeList.</summary>
        //[PropertyMapName("qrcodeList")]
        public List<SCWDeclareQRCode> qrcodeList { get; set; }

        /// <summary>Gets or sets emvList.</summary>
        //[PropertyMapName("emvList")]
        public List<SCWDeclareEMV> emvList { get; set; }
    }

    #endregion

    #region Result class

    /// <summary>The SCWDeclareResult class.</summary>
    public class SCWDeclareResult : SCWResult
    {

    }

    #endregion
}

/*

{
    "networkId": 31,
    "plazaId": 1,
    "staffId": "41023",
    "bagNumber": "232323",
    "safetyBeltNumber": "232323",
    "cashTotalAmount": 7000,
    "couponTotalAmount": 7000,
    "couponBookTotalAmount": 20,
    "cardAllowTotalAmount": 0,
    "qrcodeTotalAmount": 0,
    "emvTotalAmount": 0,
    "otherTotalAmount": 5000,
    "cashRemark": "หมายเหตุ เงินสด 7000",
    "otherRemark": "หมายเหตุ เงินอื่นๆ 0 บาท",
    "chiefId": "00333",
    "chiefName": "นายทดสอบ ระบบ",
    "declareById": "00114",
    "declareByName": "นาย.ฟัยศรน์ เฮะดือเระ",
    "shiftTypeId": 3,
    "declareDateTime": "2020-09-03T08:10:22.000+07",
    "attendanceDateTime": "2020-08-01T05:02:00.000+07",
    "departureDateTime": "2020-09-03T10:10:00.000+07",
    "operationDate": "2020-09-03T00:00:00.000+07",
    "jobList": [
        {
            "networkId": 31,
            "plazaId": 1,
            "laneId": 1,
            "jobNo": 39,
            "staffId": "41023",
            "bojDateTime": "2020-08-07T13:28:11.000+07",
            "eojDateTime": "2020-08-07T13:58:34.000+07"
        },
        {
            "networkId": 31,
            "plazaId": 1,
            "laneId": 1,
            "jobNo": 41,
            "staffId": "41023",
            "bojDateTime": "2020-08-07T14:33:14.000+07",
            "eojDateTime": "2020-08-07T14:43:43.000+07"
        }
    ],
    "cashList": [
        {
            "currencyId": 1,
            "currencyDenomId": 5,
            "denomValue": 5,
            "number": 400,
            "total": 2000
        },
        {
            "currencyId": 1,
            "currencyDenomId": 6,
            "denomValue": 10,
            "number": 500,
            "total": 5000
        }
    ],
    "couponList": [
        {
            "couponId": 4,
            "couponValue": 80,
            "number": 2,
            "total": 160
        }
    ],
    "couponBookList": [
        {
            "couponBookId": 1,
            "couponBookValue": 665,
            "number": 2,
            "total": 1330
        }
    ],
    "cardAllowList": [
        {
            "cardAllowId": 1,
            "number": 33
        }
    ],
    "qrcodeList": [],
    "emvList": []
}

*/