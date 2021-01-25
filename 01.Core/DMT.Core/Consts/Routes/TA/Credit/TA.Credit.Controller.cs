namespace DMT
{
    static partial class RouteConsts
    {
        static partial class TA
        {
            /// <summary>The Credit class.</summary>
            public static partial class Credit
            {
                /// <summary>Gets route name.</summary>
                public const string Name = "Credit";
                /// <summary>Gets base controller url.</summary>
                public const string Url = TA.Url + @"/" + Name;

                /// <summary>The Credit's TSB Controller.</summary>
                public static partial class TSB
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TSBCreditManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "TSB";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Credit.Url + @"/" + Name;
                }

                /// <summary>The Credit's User Controller.</summary>
                public static partial class User
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "UserCreditManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "User";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Credit.Url + @"/" + Name;
                }
            }
        }
    }
}
