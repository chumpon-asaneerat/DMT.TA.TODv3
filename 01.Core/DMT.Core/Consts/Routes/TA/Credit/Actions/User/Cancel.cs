namespace DMT
{
    static partial class RouteConsts
    {
        static partial class TA
        {
            static partial class Credit
            {
                static partial class User
                {
                    /// <summary>The Cancel User Credit Balance action.</summary>
                    public static class Cancel
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Cancel";
                        /// <summary>Gets route url.</summary>
                        public const string Url = User.Url + @"/" + Name;
                    }
                }
            }
        }
    }
}
