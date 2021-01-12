namespace DMT
{
    // Url : api/shift/tsb/changeshift
    static partial class RouteConsts
    {
        static partial class Shift
        {
            static partial class TSB
            {
                /// <summary>The Change TSB Shift action.</summary>
                public static class Change
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "Change";
                    /// <summary>Gets route url.</summary>
                    public const string Url = TSB.Url + @"/" + Name;
                }
            }
        }
    }
}
