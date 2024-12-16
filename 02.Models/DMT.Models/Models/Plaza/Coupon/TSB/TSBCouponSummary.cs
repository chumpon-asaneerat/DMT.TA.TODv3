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
using System.Windows.Ink;

#endregion

namespace DMT.Models
{
	#region TSBCouponSummary (For Query only)

	/// <summary>
	/// The TSBCouponSummary Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("TSBCouponSummary")]
	public class TSBCouponSummary : NTable<TSBCouponSummary>
	{
		#region Internal Variables

		// For Runtime Used
		private string _description = string.Empty;
		private bool _hasRemark = false;

		private string _TSBId = string.Empty;
		private string _TSBNameEN = string.Empty;
		private string _TSBNameTH = string.Empty;

		private string _UserId = string.Empty;
		private string _FullNameEN = string.Empty;
		private string _FullNameTH = string.Empty;

		private int _CountCouponBHT35 = 0;
        private int _CountCouponBHT40 = 0;
        private int _CountCouponBHT80 = 0;
        private int _CountCouponBHT90 = 0;

		private decimal _AmountCouponBHT35 = decimal.Zero;
        private decimal _AmountCouponBHT40 = decimal.Zero;
        private decimal _AmountCouponBHT80 = decimal.Zero;
        private decimal _AmountCouponBHT90 = decimal.Zero;

		private int _CountCouponTotal = 0;
		private decimal _CouponBHTTotal = decimal.Zero;

		private string _AmountCouponBHT35String = string.Empty;
        private string _AmountCouponBHT40String = string.Empty;
        private string _AmountCouponBHT80String = string.Empty;
        private string _AmountCouponBHT90String = string.Empty;
		private string _CouponBHTTotalString = string.Empty;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public TSBCouponSummary() : base() { }

		#endregion

		#region Private Methods

		private void CalcCouponTotal()
		{
			_CountCouponTotal = _CountCouponBHT35 + _CountCouponBHT40 + _CountCouponBHT80 + _CountCouponBHT90;
			// Raise event.
			this.RaiseChanged("CountCouponTotal");

		}

		private void CalcAmountTotal()
		{
			_CouponBHTTotal = _AmountCouponBHT35 + _AmountCouponBHT40 + _AmountCouponBHT80 + _AmountCouponBHT90;
			_AmountCouponBHT35String = NLib.Utils.StringUtils.ToThaiCurrency(_AmountCouponBHT35);
            _AmountCouponBHT40String = NLib.Utils.StringUtils.ToThaiCurrency(_AmountCouponBHT40);
            _AmountCouponBHT80String = NLib.Utils.StringUtils.ToThaiCurrency(_AmountCouponBHT80);
            _AmountCouponBHT90String = NLib.Utils.StringUtils.ToThaiCurrency(_AmountCouponBHT90);
			_CouponBHTTotalString = NLib.Utils.StringUtils.ToThaiCurrency(_CouponBHTTotal);
			// Raise event.
			this.RaiseChanged("CouponBHTTotal");
			this.RaiseChanged("AmountCouponBHT35");
            this.RaiseChanged("AmountCouponBHT40");
            this.RaiseChanged("AmountCouponBHT80");
            this.RaiseChanged("AmountCouponBHT90");
			this.RaiseChanged("CouponBHTTotalString");
		}

		#endregion

		#region Public Properties

		#region Runtime

		/// <summary>
		/// Gets or sets has remark.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets or sets HasRemark.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("Description")]
		public string Description
		{
			get { return _description; }
			set
			{
				if (_description != value)
				{
					_description = value;
					// Raise event.
					this.RaiseChanged("Description");
				}
			}
		}
		/// <summary>
		/// Gets or sets has remark.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets or sets HasRemark.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("HasRemark")]
		public bool HasRemark
		{
			get { return _hasRemark; }
			set
			{
				if (_hasRemark != value)
				{
					_hasRemark = value;
					// Raise event.
					this.RaiseChanged("HasRemark");
					this.RaiseChanged("RemarkVisibility");
				}
			}
		}
		/// <summary>
		/// Gets Remark Visibility.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets Remark Visibility.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("RemarkVisibility")]
		public System.Windows.Visibility RemarkVisibility
		{
			get { return (_hasRemark) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; }
			set { }
		}

		#endregion

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
		/// <summary>
		/// Gets or sets User Full Name EN
		/// </summary>
		[Category("User")]
		[Description("Gets or sets User Full Name EN.")]
		[ReadOnly(true)]
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
		[PropertyMapName("CountCouponBHT35")]
		public virtual int CountCouponBHT35
		{
			get { return _CountCouponBHT35; }
			set
			{
				if (_CountCouponBHT35 != value)
				{
					_CountCouponBHT35 = value;
					CalcCouponTotal();
					// Raise event.
					this.RaiseChanged("CountCouponBHT35");

				}
			}
		}
        /// <summary>
        /// Gets or sets number of 40 BHT coupon.
        /// </summary>
        [Category("Coupon")]
        [Description("Gets or sets number of 40 BHT coupon.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("CountCouponBHT40")]
        public virtual int CountCouponBHT40
        {
            get { return _CountCouponBHT40; }
            set
            {
                if (_CountCouponBHT40 != value)
                {
                    _CountCouponBHT40 = value;
                    CalcCouponTotal();
                    // Raise event.
                    this.RaiseChanged("CountCouponBHT40");

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
		[PropertyMapName("CountCouponBHT80")]
		public virtual int CountCouponBHT80
		{
			get { return _CountCouponBHT80; }
			set
			{
				if (_CountCouponBHT80 != value)
				{
					_CountCouponBHT80 = value;
					CalcCouponTotal();
					// Raise event.
					this.RaiseChanged("CountCouponBHT80");
				}
			}
		}
        /// <summary>
        /// Gets or sets number of 90 BHT coupon.
        /// </summary>
        [Category("Coupon")]
        [Description("Gets or sets number of 90 BHT coupon.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("CountCouponBHT90")]
        public virtual int CountCouponBHT90
        {
            get { return _CountCouponBHT90; }
            set
            {
                if (_CountCouponBHT90 != value)
                {
                    _CountCouponBHT90 = value;
                    CalcCouponTotal();
                    // Raise event.
                    this.RaiseChanged("CountCouponBHT90");
                }
            }
        }
        /// <summary>
        /// Gets or sets Amount of 35 BHT coupon.
        /// </summary>
        [Category("Coupon")]
		[Description("Gets or sets Amount of 35 BHT coupon.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("AmountCouponBHT35")]
		public virtual decimal AmountCouponBHT35
		{
			get { return _AmountCouponBHT35; }
			set
			{
				if (_AmountCouponBHT35 != value)
				{
					_AmountCouponBHT35 = value;
					CalcAmountTotal();
					// Raise event.
					this.RaiseChanged("AmountCouponBHT35");

				}
			}
		}
		/// <summary>
		/// Gets or sets Amount of 35 BHT coupon in string.
		/// </summary>
		[Category("Coupon")]
		[Description("Gets or sets Amount of 35 BHT coupon in string.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("AmountCouponBHT35String")]
		public virtual string AmountCouponBHT35String
		{
			get { return _AmountCouponBHT35String; }
			set { }
		}
        /// <summary>
        /// Gets or sets Amount of 40 BHT coupon.
        /// </summary>
        [Category("Coupon")]
        [Description("Gets or sets Amount of 40 BHT coupon.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("AmountCouponBHT40")]
        public virtual decimal AmountCouponBHT40
        {
            get { return _AmountCouponBHT40; }
            set
            {
                if (_AmountCouponBHT40 != value)
                {
                    _AmountCouponBHT40 = value;
                    CalcAmountTotal();
                    // Raise event.
                    this.RaiseChanged("AmountCouponBHT40");

                }
            }
        }
        /// <summary>
        /// Gets or sets Amount of 40 BHT coupon in string.
        /// </summary>
        [Category("Coupon")]
        [Description("Gets or sets Amount of 40 BHT coupon in string.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("AmountCouponBHT40String")]
        public virtual string AmountCouponBHT40String
        {
            get { return _AmountCouponBHT40String; }
            set { }
        }
        /// <summary>
        /// Gets or sets Amount of 80 BHT coupon.
        /// </summary>
        [Category("Coupon")]
		[Description("Gets or sets Amount of 80 BHT coupon.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("AmountCouponBHT80")]
		public virtual decimal AmountCouponBHT80
		{
			get { return _AmountCouponBHT80; }
			set
			{
				if (_AmountCouponBHT80 != value)
				{
					_AmountCouponBHT80 = value;
					CalcAmountTotal();
					// Raise event.
					this.RaiseChanged("AmountCouponBHT80");
				}
			}
		}
		/// <summary>
		/// Gets or sets Amount of 80 BHT coupon in string.
		/// </summary>
		[Category("Coupon")]
		[Description("Gets or sets Amount of 80 BHT coupon in string.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("AmountCouponBHT80String")]
		public virtual string AmountCouponBHT80String
		{
			get { return _AmountCouponBHT80String; }
			set { }
		}
        /// <summary>
        /// Gets or sets Amount of 90 BHT coupon.
        /// </summary>
        [Category("Coupon")]
        [Description("Gets or sets Amount of 90 BHT coupon.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("AmountCouponBHT90")]
        public virtual decimal AmountCouponBHT90
        {
            get { return _AmountCouponBHT90; }
            set
            {
                if (_AmountCouponBHT90 != value)
                {
                    _AmountCouponBHT90 = value;
                    CalcAmountTotal();
                    // Raise event.
                    this.RaiseChanged("AmountCouponBHT90");
                }
            }
        }
        /// <summary>
        /// Gets or sets Amount of 90 BHT coupon in string.
        /// </summary>
        [Category("Coupon")]
        [Description("Gets or sets Amount of 90 BHT coupon in string.")]
        [ReadOnly(true)]
        [Ignore]
        [PropertyMapName("AmountCouponBHT90String")]
        public virtual string AmountCouponBHT90String
        {
            get { return _AmountCouponBHT90String; }
            set { }
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
		public int CountCouponTotal
		{
			get { return _CountCouponTotal; }
			set { }
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
			set { }
		}
		/// <summary>
		/// Gets or sets total value in baht in string.
		/// </summary>
		[Category("Coupon")]
		[Description("Gets or sets total value in baht in string.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("CouponBHTTotalString")]
		public virtual string CouponBHTTotalString
		{
			get { return _CouponBHTTotalString; }
			set { }
		}

		#endregion

		#endregion

		#region Internal Class

		/// <summary>
		/// The internal FKs class for query data.
		/// </summary>
		public class FKs : TSBCouponSummary, IFKs<TSBCouponSummary>
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

			#region Coupon

			/// <summary>
			/// Gets or sets number of 35 BHT coupon.
			/// </summary>
			[PropertyMapName("CountCouponBHT35")]
			public override int CountCouponBHT35
			{
				get { return base.CountCouponBHT35; }
				set { base.CountCouponBHT35 = value; }
			}
            /// <summary>
            /// Gets or sets number of 40 BHT coupon.
            /// </summary>
            [PropertyMapName("CountCouponBHT40")]
            public override int CountCouponBHT40
            {
                get { return base.CountCouponBHT40; }
                set { base.CountCouponBHT40 = value; }
            }
            /// <summary>
            /// Gets or sets number of 80 BHT coupon.
            /// </summary>
            [PropertyMapName("CountCouponBHT80")]
			public override int CountCouponBHT80
			{
				get { return base.CountCouponBHT80; }
				set { base.CountCouponBHT80 = value; }
			}
            /// Gets or sets number of 90 BHT coupon.
            /// </summary>
            [PropertyMapName("CountCouponBHT90")]
            public override int CountCouponBHT90
            {
                get { return base.CountCouponBHT90; }
                set { base.CountCouponBHT90 = value; }
            }
            /// <summary>
            /// Gets or sets Amount of 35 BHT coupon.
            /// </summary>
            [PropertyMapName("AmountCouponBHT35")]
			public override decimal AmountCouponBHT35
			{
				get { return base.AmountCouponBHT35; }
				set { base.AmountCouponBHT35 = value; }
			}
            /// <summary>
            /// Gets or sets Amount of 40 BHT coupon.
            /// </summary>
            [PropertyMapName("AmountCouponBHT40")]
            public override decimal AmountCouponBHT40
            {
                get { return base.AmountCouponBHT40; }
                set { base.AmountCouponBHT40 = value; }
            }
            /// <summary>
            /// Gets or sets Amount of 80 BHT coupon.
            /// </summary>
            [PropertyMapName("AmountCouponBHT80")]
			public override decimal AmountCouponBHT80
			{
				get { return base.AmountCouponBHT80; }
				set { base.AmountCouponBHT80 = value; }
			}
            /// <summary>
            /// Gets or sets Amount of 90 BHT coupon.
            /// </summary>
            [PropertyMapName("AmountCouponBHT90")]
            public override decimal AmountCouponBHT90
            {
                get { return base.AmountCouponBHT90; }
                set { base.AmountCouponBHT90 = value; }
            }

            #endregion
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets Active TSB Coupon summaries.
        /// </summary>
        /// <returns>Returns Current Active TSB Coupon balance. If not found returns null.</returns>
        public static NDbResult<List<TSBCouponSummary>> GetTSBCouponSummaries()
		{
			var result = new NDbResult<List<TSBCouponSummary>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			var tsb = TSB.GetCurrent().Value();
			if (null == tsb)
			{
				result.ParameterIsNull();
				return result;
			}
			result = GetTSBCouponSummaries(tsb);
			return result;
		}
		/// <summary>
		/// Gets TSB Coupon summaries.
		/// </summary>
		/// <param name="tsb">The target TSB to get balance.</param>
		/// <returns>Returns List of TSB Coupon balance. If TSB not found returns null.</returns>
		public static NDbResult<List<TSBCouponSummary>> GetTSBCouponSummaries(TSB tsb)
		{
			var result = new NDbResult<List<TSBCouponSummary>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
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
					string cmd = @"
					SELECT * FROM TSBCouponSummarryView
					 WHERE TSBId = ? ";
					var rets = NQuery.Query<FKs>(cmd, tsb.TSBId).ToList();
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
		/// Gets TSB Coupon summaries.
		/// </summary>
		/// <param name="tsb">The target TSB to get balance.</param>
		/// <param name="user">The target User to get balance.</param>
		/// <returns>Returns List of TSB Coupon balance. If TSB not found returns null.</returns>
		public static NDbResult<List<TSBCouponSummary>> GetByUser(User user)
		{
			var result = new NDbResult<List<TSBCouponSummary>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			var tsb = TSB.GetCurrent().Value();
			if (null == tsb)
			{
				result.ParameterIsNull();
				return result;
			}
			result = GetByUser(tsb, user);
			return result;
		}
		/// <summary>
		/// Gets TSB Coupon summaries.
		/// </summary>
		/// <param name="tsb">The target TSB to get balance.</param>
		/// <param name="user">The target User to get balance.</param>
		/// <returns>Returns List of TSB Coupon balance. If TSB not found returns null.</returns>
		public static NDbResult<List<TSBCouponSummary>> GetByUser(TSB tsb, User user)
		{
			var result = new NDbResult<List<TSBCouponSummary>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			if (null == tsb || null == user)
			{
				result.ParameterIsNull();
				return result;
			}
			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					string cmd = @"
					SELECT * 
					  FROM TSBCouponSummarryView
					 WHERE TSBId = ?
					   AND UserId = ? ";
					var rets = NQuery.Query<FKs>(cmd, tsb.TSBId, user.UserId).ToList();
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


		#endregion
	}

	#endregion
}
