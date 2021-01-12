namespace DMT
{
    // Url: api/infrastructure
    static partial class RouteConsts
    {
        /// <summary>The Infrastructure class.</summary>
        public static partial class Infrastructure
        {
            /// <summary>Gets route name.</summary>
            public const string Name = "Infrastructure";
            /// <summary>Gets base controller url.</summary>
            public const string Url = RouteConsts.Url + @"/" + Name;

            // Url : api/infrastructure/tsb
            /// <summary>The Infrastructure's TSB Controller.</summary>
            public static partial class TSB 
            {
                /// <summary>Gets controller name.</summary>
                public const string ControllerName = "InfrastructureTSB";

                /// <summary>Gets route name.</summary>
                public const string Name = "TSB";
                /// <summary>Gets route url.</summary>
                public const string Url = Infrastructure.Url + @"/" + Name;
            }

            // Url : api/infrastructure/plazagroup
            /// <summary>The Infrastructure's PlazaGroup Controller.</summary>
            public static partial class PlazaGroup 
            {
                /// <summary>Gets controller name.</summary>
                public const string ControllerName = "InfrastructurePlazaGroup";

                /// <summary>Gets route name.</summary>
                public const string Name = "PlazaGroup";
                /// <summary>Gets route url.</summary>
                public const string Url = Infrastructure.Url + @"/" + Name;
            }

            // Url : api/infrastructure/plaza
            /// <summary>The Infrastructure's Plaza Controller.</summary>
            public static partial class Plaza 
            {
                /// <summary>Gets controller name.</summary>
                public const string ControllerName = "InfrastructurePlaza";

                /// <summary>Gets route name.</summary>
                public const string Name = "Plaza";
                /// <summary>Gets route url.</summary>
                public const string Url = Infrastructure.Url + @"/" + Name;
            }

            // Url : api/infrastructure/lane
            /// <summary>The Infrastructure's Lane Controller.</summary>
            public static partial class Lane 
            {
                /// <summary>Gets controller name.</summary>
                public const string ControllerName = "InfrastructureLane";

                /// <summary>Gets route name.</summary>
                public const string Name = "Lane";
                /// <summary>Gets route url.</summary>
                public const string Url = Infrastructure.Url + @"/" + Name;
            }
        }
    }
}
