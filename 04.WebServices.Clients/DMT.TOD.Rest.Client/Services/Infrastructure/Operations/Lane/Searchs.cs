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
                static partial class Search
                {
                    /// <summary>
                    /// Gets Plazas By TSB.
                    /// </summary>
                    /// <param name="value">The TSB.</param>
                    /// <returns>Returns Lanes by TSB.</returns>
                    public static NRestResult<List<Models.Lane>> ByTSB(Models.TSB value)
                    {
                        var ret = Execute<List<Models.Lane>>(
                            RouteConsts.TOD.Infrastructure.Lane.Search.ByTSB.Url, value);
                        return ret;
                    }
                    /// <summary>
                    /// Gets Plazas By PlazaGroup.
                    /// </summary>
                    /// <param name="value">The PlazaGroup.</param>
                    /// <returns>Returns Lanes by PlazaGroup.</returns>
                    public static NRestResult<List<Models.Lane>> ByPlazaGroup(Models.PlazaGroup value)
                    {
                        var ret = Execute<List<Models.Lane>>(
                            RouteConsts.TOD.Infrastructure.Lane.Search.ByPlazaGroup.Url, value);
                        return ret;
                    }
                    /// <summary>
                    /// Gets Plazas By Plaza.
                    /// </summary>
                    /// <param name="value">The Plaza.</param>
                    /// <returns>Returns Lanes by Plaza.</returns>
                    public static NRestResult<List<Models.Lane>> ByPlaza(Models.Plaza value)
                    {
                        var ret = Execute<List<Models.Lane>>(
                            RouteConsts.TOD.Infrastructure.Lane.Search.ByPlaza.Url, value);
                        return ret;
                    }
                }
            }
        }
    }
}
