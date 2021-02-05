namespace DMT
{
    static partial class RouteConsts
    {
        static partial class TOD
        {
            static partial class Security
            {
                static partial class User
                {
                    static partial class Search
                    {
                        /// <summary>The Gets User by UserId action.</summary>
                        public static class ById
                        {
                            /// <summary>Gets route name.</summary>
                            public const string Name = "ById";
                            /// <summary>Gets route url.</summary>
                            public const string Url = Search.Url + @"/" + Name;
                        }

                        /// <summary>The Gets User by CardId action.</summary>
                        public static class ByCardId
                        {
                            /// <summary>Gets route name.</summary>
                            public const string Name = "ByCardId";
                            /// <summary>Gets route url.</summary>
                            public const string Url = Search.Url + @"/" + Name;
                        }

                        /// <summary>The Gets User by LogIn action.</summary>
                        public static class ByLogIn
                        {
                            /// <summary>Gets route name.</summary>
                            public const string Name = "ByLogIn";
                            /// <summary>Gets route url.</summary>
                            public const string Url = Search.Url + @"/" + Name;
                        }

                        /// <summary>The Gets Users by RoleId action.</summary>
                        public static class ByRoleId
                        {
                            /// <summary>Gets route name.</summary>
                            public const string Name = "ByRoleId";
                            /// <summary>Gets route url.</summary>
                            public const string Url = Search.Url + @"/" + Name;
                        }

                        /// <summary>The Gets Users by GroupId action.</summary>
                        public static class ByGroupId
                        {
                            /// <summary>Gets route name.</summary>
                            public const string Name = "ByGroupId";
                            /// <summary>Gets route url.</summary>
                            public const string Url = Search.Url + @"/" + Name;
                        }

                        /// <summary>The Gets Users filter by User Id (increment search) and roles action.</summary>
                        public static class ByFilter
                        {
                            /// <summary>Gets route name.</summary>
                            public const string Name = "ByFilter";
                            /// <summary>Gets route url.</summary>
                            public const string Url = Search.Url + @"/" + Name;
                        }
                    }
                }
            }
        }
    }
}
