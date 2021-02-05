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
	// TODO: Need LaneId, LaneNo

	#region TSBCouponBalance (For Query only)

	/// <summary>
	/// The TSBCouponBalance Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("TSBCouponBalance")]
	public class TSBCouponBalance : NTable<TSBCouponBalance>
	{
		#region Internal Variables

		// For Runtime Used
		private string _description = string.Empty;
		private bool _hasRemark = false;

		private string _TSBId = string.Empty;
		private string _TSBNameEN = string.Empty;
		private string _TSBNameTH = string.Empty;

		private int _CouponBHT35 = 0;
		private int _CouponBHT80 = 0;
		private int _CouponTotal = 0;
		private decimal _CouponBHTTotal = decimal.Zero;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public TSBCouponBalance() : base() { }

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
		public class FKs : TSBCouponBalance, IFKs<TSBCouponBalance>
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
		/// Gets Active TSB Coupon balance.
		/// </summary>
		/// <returns>
		/// Returns Current Active TSB Coupon balance. If not found returns null.
		/// </returns>
		public static NDbResult<TSBCouponBalance> GetTSBBalance()
		{
			var result = new NDbResult<TSBCouponBalance>();
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
			result = GetTSBBalance(tsb);
			return result;
		}
		/// <summary>
		/// Gets TSB Coupon Balance.
		/// </summary>
		/// <param name="tsb">The target TSB to get balance.</param>
		/// <returns>Returns TSB Coupon balance. If TSB not found returns null.</returns>
		public static NDbResult<TSBCouponBalance> GetTSBBalance(TSB tsb)
		{
			var result = new NDbResult<TSBCouponBalance>();
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
						SELECT * 
						  FROM TSBCouponBalanceView
						 WHERE TSBId = ? ";
					var ret = NQuery.Query<FKs>(cmd, tsb.TSBId).FirstOrDefault();
					var data = ret.ToModel();
					result.Success(data);
				}
				catch(Exception ex)
				{
					med.Err(ex);
					result.Error(ex);
				}
				return result;
			}
		}
		/// <summary>
		/// Gets All TSB Coupon Balance.
		/// </summary>
		/// <returns>Returns List fo all TSB Coupon balance.</returns>
		public static NDbResult<List<TSBCouponBalance>> GetTSBBalances()
		{
			var result = new NDbResult<List<TSBCouponBalance>>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					string cmd = @"
						SELECT * 
						  FROM TSBCouponBalanceView ";
					var rets = NQuery.Query<FKs>(cmd).ToList();
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