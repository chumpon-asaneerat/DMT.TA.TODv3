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
            this.SCW = new MessageResendConfig();
            this.TAxTOD = new MessageResendConfig();
            this.TAApp = new MessageResendConfig();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets SCW message resend config.
        /// </summary>
        public MessageResendConfig SCW { get; set; }
        /// <summary>
        /// Gets or sets TAxTOD message resend config.
        /// </summary>
        public MessageResendConfig TAxTOD { get; set; }
        /// <summary>
        /// Gets or sets TAApp message resend config.
        /// </summary>
        public MessageResendConfig TAApp { get; set; }

        #endregion
    }

    #endregion
}
