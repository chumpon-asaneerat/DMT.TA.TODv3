#region Usings

using System.Collections.Generic;
using DMT.Configurations;
using DMT.Services;

#endregion

namespace DMT.Services.Operations
{
    static partial class TA
    {
        static partial class Notify
        {
            /// <summary>
            /// Register.
            /// </summary>
            /// <param name="value">The TODAppWebServiceConfig instance.</param>
            /// <returns>Returns NRestResult instance.</returns>
            public static NRestResult Register(TODAppWebServiceConfig value)
            {
                var ret = Execute(RouteConsts.TA.Notify.Register.Url, value);
                return ret;
            }
        }
    }
}
