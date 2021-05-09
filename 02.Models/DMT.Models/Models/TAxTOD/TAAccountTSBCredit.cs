#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

using Newtonsoft.Json;

#endregion

namespace DMT.Models
{
    // Server data save(update)
    /*
    {
        "tsbId": "09",
        "amnt1": 1000,
        "amnt2": 1000,
        "amnt5": 1000,
        "amnt10": 1000,
        "amnt20": 1000,
        "amnt50": 1000,
        "amnt100": 1000,
        "amnt500": 1000,
        "amnt1000": 1000,
        "updatedate": "2021-02-20:13:03.112Z",
        "remark" : null
    }
    */
    /// <summary>The TAAccountTSBCredit class.</summary>
    public class TAAccountTSBCredit
    {
        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }

        /// <summary>Gets or sets Amnt1.</summary>
        [PropertyMapName("Amnt1")]
        public decimal? Amnt1 { get; set; }
        /// <summary>Gets or sets Amnt2.</summary>
        [PropertyMapName("Amnt2")]
        public decimal? Amnt2 { get; set; }
        /// <summary>Gets or sets Amnt5.</summary>
        [PropertyMapName("Amnt5")]
        public decimal? Amnt5 { get; set; }
        /// <summary>Gets or sets Amnt10.</summary>
        [PropertyMapName("Amnt10")]
        public decimal? Amnt10 { get; set; }
        /// <summary>Gets or sets Amnt20.</summary>
        [PropertyMapName("Amnt20")]
        public decimal? Amnt20 { get; set; }
        /// <summary>Gets or sets Amnt50.</summary>
        [PropertyMapName("Amnt50")]
        public decimal? Amnt50 { get; set; }
        /// <summary>Gets or sets Amnt100.</summary>
        [PropertyMapName("Amnt100")]
        public decimal? Amnt100 { get; set; }
        /// <summary>Gets or sets Amnt500.</summary>
        [PropertyMapName("Amnt500")]
        public decimal? Amnt500 { get; set; }
        /// <summary>Gets or sets Amnt1000.</summary>
        [PropertyMapName("Amnt1000")]
        public decimal? Amnt1000 { get; set; }

        /// <summary>Gets or sets Updatedate.</summary>
        [PropertyMapName("Updatedate")]
        public DateTime? Updatedate { get; set; }
        /// <summary>Gets or sets Remark.</summary>
        [PropertyMapName("Remark")]
        public string Remark { get; set; }
    }


    // Server data result.
    /*
    {
      "TSBId": "01",
      "TSB_Th_Name": "ดินแดง",
      "Amnt1": null,
      "Amnt2": null,
      "Amnt5": null,
      "Amnt10": null,
      "Amnt20": null,
      "Amnt50": null,
      "Amnt100": null,
      "Amnt500": null,
      "Amnt1000": null,
      "BalanceDate": null,
      "BalanceRemark": null,
      "ucredit": null,
      "C35": 757,
      "C80": 1076
    }
    */
    /// <summary>The TAAccountTSBCreditResult class.</summary>
    public class TAAccountTSBCreditResult
    {
        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }
        /// <summary>Gets or sets TSB_Th_Name.</summary>
        [PropertyMapName("TSB_Th_Name")]
        public string TSB_Th_Name { get; set; }

        /// <summary>Gets or sets Amnt1.</summary>
        [PropertyMapName("Amnt1")]
        public decimal? Amnt1 { get; set; }
        /// <summary>Gets or sets Amnt2.</summary>
        [PropertyMapName("Amnt2")]
        public decimal? Amnt2 { get; set; }
        /// <summary>Gets or sets Amnt5.</summary>
        [PropertyMapName("Amnt5")]
        public decimal? Amnt5 { get; set; }
        /// <summary>Gets or sets Amnt10.</summary>
        [PropertyMapName("Amnt10")]
        public decimal? Amnt10 { get; set; }
        /// <summary>Gets or sets Amnt20.</summary>
        [PropertyMapName("Amnt20")]
        public decimal? Amnt20 { get; set; }
        /// <summary>Gets or sets Amnt50.</summary>
        [PropertyMapName("Amnt50")]
        public decimal? Amnt50 { get; set; }
        /// <summary>Gets or sets Amnt100.</summary>
        [PropertyMapName("Amnt100")]
        public decimal? Amnt100 { get; set; }
        /// <summary>Gets or sets Amnt500.</summary>
        [PropertyMapName("Amnt500")]
        public decimal? Amnt500 { get; set; }
        /// <summary>Gets or sets Amnt1000.</summary>
        [PropertyMapName("Amnt1000")]
        public decimal? Amnt1000 { get; set; }

        /// <summary>Gets Total credit amount in BHT.</summary>
        [JsonIgnore]
        public decimal? Total 
        {
            get 
            {
                decimal total = decimal.Zero;
                total += (Amnt1.HasValue) ? Amnt1.Value : decimal.Zero;
                total += (Amnt2.HasValue) ? Amnt2.Value : decimal.Zero;
                total += (Amnt5.HasValue) ? Amnt5.Value : decimal.Zero;
                total += (Amnt10.HasValue) ? Amnt10.Value : decimal.Zero;
                total += (Amnt20.HasValue) ? Amnt20.Value : decimal.Zero;
                total += (Amnt50.HasValue) ? Amnt50.Value : decimal.Zero;
                total += (Amnt100.HasValue) ? Amnt100.Value : decimal.Zero;
                total += (Amnt500.HasValue) ? Amnt500.Value : decimal.Zero;
                total += (Amnt1000.HasValue) ? Amnt1000.Value : decimal.Zero;
                return total;
            }
        }

        /// <summary>Gets or sets BalanceDate.</summary>
        [PropertyMapName("BalanceDate")]
        public DateTime? BalanceDate { get; set; }
        /// <summary>Gets or sets BalanceRemark.</summary>
        [PropertyMapName("BalanceRemark")]
        public string BalanceRemark { get; set; }

        /// <summary>Gets or sets ucredit.</summary>
        [PropertyMapName("ucredit")]
        public decimal? ucredit { get; set; }

        /// <summary>Gets or sets C35.</summary>
        [PropertyMapName("C35")]
        public int? C35 { get; set; }
        /// <summary>Gets or sets C80.</summary>
        [PropertyMapName("C80")]
        public int? C80 { get; set; }
    }
}
