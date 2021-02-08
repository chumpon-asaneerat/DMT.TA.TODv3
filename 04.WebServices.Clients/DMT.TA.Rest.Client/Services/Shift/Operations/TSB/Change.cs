#region Usings

using System.Collections.Generic;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
    {
        static partial class Shift
        {
            static partial class TSB
            {
                /// <summary>
                /// Gets Change TSB Shift.
                /// </summary>
                /// <returns>Returns Current TSB Shift instance.</returns>
                public static NRestResult Change()
                {
                    var ret = Execute(
                        RouteConsts.TA.Shift.TSB.Change.Url);
                    return ret;
                }
            }
        }
    }
}
