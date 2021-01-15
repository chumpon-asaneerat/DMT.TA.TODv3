#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TOD
    {
        static partial class Infrastructure
        {
            static partial class PlazaGroup
            {
                /// <summary>
                /// Gets all PlazaGroups.
                /// </summary>
                /// <returns>Returns all PlazaGroups.</returns>
                public static NRestResult<List<Models.PlazaGroup>> Gets()
                {
                    var ret = Execute<List<Models.PlazaGroup>>(
                        RouteConsts.TOD.Infrastructure.PlazaGroup.Gets.Url);
                    return ret;
                }
            }
        }
    }
}
