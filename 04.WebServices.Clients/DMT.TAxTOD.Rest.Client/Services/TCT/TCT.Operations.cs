#region Usings

using System;
using System.Collections.Generic;
using DMT.Models;

#endregion

namespace DMT.Services.Operations
{
    partial class TAxTOD
    {
        /// <summary>The TCT Operations class.</summary>
        public static partial class TCT
        {
            /// <summary>
            /// Execute GetUserCoupons api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult GetUserCoupons()
            {
                var ret = Execute<TAServerCouponTransaction>(
                    RouteConsts.TAxTOD.TCT.GetUserCoupons.Url, new { });
                return ret;
            }

            /// <summary>
            /// Execute SoldCoupon api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult SoldCoupon()
            {
                var ret = Execute<TAServerCouponTransaction>(
                    RouteConsts.TAxTOD.TCT.SoldCoupon.Url, new { });
                return ret;
            }

            /// <summary>
            /// Execute Check TOD Boj api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<Models.CheckTODBoj>> CheckTODBoj(Search.TAxTOD.CheckBoj value)
            {
                var ret = Execute<List<Models.CheckTODBoj>>(
                    RouteConsts.TAxTOD.TCT.CheckTODBoj.Url, value);
                return ret;
            }
        }
    }
}
