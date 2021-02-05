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
            static partial class Security 
            {
                // Url: dmt-scw/api/tod/loginAudit
                /// <summary>The loginAudit Class.</summary>
                public static partial class loginAudit
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "loginAudit";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Security.Url + @"/" + Name;
                }

                // Url: dmt-scw/api/tod/changePassword
                /// <summary>The changePassword Class.</summary>
                public static partial class changePassword
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "changePassword";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Security.Url + @"/" + Name;
                }

                // Url: dmt-scw/api/tod/passwordExpiresDays
                /// <summary>The passwordExpiresDays Class.</summary>
                public static partial class passwordExpiresDays
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "passwordExpiresDays";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Security.Url + @"/" + Name;
                }
            }
        }
    }
}
