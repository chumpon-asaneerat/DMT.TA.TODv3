﻿namespace DMT
{
    // Url : api/credit/tsb/gets
    static partial class RouteConsts
    {
        static partial class Credit
        {
            static partial class TSB
            {
                /// <summary>The Gets Current TSB Credit Balance action.</summary>
                public static class Current
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "Current";
                    /// <summary>Gets route url.</summary>
                    public const string Url = TSB.Url + @"/" + Name;
                }
            }
        }
    }
}
