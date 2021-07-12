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
            static partial class TCT
            {
                // Url: /api/users/coupons/search
                /// <summary>The Get User Coupons Class.</summary>
                public static partial class GetUserCoupons
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "UserCoupons";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/users/coupons/search";
                }

                // Url: /api/TCT/coupons/sold
                /// <summary>The Sold Coupon Class.</summary>
                public static partial class SoldCoupon
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "SoldCoupon";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/TCT/coupons/sold";
                }

                // Url: /api/TCT/CheckTODBoj
                /// <summary>The Check TOD Boj Class.</summary>
                public static partial class CheckTODBoj
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "CheckTODBoj";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/TCT/CheckTODBoj";
                }
            }
        }
    }
}
