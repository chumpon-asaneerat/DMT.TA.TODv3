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
        "tsbid": "09",
        "oldserialno": "C000527",
        "newserialno": "C000603"
    }
    */
    public class TAAEditserialno
    {
        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }
        /// <summary>Gets or sets Old Serial No.</summary>
        [PropertyMapName("Oldserialno")]
        public string Oldserialno { get; set; }
        /// <summary>Gets or sets New Serial No.</summary>
        [PropertyMapName("Newserialno")]
        public string Newserialno { get; set; }
    }
}
