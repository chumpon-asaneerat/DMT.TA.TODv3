#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    #region Parameter class

    /// <summary>The SCWCurrencyDemonList class.</summary>
    public class SCWCurrencyDemonList
    {
        /// <summary>Gets or sets networkId.</summary>
        [PropertyMapName("networkId")]
        public int? networkId { get; set; }
    }

    #endregion

    #region Result related classes

    /// <summary>The SCWCurrencyDemon class.</summary>
    public class SCWCurrencyDemon
    {
        /// <summary>Gets or sets currencyId.</summary>
        [PropertyMapName("currencyId")]
        public int currencyId { get; set; }

        /// <summary>Gets or sets currencyDenomId.</summary>
        [PropertyMapName("currencyDenomId")]
        public int currencyDenomId { get; set; }

        /// <summary>Gets or sets abbreviation.</summary>
        [PropertyMapName("abbreviation")]
        public string abbreviation { get; set; }

        /// <summary>Gets or sets description.</summary>
        [PropertyMapName("description")]
        public string description { get; set; }

        /// <summary>Gets or sets denomValue.</summary>
        [PropertyMapName("denomValue")]
        public decimal denomValue { get; set; } // ประเภทค่าเงิน 1 = ธนบัตร, 2 = เหรียญ

        /// <summary>Gets or sets denomTypeId.</summary>
        [PropertyMapName("denomTypeId")]
        public int denomTypeId { get; set; }
    }

    /// <summary>The SCWCurrencyDemonListResult class.</summary>
    public class SCWCurrencyDemonListResult
    {
        /// <summary>Gets or sets list.</summary>
        //[PropertyMapName("list")]
        public List<SCWCurrencyDemon> list { get; set; }

        /// <summary>Gets or sets status.</summary>
        [PropertyMapName("status")]
        public SCWStatus status { get; set; }
    }

    #endregion
}
