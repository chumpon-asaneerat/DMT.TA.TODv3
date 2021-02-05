#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;
using Newtonsoft.Json;

#endregion

namespace DMT.Models
{
    #region Parameter class

    /// <summary>The SCWSaveChiefDuty class.</summary>
    public class SCWSaveChiefDuty
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

        /// <summary>Gets or sets staffTypeId.</summary>
        [PropertyMapName("staffTypeId")]
        public int? staffTypeId { get; set; } // ประเภทพนักงาน 1 = chief, 2 = sup

        /// <summary>Gets or sets beginDateTime.</summary>
        [PropertyMapName("beginDateTime")]
        public DateTime? beginDateTime { get; set; }
    }

    #endregion

    #region Result class

    /// <summary>The SCWSaveChiefDutyResult class.</summary>
    public class SCWSaveChiefDutyResult
    {
        /// <summary>Gets or sets status.</summary>
        [PropertyMapName("status")]
        public SCWStatus status { get; set; }
    }

    #endregion
}
