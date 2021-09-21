#region Usings

using System;
using System.Collections.Generic;
using DMT.Models;

#endregion

namespace DMT.Services.Operations
{
    static partial class SCW
    {
        /// <summary>The Emulator Operations class.</summary>
        public static partial class Emulator
        {
            /// <summary>
            /// Execute boj api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWBOJResult.</returns>
            public static SCWBOJResult boj(SCWBOJ value)
            {
                var ret = Execute<SCWBOJResult>(
                    RouteConsts.SCW.Emulator.boj.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute eoj api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWEOJResult.</returns>
            public static SCWEOJResult eoj(SCWEOJ value)
            {
                var ret = Execute<SCWEOJResult>(
                    RouteConsts.SCW.Emulator.eoj.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute allJobs api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWAllJobResult.</returns>
            public static SCWAllJobResult allJobs(SCWAllJob value)
            {
                var ret = Execute<SCWAllJobResult>(
                    RouteConsts.SCW.Emulator.allJobs.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute removeJobs api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWRemoveJobsResult.</returns>
            public static SCWRemoveJobsResult removeJobs(SCWRemoveJobs value)
            {
                var ret = Execute<SCWRemoveJobsResult>(
                    RouteConsts.SCW.Emulator.removeJobs.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute clearJobs api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWClearJobsResult.</returns>
            public static SCWClearJobsResult clearJobs(SCWClearJobs value)
            {
                var ret = Execute<SCWClearJobsResult>(
                    RouteConsts.SCW.Emulator.clearJobs.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute add EMV api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWEMVTransactionListResult.</returns>
            public static SCWAddEMVResult addEMV(SCWAddEMV value)
            {
                var ret = Execute<SCWAddEMVResult>(
                    RouteConsts.SCW.Emulator.addEMV.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute remove EMV api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWQRCodeTransactionListResult.</returns>
            public static SCWRemoveEMVResult removeEMV(SCWRemoveEMV value)
            {
                var ret = Execute<SCWRemoveEMVResult>(
                    RouteConsts.SCW.Emulator.removeEMV.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute clear EMVs api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWClearEMVsResult.</returns>
            public static SCWClearEMVsResult clearEMVs(SCWClearEMVs value)
            {
                var ret = Execute<SCWClearEMVsResult>(
                    RouteConsts.SCW.Emulator.clearEMVs.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute add QRCode api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWAddQRCodeResult.</returns>
            public static SCWAddQRCodeResult addQRCode(SCWAddQRCode value)
            {
                var ret = Execute<SCWAddQRCodeResult>(
                    RouteConsts.SCW.Emulator.addQRCode.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute remove QRCode api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWRemoveQRCodeResult.</returns>
            public static SCWRemoveQRCodeResult removeQRCode(SCWRemoveQRCode value)
            {
                var ret = Execute<SCWRemoveQRCodeResult>(
                    RouteConsts.SCW.Emulator.removeQRCode.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute clear QRCodes api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of SCWClearQRCodesResult.</returns>
            public static SCWClearQRCodesResult clearQRCodes(SCWClearQRCodes value)
            {
                var ret = Execute<SCWClearQRCodesResult>(
                    RouteConsts.SCW.Emulator.clearQRCodes.Url, value);
                return ret;
            }
        }
    }
}
