namespace DMT
{
    // Url : api/coupon/tsb/transaction/gets
    static partial class RouteConsts
    {
        static partial class Coupon
        {
            static partial class TSB
            {
                static partial class Transaction
                {
                    /// <summary>The Gets TSB Coupon Transaction(s) action.</summary>
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
