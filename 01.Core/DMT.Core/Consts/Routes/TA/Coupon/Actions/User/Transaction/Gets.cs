namespace DMT
{
    static partial class RouteConsts
    {
        static partial class TA
        {
            static partial class Coupon
            {
                static partial class User
                {
                    static partial class Transaction
                    {
                        /// <summary>The Gets User Coupon Transaction(s) action.</summary>
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
}
