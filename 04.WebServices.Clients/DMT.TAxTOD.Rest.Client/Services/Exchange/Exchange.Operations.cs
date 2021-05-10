#region Usings

using System;
using System.Collections.Generic;
using DMT.Models;

#endregion

namespace DMT.Services.Operations
{
    partial class TAxTOD
    {
        /// <summary>The Exchange Operations class.</summary>
        public static partial class Exchange
        {
            /// <summary>
            /// Execute Gets api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<TAAExchangeSummary>> Gets(
                string value)
            {
                var ret = Execute<List<TAAExchangeSummary>>(
                    RouteConsts.TAxTOD.Exchange.Gets.Url, new { status = value });
                return ret;
            }
        }
    }
}
