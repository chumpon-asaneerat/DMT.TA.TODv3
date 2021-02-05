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
                    /// <summary>The Gets Completed User Credit Balance action.</summary>
                    public static class Completed
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Completed";
                        /// <summary>Gets route url.</summary>
                        public const string Url = User.Url + @"/" + Name;
                    }
                }
            }
        }
    }
}
