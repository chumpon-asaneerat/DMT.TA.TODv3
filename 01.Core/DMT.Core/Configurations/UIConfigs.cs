#region Using

using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

#endregion

namespace DMT.Configurations
{
    #region StatusBarConfig

    /// <summary>
    /// The StatusBarConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class StatusBarConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public StatusBarConfig() : base()
        {
            this.Visible = false;
            this.IntervalSeconds = 60;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets Visibility.</summary>
        public bool Visible { get; set; }
        /// <summary>Gets or sets Interval in seconds. Default 5 seomds.</summary>
        public int IntervalSeconds { get; set; }

        #endregion
    }

    #endregion
}
