#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;

#endregion

namespace DMT
{
    static partial class RouteConsts
    {
        /// <summary>The TAxTOD class.</summary>
        public static partial class TAxTOD
        {
            static partial class TACredit
            {
                // Url: /api/account/tsbcredit/save
                /// <summary>The TA Credit Low Limit Save Class.</summary>
                public static partial class Save
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "Save";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/ta/creditlowlimit/save";
                }

                // Url: /api/account/tsbcredit
                /// <summary>The TA Credit Low Limit Gets (Search) Class.</summary>
                public static partial class Gets
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "Gets";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/ta/creditlowlimit/get";
                }
            }
        }
    }
}
