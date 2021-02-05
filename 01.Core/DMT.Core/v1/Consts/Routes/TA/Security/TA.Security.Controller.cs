namespace DMT
{
    static partial class RouteConsts
    {
        partial class TA 
        {
            /// <summary>The Security Controller.</summary>
            public static partial class Security
            {
                /// <summary>Gets controller name.</summary>
                public const string Name = "Security";
                /// <summary>Gets base controller url.</summary>
                public const string Url = TA.Url + @"/" + Name;

                /// <summary>The Security's Role Controller.</summary>
                public static partial class Role
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TAARoleManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "Role";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Security.Url + @"/" + Name;
                }

                /// <summary>The Security's User Controller.</summary>
                public static partial class User
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TAAUserManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "User";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Security.Url + @"/" + Name;

                    /// <summary>The Seacch class.</summary>
                    public static partial class Search
                    {
                        /// <summary>Gets route url.</summary>
                        public const string Url = User.Url + @"/Search";
                    }
                }
            }
        }
    }
}
