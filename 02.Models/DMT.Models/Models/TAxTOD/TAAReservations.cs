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
}
