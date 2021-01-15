namespace DMT
{
    static partial class RouteConsts
    {
        partial class TA
        {
            /// <summary>The Shift Controller.</summary>
            public static partial class Shift
            {
                /// <summary>Gets controller name.</summary>
                public const string ControllerName = "Shift";
                /// <summary>Gets base controller url.</summary>
                public const string Url = TA.Url + @"/" + ControllerName;

                // Url: api/shift/tsb
                /// <summary>The Shift's TSB Controller.</summary>
                public static partial class TSB
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TSBShiftManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "TSB";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Shift.Url + @"/" + Name;
                }

                // Url: api/shift/user
                /// <summary>The Shift's User Controller.</summary>
                public static partial class User
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "UserShiftManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "TSB";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Shift.Url + @"/" + Name;

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
