#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
    {
        static partial class Infrastructure
        {
            static partial class Plaza
            {
                /// <summary>
                /// Save Plaza.
                /// </summary>
                /// <returns>Returns Saved Plaza.</returns>
                public static NRestResult<Models.Plaza> Save(Models.Plaza value)
                {
                    var ret = Execute<Models.Plaza>(
                        RouteConsts.TA.Infrastructure.Plaza.Save.Url, value);
                    return ret;
                }
            }
        }
    }
}
