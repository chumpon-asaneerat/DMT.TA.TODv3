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
            static partial class SAP
            {
                // Url: /api/account/sap/getcustomer
                /// <summary>The Get Customers Class.</summary>
                public static partial class GetCustomers
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "GetCustomers";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/sap/getcustomer";
                }

                // Url: /api/account/sap/tsblist
                /// <summary>The Get TSB List Class.</summary>
                public static partial class GetTSBs
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "GetTSBs";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/sap/tsblist";
                }

                // Url: /api/account/sap/couponsoldlist
                /// <summary>The Get Coupon Sold List Class.</summary>
                public static partial class GetCouponSolds
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "GetCouponSolds";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/sap/couponsoldlist";
                }

                // Url: /api/account/sap/save/ar
                /// <summary>The Save (insert) AR Head and related ARLine, ARSerial in one call.</summary>
                public static partial class SaveAR
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "SaveAR";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/sap/save/ar";
                }

                //------------------------------------------------------------------------------
                // Note: Below route use SaveAR instead.
                //
                // Url: /api/account/sap/save/arhead
                /// <summary>The Save (insert) AR Head Class. Note. Use SaveAR instead.</summary>
                public static partial class SaveARHead
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "SaveARHead";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/sap/save/arhead";
                }
                // Url: /api/account/sap/couponsoldlist
                /// <summary>The Get AR Head List Class. Note. Use SaveAR instead.</summary>
                public static partial class GetARHeads
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "GetARHeads";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/sap/couponsoldlist";
                }

                // Url: /api/account/sap/get/arsumcoupon
                /// <summary>The Get AR Sum Coupon List Class. Note. Use SaveAR instead.</summary>
                public static partial class GetARSumCoupons
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "GetARSumCoupons";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/sap/get/arsumcoupon";
                }
                // Url: /api/account/sap/save/arline
                /// <summary>The Save (insert) AR Line Class. Note. Use SaveAR instead.</summary>
                public static partial class SaveARLine
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "SaveARLine";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/sap/save/arline";
                }

                // Url: /api/account/sap/get/arcouponlist
                /// <summary>The Get AR Coupon List Class. Note. Use SaveAR instead.</summary>
                public static partial class GetARCoupons
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "GetARCoupons";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/sap/get/arcouponlist";
                }
                // Url: /api/account/sap/save/arserial
                /// <summary>The Save (insert) AR Serial Class. Note. Use SaveAR instead.</summary>
                public static partial class SaveARSerial
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "SaveARSerial";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/sap/save/arserial";
                }
                //------------------------------------------------------------------------------
            }
        }
    }
}
