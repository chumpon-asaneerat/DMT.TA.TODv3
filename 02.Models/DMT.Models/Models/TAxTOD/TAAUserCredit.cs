#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    // Server data save(update)
    /*
    {
        "tsbId": "09",
        "userId": "00111",
        "userprefix": "นาย",
        "userfirstname": "หัสกร",
        "userlastname": "ทิพยไพศาล",
        "bagno": "1245",
        "credit": 10000,
        "flag": 0,
        "creditdate": "2021-02-20:13:03.112Z"
    }
    */
    /// <summary>The TAAUserCredit class.</summary>
    public class TAAUserCredit
    {
        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }

        /// <summary>Gets or sets UserId.</summary>
        [PropertyMapName("UserId")]
        public string UserId { get; set; }
        /// <summary>Gets or sets UserPrefix.</summary>
        [PropertyMapName("UserPrefix")]
        public string UserPrefix { get; set; }
        /// <summary>Gets or sets UserFirstName.</summary>
        [PropertyMapName("UserFirstName")]
        public string UserFirstName { get; set; }
        /// <summary>Gets or sets UserLastName.</summary>
        [PropertyMapName("UserLastName")]
        public string UserLastName { get; set; }

        /// <summary>Gets or sets BagNo.</summary>
        [PropertyMapName("BagNo")]
        public string BagNo { get; set; }
        /// <summary>Gets or sets Credit.</summary>
        [PropertyMapName("Credit")]
        public decimal? Credit { get; set; }
        /// <summary>Gets or sets CreditDate.</summary>
        [PropertyMapName("CreditDate")]
        public DateTime? CreditDate { get; set; }

        /// <summary>Gets or sets flag.</summary>
        [PropertyMapName("flag")]
        public int? flag { get; set; }
    }


    /*
    {
          "TSBId": "09",
          "TSB_Th_Name": "อนุสรณ์สถาน",
          "UserId": "00111",
          "username": "นาย หัสกร ทิพยไพศาล",
          "Bagno": "1245",
          "Credit": 10000,
          "creditdate": "2021-02-20T13:03:11.200Z",
          "C35": null,
          "C80": null
    }
    */
    /// <summary>The TAAUserCreditResult class.</summary>
    public class TAAUserCreditResult
    {
        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }
        /// <summary>Gets or sets TSB_Th_Name.</summary>
        [PropertyMapName("TSB_Th_Name")]
        public string TSB_Th_Name { get; set; }

        /// <summary>Gets or sets UserId.</summary>
        [PropertyMapName("UserId")]
        public string UserId { get; set; }
        /// <summary>Gets or sets username.</summary>
        [PropertyMapName("username")]
        public string username { get; set; }

        /// <summary>Gets or sets Bagno.</summary>
        [PropertyMapName("Bagno")]
        public string Bagno { get; set; }
        /// <summary>Gets or sets Credit.</summary>
        [PropertyMapName("Credit")]
        public decimal? Credit { get; set; }
        /// <summary>Gets or sets creditdate.</summary>
        [PropertyMapName("creditdate")]
        public DateTime? creditdate { get; set; }

        /// <summary>Gets or sets C35.</summary>
        [PropertyMapName("C35")]
        public int? C35 { get; set; }
        /// <summary>Gets or sets C80.</summary>
        [PropertyMapName("C80")]
        public int? C80 { get; set; }
    }
}
