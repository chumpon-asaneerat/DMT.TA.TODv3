#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
    {
        static partial class Infrastructure
        {
            static partial class TSB
            {
                /// <summary>
                /// Set Active TSB.
                /// </summary>
                /// <returns>Returns NRestResult instance.</returns>
                public static NRestResult SetActive(Models.TSB value)
                {
                    var ret = Execute(
                        RouteConsts.TA.Infrastructure.TSB.SetActive.Url, value);
                    return ret;
                }
            }
        }
    }
}
