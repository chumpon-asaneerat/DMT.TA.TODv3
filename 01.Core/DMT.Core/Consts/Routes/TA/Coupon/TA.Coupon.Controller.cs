namespace DMT
{
    static partial class RouteConsts
    {
        static partial class TA
        {
            /// <summary>The Coupon class.</summary>
            public static partial class Coupon
            {
                /// <summary>Gets route name.</summary>
                public const string Name = "Coupon";
                /// <summary>Gets base controller url.</summary>
                public const string Url = TA.Url + @"/" + Name;

                /// <summary>The Coupon's TSB Controller.</summary>
                public static partial class TSB
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TSBCouponManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "TSB";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Coupon.Url + @"/" + Name;
                }

                /// <summary>The Coupon's User Controller.</summary>
                public static partial class User
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "UserCouponManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "User";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Coupon.Url + @"/" + Name;
                }
            }
        }
    }
}
