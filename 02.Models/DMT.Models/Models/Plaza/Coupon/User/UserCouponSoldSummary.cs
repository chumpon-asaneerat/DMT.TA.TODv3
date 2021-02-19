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

        /// <summary>
        /// Gets User Coupon Sold Summary.
        /// </summary>
        /// <param name="plazaGroup">The target PlazaGroup to get balance.</param>
        /// <param name="user">The target User to get balance.</param>
        /// <param name="start">The start of sold date.</param>
        /// <param name="end">The end of sold date.</param>
        /// <returns>
        /// Returns User Coupon Sold Summary. If not found returns null.
        /// </returns>
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
        /// <summary>
        /// Gets User Coupon Sold Summary.
        /// </summary>
        /// <param name="tsb">The target TSB to get balance.</param>
        /// <param name="plazaGroup">The target PlazaGroup to get balance.</param>
        /// <param name="user">The target User to get balance.</param>
        /// <param name="start">The start of sold date.</param>
        /// <param name="end">The end of sold date.</param>
        /// <returns>
        /// Returns User Coupon Sold Summary. If not found returns null.
        /// </returns>
        public static NDbResult<UserCouponSoldSummary> GetCouponSoldSummary(TSB tsb, PlazaGroup plazaGroup,
            User user, DateTime? start, DateTime? end)
        {
            var result = new NDbResult<UserCouponSoldSummary>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (null == tsb || null == plazaGroup || null == user)
            {
                result.ParameterIsNull();
                return result;
            }

            if (!start.HasValue || start.Value == DateTime.MinValue)
            {
                // Check Start Date.
                result.ParameterIsNull();
                return result;
            }

            DateTime dt1 = start.Value;
            DateTime dt2 = (end.HasValue && end.Value != DateTime.MinValue) ? end.Value : DateTime.Now;

            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    //CouponBHT35
                    //CouponBHT80
                    //Price

                    string cmd = @"
                        SELECT * 
                          FROM TSBCouponBalanceView
                         WHERE TSBId = ? ";
                    var ret = NQuery.Query<FKs>(cmd, tsb.TSBId).FirstOrDefault();
                    var data = ret.ToModel();
                    result.Success(data);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    result.Error(ex);
                }
                return result;
            }
        }

        #endregion
    }

    #endregion
}
