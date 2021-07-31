#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

using Newtonsoft.Json;

#endregion

namespace DMT.Models
{
    // http://localhost:8000/api/account/request/getlist
    // Server data parameter
    /*
    {
        "status": "R" 
    }
    */
    // Server data result.
    /*
    {
        "TSBId": "09",
        "TSB_Th_Name": "อนุสรณ์สถาน",
        "RequestId": 1,
        "RequestDate": "2021-04-13T22:03:11.200Z",
        "ExchangeBHT": 10000,
        "BorrowBHT": 0,
        "AdditionalBHT": 0,
        "PeriodBegin": null,
        "PeriodEnd": null,
        "RequestRemark": "ทดสอบ",
        "TSBRequestBy": "00444",
        "AppExchangeBHT": null,
        "AppBorrowBHT": null,
        "AppAdditionalBHT": null,
        "ApproveBy": null,
        "ApproveDate": null,
        "ApproveRemark": null
    }
    */
    /// <summary>The TAAExchangeSummary class.</summary>
    public class TAAExchangeSummary
    {
        /// <summary>Gets or sets Selected.</summary>
        [JsonIgnore]
        public bool Selected { get; set; }

        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }
        /// <summary>Gets or sets TSB_Th_Name.</summary>
        [PropertyMapName("TSB_Th_Name")]
        public string TSB_Th_Name { get; set; }

        /// <summary>Gets or sets RequestId.</summary>
        [PropertyMapName("RequestId")]
        public int? RequestId { get; set; }
        /// <summary>Gets or sets RequestDate.</summary>
        [PropertyMapName("RequestDate")]
        public DateTime? RequestDate { get; set; }
        /// <summary>Gets or sets RequestDate.</summary>
        [JsonIgnore]
        public string RequestDateString
        {
            get
            {
                var ret = (!this.RequestDate.HasValue || this.RequestDate.Value == DateTime.MinValue) ?
                    "" : this.RequestDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }

        /// <summary>Gets or sets ExchangeBHT.</summary>
        [PropertyMapName("ExchangeBHT")]
        public decimal? ExchangeBHT { get; set; }
        /// <summary>Gets or sets BorrowBHT.</summary>
        [PropertyMapName("BorrowBHT")]
        public decimal? BorrowBHT { get; set; }
        /// <summary>Gets or sets AdditionalBHT.</summary>
        [PropertyMapName("AdditionalBHT")]
        public decimal? AdditionalBHT { get; set; }

        /// <summary>Gets Total Request amount in BHT.</summary>
        [JsonIgnore]
        public decimal? Total
        {
            get
            {
                decimal total = decimal.Zero;
                total += (ExchangeBHT.HasValue) ? ExchangeBHT.Value : decimal.Zero;
                total += (BorrowBHT.HasValue) ? BorrowBHT.Value : decimal.Zero;
                total += (AdditionalBHT.HasValue) ? AdditionalBHT.Value : decimal.Zero;
                return total;
            }
        }

        /// <summary>Gets or sets PeriodBegin.</summary>
        [PropertyMapName("PeriodBegin")]
        public DateTime? PeriodBegin { get; set; }
        /// <summary>Gets or sets PeriodBeginDateString.</summary>
        [JsonIgnore]
        public string PeriodBeginDateString
        {
            get
            {
                var ret = (!this.PeriodBegin.HasValue || this.PeriodBegin.Value == DateTime.MinValue) ?
                    "" : this.PeriodBegin.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>Gets or sets PeriodEnd.</summary>
        [PropertyMapName("PeriodEnd")]
        public DateTime? PeriodEnd { get; set; }
        /// <summary>Gets or sets PeriodEndDateString.</summary>
        [JsonIgnore]
        public string PeriodEndDateString
        {
            get
            {
                var ret = (!this.PeriodEnd.HasValue || this.PeriodEnd.Value == DateTime.MinValue) ?
                    "" : this.PeriodEnd.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>Gets or sets RequestRemark.</summary>
        [PropertyMapName("RequestRemark")]
        public string RequestRemark { get; set; }
        /// <summary>Gets or sets TSBRequestBy.</summary>
        [PropertyMapName("TSBRequestBy")]
        public string TSBRequestBy { get; set; }

        /// <summary>Gets or sets AppExchangeBHT.</summary>
        [PropertyMapName("AppExchangeBHT")]
        public decimal? AppExchangeBHT { get; set; }
        /// <summary>Gets or sets AppBorrowBHT.</summary>
        [PropertyMapName("AppBorrowBHT")]
        public decimal? AppBorrowBHT { get; set; }
        /// <summary>Gets or sets AppAdditionalBHT.</summary>
        [PropertyMapName("AppAdditionalBHT")]
        public decimal? AppAdditionalBHT { get; set; }

        /// <summary>Gets Approve Total Request amount in BHT.</summary>
        [JsonIgnore]
        public decimal? ApproveTotal
        {
            get
            {
                decimal total = decimal.Zero;
                total += (AppExchangeBHT.HasValue) ? AppExchangeBHT.Value : decimal.Zero;
                total += (AppBorrowBHT.HasValue) ? AppBorrowBHT.Value : decimal.Zero;
                total += (AppAdditionalBHT.HasValue) ? AppAdditionalBHT.Value : decimal.Zero;
                return total;
            }
        }

        /// <summary>Gets or sets ApproveBy.</summary>
        [PropertyMapName("ApproveBy")]
        public string ApproveBy { get; set; }
        /// <summary>Gets or sets ApproveDate.</summary>
        [PropertyMapName("ApproveDate")]
        public DateTime? ApproveDate { get; set; }
        /// <summary>Gets or sets ApproveDateString.</summary>
        [JsonIgnore]
        public string ApproveDateString
        {
            get
            {
                var ret = (!this.ApproveDate.HasValue || this.ApproveDate.Value == DateTime.MinValue) ?
                    "" : this.ApproveDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>Gets or sets ApproveRemark.</summary>
        [PropertyMapName("ApproveRemark")]
        public string ApproveRemark { get; set; }
    }

    // // http://localhost:8000/api/account/request/getdetail (as parameter)
    /*
    {
        "requestid": 1 ,
        "tsbId": "09"
    }
    */
    // http://localhost:8000/api/account/request/getdetail (as result)
    // http://localhost:8000/api/account/request/savedetail (as parameter)
    // http://localhost:8000/api/account/request/approve (as parameter)
    /*
    {
      "RequestId": 1,
      "TSBId": "09",
      "CurrencyDenomId": 5,
      "CurrencyValue": 5000,
      "CurrencyCount": 500
    }
    */
    public class TAAExchangeItem
    {
        /// <summary>Gets or sets RequestId.</summary>
        [PropertyMapName("RequestId")]
        public int? RequestId { get; set; }
        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }

        /// <summary>Gets or sets CurrencyDenomId.</summary>
        [PropertyMapName("CurrencyDenomId")]
        public int? CurrencyDenomId { get; set; }

        /// <summary>Gets or sets CurrencyValue.</summary>
        [PropertyMapName("CurrencyValue")]
        public decimal? CurrencyValue { get; set; }

        /// <summary>Gets or sets CurrencyCount.</summary>
        [PropertyMapName("CurrencyCount")]
        public decimal? CurrencyCount { get; set; }
    }

    // Server data result.
    // http://localhost:8000/api/account/request/getapprove (as result)
    /*
    {
      "RequestId": 1,
      "TSBId": "09",
      "RequestDate": "2021-04-13T22:03:11.200Z",
      "FinishFlag": "0",
      "TSBRequestBy": "00444",
      "ExchangeBHT": 10000,
      "BorrowBHT": 0,
      "AdditionalBHT": 0,
      "PeriodBegin": null,
      "PeriodEnd": null,
      "RequestRemark": "ทดสอบ",
      "Status": "A",
      "AppExchangeBHT": null,
      "AppBorrowBHT": null,
      "AppAdditionalBHT": null,
      "ApproveDate": null,
      "ApproveBy": null,
      "ApproveRemark": null,
      "TSBReceiveDate": null,
      "TSBReceiveBy": null,
      "TSBReceiveRemark": null,
      "LastUpdate": "2021-04-13T22:03:11.200Z"
    }
    */
    /// <summary>The TAAApproveSummary class.</summary>
    public class TAAApproveSummary
    {
        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }
        /// <summary>Gets or sets RequestId.</summary>
        [PropertyMapName("RequestId")]
        public int? RequestId { get; set; }
        /// <summary>Gets or sets RequestDate.</summary>
        [PropertyMapName("RequestDate")]
        public DateTime? RequestDate { get; set; }
        /// <summary>Gets or sets RequestDate.</summary>
        [JsonIgnore]
        public string RequestDateString
        {
            get
            {
                var ret = (!this.RequestDate.HasValue || this.RequestDate.Value == DateTime.MinValue) ?
                    "" : this.RequestDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }

        /// <summary>Gets or sets ExchangeBHT.</summary>
        [PropertyMapName("ExchangeBHT")]
        public decimal? ExchangeBHT { get; set; }
        /// <summary>Gets or sets BorrowBHT.</summary>
        [PropertyMapName("BorrowBHT")]
        public decimal? BorrowBHT { get; set; }
        /// <summary>Gets or sets AdditionalBHT.</summary>
        [PropertyMapName("AdditionalBHT")]
        public decimal? AdditionalBHT { get; set; }

        /// <summary>Gets Total Request amount in BHT.</summary>
        [JsonIgnore]
        public decimal? Total
        {
            get
            {
                decimal total = decimal.Zero;
                total += (ExchangeBHT.HasValue) ? ExchangeBHT.Value : decimal.Zero;
                total += (BorrowBHT.HasValue) ? BorrowBHT.Value : decimal.Zero;
                total += (AdditionalBHT.HasValue) ? AdditionalBHT.Value : decimal.Zero;
                return total;
            }
        }

        /// <summary>Gets or sets PeriodBegin.</summary>
        [PropertyMapName("PeriodBegin")]
        public DateTime? PeriodBegin { get; set; }
        /// <summary>Gets or sets PeriodBeginDateString.</summary>
        [JsonIgnore]
        public string PeriodBeginDateString
        {
            get
            {
                var ret = (!this.PeriodBegin.HasValue || this.PeriodBegin.Value == DateTime.MinValue) ?
                    "" : this.PeriodBegin.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>Gets or sets PeriodEnd.</summary>
        [PropertyMapName("PeriodEnd")]
        public DateTime? PeriodEnd { get; set; }
        /// <summary>Gets or sets PeriodEndDateString.</summary>
        [JsonIgnore]
        public string PeriodEndDateString
        {
            get
            {
                var ret = (!this.PeriodEnd.HasValue || this.PeriodEnd.Value == DateTime.MinValue) ?
                    "" : this.PeriodEnd.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>Gets or sets RequestRemark.</summary>
        [PropertyMapName("RequestRemark")]
        public string RequestRemark { get; set; }
        /// <summary>Gets or sets TSBRequestBy.</summary>
        [PropertyMapName("TSBRequestBy")]
        public string TSBRequestBy { get; set; }

        /// <summary>Gets or sets AppExchangeBHT.</summary>
        [PropertyMapName("AppExchangeBHT")]
        public decimal? AppExchangeBHT { get; set; }
        /// <summary>Gets or sets AppBorrowBHT.</summary>
        [PropertyMapName("AppBorrowBHT")]
        public decimal? AppBorrowBHT { get; set; }
        /// <summary>Gets or sets AppAdditionalBHT.</summary>
        [PropertyMapName("AppAdditionalBHT")]
        public decimal? AppAdditionalBHT { get; set; }

        /// <summary>Gets Approve Total Request amount in BHT.</summary>
        [JsonIgnore]
        public decimal? ApproveTotal
        {
            get
            {
                decimal total = decimal.Zero;
                total += (AppExchangeBHT.HasValue) ? AppExchangeBHT.Value : decimal.Zero;
                total += (AppBorrowBHT.HasValue) ? AppBorrowBHT.Value : decimal.Zero;
                total += (AppAdditionalBHT.HasValue) ? AppAdditionalBHT.Value : decimal.Zero;
                return total;
            }
        }

        /// <summary>Gets or sets ApproveBy.</summary>
        [PropertyMapName("ApproveBy")]
        public string ApproveBy { get; set; }
        /// <summary>Gets or sets ApproveDate.</summary>
        [PropertyMapName("ApproveDate")]
        public DateTime? ApproveDate { get; set; }
        /// <summary>Gets or sets ApproveDateString.</summary>
        [JsonIgnore]
        public string ApproveDateString
        {
            get
            {
                var ret = (!this.ApproveDate.HasValue || this.ApproveDate.Value == DateTime.MinValue) ?
                    "" : this.ApproveDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>Gets or sets ApproveRemark.</summary>
        [PropertyMapName("ApproveRemark")]
        public string ApproveRemark { get; set; }

        /// <summary>Gets or sets FinishFlag.</summary>
        [PropertyMapName("FinishFlag")]
        public int? FinishFlag { get; set; }
        /// <summary>Gets or sets Status.</summary>
        [PropertyMapName("Status")]
        public string Status { get; set; }

        /// <summary>Gets or sets TSBReceiveDate.</summary>
        [PropertyMapName("TSBReceiveDate")]
        public DateTime? TSBReceiveDate { get; set; }
        /// <summary>Gets or sets TSBReceiveBy.</summary>
        [PropertyMapName("TSBReceiveBy")]
        public string TSBReceiveBy { get; set; }
        /// <summary>Gets or sets TSBReceiveRemark.</summary>
        [PropertyMapName("TSBReceiveRemark")]
        public string TSBReceiveRemark { get; set; }

        /// <summary>Gets or sets LastUpdate.</summary>
        [PropertyMapName("LastUpdate")]
        public DateTime? LastUpdate { get; set; }
    }

    // Server data result.
    // http://localhost:8000/api/account/request/getapprovedetail (as result)
    /*
    {
      "CurrencyDenomId": 1,
      "Description": "25 Satang",
      "RequestID": null,
      "TSBId": null,
      "RequestValue": null,
      "RequestCount": null,
      "ApproveValue": null,
      "ApproveCount": null
    }
    */
    public class TAAApproveItem
    {
        /// <summary>Gets or sets RequestId.</summary>
        [PropertyMapName("RequestId")]
        public int? RequestId { get; set; }
        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }

        /// <summary>Gets or sets CurrencyDenomId.</summary>
        [PropertyMapName("CurrencyDenomId")]
        public int? CurrencyDenomId { get; set; }
        /// <summary>Gets or sets Description.</summary>
        [PropertyMapName("Description")]
        public string Description { get; set; }

        /// <summary>Gets or sets RequestValue.</summary>
        [PropertyMapName("RequestValue")]
        public decimal? RequestValue { get; set; }
        /// <summary>Gets or sets RequestCount.</summary>
        [PropertyMapName("RequestCount")]
        public decimal? RequestCount { get; set; }

        /// <summary>Gets or sets ApproveValue.</summary>
        [PropertyMapName("ApproveValue")]
        public decimal? ApproveValue { get; set; }
        /// <summary>Gets or sets ApproveCount.</summary>
        [PropertyMapName("ApproveCount")]
        public decimal? ApproveCount { get; set; }
    }
}

// for TA App used
namespace DMT.Models
{
    /*
    {
        "requestid": 1 ,
        "tsbId": "08",
        "exchangebht" : 20000,
        "borrowbht" : 0,
        "additionalbht" : 0,
        "periodbegin" : null,
        "periodend" : null,
        "remark" : "ทดสอบ",
        "finishflag" : 0 ,
        "userid" : "00444" ,
        "tranactiondate" : "2021-04-13:22:03.112Z" ,
        "status" : "R"
    }
    */
    public class TAAExchangeHeader
    {
        /// <summary>Gets or sets RequestId.</summary>
        [PropertyMapName("RequestId")]
        public int? RequestId { get; set; }
        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }

        /// <summary>Gets or sets ExchangeBHT.</summary>
        [PropertyMapName("ExchangeBHT")]
        public decimal? ExchangeBHT { get; set; }
        /// <summary>Gets or sets BorrowBHT.</summary>
        [PropertyMapName("BorrowBHT")]
        public decimal? BorrowBHT { get; set; }
        /// <summary>Gets or sets AdditionalBHT.</summary>
        [PropertyMapName("AdditionalBHT")]
        public decimal? AdditionalBHT { get; set; }
        /// <summary>Gets or sets PeriodBegin.</summary>
        [PropertyMapName("PeriodBegin")]
        public DateTime? PeriodBegin { get; set; }
        /// <summary>Gets or sets PeriodBeginDateString.</summary>
        [JsonIgnore]
        public string PeriodBeginDateString
        {
            get
            {
                var ret = (!this.PeriodBegin.HasValue || this.PeriodBegin.Value == DateTime.MinValue) ?
                    "" : this.PeriodBegin.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>Gets or sets PeriodEnd.</summary>
        [PropertyMapName("PeriodEnd")]
        public DateTime? PeriodEnd { get; set; }
        /// <summary>Gets or sets PeriodEndDateString.</summary>
        [JsonIgnore]
        public string PeriodEndDateString
        {
            get
            {
                var ret = (!this.PeriodEnd.HasValue || this.PeriodEnd.Value == DateTime.MinValue) ?
                    "" : this.PeriodEnd.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>Gets or sets Remark.</summary>
        [PropertyMapName("Remark")]
        public string Remark { get; set; }

        [PropertyMapName("UserId")]
        public string UserId { get; set; }

        /// <summary>Gets or sets TransactionDate.</summary>
        [PropertyMapName("TransactionDate")]
        public DateTime? TransactionDate { get; set; }
        /// <summary>Gets or sets TransactionDateTimeString.</summary>
        [JsonIgnore]
        public string TransactionDateTimeString
        {
            get
            {
                var ret = (!this.TransactionDate.HasValue || this.TransactionDate.Value == DateTime.MinValue) ?
                    "" : this.TransactionDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
                return ret;
            }
            set { }
        }

        /// <summary>Gets or sets FinishFlag.</summary>
        [PropertyMapName("FinishFlag")]
        public int? FinishFlag { get; set; }
        /// <summary>Gets or sets Status.</summary>
        [PropertyMapName("Status")]
        public string Status { get; set; }
    }
}