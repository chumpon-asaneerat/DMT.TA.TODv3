#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;

#endregion

namespace DMT
{
    static partial class RouteConsts
    {
        /// <summary>The TAxTOD class.</summary>
        public static partial class TAxTOD
        {
            static partial class TOD
            {
                /// <summary>The TSB Shift Class.</summary>
                public static partial class TSBShift
                {
                    // Url: /api/tod/tsbshift/save
                    /// <summary>The Save Class.</summary>
                    public static partial class Save
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Save";
                        /// <summary>Gets route url.</summary>
                        public const string Url = @"/api/tod/tsbshift/save";
                    }
                }
                /// <summary>The User Shift Class.</summary>
                public static partial class UserShift
                {
                    // Url: /api/tod/usershift/save
                    /// <summary>The Save Class.</summary>
                    public static partial class Save
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Save";
                        /// <summary>Gets route url.</summary>
                        public const string Url = @"/api/tod/usershift/save";
                    }
                }
                /// <summary>The Revenue Entry Save Class.</summary>
                public static partial class RevenueEntry
                {
                    // Url: /api/tod/revenueentry/save
                    /// <summary>The Save Class.</summary>
                    public static partial class Save
                    {
                        /// <summary>Gets route name.</summary>
                        public const string Name = "Save";
                        /// <summary>Gets route url.</summary>
                        public const string Url = @"/api/ta/revenueentry/save";
                    }
                }
            }
        }
    }
}
