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

        /// <summary>
        /// The internal FKs class for query data.
        /// </summary>
        public class FKs : UserCouponSoldSummary, IFKs<UserCouponSoldSummary>
        {

        }

        #endregion

        #region Static Methods

        public static NDbResult<UserCouponSoldSummary> GetCouponSoldSummary(PlazaGroup plazaGroup,
            User user, DateTime? start, DateTime? end)
        {
            var result = new NDbResult<UserCouponSoldSummary>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            var tsb = TSB.GetCurrent().Value();
            if (null == tsb || null == plazaGroup || null == user)
            {
                result.ParameterIsNull();
                return result;
            }
            result = GetCouponSoldSummary(tsb, plazaGroup, user, start, end);
            return result;
        }

        public static NDbResult<UserCouponSoldSummary> GetCouponSoldSummary(TSB tsb, PlazaGroup plazaGroup,
            User user, DateTime? start, DateTime? end)
        {
            var result = new NDbResult<UserCouponSoldSummary>();

            return result;
        }

        #endregion
    }

    #endregion
}
