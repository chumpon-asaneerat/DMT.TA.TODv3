#region Usings

using System;
using System.Collections.Generic;
using DMT.Models;

#endregion

namespace DMT.Services.Operations
{
    partial class TAxTOD
    {
        /// <summary>The Credit Operations class.</summary>
        public static partial class Credit
        {
            public static partial class TSB
            {
                /// <summary>
                /// Execute Gets api.
                /// </summary>
                /// <returns>Returns instance of NRestResult.</returns>
                public static NRestResult<List<TAATSBCreditResult>> Gets()
                {
                    var ret = Execute<List<TAATSBCreditResult>>(
                        RouteConsts.TAxTOD.Credit.TSB.Gets.Url, new { });
                    return ret;
                }
                /// <summary>
                /// Execute Save api.
                /// </summary>
                /// <param name="value">The api parameter.</param>
                /// <returns>Returns instance of NRestResult.</returns>
                public static NRestResult Save(
                    TAATSBCredit value)
                {
                    var ret = Execute<TAATSBCredit>(
                        RouteConsts.TAxTOD.Credit.TSB.Save.Url, value);
                    return ret;
                }
            }

            public static partial class User
            {
                /// <summary>
                /// Execute Gets api.
                /// </summary>
                /// <param name="tsbId">The TSBId</param>
                /// <returns>Returns instance of NRestResult.</returns>
                public static NRestResult<List<TAAUserCreditResult>> Gets(string tsbId)
                {
                    var ret = Execute<List<TAAUserCreditResult>>(
                        RouteConsts.TAxTOD.Credit.User.Gets.Url, new { tsbId = tsbId });
                    return ret;
                }
                /// <summary>
                /// Execute Save api.
                /// </summary>
                /// <param name="value">The api parameter.</param>
                /// <returns>Returns instance of NRestResult.</returns>
                public static NRestResult Save(
                    TAAUserCredit value)
                {
                    var ret = Execute<TAAUserCredit>(
                        RouteConsts.TAxTOD.Credit.User.Save.Url, value);
                    return ret;
                }
            }
        }
    }
}
