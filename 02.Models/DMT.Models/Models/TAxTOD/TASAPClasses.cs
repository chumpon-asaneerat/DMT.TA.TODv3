#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion


// SAPCustomer: Url: /api/account/sap/getcustomer
namespace DMT.Models
{
    // Server data result.
    /*
    {
        "CardCode": "111",
        "CardName": "Novatec Healthcare Co.,Ltd.",
        "UseFlag": 1
    }
    */
    /// <summary>The SAPCustomer class.</summary>
    public class SAPCustomer
    {
        /// <summary>Gets or sets CardCode.</summary>
        [PropertyMapName("CardCode")]
        public string CardCode { get; set; }
        /// <summary>Gets or sets CardName.</summary>
        [PropertyMapName("CardName")]
        public string CardName { get; set; }
        /// <summary>Gets or sets UseFlag.</summary>
        [PropertyMapName("UseFlag")]
        public int UseFlag { get; set; }
    }

    // Server data parameter.
    /*
    {
        "searchtext": null
    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {
                #region SearchCustomer

                /// <summary>
                /// SearchCustomer.
                /// </summary>
                public class SearchCustomer : NSearch<SearchCustomer>
                {
                    #region Public Properties

                    /// <summary>
                    /// Gets or sets searchtext.
                    /// </summary>
                    public string searchtext { get; set; }

                    #endregion

                    #region Static Method (Create)

                    /// <summary>
                    /// Create Search instance.
                    /// </summary>
                    /// <param name="filter">The filter.</param>
                    /// <returns>Returns Search instance.</returns>
                    public static SearchCustomer Create(
                        string filter = "")
                    {
                        var ret = new SearchCustomer();
                        ret.searchtext = filter;
                        return ret;
                    }

                    #endregion
                }

                #endregion
            }
        }
    }
}

// SAPTSB: Url: /api/account/sap/tsblist
namespace DMT.Models
{
    // Server data result
    /*
    {
      "TSBId": "09",
      "TSB_Th_Name": "อนุสรณ์สถาน",
      "TSB_Eng_name": "ANUSORN SATHAN",
      "SapWhsCode": "CAS",
      "TollwayID": 9
    }
    */
    // Server data parameter.
    /*
    {

    }
    */
    /// <summary>The SAPTSB class.</summary>
    public class SAPTSB
    {
        /// <summary>Gets or sets TSBId.</summary>
        [PropertyMapName("TSBId")]
        public string TSBId { get; set; }
        /// <summary>Gets or sets TSB_Th_Name.</summary>
        [PropertyMapName("TSB_Th_Name")]
        public string TSB_Th_Name { get; set; }
        /// <summary>Gets or sets TSB_Eng_name.</summary>
        [PropertyMapName("TSB_Eng_name")]
        public string TSB_Eng_name { get; set; }
        /// <summary>Gets or sets SapWhsCode.</summary>
        [PropertyMapName("SapWhsCode")]
        public string SapWhsCode { get; set; }
        /// <summary>Gets or sets TollwayID.</summary>
        [PropertyMapName("TollwayID")]
        public int TollwayID { get; set; }
    }
}

namespace DMT.Models
{
    // Url: /api/account/sap/couponsoldlist

    // Server data parameter.
    /*
    {

    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {

            }
        }
    }
}

namespace DMT.Models
{
    // Url: /api/account/sap/save/arhead

    // Server data parameter.
    /*
    {

    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {

            }
        }
    }
}

namespace DMT.Models
{
    // Url: /api/account/sap/couponsoldlist

    // Server data parameter.
    /*
    {

    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {

            }
        }
    }
}

namespace DMT.Models
{
    // Url: /api/account/sap/get/arsumcoupon

    // Server data parameter.
    /*
    {

    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {

            }
        }
    }
}

namespace DMT.Models
{
    // Url: /api/account/sap/save/arline

    // Server data parameter.
    /*
    {

    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {

            }
        }
    }
}

namespace DMT.Models
{
    // Url: /api/account/sap/get/arcouponlist

    // Server data parameter.
    /*
    {

    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {

            }
        }
    }
}

namespace DMT.Models
{
    // Url: /api/account/sap/save/arserial

    // Server data parameter.
    /*
    {

    }
    */
    static partial class Search
    {
        public static partial class TAxTOD
        {
            public static partial class SAP
            {

            }
        }
    }
}
