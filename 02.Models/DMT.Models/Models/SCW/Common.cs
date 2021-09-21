#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;
using DMT;
// required for JsonIgnore.
using Newtonsoft.Json;

#endregion

namespace DMT.Models
{
    /// <summary>The SCWStatus class.</summary>
    public class SCWStatus
    {
        /// <summary>
        /// Gets or sets code. 
        /// S200 = Success, 
        /// F500 = API Error, 
        /// F203 = User not authenticated, 
        /// F302 = API Bad Request
        /// </summary>
        [PropertyMapName("code")]
        public string code { get; set; }

        /// <summary>Gets or sets message.</summary>
        [PropertyMapName("message")]
        public string message { get; set; }
    }

    /// <summary>The Common SCWResult class.</summary>
    public class SCWResult
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SCWResult() : base() 
        {
            this.HttpStatus = HttpStatus.None;
            this.Description = string.Empty;
        }

        /// <summary>Gets or sets http status.</summary>
        [JsonIgnore]
        [PropertyMapName("HttpStatus")]
        public HttpStatus HttpStatus { get; set; }
        /// <summary>Gets or sets http status Description.</summary>
        [JsonIgnore]
        [PropertyMapName("Description")]
        
        public string Description { get; set; }
        /// <summary>
        /// Checks is Ok (not include Http status).
        /// </summary>
        [JsonIgnore]
        [PropertyMapName("Ok")]
        public bool Ok
        {
            get 
            {
                bool statusObjOk = (null != this.status && !string.IsNullOrWhiteSpace(this.status.code));
                bool servDbsOk = (statusObjOk && this.status.code.ToUpperInvariant() == "S200");
                return statusObjOk && servDbsOk; 
            }
            set { }
        }
        /// <summary>Gets or sets status.</summary>
        [PropertyMapName("status")]
        public SCWStatus status { get; set; }
    }
}
