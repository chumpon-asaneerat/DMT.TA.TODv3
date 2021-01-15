#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TOD
    {
        static partial class Infrastructure
        {
            static partial class Lane
            {
                /// <summary>
                /// Gets all Lanes.
                /// </summary>
                /// <returns>Returns all Lanes.</returns>
                public static NRestResult<List<Models.Lane>> Gets()
                {
                    var ret = Execute< List<Models.Lane>>(
                        RouteConsts.TOD.Infrastructure.Lane.Gets.Url);
                    return ret;
                }
            }
        }
    }
}
