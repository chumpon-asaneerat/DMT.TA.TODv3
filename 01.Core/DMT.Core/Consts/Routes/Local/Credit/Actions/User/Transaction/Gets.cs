namespace DMT
{
    // Url : api/credit/user/transaction/gets
    static partial class RouteConsts
    {
        static partial class Credit
        {
            static partial class User
            {
                static partial class Transaction
                {
                    /// <summary>The Gets User Credit Transaction(s) action.</summary>
                    public static class Gets
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Gets";
                        /// <summary>Gets route url.</summary>
                        public const string Url = Transaction.Url + @"/" + Name;
                    }
                }
            }
        }
    }
}
