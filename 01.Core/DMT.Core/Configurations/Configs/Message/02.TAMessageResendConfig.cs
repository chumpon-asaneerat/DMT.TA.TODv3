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
    #region TAMessageResendConfig

    /// <summary>
    /// The TAMessageResendConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TAMessageResendConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TAMessageResendConfig() : base() 
        {
            this.SCW = new MessageResendConfig();
            this.TAxTOD = new MessageResendConfig();
            this.TODApps = new MessageResendConfig();
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
        /// Gets or sets TODApps message resend config.
        /// </summary>
        public MessageResendConfig TODApps { get; set; }

        #endregion
    }

    #endregion
}
