#region Usings

using System;
using System.Collections.Generic;
using DMT.Models;

#endregion

namespace DMT.Services.Operations
{
    partial class TAxTOD
    {
        /// <summary>The Coupon Operations class.</summary>
        public static partial class Coupon
        {
            /// <summary>
            /// Execute Gets api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<TAServerCouponTransaction>> Gets(
                Search.TAxTOD.Coupon.Gets value)
            {
                var ret = Execute<List<TAServerCouponTransaction>>(
                    RouteConsts.TAxTOD.Coupon.Gets.Url, value);
                return ret;
            }

            /// <summary>
            /// Execute Save api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult Save(
                TAServerCouponTransaction value)
            {
                var ret = Execute<TAServerCouponTransaction>(
                    RouteConsts.TAxTOD.Coupon.Save.Url, value);
                return ret;
            }
        }
    }
}
