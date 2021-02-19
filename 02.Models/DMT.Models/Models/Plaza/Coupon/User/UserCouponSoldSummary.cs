#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using NLib;
using NLib.Design;
using NLib.Reflection;

using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
// required for JsonIgnore attribute.
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Reflection;

#endregion

namespace DMT.Models
{
    #region UserCouponSoldSummary (For Query only)

    /// <summary>
    /// The UserCouponSoldSummary Data Model class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("UserCouponSoldSummary")]
    public class UserCouponSoldSummary : NTable<UserCouponSoldSummary>
    {
        #region Internal Variables

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserCouponSoldSummary() : base() { }

        #endregion

        #region Private Methods

        #endregion

        #region Public Properties

        #endregion

        #region Internal Class

        #endregion

        #region Static Methods

        #endregion
    }

    #endregion
}
