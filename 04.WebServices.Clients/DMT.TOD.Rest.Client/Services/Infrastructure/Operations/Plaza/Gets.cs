#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TOD
    {
        static partial class Infrastructure
        {
            static partial class Plaza
            {
                /// <summary>
                /// Gets all Plazas.
                /// </summary>
                /// <returns>Returns all Plazas.</returns>
                public static NRestResult<List<Models.Plaza>> Gets()
                {
                    var ret = Execute<List<Models.Plaza>>(
                        RouteConsts.TOD.Infrastructure.Plaza.Gets.Url);
                    return ret;
                }
            }
        }
    }
}
