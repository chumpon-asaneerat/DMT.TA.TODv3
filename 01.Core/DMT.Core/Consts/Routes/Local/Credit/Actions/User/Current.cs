namespace DMT
{
    // Url : api/credit/user/gets
    static partial class RouteConsts
    {
        static partial class Credit
        {
            static partial class User
            {
                /// <summary>The Gets Current User Credit Balance action.</summary>
                public static class Current
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "Current";
                    /// <summary>Gets route url.</summary>
                    public const string Url = User.Url + @"/" + Name;
                }
            }
        }
    }
}
