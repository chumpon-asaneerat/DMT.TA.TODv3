namespace DMT
{
    static partial class RouteConsts
    {
        partial class TOD
        {
            /// <summary>The Shift Controller.</summary>
            public static partial class Shift
            {
                /// <summary>Gets controller name.</summary>
                public const string ControllerName = "TODShift";

                /// <summary>Gets route name.</summary>
                public const string Name = "Shift";
                /// <summary>Gets base controller url.</summary>
                public const string Url = TOD.Url + @"/" + Name;

                // Url: api/todshift/tsb
                /// <summary>The TOD TSB Shift Controller.</summary>
                public static partial class TSB 
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TODTSBShiftManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "TSB";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Shift.Url + @"/" + Name;
                }

                // Url: api/todshift/user
                /// <summary>The TOD User Shift Controller.</summary>
                public static partial class User 
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TODUserShiftManage";

                    /// <summary>Gets route name.</summary>
                    public const string Name = "User";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Shift.Url + @"/" + Name;
                }
            }
        }
    }
}
