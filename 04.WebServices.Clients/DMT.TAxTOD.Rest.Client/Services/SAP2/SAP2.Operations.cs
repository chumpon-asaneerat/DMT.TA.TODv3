#region Usings

using System;
using System.Collections.Generic;
using System.Windows.Media;
using DMT.Models;

#endregion

namespace DMT.Services.Operations
{
    partial class TAxTOD
    {
        /// <summary>The SAP Operations class.</summary>
        public static partial class SAP2
        {
            /// <summary>
            /// Execute GetCouponMasters api.
            /// </summary>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<CouponMaster>, NRestOut> GetCouponMasters()
            {
                var ret = Execute<List<CouponMaster>, NRestOut>(
                    RouteConsts.TAxTOD.SAP2.GetCouponMasters.Url, new { });
                return ret;
            }

            /// <summary>
            /// Execute GetStorageLocations api.
            /// </summary>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<Storagelocation>, NRestOut> GetStorageLocations(string tsbId)
            {
                var ret = Execute<List<Storagelocation>, NRestOut>(
                    RouteConsts.TAxTOD.SAP2.GetStorageLocations.Url, new { tsbid = tsbId });
                return ret;
            }

            /// <summary>
            /// Execute GetReservationCurrentRunningNo api.
            /// </summary>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<ReserveRunningNo>, NRestOut> GetReservationCurrentRunningNo(
                string tsbId, string sYear)
            {
                var ret = Execute<List<ReserveRunningNo>, NRestOut>(
                    RouteConsts.TAxTOD.SAP2.GetReservationCurrentRunningNo.Url, 
                    new { tsbid = tsbId, year = sYear });
                return ret;
            }
            /// <summary>
            /// Execute SaveReservation api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult SaveReservation(ReserveRequest value)
            {
                var ret = Execute(RouteConsts.TAxTOD.SAP2.SaveReservation.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute UpdateReservationCurrentRunningNo api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult UpdateReservationCurrentRunningNo(string tsbId, int runningNo)
            {
                var ret = Execute(RouteConsts.TAxTOD.SAP2.UpdateReservationCurrentRunningNo.Url, 
                    new
                    {
                        tsbid = tsbId,
                        runningno = runningNo
                    });
                return ret;
            }
            /// <summary>
            /// Execute SearchReservation api.
            /// </summary>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<ReserveDocument>, NRestOut> SearchReservation(
                string baseDate, string sReqStatus, string sTransfer)
            {
                var ret = Execute<List<ReserveDocument>, NRestOut>(
                    RouteConsts.TAxTOD.SAP2.SearchReservation.Url,
                    new { basedate = baseDate, sendresult = sReqStatus, transfer = sTransfer });
                return ret;
            }
            /// <summary>
            /// Execute GetReservationItems api.
            /// </summary>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<ReserveDocumentItem>, NRestOut> GetReservationItems(
                string goodsRecipient)
            {
                var ret = Execute<List<ReserveDocumentItem>, NRestOut>(
                    RouteConsts.TAxTOD.SAP2.GetReservationItems.Url,
                    new { goodsrecipient = goodsRecipient });
                return ret;
            }
        }
    }
}
