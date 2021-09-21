#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    #region Parameter class

    /// <summary>The SCWCardAllowList class.</summary>
    public class SCWCardAllowList
    {
        /// <summary>Gets or sets networkId.</summary>
        [PropertyMapName("networkId")]
        public int? networkId { get; set; }
    }

    #endregion

    #region Result related classes

    /// <summary>The SCWCardAllow class.</summary>
    public class SCWCardAllow
    {
        /// <summary>Gets or sets cardAllowId.</summary>
        [PropertyMapName("cardAllowId")]
        public int cardAllowId { get; set; }

        /// <summary>Gets or sets abbreviation.</summary>
        [PropertyMapName("abbreviation")]
        public string abbreviation { get; set; }

        /// <summary>Gets or sets description.</summary>
        [PropertyMapName("description")]
        public string description { get; set; }
    }

    #endregion

    #region Result related classes

    /// <summary>The SCWCardAllowListResult class.</summary>
    public class SCWCardAllowListResult : SCWResult
    {
        /// <summary>Gets or sets list.</summary>
        //[PropertyMapName("list")]
        public List<SCWCardAllow> list { get; set; }
    }

    #endregion
}
