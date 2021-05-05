#region Using

using System;
using System.Collections.Generic;
using NLib.Reflection;

#endregion

namespace DMT.Models
{
    // Url: /api/account/sap/getcustomer
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

namespace DMT.Models
{
    // Url: /api/account/sap/tsblist

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
