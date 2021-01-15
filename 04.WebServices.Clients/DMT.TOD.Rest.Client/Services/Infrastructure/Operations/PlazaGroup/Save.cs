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
                /// Save PlazaGroup.
                /// </summary>
                /// <returns>Returns Saved PlazaGroup.</returns>
                public static NRestResult<Models.PlazaGroup> Save(Models.PlazaGroup value)
                {
                    var ret = Execute<Models.PlazaGroup>(
                        RouteConsts.TOD.Infrastructure.PlazaGroup.Save.Url, value);
                    return ret;
                }
            }
        }
    }
}
