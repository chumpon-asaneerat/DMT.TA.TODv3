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
                    static partial class Transaction
                    {
                        /// <summary>The Save User Credit Transaction(s) action.</summary>
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
}
