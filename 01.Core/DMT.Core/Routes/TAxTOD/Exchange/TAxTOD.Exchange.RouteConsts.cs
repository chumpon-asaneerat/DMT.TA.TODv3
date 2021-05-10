using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMT
{
    static partial class RouteConsts
    {
        /// <summary>The TAxTOD class.</summary>
        public static partial class TAxTOD
        {
            static partial class Exchange
            {
                // Url: /api/account/request/getlist
                /// <summary>The Request Exchange Gets (Search) Class.</summary>
                public static partial class Gets
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "Gets";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/request/getlist";
                }
            }
        }
    }
}
