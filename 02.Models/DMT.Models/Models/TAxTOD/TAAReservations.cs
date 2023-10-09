#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

using Newtonsoft.Json;

#endregion

namespace DMT.Models
{
    /*
    {
      "MATERIAL_NUM": "310000030",
      "Description": "คูปอง 30 บาท",
      "UOM": "BOK",
      "Price": 600,
      "CouponType": "30"
    }    
    */
    public class CouponMaster
    {
        public string MATERIAL_NUM { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public decimal Price { get; set; }
        public string CouponType { get; set; }
    }

    public class Storagelocation
    {
        public string Storage_location { get; set; }
    }

    public class ReserveRunningNo
    {
        public string PrefixTxt { get; set; }
        public int RunningNo { get; set; }
    }

    public class ReserveHead
    {
        public string basedate { get; set; }
        public string movementtype { get; set; } = "311"; // Fixed
        public string goodsrecipient { get; set; }
        public string receivingstor { get; set; }
        public string userid { get; set; }
        public string postingdate { get; set; }
        public string mat_slip { get; set; }
        public string headertext { get; set; }

        public List<ReserveItem> items { get; set; } = new List<ReserveItem>();
    }

    public class ReserveItem
    {
        public string itemnumber { get; set; }
        public string goodsrecipient { get; set; }
        public string materialnum { get; set;}
        public int quantity { get; set; }
        public string unit { get; set; } = "BOK"; // Fixed
        public string plant { get; set; } = "1010"; // Fixed
        public string fromstor { get; set; } = "CDMT"; // Fixed
    }
    /*

{
    "basedate": "20230826",
    "movementtype": "311",
    "goodsrecipient": "TADD20230001",
    "receivingstor": "CCDD",
    "userid": "20001",
    "postingdate": null,
    "mat_slip": null,
    "headertext": null,
    "items": [
        { 
            "itemnumber": "1", 
            "goodsrecipient": "TADD20230001", 
            "materialnum": "310000080",
            "quantity": 100,
            "unit": "BOK",
            "plant": "1010",
            "fromstor": "CDMT"
        },
        { 
            "itemnumber": "2", 
            "goodsrecipient": "TADD20230001", 
            "materialnum": "310000035",
            "quantity": 50,
            "unit": "BOK",
            "plant": "1010",
            "fromstor": "CDMT"
        }
    ]
}
    */
}
