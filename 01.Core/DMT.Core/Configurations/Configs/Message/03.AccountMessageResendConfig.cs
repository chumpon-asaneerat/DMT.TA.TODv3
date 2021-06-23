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
    #region AccountMessageResendConfig

    /// <summary>
    /// The AccountMessageResendConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class AccountMessageResendConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountMessageResendConfig() : base() 
        {
            this.SCW = new MessageResendConfig();
            this.TAxTOD = new MessageResendConfig();
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

        #endregion
    }

    #endregion
}
