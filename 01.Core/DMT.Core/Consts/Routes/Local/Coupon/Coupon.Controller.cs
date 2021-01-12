namespace DMT
{
    // Url: api/coupon
    static partial class RouteConsts
    {
        /// <summary>The Coupon class.</summary>
        public static partial class Coupon
        {
            /// <summary>Gets route name.</summary>
            public const string Name = "Coupon";
            /// <summary>Gets base controller url.</summary>
            public const string Url = RouteConsts.Url + @"/" + Name;

            // Url : api/coupon/tsb
            /// <summary>The Coupon's TSB Controller.</summary>
            public static partial class TSB
            {
                /// <summary>Gets controller name.</summary>
                public const string ControllerName = "TSBCouponManage";

                /// <summary>Gets route name.</summary>
                public const string Name = "TSB";
                /// <summary>Gets route url.</summary>
                public const string Url = Coupon.Url + @"/" + Name;

                // Url : api/coupon/tsb/transaction
                /// <summary>The Coupon's TSB Transaction Controller.</summary>
                public static partial class Transaction
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TSBCouponTransactionManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "Transaction";
                    /// <summary>Gets route url.</summary>
                    public const string Url = TSB.Url + @"/" + Name;
                }
            }

            // Url : api/coupon/user
            /// <summary>The Coupon's User Controller.</summary>
            public static partial class User
            {
                /// <summary>Gets controller name.</summary>
                public const string ControllerName = "UserCouponManage";

                /// <summary>Gets route name.</summary>
                public const string Name = "User";
                /// <summary>Gets route url.</summary>
                public const string Url = Coupon.Url + @"/" + Name;

                // Url : api/coupon/user/transaction
                /// <summary>The Coupon's User Transaction Controller.</summary>
                public static partial class Transaction
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "UserCouponTransactionManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "Transaction";
                    /// <summary>Gets route url.</summary>
                    public const string Url = User.Url + @"/" + Name;
                }
            }
        }
    }
}
