#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using NLib;
using NLib.IO;
using Newtonsoft.Json;
using NLib.Controls.Design;

#endregion

namespace DMT.Configurations
{
    #region TODMessageResendConfig

    /// <summary>
    /// The TODMessageResendConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TODMessageResendConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TODMessageResendConfig() : base() 
        {
            this.IntervalSeconds = 3600;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets Interval in seconds. Default 3600 seomds.</summary>
        public int IntervalSeconds { get; set; }

        #endregion
    }

    #endregion
}
