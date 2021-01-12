namespace DMT
{
    // Url: api/ta/Notify
    static partial class RouteConsts
    {
        /// <summary>The TA class.</summary>
        public static partial class TA
        {
            /// <summary>Gets name.</summary>
            public const string Name = "TA";
            /// <summary>Gets base url.</summary>
            public const string Url = RouteConsts.Url + @"/" + Name;

            /// <summary>The Notify Controller.</summary>
            public static partial class Notify
            {
                /// <summary>Gets controller name.</summary>
                public const string ControllerName = "TANotify";
                /// <summary>Gets base controller url.</summary>
                public const string Url = TA.Url + @"/" + ControllerName;
            }
        }
    }
}
