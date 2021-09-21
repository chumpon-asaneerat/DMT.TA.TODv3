#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;
using Newtonsoft.Json;

#endregion

namespace DMT.Models
{
    #region Parameter class

    /// <summary>The SCWLogInAudit class.</summary>
    public class SCWLogInAudit
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

        /// <summary>Gets or sets status.</summary>
        [PropertyMapName("status")]
        public string status { get; set; } // "fail", "success"

        /// <summary>Gets or sets description.</summary>
        [PropertyMapName("description")]
        public string description { get; set; } // "invalid staff id", "etc."
    }

    #endregion

    #region Result related classes

    /// <summary>The SCWLogInAuditResult class.</summary>
    public class SCWLogInAuditResult : SCWResult
    {

    }

    #endregion
}
