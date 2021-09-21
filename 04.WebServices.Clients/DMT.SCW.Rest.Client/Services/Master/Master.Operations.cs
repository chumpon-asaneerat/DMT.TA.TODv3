#region Usings

using System;
using System.Collections.Generic;
using DMT.Models;

#endregion

namespace DMT.Services.Operations
{
    static partial class SCW
    {
        /// <summary>The Master Operations class.</summary>
        public static partial class Master
        {
            /// <summary>
            /// Execute cardAllowList api.
            /// </summary>
            /// <returns>Returns instance of SCWCardAllowListResult.</returns>
            public static SCWCardAllowListResult cardAllowList()
            {
                SCWCardAllowList value = new SCWCardAllowList();
                value.networkId = SCW.NetworkId;
                return cardAllowList(value);
            }
            /// <summary>
            /// Execute cardAllowList api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWCardAllowListResult.</returns>
            public static SCWCardAllowListResult cardAllowList(SCWCardAllowList value)
            {
                var ret = Execute<SCWCardAllowListResult>(
                    RouteConsts.SCW.Master.cardAllowList.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute couponList api.
            /// </summary>
            /// <returns>Returns instance of SCWCouponListResult.</returns>
            public static SCWCouponListResult couponList()
            {
                SCWCouponList value = new SCWCouponList();
                value.networkId = SCW.NetworkId;
                return couponList(value);
            }
            /// <summary>
            /// Execute couponList api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWCouponListResult.</returns>
            public static SCWCouponListResult couponList(SCWCouponList value)
            {
                var ret = Execute<SCWCouponListResult>(
                    RouteConsts.SCW.Master.couponList.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute couponBookList api.
            /// </summary>
            /// <returns>Returns instance of SCWCouponBookListResult.</returns>
            public static SCWCouponBookListResult couponBookList()
            {
                SCWCouponBookList value = new SCWCouponBookList();
                value.networkId = SCW.NetworkId;
                return couponBookList(value);
            }
            /// <summary>
            /// Execute couponBookList api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWCouponBookListResult.</returns>
            public static SCWCouponBookListResult couponBookList(SCWCouponBookList value)
            {
                var ret = Execute<SCWCouponBookListResult>(
                    RouteConsts.SCW.Master.couponBookList.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute currencyDenomList api.
            /// </summary>
            /// <returns>Returns instance of SCWCurrencyDemonListResult.</returns>
            public static SCWCurrencyDemonListResult currencyDenomList()
            {
                SCWCurrencyDemonList value = new SCWCurrencyDemonList();
                value.networkId = SCW.NetworkId;
                return currencyDenomList(value);
            }
            /// <summary>
            /// Execute currencyDenomList api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWCurrencyDemonListResult.</returns>
            public static SCWCurrencyDemonListResult currencyDenomList(SCWCurrencyDemonList value)
            {
                var ret = Execute<SCWCurrencyDemonListResult>(
                    RouteConsts.SCW.Master.currencyDenomList.Url, value);
                return ret;
            }
        }
    }
}
