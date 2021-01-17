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
            this.Name = string.Empty;
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
            if (null == obj || !(obj is StatusBarConfig)) return false;
            return this.GetString() == (obj as StatusBarConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string code = this.Name;
            return code;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets Name.</summary>
        public string Name { get; set; }
        /// <summary>Gets or sets Visibility.</summary>
        public bool Visible { get; set; }

        #endregion
    }

    #endregion
}
