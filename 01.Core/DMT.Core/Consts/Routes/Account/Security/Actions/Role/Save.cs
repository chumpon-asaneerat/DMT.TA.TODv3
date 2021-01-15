namespace DMT
{
    static partial class RouteConsts
    {
        static partial class Account
        {
            static partial class Security
            {
                static partial class Role
                {
                    /// <summary>The Save Role action.</summary>
                    public static class Save
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Save";
                        /// <summary>Gets route url.</summary>
                        public const string Url = Role.Url + @"/" + Name;
                    }
                }
            }
        }
    }
}
