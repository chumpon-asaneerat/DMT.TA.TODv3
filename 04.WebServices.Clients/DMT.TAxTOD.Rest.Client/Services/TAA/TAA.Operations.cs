#region Usings

using System;
using System.Collections.Generic;
using DMT.Models;

#endregion

namespace DMT.Services.Operations
{
    partial class TAxTOD
    {
        /// <summary>The TAA Operations class.</summary>
        public static partial class TAA
        {
            /// <summary>
            /// Execute IsAlive api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<Models.IsAliveResult> IsAlive()
            {
                var ret = Execute<Models.IsAliveResult>(
                    RouteConsts.TAxTOD.TAA.IsAlive.Url, new { });
                return ret;
            }
        }
    }
}
