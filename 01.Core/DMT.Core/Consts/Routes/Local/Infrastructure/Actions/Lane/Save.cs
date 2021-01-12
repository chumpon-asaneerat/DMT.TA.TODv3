namespace DMT
{
    // Url : api/infrastructure/lane/save
    static partial class RouteConsts
    {
        static partial class Infrastructure
        {
            static partial class Lane
            {
                /// <summary>The Save Lane action.</summary>
                public static class Save
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "Save";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Lane.Url + @"/" + Name;
                }
            }
        }
    }
}
