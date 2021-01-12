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
                /// Gets all TSBs.
                /// </summary>
                /// <returns>Returns all TSBs.</returns>
                public static NRestResult<List<Models.TSB>> Gets()
                {
                    var ret = Execute<List<Models.TSB>>(
                        RouteConsts.Infrastructure.TSB.Gets.Url);
                    return ret;
                }
            }
        }
    }
}
