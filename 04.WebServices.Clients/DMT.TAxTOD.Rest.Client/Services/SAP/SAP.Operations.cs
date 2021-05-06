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
                Search.TAxTOD.SAP.Customers value)
            {
                var ret = Execute<List<SAPCustomer>, NRestOut>(
                    RouteConsts.TAxTOD.SAP.GetCustomers.Url, value);
                return ret;
            }

            /// <summary>
            /// Execute GetTSBs api.
            /// </summary>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<SAPTSB>, NRestOut> GetTSBs()
            {
                var ret = Execute<List<SAPTSB>, NRestOut>(
                    RouteConsts.TAxTOD.SAP.GetTSBs.Url, new { });
                return ret;
            }

            /// <summary>
            /// Execute GetCouponSolds api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<SAPCouponSold>, NRestOut> GetCouponSolds(
                Search.TAxTOD.SAP.CouponSolds value)
            {
                var ret = Execute<List<SAPCouponSold>, NRestOut>(
                    RouteConsts.TAxTOD.SAP.GetCouponSolds.Url, value);
                return ret;
            }
        }
    }
}
