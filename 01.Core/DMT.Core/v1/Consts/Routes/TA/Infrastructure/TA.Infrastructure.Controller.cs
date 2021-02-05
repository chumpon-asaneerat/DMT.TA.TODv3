namespace DMT
{
    static partial class RouteConsts
    {
        partial class TA 
        {
            /// <summary>The Infrastructure class.</summary>
            public static partial class Infrastructure
            {
                /// <summary>Gets route name.</summary>
                public const string Name = "Infrastructure";
                /// <summary>Gets base controller url.</summary>
                public const string Url = TA.Url + @"/" + Name;

                /// <summary>The Infrastructure's TSB Controller.</summary>
                public static partial class TSB
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TAATSBManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "TSB";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Infrastructure.Url + @"/" + Name;
                }

                /// <summary>The Infrastructure's PlazaGroup Controller.</summary>
                public static partial class PlazaGroup
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TAAPlazaGroupManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "PlazaGroup";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Infrastructure.Url + @"/" + Name;

                    /// <summary>The Seacch class.</summary>
                    public static partial class Search
                    {
                        /// <summary>Gets route url.</summary>
                        public const string Url = PlazaGroup.Url + @"/Search";
                    }
                }

                /// <summary>The Infrastructure's Plaza Controller.</summary>
                public static partial class Plaza
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TAAPlazaManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "Plaza";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Infrastructure.Url + @"/" + Name;

                    /// <summary>The Seacch class.</summary>
                    public static partial class Search
                    {
                        /// <summary>Gets route url.</summary>
                        public const string Url = Plaza.Url + @"/Search";
                    }
                }

                /// <summary>The Infrastructure's Lane Controller.</summary>
                public static partial class Lane
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TAALaneManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "Lane";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Infrastructure.Url + @"/" + Name;

                    /// <summary>The Seacch class.</summary>
                    public static partial class Search
                    {
                        /// <summary>Gets route url.</summary>
                        public const string Url = Lane.Url + @"/Search";
                    }
                }
            }
        }
    }
}
