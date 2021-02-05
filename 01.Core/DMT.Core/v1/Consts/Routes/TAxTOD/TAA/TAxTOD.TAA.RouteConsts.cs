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
            static partial class TAA
            {
                // Url: /api/taa/IsAlive
                /// <summary>The Save Class.</summary>
                public static partial class IsAlive
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "isalive";
                    /// <summary>Gets route url.</summary>
                    public const string Url = TAA.Url + @"/" + Name;
                }
            }
        }
    }
}
