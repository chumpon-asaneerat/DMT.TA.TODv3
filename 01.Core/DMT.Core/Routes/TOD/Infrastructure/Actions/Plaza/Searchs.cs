namespace DMT
{
    static partial class RouteConsts
    {
        static partial class TOD
        {
            static partial class Infrastructure
            {
                static partial class Plaza
                {
                    static partial class Search
                    {
                        /// <summary>The Gets Plaza by TSB action.</summary>
                        public static class ByTSB
                        {
                            /// <summary>Gets route name.</summary>
                            public const string Name = "ByTSB";
                            /// <summary>Gets route url.</summary>
                            public const string Url = Search.Url + @"/" + Name;
                        }

                        /// <summary>The Gets Plaza by PlazaGroup action.</summary>
                        public static class ByPlazaGroup
                        {
                            /// <summary>Gets route name.</summary>
                            public const string Name = "ByPlazaGroup";
                            /// <summary>Gets route url.</summary>
                            public const string Url = Search.Url + @"/" + Name;
                        }
                    }
                }
            }
        }
    }
}
