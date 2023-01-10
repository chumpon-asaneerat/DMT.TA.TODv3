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

        private string _TSBId = string.Empty;
        private string _TSBNameEN = string.Empty;
        private string _TSBNameTH = string.Empty;

        private string _UserId = string.Empty;
        private string _FullNameEN = string.Empty;
        private string _FullNameTH = string.Empty;

        private int _CouponBHT35 = 0;
        private int _CouponBHT80 = 0;
        private int _CouponTotal = 0;

        private decimal _CouponBHT35Total = decimal.Zero;
        private decimal _CouponBHT80Total = decimal.Zero;
        private decimal _CouponBHTTotal = decimal.Zero;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserCouponSoldSummary() : base() { }

        #endregion

        #region Private Methods

        private void CalcCouponTotal()
        {
            _CouponTotal = _CouponBHT35 + _CouponBHT80;
            // Raise event.
            this.RaiseChanged("CouponTotal");

        }

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
        /// <summary>
        /// Gets or sets TSB Name EN.
        /// </summary>
        [Category("TSB")]
        [Description("Gets or sets TSB Name EN.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("TSBNameEN")]
        public virtual string TSBNameEN
        {
            get
            {
                return _TSBNameEN;
            }
            set
            {
                if (_TSBNameEN != value)
                {
                    _TSBNameEN = value;
                    this.RaiseChanged("TSBNameEN");
                }
            }
        }
        /// <summary>
        /// Gets or sets TSB Name TH.
        /// </summary>
        [Category("TSB")]
        [Description("Gets or sets TSB Name TH.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("TSBNameTH")]
        public virtual string TSBNameTH
        {
            get
            {
                return _TSBNameTH;
            }
            set
            {
                if (_TSBNameTH != value)
                {
                    _TSBNameTH = value;
                    this.RaiseChanged("TSBNameTH");
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
        /// <summary>
        /// Gets or sets User Full Name EN
        /// </summary>
        [Category("User")]
        [Description("Gets or sets User Full Name EN.")]
        [ReadOnly(true)]
        [Ignore]
        [MaxLength(150)]
        [PropertyMapName("FullNameEN")]
        public virtual string FullNameEN
        {
            get
            {
                return _FullNameEN;
            }
            set
            {
                if (_FullNameEN != value)
                {
                    _FullNameEN = value;
                    this.RaiseChanged("FullNameEN");
                }
            }
        }
        /// <summary>
        /// Gets or sets User Full Name TH
        /// </summary>
        [Category("User")]
        [Description("Gets or sets User Full Name TH.")]
        [ReadOnly(true)]
        [Ignore]
        [MaxLength(150)]
        [PropertyMapName("FullNameTH")]
        public virtual string FullNameTH
        {
            get
            {
                return _FullNameTH;
            }
            set
            {
                if (_FullNameTH != value)
                {
                    _FullNameTH = value;
                    this.RaiseChanged("FullNameTH");
                }
            }
        }

        #endregion

        #region Coupon

        /// <summary>
        /// Gets or sets number of 35 BHT coupon.
        /// </summary>
        [Category("Coupon")]
        [Description("Gets or sets number of 35 BHT coupon.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("CouponBHT35")]
        public virtual int CouponBHT35
        {
            get { return _CouponBHT35; }
            set
            {
                if (_CouponBHT35 != value)
                {
                    _CouponBHT35 = value;
                    CalcCouponTotal();
                    // Raise event.
                    this.RaiseChanged("CouponBHT35");

                }
            }
        }
        /// <summary>
        /// Gets or sets number of 80 BHT coupon.
        /// </summary>
        [Category("Coupon")]
        [Description("Gets or sets number of 80 BHT coupon.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("CouponBHT80")]
        public virtual int CouponBHT80
        {
            get { return _CouponBHT80; }
            set
            {
                if (_CouponBHT80 != value)
                {
                    _CouponBHT80 = value;
                    CalcCouponTotal();
                    // Raise event.
                    this.RaiseChanged("CouponBHT80");
                }
            }
        }
        /// <summary>
        /// Gets calculate coupon total (book count).
        /// </summary>
        [Category("Coupon")]
        [Description("Gets calculate coupon total (book count).")]
        [ReadOnly(true)]
        [Ignore]
        [JsonIgnore]
        [PropertyMapName("CouponTotal")]
        public int CouponTotal
        {
            get { return _CouponTotal; }
            set { }
        }
        /// <summary>
        /// Gets or sets total coupon 35 in baht.
        /// </summary>
        [Category("Coupon")]
        [Description("Gets or sets total coupon 35 in baht.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("CouponBHT35Total")]
        public virtual decimal CouponBHT35Total
        {
            get { return _CouponBHT35Total; }
            set
            {
                if (_CouponBHT35Total != value)
                {
                    _CouponBHT35Total = value;
                    // Raise event.
                    this.RaiseChanged("CouponBHT35Total");
                }
            }
        }
        /// <summary>
        /// Gets or sets total coupon 80 in baht.
        /// </summary>
        [Category("Coupon")]
        [Description("Gets or sets total coupon 80 in baht.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("CouponBHT80Total")]
        public virtual decimal CouponBHT80Total
        {
            get { return _CouponBHT80Total; }
            set
            {
                if (_CouponBHT80Total != value)
                {
                    _CouponBHT80Total = value;
                    // Raise event.
                    this.RaiseChanged("CouponBHT80Total");
                }
            }
        }
        /// <summary>
        /// Gets or sets total value in baht.
        /// </summary>
        [Category("Coupon")]
        [Description("Gets or sets total value in baht.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("CouponBHTTotal")]
        public virtual decimal CouponBHTTotal
        {
            get { return _CouponBHTTotal; }
            set
            {
                if (_CouponBHTTotal != value)
                {
                    _CouponBHTTotal = value;
                    // Raise event.
                    this.RaiseChanged("CouponBHTTotal");
                }
            }
        }

        #endregion

        #endregion

        #region Internal Class

        /// <summary>
        /// The internal FKs class for query data.
        /// </summary>
        public class FKs : UserCouponSoldSummary, IFKs<UserCouponSoldSummary>
        {
            #region TSB

            /// <summary>
            /// Gets or sets TSB Name EN.
            /// </summary>
            [MaxLength(100)]
            [PropertyMapName("TSBNameEN")]
            public override string TSBNameEN
            {
                get { return base.TSBNameEN; }
                set { base.TSBNameEN = value; }
            }
            /// <summary>
            /// Gets or sets TSB Name TH.
            /// </summary>
            [MaxLength(100)]
            [PropertyMapName("TSBNameTH")]
            public override string TSBNameTH
            {
                get { return base.TSBNameTH; }
                set { base.TSBNameTH = value; }
            }

            #endregion

            #region User

            /// <summary>
            /// Gets or sets UserId.
            /// </summary>
            [MaxLength(10)]
            [PropertyMapName("UserId")]
            public override string UserId
            {
                get { return base.UserId; }
                set { base.UserId = value; }
            }
            /// <summary>
            /// Gets or sets Full Name EN.
            /// </summary>
            [MaxLength(150)]
            [PropertyMapName("FullNameEN")]
            public override string FullNameEN
            {
                get { return base.FullNameEN; }
                set { base.FullNameEN = value; }
            }
            /// <summary>
            /// Gets or sets Full Name TH.
            /// </summary>
            [MaxLength(150)]
            [PropertyMapName("FullNameTH")]
            public override string FullNameTH
            {
                get { return base.FullNameTH; }
                set { base.FullNameTH = value; }
            }

            #endregion

            #region Coupon

            /// <summary>
            /// Gets or sets number of 35 BHT coupon.
            /// </summary>
            [PropertyMapName("CouponBHT35")]
            public override int CouponBHT35
            {
                get { return base.CouponBHT35; }
                set { base.CouponBHT35 = value; }
            }
            /// <summary>
            /// Gets or sets number of 80 BHT coupon.
            /// </summary>
            [PropertyMapName("CouponBHT80")]
            public override int CouponBHT80
            {
                get { return base.CouponBHT80; }
                set { base.CouponBHT80 = value; }
            }
            /// <summary>
            /// Gets or sets coupon 35 total in baht.
            /// </summary>
            [PropertyMapName("CouponBHT35Total")]
            public override decimal CouponBHT35Total
            {
                get { return base.CouponBHT35Total; }
                set { base.CouponBHT35Total = value; }
            }
            /// <summary>
            /// Gets or sets coupon 80 total in baht.
            /// </summary>
            [PropertyMapName("CouponBHT80Total")]
            public override decimal CouponBHT80Total
            {
                get { return base.CouponBHT80Total; }
                set { base.CouponBHT80Total = value; }
            }
            /// <summary>
            /// Gets or sets total value in baht.
            /// </summary>
            [PropertyMapName("CouponBHTTotal")]
            public override decimal CouponBHTTotal
            {
                get { return base.CouponBHTTotal; }
                set { base.CouponBHTTotal = value; }
            }

            #endregion
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
        public static NDbResult<UserCouponSoldSummary> GetCouponSoldSummary(
            //PlazaGroup plazaGroup, // Ignore PlazaGroup
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
            if (null == tsb ||
                //null == plazaGroup || // Ignore PlazaGroup
                null == user)
            {
                result.ParameterIsNull();
                return result;
            }
            result = GetCouponSoldSummary(tsb, /*plazaGroup, */ user, start, end);
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
        public static NDbResult<UserCouponSoldSummary> GetCouponSoldSummary(TSB tsb,
            //PlazaGroup plazaGroup, // Ignore PlazaGroup
            User user, DateTime? start, DateTime? end)
        {
            var result = new NDbResult<UserCouponSoldSummary>();
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

            if (!start.HasValue || start.Value == DateTime.MinValue)
            {
                // Check Start Date.
                result.ParameterIsNull();
                return result;
            }

            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    var coupons = TSBCouponTransaction.GetUserCouponSoldByLaneTransactions(tsb,
                        //plazaGroup, // Ignore plazagroup
                        user, start, end).Value();
                    if (null != coupons)
                    {
                        var data = new UserCouponSoldSummary();
                        data.TSBId = tsb.TSBId;
                        data.TSBNameEN = tsb.TSBNameEN;
                        data.TSBNameTH = tsb.TSBNameTH;
                        data.UserId = user.UserId;
                        data.FullNameEN = user.FullNameEN;
                        data.FullNameTH = user.FullNameTH;
                        data.CouponBHT35 = 0;
                        data.CouponBHT80 = 0;
                        data.CouponBHT35Total = decimal.Zero;
                        data.CouponBHT80Total = decimal.Zero;
                        data.CouponBHTTotal = decimal.Zero;

                        coupons.ForEach(coupon =>
                        {
                            if (null != coupon)
                            {
                                if (coupon.CouponType == CouponType.BHT35)
                                {
                                    data.CouponBHT35++;
                                    data.CouponBHT35Total += coupon.Price;
                                    data.CouponBHTTotal += coupon.Price;
                                }
                                else if (coupon.CouponType == CouponType.BHT80)
                                {
                                    data.CouponBHT80++;
                                    data.CouponBHT80Total += coupon.Price;
                                    data.CouponBHTTotal += coupon.Price;
                                }
                            }
                        });

                        result.Success(data);
                    }
                    else
                    {
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
