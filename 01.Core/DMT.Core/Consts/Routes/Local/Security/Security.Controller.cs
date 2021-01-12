namespace DMT
{
    // Url: api/security
    static partial class RouteConsts
    {
        /// <summary>The Security Controller.</summary>
        public static partial class Security
        {
            /// <summary>Gets controller name.</summary>
            public const string Name = "Security";
            /// <summary>Gets base controller url.</summary>
            public const string Url = RouteConsts.Url + @"/" + Name;

            // Url : api/security/role
            /// <summary>The Security's Role Controller.</summary>
            public static partial class Role
            {
                /// <summary>Gets controller name.</summary>
                public const string ControllerName = "RoleManage";

                /// <summary>Gets route name.</summary>
                public const string Name = "Role";
                /// <summary>Gets route url.</summary>
                public const string Url = Security.Url + @"/" + Name;
            }

            // Url : api/security/user
            /// <summary>The Security's User Controller.</summary>
            public static partial class User
            {
                /// <summary>Gets controller name.</summary>
                public const string ControllerName = "UserManage";

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
