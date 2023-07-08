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
using Newtonsoft.Json.Linq;
using System.Windows;

#endregion

namespace DMT.Models
{
    /// <summary>
    /// The TSBCouponBorrowStatus Data Model class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("TSBCouponBorrowStatus")]
    public class TSBCouponBorrowStatus : NTable<TSBCouponBorrowStatus>
    {
        #region Internal Variables

        private string _TSBId = string.Empty;
        private string _UserId = string.Empty;
        private string _CouponId = string.Empty;

        #endregion

        #region Public Properties

        #region TSB

        /// <summary>
        /// Gets or sets TSBId.
        /// </summary>
        [Category("TSB")]
        [Description("Gets or sets TSBId.")]
        [ReadOnly(true)]
        [MaxLength(10)]
        [PropertyMapName("TSBId")]
        public string TSBId
        {
            get
            {
                return _TSBId;
            }
            set
            {
                if (_TSBId != value)
                {
                    _TSBId = value;
                    this.RaiseChanged("TSBId");
                }
            }
        }

        #endregion

        #region User

        /// <summary>
        /// Gets or sets User Id
        /// </summary>
        [Category("User")]
        [Description("Gets or sets User Id.")]
        [ReadOnly(true)]
        [MaxLength(10)]
        [PropertyMapName("UserId")]
        public string UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                if (_UserId != value)
                {
                    _UserId = value;
                    this.RaiseChanged("UserId");
                }
            }
        }

        #endregion

        #region Coupon

        /// Gets or sets coupon id.
        /// </summary>
        [Category("Coupon")]
        [Description("Gets or sets coupon id.")]
        [ReadOnly(true)]
        [MaxLength(20)]
        [PropertyMapName("CouponId")]
        public string CouponId
        {
            get { return _CouponId; }
            set
            {
                if (_CouponId != value)
                {
                    _CouponId = value;
                    // Raise event.
                    this.RaiseChanged("CouponId");
                }
            }
        }

        #endregion

        #endregion

        #region Internal Class

        /// <summary>
        /// The internal FKs class for query data.
        /// </summary>
        public class FKs : TSBCouponBorrowStatus, IFKs<TSBCouponBorrowStatus>
        {

        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets User OnHand Coupons.
        /// </summary>
        /// <param name="tsb">The target TSB to get coupon.</param>
        /// <param name="user">The target User to get balance.</param>
        /// <returns>Returns User Borrow Coupons. If TSB not found returns null.</returns>
        public static NDbResult<List<TSBCouponBorrowStatus>> GetUserOnHandCoupons(TSB tsb, User user)
        {
            var result = new NDbResult<List<TSBCouponBorrowStatus>>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (null == tsb ||
                //null == plazaGroup || 
                null == user)
            {
                result.ParameterIsNull();
                return result;
            }

            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * ";
                    cmd += "  FROM TSBCouponBorrowStatus ";
                    cmd += " WHERE TSBId = ? ";
                    cmd += "   AND UserId = ? ";

                    var rets = NQuery.Query<FKs>(cmd, tsb.TSBId,
                        user.UserId).ToList();
                    var results = rets.ToModels();
                    result.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    result.Error(ex);
                }
                return result;
            }
        }
        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public static NDbResult<TSBCouponBorrowStatus> Get(string couponId)
        {
            var result = new NDbResult<TSBCouponBorrowStatus>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (string.IsNullOrWhiteSpace(couponId))
            {
                result.ParameterIsNull();
                return result;
            }
            var tsb = TSB.GetCurrent().Value();
            if (null == tsb)
            {
                result.ParameterIsNull();
                return result;
            }

            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * ";
                    cmd += "  FROM TSBCouponBorrowStatus ";
                    cmd += " WHERE TSBId = ? ";
                    cmd += "   AND CouponId = ? ";

                    var rets = NQuery.Query<FKs>(cmd,
                        tsb.TSBId, couponId).ToList();

                    if (rets.Count > 0) 
                    {
                        var ret = rets[0];
                        result.Success(ret);
                    }
                    else
                    {
                        result.Success(null);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    result.Error(ex);
                }
                return result;
            }
        }
        /// <summary>
        /// Insert.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        private static NDbResult Insert(string userId, string couponId)
        {
            var result = new NDbResult();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(couponId))
            {
                result.ParameterIsNull();
                return result;
            }
            var tsb = TSB.GetCurrent().Value();
            if (null == tsb)
            {
                result.ParameterIsNull();
                return result;
            }

            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "INSERT INTO TSBCouponBorrowStatus (TSBId, UserId, CouponId) ";
                    cmd += " VALUES (?, ?, ?) ";

                    var ret = NQuery.Execute(cmd,
                        tsb.TSBId, userId, couponId);
                    result.Success();

                    return result;
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    result.Error(ex);
                }
                return result;
            }
        }
        /// <summary>
        /// Remove (Delete).
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        private static NDbResult Remove(string couponId)
        {
            var result = new NDbResult();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (string.IsNullOrWhiteSpace(couponId))
            {
                result.ParameterIsNull();
                return result;
            }
            var tsb = TSB.GetCurrent().Value();
            if (null == tsb)
            {
                result.ParameterIsNull();
                return result;
            }

            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "DELETE FROM TSBCouponBorrowStatus ";
                    cmd += " WHERE TSBId = ?";
                    cmd += "   AND CouponId = ?";

                    var ret = NQuery.Execute(cmd,
                        tsb.TSBId, couponId);
                    result.Success();

                    return result;
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    result.Error(ex);
                }
                return result;
            }
        }
        /// <summary>
        /// Set Borrow Coupon.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public static NDbResult Borrow(string userId, string couponId)
        {
            var result = new NDbResult();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(couponId))
            {
                result.ParameterIsNull();
                return result;
            }

            var row = Get(couponId).Value();
            if (null == row)
            {
                result = Insert(userId, couponId);
            }
            result.Success();

            return result;
        }
        /// <summary>
        /// Set Return Coupon.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public static NDbResult Return(string couponId)
        {
            var result = new NDbResult();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (string.IsNullOrWhiteSpace(couponId))
            {
                result.ParameterIsNull();
                return result;
            }

            var row = Get(couponId).Value();
            if (null != row)
            {
                result = Remove(couponId);
            }

            result.Success();

            return result;
        }

        #endregion
    }
}
