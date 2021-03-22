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
            }

            static partial class Credit
            {
                static partial class TSB
                {
                    // Url: /api/account/tsbcredit/save
                    /// <summary>The Account TSB Credit Save Class.</summary>
                    public static partial class Save
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Save";
                        /// <summary>Gets route url.</summary>
                        public const string Url = @"/api/account/tsbcredit/save";
                    }

                    // Url: /api/account/tsbcredit
                    /// <summary>The Account TSB Credit Gets (Search) Class.</summary>
                    public static partial class Gets
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Gets";
                        /// <summary>Gets route url.</summary>
                        public const string Url = @"/api/account/tsbcredit";
                    }
                }

                static partial class User
                {
                    // Url: /api/account/UserCredit/save
                    /// <summary>The Account User Credit Save Class.</summary>
                    public static partial class Save
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Save";
                        /// <summary>Gets route url.</summary>
                        public const string Url = @"/api/account/UserCredit/save";
                    }

                    // Url: /api/account/usercredit/search
                    /// <summary>The Account User Credit Gets (Search) Class.</summary>
                    public static partial class Gets
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Gets";
                        /// <summary>Gets route url.</summary>
                        public const string Url = @"/api/account/usercredit/search";
                    }
                }
            }
        }
    }
}
