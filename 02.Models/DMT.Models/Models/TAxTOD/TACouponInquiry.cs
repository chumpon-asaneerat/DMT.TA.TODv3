#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

using Newtonsoft.Json;

#endregion

namespace DMT.Models
{
    // 1 = สต๊อก , 2 = ด่านขาย , 3 = ด่านปิดการขาย , 4 = สร้าง Invoice, 5 = Out of Stock , 0 = เลือกทั้งหมด
    public class InquiryStatus
    {
        /// <summary>Gets or sets ItemStatusDigit.</summary>
        [PropertyMapName("ItemStatusDigit")]
        public int ItemStatusDigit { get; set; }
        /// <summary>Gets or sets ItemStatus.</summary>
        [PropertyMapName("ItemStatus")]
        public string ItemStatus { get; set; }

        public static List<InquiryStatus> Gets()
        {
            var ret = new List<InquiryStatus>();
            ret.Add(new InquiryStatus() { ItemStatusDigit = 0, ItemStatus = "[เลือกทั้งหมด]" });
            ret.Add(new InquiryStatus() { ItemStatusDigit = 1, ItemStatus = "สต๊อก" });
            ret.Add(new InquiryStatus() { ItemStatusDigit = 2, ItemStatus = "จ่ายพนักงาน" });
            ret.Add(new InquiryStatus() { ItemStatusDigit = 3, ItemStatus = "พนักงานขาย" });
            ret.Add(new InquiryStatus() { ItemStatusDigit = 4, ItemStatus = "หัวหน้าขาย" });
            ret.Add(new InquiryStatus() { ItemStatusDigit = 5, ItemStatus = "รายการขาย" });
            //ret.Add(new InquiryStatus() { ItemStatusDigit = 2, ItemStatus = "ด่านขาย" });
            //ret.Add(new InquiryStatus() { ItemStatusDigit = 3, ItemStatus = "ด่านปิดการขาย" });
            //ret.Add(new InquiryStatus() { ItemStatusDigit = 4, ItemStatus = "สร้าง Invoice" });
            //ret.Add(new InquiryStatus() { ItemStatusDigit = 5, ItemStatus = "Out of Stock" });
            return ret;
        }
    }

    // Server data result.
    /*
    {
      "SAPItemCode": "C35",
      "SAPIntrSerial": "ข011648",
      "SAPSysSerial": 749762,
      "SAPTransferNo": "634500032",
      "SAPTransferDate": "2020-01-14T00:00:00.000Z",
      "ItemStatus": "สต๊อก",
      "ItemStatusDigit": 1,
      "TollWayName": "ด่านอนุสรณ์สถาน",
      "TollWayId": 9,
      "WorkingDate": null,
      "ShiftName": null,
      "SAPARInvoice": null,
      "SAPARDate": null,
      "ShiftId": 0,
      "SoldBy": null,
      "SoldDate": null,
      "LaneId": null
    }
    */
    /// <summary>The TACouponInquiry class.</summary>
    public class TACouponInquiry
    {
        /// <summary>Gets or sets SAPItemCode.</summary>
        [PropertyMapName("SAPItemCode")]
        public string SAPItemCode { get; set; }
        /// <summary>Gets or sets SAPIntrSerial.</summary>
        [PropertyMapName("SAPIntrSerial")]
        public string SAPIntrSerial { get; set; }
        /// <summary>Gets or sets SAPSysSerial.</summary>
        [PropertyMapName("SAPSysSerial")]
        public int? SAPSysSerial { get; set; }
        /// <summary>Gets or sets SAPARInvoice.</summary>
        [PropertyMapName("SAPARInvoice")]
        public string SAPARInvoice { get; set; }
        /// <summary>Gets or sets SAPARDate.</summary>
        [PropertyMapName("SAPARDate")]
        public DateTime? SAPARDate { get; set; }
        /// <summary>Gets or sets SAPTransferNo.</summary>
        [PropertyMapName("SAPTransferNo")]
        public string SAPTransferNo { get; set; }
        /// <summary>Gets or sets SAPTransferDate.</summary>
        [PropertyMapName("SAPTransferDate")]
        public DateTime? SAPTransferDate { get; set; }
        /// <summary>Gets or sets SAPTransferDateString.</summary>
        [JsonIgnore]
        public string SAPTransferDateString
        {
            get 
            {
                var ret = (!this.SAPTransferDate.HasValue || this.SAPTransferDate.Value == DateTime.MinValue) ?
                    "" : this.SAPTransferDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }

        /// <summary>Gets or sets ItemStatusDigit.</summary>
        [PropertyMapName("ItemStatusDigit")]
        public int? ItemStatusDigit { get; set; }
        /// <summary>Gets or sets ItemStatus.</summary>
        [PropertyMapName("ItemStatus")]
        public string ItemStatus { get; set; }

        /// <summary>Gets or sets TollWayId.</summary>
        [PropertyMapName("TollWayId")]
        public int? TollWayId { get; set; }
        /// <summary>Gets or sets TollWayName.</summary>
        [PropertyMapName("TollWayName")]
        public string TollWayName { get; set; }

        /// <summary>Gets or sets WorkingDate.</summary>
        [PropertyMapName("WorkingDate")]
        public DateTime? WorkingDate { get; set; }
        /// <summary>Gets or sets WorkingDateString.</summary>
        [JsonIgnore]
        public string WorkingDateString
        {
            get
            {
                var ret = (!this.WorkingDate.HasValue || this.WorkingDate.Value == DateTime.MinValue) ?
                    "" : this.WorkingDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }

        /// <summary>Gets or sets ShiftId.</summary>
        [PropertyMapName("ShiftId")]
        public int? ShiftId { get; set; }
        /// <summary>Gets or sets ShiftName.</summary>
        [PropertyMapName("ShiftName")]
        public string ShiftName { get; set; }

        /// <summary>Gets or sets LaneId.</summary>
        [PropertyMapName("LaneId")]
        public string LaneId { get; set; }
        /// <summary>Gets or sets SoldBy.</summary>
        [PropertyMapName("SoldBy")]
        public string SoldBy { get; set; }
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
    }
    // Search
    /*
    {
        "SAPItemCode": "C35",
        "SAPIntrSerial": null,
        "SAPTransferNo": null,
        "ItemStatusDigit": null,
        "TollWayId": 9,
        "WorkingDateFrom": null,
        "WorkingDateTo": null,
        "SAPARInvoice": "",
        "ShiftId": null
    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class Coupon
            {
                #region Inquiry

                /// <summary>
                /// Inquiry.
                /// </summary>
                public class Inquiry : NSearch<Inquiry>
                {
                    #region Public Properties

                    /// <summary>
                    /// Gets or sets SAPItemCode.
                    /// </summary>
                    public string SAPItemCode { get; set; }
                    /// <summary>
                    /// Gets or sets SAPIntrSerial.
                    /// </summary>
                    public string SAPIntrSerial { get; set; }
                    /// <summary>
                    /// Gets or sets SAPTransferNo.
                    /// </summary>
                    public string SAPTransferNo { get; set; }
                    /// <summary>
                    /// Gets or sets ItemStatusDigit.
                    /// </summary>
                    public int? ItemStatusDigit { get; set; }
                    /// <summary>
                    /// Gets or sets TollWayId (9).
                    /// </summary>
                    public int? TollWayId { get; set; }
                    /// <summary>
                    /// Gets or sets WorkingDateFrom.
                    /// </summary>
                    public DateTime? WorkingDateFrom { get; set; }
                    /// <summary>
                    /// Gets or sets WorkingDateTo.
                    /// </summary>
                    public DateTime? WorkingDateTo { get; set; }

                    /// <summary>
                    /// Gets or sets SAPARInvoice.
                    /// </summary>
                    public string SAPARInvoice { get; set; }
                    /// <summary>
                    /// Gets or sets ShiftId.
                    /// </summary>
                    public int? ShiftId { get; set; }

                    #endregion

                    #region Static Method (Create)

                    /// <summary>
                    /// Create Search instance.
                    /// </summary>
                    /// <param name="sapItemCode">The SAPItemCode.</param>
                    /// <param name="sapIntrSerial">The SAPIntrSerial.</param>
                    /// <param name="sapTransferNo">The SAPTransferNo.</param>
                    /// <param name="sapARInvoice">The SAPARInvoice.</param>
                    /// <param name="itemStatusDigit">The ItemStatusDigit.</param>
                    /// <param name="tollWayId">The TollWayId.</param>
                    /// <param name="shiftId">The ShiftId.</param>
                    /// <param name="workingDateFrom">The WorkingDateFrom.</param>
                    /// <param name="workingDateTo">The WorkingDateTo.</param>
                    /// <returns>Returns Search instance.</returns>
                    public static Inquiry Create(
                        string sapItemCode,
                        string sapIntrSerial, 
                        string sapTransferNo = null,
                        string sapARInvoice = "",
                        int? itemStatusDigit = new int?(), 
                        int? tollWayId = 0,
                        int? shiftId = new int?(),
                        DateTime? workingDateFrom = new DateTime?(),
                        DateTime? workingDateTo = new DateTime?())
                    {
                        var ret = new Inquiry();
                        ret.SAPItemCode = sapItemCode;
                        ret.SAPIntrSerial = sapIntrSerial;
                        ret.SAPTransferNo = sapTransferNo;
                        ret.ItemStatusDigit = itemStatusDigit;
                        ret.TollWayId = tollWayId;
                        ret.WorkingDateFrom = workingDateFrom;
                        ret.WorkingDateTo = workingDateTo;
                        ret.SAPARInvoice = (string.IsNullOrWhiteSpace(sapARInvoice)) ? string.Empty : sapARInvoice;
                        ret.ShiftId = shiftId;
                        return ret;
                    }

                    #endregion
                }

                #endregion
            }
        }
    }
}
