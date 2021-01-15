namespace DMT
{
    static partial class RouteConsts
    {
        static partial class Account
        {
            static partial class Infrastructure
            {
                static partial class TSB
                {
                    /// <summary>The Save TSB action.</summary>
                    public static class Save
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Save";
                        /// <summary>Gets route url.</summary>
                        public const string Url = TSB.Url + @"/" + Name;
                    }
                }
            }
        }
    }
}
