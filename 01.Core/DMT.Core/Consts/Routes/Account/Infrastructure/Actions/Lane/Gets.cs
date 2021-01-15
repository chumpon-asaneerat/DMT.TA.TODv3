namespace DMT
{
    static partial class RouteConsts
    {
        static partial class Account
        {
            static partial class Infrastructure
            {
                static partial class Lane
                {
                    /// <summary>The Gets all Lanes action.</summary>
                    public static class Gets
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Gets";
                        /// <summary>Gets route url.</summary>
                        public const string Url = Lane.Url + @"/" + Name;
                    }
                }
            }
        }
    }
}
