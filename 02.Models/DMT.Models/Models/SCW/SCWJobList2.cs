#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;
using Newtonsoft.Json;

#endregion

namespace DMT.Models
{
    #region Parameter class

    /// <summary>The SCWJobList2 (parameter) class.</summary>
    public class SCWJobList2
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

        /// <summary>Gets or sets operationDate.</summary>
        [PropertyMapName("operationDate")]
        public DateTime? operationDate { get; set; }
    }

    #endregion

    #region Result related classes

    /// <summary>The SCWJob2 class.</summary>
    public class SCWJob2
    {
        /// <summary>Gets or sets networkId.</summary>
        [PropertyMapName("networkId")]
        public int? networkId { get; set; }

        /// <summary>Gets or sets plazaId.</summary>
        [PropertyMapName("plazaId")]
        public int? plazaId { get; set; }

        /// <summary>Gets or sets laneId.</summary>
        [PropertyMapName("laneId")]
        public int? laneId { get; set; }

        /// <summary>Gets or sets jobNo.</summary>
        [PropertyMapName("jobNo")]
        public int? jobNo { get; set; }

        /// <summary>Gets or sets staffId.</summary>
        [PropertyMapName("staffId")]
        public string staffId { get; set; }

        /// <summary>Gets or sets bojDateTime.</summary>
        [PropertyMapName("bojDateTime")]
        public DateTime? bojDateTime { get; set; }

        /// <summary>Gets or sets eojDateTime.</summary>
        [PropertyMapName("eojDateTime")]
        public DateTime? eojDateTime { get; set; }
    }

    /// <summary>The SCWJobList2Result class.</summary>
    public class SCWJobList2Result : SCWResult
    {
        /// <summary>Gets or sets list.</summary>
        //[PropertyMapName("list")]
        public List<SCWJob2> list { get; set; }
    }

    #endregion
}
