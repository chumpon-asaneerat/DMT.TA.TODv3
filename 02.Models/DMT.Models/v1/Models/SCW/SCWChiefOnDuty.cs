#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    #region Parameter class

    /// <summary>The SCWChiefOnDuty class.</summary>
    public class SCWChiefOnDuty
    {
        /// <summary>Gets or sets networkId.</summary>
        [PropertyMapName("networkId")]
        public int? networkId { get; set; }

        /// <summary>Gets or sets plazaId.</summary>
        [PropertyMapName("plazaId")]
        public int? plazaId { get; set; }

        /// <summary>Gets or sets staffTypeId.</summary>
        [PropertyMapName("staffTypeId")]
        public int? staffTypeId { get; set; } // ประเภทพนักงาน 1 = chief, 2 = sup
    }

    #endregion

    #region Result related classes

    /// <summary>The SCWChiefOnDutyResult class.</summary>
    public class SCWChiefOnDutyResult
    {
        /// <summary>Gets or sets status.</summary>
        [PropertyMapName("status")]
        public SCWStatus status { get; set; }
    }

    #endregion
}
