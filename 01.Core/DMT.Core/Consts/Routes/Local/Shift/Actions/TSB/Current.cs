namespace DMT
{
    // Url : api/shift/tsb/current
    static partial class RouteConsts
    {
        static partial class Shift
        {
            static partial class TSB
            {
                /// <summary>The Gets Current TSB Shift action.</summary>
                public static class Current
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "Current";
                    /// <summary>Gets route url.</summary>
                    public const string Url = TSB.Url + @"/" + Name;
                }
            }
        }
    }
}
