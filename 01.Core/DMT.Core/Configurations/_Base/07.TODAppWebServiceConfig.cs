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
    #region TODAppWebServiceConfig (Need for TOD App Web Service class below)

    /// <summary>
    /// The TODPlazaConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TODPlazaConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TODPlazaConfig() : base()
        {
            this.PlazaId = 0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Plaza Id.
        /// </summary>
        public int PlazaId { get; set; }

        #endregion
    }

    #endregion

    // TODO: Need TOD Client Name, TSB Display Name property.

    #region TODAppWebServiceConfig (For TOD App Web Service)

    /// <summary>
    /// The TODAppWebServiceConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TODAppWebServiceConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TODAppWebServiceConfig()
        {
            this.Service = new WebServiceConfig()
            {
                Protocol = "http",
                HostName = "localhost",
                PortNumber = 9002,
                UserName = "DMTUSER",
                Password = "DMTPASS"
            };
            Plazas = new List<TODPlazaConfig>();
            Plazas.Add(new TODPlazaConfig() { PlazaId = 15 });
            Plazas.Add(new TODPlazaConfig() { PlazaId = 16 });
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
            if (null == obj || !(obj is TODAppWebServiceConfig)) return false;
            return this.GetString() == (obj as TODAppWebServiceConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            if (null != this.Service)
                return string.Format("{0}", this.Service.GetString());
            else return "TOD App http is null.";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Http service.
        /// </summary>
        public WebServiceConfig Service { get; set; }
        /// <summary>
        /// Gets or sets Plazas.
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<TODPlazaConfig> Plazas { get; set; }

        #endregion
    }

    #endregion
}
