#region Usings

using System;
using System.Collections.Generic;
using DMT.Models;

#endregion

namespace DMT.Services.Operations
{
    partial class TAxTOD
    {
        /// <summary>The Account Operations class.</summary>
        public static partial class AccountCredit
        {
            public static partial class TSB
            {
                /// <summary>
                /// Execute Gets api.
                /// </summary>
                /// <returns>Returns instance of NRestResult.</returns>
                public static NRestResult<List<TAAccountTSBCreditResult>> Gets()
                {
                    var ret = Execute<List<TAAccountTSBCreditResult>>(
                        RouteConsts.TAxTOD.AccountCredit.TSB.Gets.Url, new { });
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
                public static NRestResult<List<TAAccountUserCreditResult>> Gets(string tsbId)
                {
                    var ret = Execute<List<TAAccountUserCreditResult>>(
                        RouteConsts.TAxTOD.AccountCredit.User.Gets.Url, new { tsbId = tsbId });
                    return ret;
                }
            }
        }
    }
}
