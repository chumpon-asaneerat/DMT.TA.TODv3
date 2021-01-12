#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class Plaza
    {
        static partial class Infrastructure
        {
            static partial class Plaza
            {
                static partial class Search
                {
                    /// <summary>
                    /// Gets Plazas By TSB.
                    /// </summary>
                    /// <param name="value">The TSB.</param>
                    /// <returns>Returns Plazas by TSB.</returns>
                    public static NRestResult<List<Models.Plaza>> ByTSB(Models.TSB value)
                    {
                        var ret = Execute<List<Models.Plaza>>(
                            RouteConsts.Infrastructure.Plaza.Search.ByTSB.Url, value);
                        return ret;
                    }

                    /// <summary>
                    /// Gets Plazas By PlazaGroup.
                    /// </summary>
                    /// <param name="value">The PlazaGroup.</param>
                    /// <returns>Returns Plazas by PlazaGroup.</returns>
                    public static NRestResult<List<Models.Plaza>> ByPlazaGroup(Models.PlazaGroup value)
                    {
                        var ret = Execute<List<Models.Plaza>>(
                            RouteConsts.Infrastructure.Plaza.Search.ByPlazaGroup.Url, value);
                        return ret;
                    }
                }
            }
        }
    }
}
