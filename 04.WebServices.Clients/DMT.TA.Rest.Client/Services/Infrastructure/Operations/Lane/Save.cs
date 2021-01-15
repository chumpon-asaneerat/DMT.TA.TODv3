#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
    {
        static partial class Infrastructure
        {
            static partial class Lane
            {
                /// <summary>
                /// Save Lane.
                /// </summary>
                /// <returns>Returns Saved Lane.</returns>
                public static NRestResult<Models.Lane> Save(Models.Lane value)
                {
                    var ret = Execute<Models.Lane>(
                        RouteConsts.TA.Infrastructure.Lane.Save.Url, value);
                    return ret;
                }
            }
        }
    }
}
