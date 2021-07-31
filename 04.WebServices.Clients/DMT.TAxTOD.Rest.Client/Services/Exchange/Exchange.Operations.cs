#region Usings

using System;
using System.Collections.Generic;
using DMT.Models;

#endregion

namespace DMT.Services.Operations
{
    partial class TAxTOD
    {
        /// <summary>The Exchange Operations (Account) class.</summary>
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

        /// <summary>The Exchange Operations (TA) class.</summary>
        public static partial class Exchange
        {
            /*
            /// <summary>
            /// Execute GetApproves api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<TAAExchangeHead>> GetApproves(
                string tsbId, DateTime? transactionDate)
            {
                var ret = Execute<List<TAAExchangeHead>>(
                    RouteConsts.TAxTOD.Exchange.GetApproves.Url, new { status = value });
                return ret;
            }
            */
        }
    }
}
