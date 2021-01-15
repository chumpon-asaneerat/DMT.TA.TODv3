#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
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
                        RouteConsts.TA.Infrastructure.PlazaGroup.Gets.Url);
                    return ret;
                }
            }
        }
    }
}
