#region Using

using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

#endregion

namespace DMT.Configurations
{
    #region PlazaInfoConfig

    /// <summary>
    /// The PlazaInfoConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class PlazaInfoConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlazaInfoConfig() : base()
        {
            this.TSBId = "09";
            this.PlazaId = "090";
            this.PlazaNameEN = "PlazaNameEN";
            this.PlazaNameTH = "PlazaNameTH";
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets TSBId.</summary>
        public string TSBId { get; set; }
        /// <summary>Gets or sets PlazaId.</summary>
        public string PlazaId { get; set; }
        /// <summary>Gets or sets PlazaNameEN.</summary>
        public string PlazaNameEN { get; set; }
        /// <summary>Gets or sets PlazaNameTH.</summary>
        public string PlazaNameTH { get; set; }

        #endregion
    }

    #endregion
}
