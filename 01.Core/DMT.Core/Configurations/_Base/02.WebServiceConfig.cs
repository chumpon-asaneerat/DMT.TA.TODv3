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
    #region WebServiceConfig (Common Web Service Config)

    /// <summary>
    /// The WebServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class WebServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public WebServiceConfig()
        {
            this.Protocol = "http";
            this.HostName = "localhost";
            this.PortNumber = 9000;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// IsEquals.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsEquals(object obj)
        {
            if (null == obj || !(obj is WebServiceConfig)) return false;
            return this.GetString() == (obj as WebServiceConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string code = string.Format("{0}://{1}:{2}",
                this.Protocol, this.HostName, this.PortNumber);
            return code;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets protocol.
        /// </summary>
        public string Protocol { get; set; }
        /// <summary>
        /// Gets or sets Host Name or IP Address.
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// Gets or sets port number.
        /// </summary>
        public int PortNumber { get; set; }
        /// <summary>
        /// Gets or sets User Name.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; }

        #endregion
    }

    #endregion
}
