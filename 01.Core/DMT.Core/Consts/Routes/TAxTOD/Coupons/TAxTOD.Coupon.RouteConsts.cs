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
            static partial class Coupon
            {
                // Url: /api/users/coupons/Save
                /// <summary>The Save Class.</summary>
                public static partial class Save
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "save";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Coupon.Url + @"/" + Name;
                }

                // Url: /api/users/coupons/getlist2
                /// <summary>The Gets Class.</summary>
                public static partial class Gets
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "getlist2";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Coupon.Url + @"/" + Name;
                }

                // Url: /api/TA/coupons/updatereceive
                /// <summary>The Received Class.</summary>
                public static partial class Received
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "Received";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Coupon.Url + @"/" + Name;
                }
            }
        }
    }
}
