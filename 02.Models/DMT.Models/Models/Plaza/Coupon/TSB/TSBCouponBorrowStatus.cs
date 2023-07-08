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
        private string _PlazaGroupId = string.Empty;
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

        #region PlazaGroup

        /// <summary>
        /// Gets or sets Plaza Group Id.
        /// </summary>
        [Category("Plaza Group")]
        [Description("Gets or sets Plaza Group Id.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("PlazaGroupId")]
        public virtual string PlazaGroupId
        {
            get
            {
                return _PlazaGroupId;
            }
            set
            {
                if (_PlazaGroupId != value)
                {
                    _PlazaGroupId = value;
                    this.RaiseChanged("PlazaGroupId");
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
        /// Get.
        /// </summary>
        /// <param name="plazaGroupId"></param>
        /// <param name="userId"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public static NDbResult<TSBCouponBorrowStatus> Get(string plazaGroupId, string userId, 
            string couponId)
        {
            var result = new NDbResult<TSBCouponBorrowStatus>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (string.IsNullOrWhiteSpace(plazaGroupId) || string.IsNullOrWhiteSpace(userId) || 
                string.IsNullOrWhiteSpace(couponId))
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
                    cmd += "   AND PlazaGroupId = ? ";
                    cmd += "   AND UserId = ? ";
                    cmd += "   AND CouponId = ? ";

                    var ret = NQuery.Query<FKs>(cmd,
                        tsb.TSBId, plazaGroupId, userId, couponId).FirstOrDefault().ToModel();
                    result.Success(ret);

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
        /// <param name="plazaGroupId"></param>
        /// <param name="userId"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        private static NDbResult Insert(string plazaGroupId, string userId, 
            string couponId)
        {
            var result = new NDbResult();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (string.IsNullOrWhiteSpace(plazaGroupId) || string.IsNullOrWhiteSpace(userId) ||
                string.IsNullOrWhiteSpace(couponId))
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
                    cmd += "INSERT INTO TSBCouponBorrowStatus (TSBId, PlazaGroupId, UserId, CouponId) ";
                    cmd += " VALUES (?, ?, ?, ?) ";

                    var ret = NQuery.Execute(cmd,
                        tsb.TSBId, plazaGroupId, userId, couponId);
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
        /// <param name="plazaGroupId"></param>
        /// <param name="userId"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        private static NDbResult Remove(string plazaGroupId, string userId,
            string couponId)
        {
            var result = new NDbResult();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (string.IsNullOrWhiteSpace(plazaGroupId) || string.IsNullOrWhiteSpace(userId) ||
                string.IsNullOrWhiteSpace(couponId))
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
                    cmd += "   AND PlazaGroupId = ?";
                    cmd += "   AND UserId = ? ";
                    cmd += "   AND CouponId = ?";

                    var ret = NQuery.Execute(cmd,
                        tsb.TSBId, plazaGroupId, userId, couponId);
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
        /// <param name="plazaGroupId"></param>
        /// <param name="userId"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public static NDbResult Borrow(string plazaGroupId, string userId, string couponId)
        {
            var result = new NDbResult();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (string.IsNullOrWhiteSpace(plazaGroupId) || string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(couponId))
            {
                result.ParameterIsNull();
                return result;
            }

            var row = Get(plazaGroupId, userId, couponId).Value();
            if (null == row)
            {
                result = Insert(plazaGroupId, userId, couponId);
            }
            result.Success();

            return result;
        }
        /// <summary>
        /// Set Return Coupon.
        /// </summary>
        /// <param name="plazaGroupId"></param>
        /// <param name="userId"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public static NDbResult Return(string plazaGroupId, string userId, string couponId)
        {
            var result = new NDbResult();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (string.IsNullOrWhiteSpace(plazaGroupId) || string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(couponId))
            {
                result.ParameterIsNull();
                return result;
            }

            var row = Get(plazaGroupId, userId, couponId).Value();
            if (null != row)
            {
                result = Remove(plazaGroupId, userId, couponId);
            }

            result.Success();

            return result;
        }

        #endregion
    }
}
