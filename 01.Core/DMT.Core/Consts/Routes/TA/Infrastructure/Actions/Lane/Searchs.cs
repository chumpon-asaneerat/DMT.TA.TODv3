namespace DMT
{
    static partial class RouteConsts
    {
        static partial class TA
        {
            static partial class Infrastructure
            {
                static partial class Lane
                {
                    static partial class Search
                    {
                        /// <summary>The Gets Lane by TSB action.</summary>
                        public static class ByTSB
                        {
                            /// <summary>Gets route name.</summary>
                            public const string Name = "ByTSB";
                            /// <summary>Gets route url.</summary>
                            public const string Url = Search.Url + @"/" + Name;
                        }

                        /// <summary>The Gets Lane by PlazaGroup action.</summary>
                        public static class ByPlazaGroup
                        {
                            /// <summary>Gets route name.</summary>
                            public const string Name = "ByPlazaGroup";
                            /// <summary>Gets route url.</summary>
                            public const string Url = Search.Url + @"/" + Name;
                        }

                        /// <summary>The Gets Lane by Plaza action.</summary>
                        public static class ByPlaza
                        {
                            /// <summary>Gets route name.</summary>
                            public const string Name = "ByPlaza";
                            /// <summary>Gets route url.</summary>
                            public const string Url = Search.Url + @"/" + Name;
                        }
                    }
                }
            }
        }
    }
}
