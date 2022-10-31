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
                    public const string Url = @"/api/users/coupons/save";
                }

                // Url: /api/users/coupons/getlist2
                /// <summary>The Gets Class.</summary>
                public static partial class Gets
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "getlist2";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/users/coupons/getlist2";
                }

                // Url: /api/TA/coupons/updatereceive
                /// <summary>The Received Class.</summary>
                public static partial class Received
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "Received";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/TA/coupons/updatereceive";
                }

                // Url: /api/TA/coupons/inquiry
                /// <summary>The Inquiry Class.</summary>
                public static partial class Inquiry
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "Inquiry";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/TA/coupons/inquiry";
                }

                // Url: /api/TA/coupons/editserialno
                /// <summary>The EditSerialNo Class.</summary>
                public static partial class EditSerialNo
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "EditSerialNo";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/TA/coupons/editserialno";
                }

            }
        }
    }
}
