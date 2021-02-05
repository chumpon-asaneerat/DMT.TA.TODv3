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
            static partial class Emulator
            {
                // Url: dmt-scw/api/emu/boj
                /// <summary>The boj Class.</summary>
                public static partial class boj
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "boj";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Emulator.Url + @"/" + Name;
                }

                // Url: dmt-scw/api/emu/eoj
                /// <summary>The eoj Class.</summary>
                public static partial class eoj
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "eoj";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Emulator.Url + @"/" + Name;
                }

                // Url: dmt-scw/api/emu/jobs
                /// <summary>The eoj Class.</summary>
                public static partial class allJobs
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "jobs";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Emulator.Url + @"/" + Name;
                }

                // Url: dmt-scw/api/emu/removes
                /// <summary>The removeJobs Class.</summary>
                public static partial class removeJobs
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "removes";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Emulator.Url + @"/" + Name;
                }

                // Url: dmt-scw/api/emu/cls
                /// <summary>The clearJobs Class.</summary>
                public static partial class clearJobs
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "cls";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Emulator.Url + @"/" + Name;
                }

                // Url: dmt-scw/api/emu/emv/add
                /// <summary>The addEMV Class.</summary>
                public static partial class addEMV
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "add";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Emulator.Url + @"/emv/" + Name;
                }

                // Url: dmt-scw/api/emu/emv/remove
                /// <summary>The removeEMV Class.</summary>
                public static partial class removeEMV
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "remove";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Emulator.Url + @"/emv/" + Name;
                }

                // Url: dmt-scw/api/emu/emv/cls
                /// <summary>The clearEMVs Class.</summary>
                public static partial class clearEMVs
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "cls";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Emulator.Url + @"/emv/" + Name;
                }

                // Url: dmt-scw/api/emu/qrcode/add
                /// <summary>The addEMV Class.</summary>
                public static partial class addQRCode
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "add";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Emulator.Url + @"/qrcode/" + Name;
                }

                // Url: dmt-scw/api/emu/qrcode/remove
                /// <summary>The removeEMV Class.</summary>
                public static partial class removeQRCode
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "remove";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Emulator.Url + @"/qrcode/" + Name;
                }

                // Url: dmt-scw/api/emu/qrcode/cls
                /// <summary>The clearEMVs Class.</summary>
                public static partial class clearQRCodes
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "cls";
                    /// <summary>Gets route url.</summary>
                    public const string Url = Emulator.Url + @"/qrcode/" + Name;
                }
            }
        }
    }
}
