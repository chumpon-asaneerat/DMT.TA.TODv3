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
                /// Save TSB.
                /// </summary>
                /// <returns>Returns Saved TSB.</returns>
                public static NRestResult<Models.TSB> Save(Models.TSB value)
                {
                    var ret = Execute<Models.TSB>(
                        RouteConsts.TOD.Infrastructure.TSB.Save.Url, value);
                    return ret;
                }
            }
        }
    }
}
