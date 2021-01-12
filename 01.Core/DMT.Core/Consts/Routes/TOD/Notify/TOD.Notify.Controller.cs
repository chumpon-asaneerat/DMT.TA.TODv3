namespace DMT
{
    // Url: api/tod/Notify
    static partial class RouteConsts
    {
        /// <summary>The TOD class.</summary>
        public static partial class TOD
        {
            /// <summary>Gets name.</summary>
            public const string Name = "TOD";
            public const string Url = RouteConsts.Url + @"/" + Name;

            /// <summary>The Notify Controller.</summary>
            public static partial class Notify
            {
                /// <summary>Gets controller name.</summary>
                public const string ControllerName = "TODNotify";

                /// <summary>Gets base controller url.</summary>
                public const string Url = TOD.Url + @"/" + ControllerName;
            }
        }
    }
}
