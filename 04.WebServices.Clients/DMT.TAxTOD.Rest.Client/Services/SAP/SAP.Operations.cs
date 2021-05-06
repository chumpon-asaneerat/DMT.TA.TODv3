#region Usings

using System;
using System.Collections.Generic;
using DMT.Models;

#endregion

namespace DMT.Services.Operations
{
    partial class TAxTOD
    {
        /// <summary>The SAP Operations class.</summary>
        public static partial class SAP
        {
            /// <summary>
            /// Execute GetCustomers api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<SAPCustomer>, NRestOut> GetCustomers(
                Search.TAxTOD.SAP.SearchCustomer value)
            {
                var ret = Execute<List<SAPCustomer>, NRestOut>(
                    RouteConsts.TAxTOD.SAP.GetCustomers.Url, value);
                return ret;
            }

            /// <summary>
            /// Execute GetTSBs api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<SAPTSB>, NRestOut> GetTSBs()
            {
                var ret = Execute<List<SAPTSB>, NRestOut>(
                    RouteConsts.TAxTOD.SAP.GetTSBs.Url, new { });
                return ret;
            }
        }
    }
}
