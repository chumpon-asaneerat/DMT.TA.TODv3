#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class Plaza
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
                        RouteConsts.Infrastructure.TSB.Current.Url);
                    return ret;
                }
            }
        }
    }
}
