#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;

#endregion

namespace DMT
{
    static partial class RouteConsts
    {
        /// <summary>The SCW class.</summary>
        public static partial class SCW
        {
            /// <summary>Gets route url.</summary>
            public const string Url = @"dmt-scw/api/tod";

            /// <summary>The Master class.</summary>
            public static partial class Master
            {
                /// <summary>Gets route url.</summary>
                public const string Url = SCW.Url;
            }

            /// <summary>The Security class.</summary>
            public static partial class Security
            {
                /// <summary>Gets route url.</summary>
                public const string Url = SCW.Url;
            }

            /// <summary>The TOD class.</summary>
            public static partial class TOD
            {
                /// <summary>Gets route url.</summary>
                public const string Url = SCW.Url;
            }

            /// <summary>The Emulator class.</summary>
            public static partial class Emulator
            {
                /// <summary>Gets route url.</summary>
                public const string Url = @"dmt-scw/api/emu";
            }
        }
    }
}
