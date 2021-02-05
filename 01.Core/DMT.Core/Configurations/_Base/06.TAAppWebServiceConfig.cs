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
    #region TAAppWebServiceConfig (For TA App Web Service)

    /// <summary>
    /// The TAAppWebServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TAAppWebServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TAAppWebServiceConfig()
        {
            this.Service = new WebServiceConfig()
            {
                Protocol = "http",
                HostName = "localhost",
                PortNumber = 9001,
                UserName = "DMTUSER",
                Password = "DMTPASS"
            };
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
            if (null == obj || !(obj is TAAppWebServiceConfig)) return false;
            return this.GetString() == (obj as TAAppWebServiceConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            if (null != this.Service)
                return string.Format("{0}", this.Service.GetString());
            else return "TA App http is null.";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Http service.
        /// </summary>
        public WebServiceConfig Service { get; set; }

        #endregion
    }

    #endregion
}
