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
                /// <summary>Gets base controller url.</summary>
                public const string Url = TOD.Url + @"/" + ControllerName;

                /// <summary>The TOD TSB Shift Controller.</summary>
                public static partial class TSB 
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TODTSBShift";
                    /// <summary>Gets base controller url.</summary>
                    public const string Url = Shift.Url + @"/" + ControllerName;
                }

                /// <summary>The TOD User Shift Controller.</summary>
                public static partial class User 
                {
                    /// <summary>Gets controller name.</summary>
                    public const string ControllerName = "TODUserShift";
                    /// <summary>Gets base controller url.</summary>
                    public const string Url = Shift.Url + @"/" + ControllerName;
                }
            }
        }
    }
}
