#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace DMT
{
    static partial class RouteConsts
    {
        /// <summary>The TAxTOD class.</summary>
        public static partial class TAxTOD
        {
            // Account
            static partial class Exchange
            {
                // Url: /api/account/request/getlist
                /// <summary>The Request Exchange Gets (Search) Class.</summary>
                public static partial class Gets
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "Gets";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/request/getlist";
                }
                // Url: /api/account/request/getdetail
                /// <summary>The Gets Request Exchange Items Class.</summary>
                public static partial class GetRequestItems
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "GetRequestItems";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/request/getdetail";
                }
                // Url: /api/account/request/approve
                /// <summary>The Approve Request Item Class.</summary>
                public static partial class ApproveRequestItem
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "ApproveRequestItem";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/request/approve";
                }
            }

            static partial class Exchange
            {
                // Url: /api/account/request/save
                /// <summary>The Request Exchange Save Header Class.</summary>
                public static partial class SaveRequestDocument
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "SaveRequestDocument";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/request/save";
                }
                // Url: /api/account/request/savedetail
                /// <summary>The Request Exchange Save Detail Class.</summary>
                public static partial class SaveRequestItem
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "SaveRequestItem";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/request/savedetail";
                }
                // Url: /api/account/request/getapprove
                /// <summary>The Request Exchange Get Approves Class.</summary>
                public static partial class GetApproves
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "GetApproves";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/request/getapprove";
                }
                // Url: /api/account/request/getapprovedetail
                /// <summary>The Request Exchange Get Approve Items Class.</summary>
                public static partial class GetApproveItems
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "GetApproveItems";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/request/getapprovedetail";
                }
            }

            static partial class Exchange
            {
                // Url: /api/account/appcredit/list
                /// <summary>The Request Exchange Get TSB Approve Credit Balance Items Class.</summary>
                public static partial class GetTSBApproveCredits
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "GetTSBApproveCredits";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/appcredit/list";
                }

                // Url: /api/account/appcredit/translist
                /// <summary>The Request Exchange Get TSB Approve Credit Balance Items Class.</summary>
                public static partial class GetTSBApproveCreditTransactions
                {
                    /// <summary>Gets route name.</summary>
                    public const string Name = "GetTSBApproveCreditTransactions";
                    /// <summary>Gets route url.</summary>
                    public const string Url = @"/api/account/appcredit/translist";
                }
            }
        }
    }
}
