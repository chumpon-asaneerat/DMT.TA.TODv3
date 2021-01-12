#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;

#endregion

namespace DMT
{
    static partial class RouteConsts
    {
        static partial class SCW
        {
            static partial class Master 
            {
                // Url: dmt-scw/api/tod/currencyDenomList
                /// <summary>The currencyDenomList Class.</summary>
                public static partial class currencyDenomList
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "currencyDenomList";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Master.Url + @"/" + Name;
                }

                // Url: dmt-scw/api/tod/couponList
                /// <summary>The couponList Class.</summary>
                public static partial class couponList
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "couponList";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Master.Url + @"/" + Name;
                }

                // Url: dmt-scw/api/tod/couponBookList
                /// <summary>The couponBookList Class.</summary>
                public static partial class couponBookList
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "couponBookList";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Master.Url + @"/" + Name;
                }

                // Url: dmt-scw/api/tod/cardAllowList
                /// <summary>The cardAllowList Class.</summary>
                public static partial class cardAllowList
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "cardAllowList";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Master.Url + @"/" + Name;
                }
            }
        }
    }
}
