namespace DMT
{
    // Url : api/credit/tsb/transaction/save
    static partial class RouteConsts
    {
        static partial class Credit
        {
            static partial class TSB
            {
                static partial class Transaction
                {
                    /// <summary>The Save TSB Credit Transaction(s) action.</summary>
                    public static class Save
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Save";
                        /// <summary>Gets route url.</summary>
                        public const string Url = Transaction.Url + @"/" + Name;
                    }
                }
            }
        }
    }
}
