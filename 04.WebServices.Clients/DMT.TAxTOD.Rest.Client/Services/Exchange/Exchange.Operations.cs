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
            /// <summary>
            /// Execute Get Request Items api.
            /// </summary>
            /// <param name="tsbId">The api parameter (TSBId).</param>
            /// <param name="requestId">The api parameter (RequestId).</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<TAAExchangeItem>> GetRequestItems(
                string tsbId, int requestId)
            {
                var ret = Execute<List<TAAExchangeItem>>(
                    RouteConsts.TAxTOD.Exchange.GetRequestItems.Url, new { TSBId = tsbId, RequestId = requestId });
                return ret;
            }
            /// <summary>
            /// Execute Approve Request Item api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult ApproveRequestItem(TAAExchangeItem value)
            {
                var ret = Execute(
                    RouteConsts.TAxTOD.Exchange.ApproveRequestItem.Url, value);
                return ret;
            }
        }

        /// <summary>The Exchange Operations (TA) class.</summary>
        public static partial class Exchange
        {
            /// <summary>
            /// Execute Save Request Document api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult SaveRequestDocument(TAAExchangeHeader value)
            {
                var ret = Execute(
                    RouteConsts.TAxTOD.Exchange.SaveRequestDocument.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute Save Request Item (detail) api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult SaveRequestItem(TAARequestExchangeItem value)
            {
                var ret = Execute(
                    RouteConsts.TAxTOD.Exchange.SaveRequestItem.Url, value);
                return ret;
            }
            /// <summary>
            /// Execute Get Approves api.
            /// </summary>
            /// <param name="tsbId">The api parameter (TSBId).</param>
            /// <param name="transactionDate">The api parameter (TransactionDate).</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<TAAApproveSummary>> GetApproves(
                string tsbId, DateTime? transactionDate)
            {
                var ret = Execute<List<TAAApproveSummary>>(
                    RouteConsts.TAxTOD.Exchange.GetApproves.Url, new { TSBId = tsbId, TransDate = transactionDate });
                return ret;
            }
            /// <summary>
            /// Execute Get Approve Items api.
            /// </summary>
            /// <param name="tsbId">The api parameter (TSBId).</param>
            /// <param name="requestId">The api parameter (RequestId).</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult<List<TAAApproveItem>> GetApproveItems(
                string tsbId, int? requestId)
            {
                var ret = Execute<List<TAAApproveItem>>(
                    RouteConsts.TAxTOD.Exchange.GetApproveItems.Url, new { TSBId = tsbId, RequestId = requestId });
                return ret;
            }
            /// <summary>
            /// Execute Save Approve Item (detail) api.
            /// </summary>
            /// <param name="value">The api parameter.</param>
            /// <returns>Returns instance of NRestResult.</returns>
            public static NRestResult SaveApproveItem(TAAApproveExchangeItem value)
            {
                var ret = Execute(
                    RouteConsts.TAxTOD.Exchange.ApproveRequestItem.Url, value);
                return ret;
            }

            /// <summary>
            /// Get TSB Approve Credits.
            /// </summary>
            /// <returns></returns>
            public static NRestResult<List<TAATSBApproveCredit>> GetTSBApproveCredits()
            {
                var ret = Execute<List<TAATSBApproveCredit>>(
                    RouteConsts.TAxTOD.Exchange.GetTSBApproveCredits.Url, new { });
                return ret;
            }
            /// <summary>
            /// Get TSB Approve Credit Transactions.
            /// </summary>
            /// <param name="tsbId">The TSB Id.</param>
            /// <returns></returns>
            public static NRestResult<List<TAATSBApproveCreditTransaction>> GetTSBApproveCreditTransactions(string tsbId)
            {
                var ret = Execute<List<TAATSBApproveCreditTransaction>>(
                    RouteConsts.TAxTOD.Exchange.GetTSBApproveCreditTransactions.Url, new { TSBId = tsbId });
                return ret;
            }
        }
    }
}
