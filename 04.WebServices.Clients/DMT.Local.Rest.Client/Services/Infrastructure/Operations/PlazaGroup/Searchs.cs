#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class Plaza
    {
        static partial class Infrastructure
        {
            static partial class PlazaGroup
            {
                static partial class Search
                {
                    /// <summary>
                    /// Gets PlazaGroups By TSB.
                    /// </summary>
                    /// <param name="value">The TSB.</param>
                    /// <returns>Returns PlazaGroups by TSB.</returns>
                    public static NRestResult<List<Models.PlazaGroup>> ByTSB(Models.TSB value)
                    {
                        var ret = Execute<List<Models.PlazaGroup>>(
                            RouteConsts.Infrastructure.PlazaGroup.Search.ByTSB.Url, value);
                        return ret;
                    }
                }
            }
        }
    }
}
