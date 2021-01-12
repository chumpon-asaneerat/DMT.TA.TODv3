namespace DMT
{
    // Url : api/infrastructure/tsb/setactive
    static partial class RouteConsts
    {
        static partial class Infrastructure
        {
            static partial class TSB
            {
                /// <summary>The Set Current (Active) TSB action.</summary>
                public static class SetActive
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "SetActive";
                    /// <summary>Gets route url.</summary>
                    public const string Url = TSB.Url + @"/" + Name;
                }
            }
        }
    }
}
