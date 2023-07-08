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
    #region UserCouponOnHandSummary (For Query only)

    /// <summary>
    /// The UserCouponOnHandSummary Data Model class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("UserCouponSoldSummary")]
    public class UserCouponOnHandSummary : NTable<UserCouponOnHandSummary>
    {
        #region Internal Variables

        private string _TSBId = string.Empty;
        private string _UserId = string.Empty;
        private List<string> _Coupons = new List<string>();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserCouponOnHandSummary() : base() { }

        #endregion

        #region Public Properties

        #region TSB

        /// <summary>
        /// Gets or sets TSBId.
        /// </summary>
        [Category("TSB")]
        [Description("Gets or sets TSBId.")]
        [ReadOnly(true)]
        [Ignore]
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
        [Ignore]
        [MaxLength(10)]
        [PropertyMapName("UserId")]
        public virtual string UserId
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

        /// <summary>
        /// Gets or sets Coupons on hand.
        /// </summary>
        [Category("Coupon")]
        [Description("Gets or sets Coupons on hand.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("Coupons")]
        public virtual List<string> Coupons
        {
            get { return _Coupons; }
            set
            {
                if (_Coupons != value)
                {
                    _Coupons = value;
                    // Raise event.
                    this.RaiseChanged("TotalOnHandRows");
                }
            }
        }

        #endregion

        #endregion

        #region Internal Class

        /// <summary>
        /// The internal FKs class for query data.
        /// </summary>
        public class FKs : UserCouponOnHandSummary, IFKs<UserCouponOnHandSummary>
        {

        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets User Coupon OnHand Summary.
        /// </summary>
        /// <param name="plazaGroup">The target PlazaGroup to get balance.</param>
        /// <param name="user">The target User to get balance.</param>
        /// <returns>
        /// Returns User Coupon OnHand Summary. If not found returns null.
        /// </returns>
        public static NDbResult<UserCouponOnHandSummary> GetCouponOnHandSummary(User user)
        {
            var result = new NDbResult<UserCouponOnHandSummary>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            var tsb = TSB.GetCurrent().Value();
            if (null == tsb || null == user)
            {
                result.ParameterIsNull();
                return result;
            }
            result = GetCouponOnHandSummary(tsb, user);
            return result;
        }
        /// <summary>
        /// Gets User Coupon OnHand Summary.
        /// </summary>
        /// <param name="tsb">The target TSB to get balance.</param>
        /// <param name="plazaGroup">The target PlazaGroup to get balance.</param>
        /// <param name="user">The target User to get balance.</param>
        /// <returns>
        /// Returns User Coupon OnHand Summary. If not found returns null.
        /// </returns>
        public static NDbResult<UserCouponOnHandSummary> GetCouponOnHandSummary(TSB tsb, User user)
        {
            var result = new NDbResult<UserCouponOnHandSummary>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (null == tsb ||
                //null == plazaGroup || Ignore PlazaGroup
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
                    var coupons = TSBCouponBorrowStatus.GetUserOnHandCoupons(tsb, user).Value();

                    string msg;

                    if (null != coupons)
                    {
                        msg = string.Format("ตรวจสอบจำนวนคูปองที่จ่ายให้ พก : {0} รายการ", coupons.Count);
                        med.Info(msg);

                        var data = new UserCouponOnHandSummary();
                        data.TSBId = tsb.TSBId;
                        data.UserId = user.UserId;
                        data.Coupons = new List<string>();
                        coupons.ForEach(coupon => 
                        {
                            data.Coupons.Add(coupon.CouponId);
                        });

                        result.Success(data);
                    }
                    else
                    {
                        msg = "ตรวจสอบจำนวนคูปองที่จ่ายให้ พก : 0 รายการ";
                        med.Info(msg);

                        result.Success(null);
                    }
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
