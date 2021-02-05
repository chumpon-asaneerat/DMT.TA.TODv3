#region Using

using System;
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
    #region PlazaAppConfig (Combine configuration used in Plaza Config and Plaza Simulator applicaltion)

    /// <summary>
    /// The PlazaAppConfig class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class PlazaAppConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlazaAppConfig() : base()
        {
            this.DMT = new DMTConfig();
            this.SCW = new SCWWebServiceConfig();
            this.TAxTOD = new TAxTODWebServiceConfig();
            this.TAApp = new TAAppWebServiceConfig();
            this.TODApp = new TODAppWebServiceConfig();
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
            if (null == obj || !(obj is PlazaAppConfig)) return false;
            return this.GetString() == (obj as PlazaAppConfig).GetString();
        }
        /// <summary>
        /// GetString.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string code = string.Empty;
            // Application
            if (null == this.DMT)
            {
                code += "DMT: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("DMT: {0}",
                    this.DMT.GetString()) + Environment.NewLine;
            }
            // SCW server
            if (null == this.SCW)
            {
                code += "SCW: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("SCW: {0}",
                    this.SCW.GetString()) + Environment.NewLine;
            }
            // TAxTOD Server
            if (null == this.TAxTOD)
            {
                code += "TAxTOD: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("TAxTOD: {0}",
                    this.TAxTOD.GetString()) + Environment.NewLine;
            }
            // TA Application (Plaza)
            if (null == this.TAApp)
            {
                code += "TAApp: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("TAApp: {0}",
                    this.TAApp.GetString()) + Environment.NewLine;
            }
            // TOD Application (Plaza)
            if (null == this.TODApp)
            {
                code += "TODApp: null" + Environment.NewLine;
            }
            else
            {
                code += string.Format("TODApp: {0}",
                    this.TODApp.GetString()) + Environment.NewLine;
            }
            return code;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets DMT Config.
        /// </summary>
        public DMTConfig DMT { get; set; }
        /// <summary>
        /// Gets or sets SCW Service Config.
        /// </summary>
        public SCWWebServiceConfig SCW { get; set; }
        /// <summary>
        /// Gets or sets TAxTOD Service Config.
        /// </summary>
        public TAxTODWebServiceConfig TAxTOD { get; set; }
        /// <summary>
        /// Gets or sets TA App Service Config (local server).
        /// </summary>
        public TAAppWebServiceConfig TAApp { get; set; }
        /// <summary>
        /// Gets or sets TOD App Service Config (local server).
        /// </summary>
        public TODAppWebServiceConfig TODApp { get; set; }

        #endregion
    }

    #endregion
}
