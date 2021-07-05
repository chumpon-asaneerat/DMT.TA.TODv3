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
    #region SupAdjWebSocketConfig

    /// <summary>
    /// The SupAdjWebSocketConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SupAdjWebSocketConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SupAdjWebSocketConfig() : base() 
        {
            this.Protocol = "ws";
            this.HostName = "127.0.0.1";
            this.Route = "";
            this.PortNumber = 8090;
            this.Enabled = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Protocol.
        /// </summary>
        public string Protocol { get; set; }
        /// <summary>
        /// Gets or sets Host.
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// Gets or sets Route (default is empty).
        /// </summary>
        public string Route { get; set; }
        /// <summary>
        /// Gets or sets Port Number.
        /// </summary>
        public int PortNumber { get; set; }
        /// <summary>
        /// Gets or sets Enabled.
        /// </summary>
        public bool Enabled { get; set; }

        #endregion
    }

    #endregion
}
