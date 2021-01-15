#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TOD
    {
        static partial class Infrastructure
        {
            static partial class TSB
            {
                /// <summary>
                /// Gets Current (Active) TSB.
                /// </summary>
                /// <returns>Returns Active TSB.</returns>
                public static NRestResult<Models.TSB> Current()
                {
                    var ret = Execute<Models.TSB>(
                        RouteConsts.TOD.Infrastructure.TSB.Current.Url);
                    return ret;
                }
            }
        }
    }
}
