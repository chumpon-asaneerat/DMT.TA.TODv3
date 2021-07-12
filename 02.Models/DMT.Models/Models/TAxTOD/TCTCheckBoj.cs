#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

using Newtonsoft.Json;

#endregion

namespace DMT.Models
{
    // Server data result.
    /// <summary>
    /// The TOD Check Boj class. (Used by TCT and TA app).
    /// </summary>
    public class CheckTODBoj
    {
        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }
        /// <summary>Gets or sets UserId.</summary>
        [PropertyMapName("UserId")]
        public string UserId { get; set; }
    }

    static partial class Search
    {
        static partial class TAxTOD
        {
            #region CheckBoj

            public class CheckBoj : NSearch<CheckBoj>
            {
                #region Public Properties

                /// <summary>Gets or sets TSBId.</summary>
                public string TSBId { get; set; }
                /// <summary>Gets or sets UserId.</summary>
                public string UserId { get; set; }

                #endregion

                #region Static Method (Create)

                /// <summary>
                /// Create Search instance.
                /// </summary>
                /// <param name="sapItemCode">The SAPItemCode.</param>
                /// <param name="sapIntrSerial">The SAPIntrSerial.</param>
                /// <returns>Returns Search instance.</returns>
                public static CheckBoj Create(
                    string tsbId,
                    string userId)
                {
                    var ret = new CheckBoj();
                    ret.TSBId = tsbId;
                    ret.UserId = userId;
                    return ret;
                }

                #endregion
            }

            #endregion
        }
    }
}
