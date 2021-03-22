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
            /// <summary>Gets route url.</summary>
            public const string Url = @"api";

            /// <summary>The Coupon class.</summary>
            public static partial class Coupon 
            {                
                /// <summary>Gets route url.</summary>
                public const string Url = TAxTOD.Url + @"/users/coupons";
            }

            /// <summary>The Account Credit class.</summary>
            public static partial class Credit
            {
                /// <summary>Gets route url.</summary>
                public const string Url = TAxTOD.Url + @"/account";

                /// <summary>The Account TSB Credit class.</summary>
                public static partial class TSB { }

                /// <summary>The Account User Credit class.</summary>
                public static partial class User { }
            }

            /// <summary>The TAA class.</summary>
            public static partial class TAA
            {
                /// <summary>Gets route url.</summary>
                public const string Url = TAxTOD.Url + @"/taa";
            }

            /// <summary>The TCT class.</summary>
            public static partial class TCT { }
        }
    }
}
