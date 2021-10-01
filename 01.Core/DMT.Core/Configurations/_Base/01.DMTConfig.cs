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
    #region DMTConfig (Common DMT Consts Information)

    /// <summary>
    /// The DMT Config class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class DMTConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public DMTConfig()
        {
            this.network = "4";
            this.tsb = "97";
            this.terminal = "49701";
            this.networkId = 10;
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
            if (null == obj || !(obj is DMTConfig)) return false;
            return this.GetString() == (obj as DMTConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string code = string.Format("network:{0}, tsb:{1}, terminal:{2}, networkId:{3}",
                this.network, this.tsb, this.terminal, this.networkId);
            return code;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets network.
        /// </summary>
        public string network { get; set; }
        /// <summary>
        /// Gets or sets tsb.
        /// </summary>
        public string tsb { get; set; }
        /// <summary>
        /// Gets or sets terminal.
        /// </summary>
        public string terminal { get; set; }
        /// <summary>
        /// Gets or sets networkId.
        /// </summary>
        public int networkId { get; set; }

        #endregion
    }

    #endregion
}
