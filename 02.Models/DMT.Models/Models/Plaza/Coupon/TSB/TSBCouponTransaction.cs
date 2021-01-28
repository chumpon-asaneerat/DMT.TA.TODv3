#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

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
	#region TSBCouponTransaction

	/// <summary>
	/// The TSBCouponTransaction Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("TSBCouponTransaction")]
	public class TSBCouponTransaction : NTable<TSBCouponTransaction>
	{
		#region Enum

		/// <summary>
		/// The Transaction Types Enum.
		/// </summary>
		public enum TransactionTypes : int
		{
			/// <summary>
			/// Cancel or Remove
			/// </summary>
			CancelOrRemove = 0,
			/// <summary>
			/// TSB Stock
			/// </summary>
			Stock = 1,
			/// <summary>
			/// Borrow By User on Lane
			/// </summary>
			Lane = 2,
			/// <summary>
			/// Sold By User on Lane
			/// </summary>
			SoldByLane = 3,
			/// <summary>
			/// Sold By Supervisor on TSB
			/// </summary>
			SoldByTSB = 4
		}
		/// <summary>
		/// The Finished Flags Enum.
		/// </summary>
		public enum FinishedFlags : int
		{
			/// <summary>
			/// Completed
			/// </summary>
			Completed = 0,
			/// <summary>
			/// Avaliable
			/// </summary>
			Avaliable = 1
		}

		#endregion

		#region Internal Variables

		// server pk.
		private int _CouponPK = 0;

		private int _TransactionId = 0;
		private DateTime _TransactionDate = DateTime.MinValue;
		private TransactionTypes _TransactionType = TransactionTypes.Stock;

		// Coupon 
		private string _CouponId = string.Empty;
		private CouponType _CouponType = CouponType.BHT35;
		private decimal _Price = 665;
		// TSB
		private string _TSBId = string.Empty;
		private string _TSBNameEN = string.Empty;
		private string _TSBNameTH = string.Empty;
		// User
		private string _UserId = string.Empty;
		private string _FullNameEN = string.Empty;
		private string _FullNameTH = string.Empty;
		private DateTime _UserReceivedDate = DateTime.MinValue;
		// Sold
		private DateTime _SoldDate = DateTime.MinValue;
		private string _SoldBy = string.Empty;
		private string _SoldByFullNameEN = string.Empty;
		private string _SoldByFullNameTH = string.Empty;

		private FinishedFlags _FinishFlag = FinishedFlags.Avaliable;

		private int _Status = 0;
		private DateTime _LastUpdate = DateTime.MinValue;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public TSBCouponTransaction() : base() { }

		#endregion

		#region Public Properties

		#region Runtime

		/// <summary>
		/// Gets Foreground color.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets Foreground color.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyMapName("Foreground")]
		public SolidColorBrush Foreground
		{
			get 
			{
				bool isSold = TransactionType == TransactionTypes.SoldByLane || 
					TransactionType == TransactionTypes.SoldByTSB;
				return (!isSold) ? BlackForeground : RedForeground; 
			}
			set { }
		}

		#endregion

		#region Server

		/// <summary>
		/// Gets or sets CouponPK.
		/// </summary>
		[Category("Server")]
		[Description(" Gets or sets CouponPK.")]
		[ReadOnly(true)]
		[PropertyMapName("CouponPK")]
		public int CouponPK
		{
			get
			{
				return _CouponPK;
			}
			set
			{
				if (_CouponPK != value)
				{
					_CouponPK = value;
					this.RaiseChanged("CouponPK");
				}
			}
		}

		#endregion

		#region Common

		/// <summary>
		/// Gets or sets TransactionId
		/// </summary>
		[Category("Common")]
		[Description(" Gets or sets TransactionId")]
		[ReadOnly(true)]
		[PrimaryKey, AutoIncrement]
		[PropertyMapName("TransactionId")]
		public int TransactionId
		{
			get
			{
				return _TransactionId;
			}
			set
			{
				if (_TransactionId != value)
				{
					_TransactionId = value;
					this.RaiseChanged("TransactionId");
				}
			}
		}
		/// <summary>
		/// Gets or sets Transaction Date.
		/// </summary>
		[Category("Common")]
		[Description(" Gets or sets Transaction Date")]
		[ReadOnly(true)]
		[PropertyMapName("TransactionDate")]
		public DateTime TransactionDate
		{
			get
			{
				return _TransactionDate;
			}
			set
			{
				if (_TransactionDate != value)
				{
					_TransactionDate = value;
					this.RaiseChanged("TransactionDate");
					this.RaiseChanged("TransactionDateString");
					this.RaiseChanged("TransactionTimeString");
					this.RaiseChanged("TransactionDateTimeString");
				}
			}
		}
		/// <summary>
		/// Gets Transaction Date String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Transaction Date String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string TransactionDateString
		{
			get
			{
				var ret = (this.TransactionDate == DateTime.MinValue) ? "" : this.TransactionDate.ToThaiDateTimeString("dd/MM/yyyy");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Transaction Time String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Transaction Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string TransactionTimeString
		{
			get
			{
				var ret = (this.TransactionDate == DateTime.MinValue) ? "" : this.TransactionDate.ToThaiTimeString();
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Transaction Date Time String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Transaction Date Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string TransactionDateTimeString
		{
			get
			{
				var ret = (this.TransactionDate == DateTime.MinValue) ? "" : this.TransactionDate.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets or sets Transaction Type.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Transaction Type.")]
		[ReadOnly(true)]
		[PropertyMapName("TransactionType")]
		public TransactionTypes TransactionType
		{
			get { return _TransactionType; }
			set
			{
				if (_TransactionType != value)
				{
					_TransactionType = value;
					this.RaiseChanged("TransactionType");
					this.RaiseChanged("Foreground"); // raise event.
				}
			}
		}
		/// <summary>
		/// Gets or sets Finish Flag (0: Completed, 1: Avaliable).
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Is Finished (0: Completed, 1: Avaliable).")]
		[ReadOnly(true)]
		[PropertyMapName("FinishFlag")]
		public virtual FinishedFlags FinishFlag
		{
			get
			{
				return _FinishFlag;
			}
			set
			{
				if (_FinishFlag != value)
				{
					_FinishFlag = value;
					this.RaiseChanged("FinishFlag");
				}
			}
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

		#region User Receive Date

		/// <summary>
		/// Gets or sets User Receive Date.
		/// </summary>
		[Category("User")]
		[Description(" Gets or sets User Receive Date")]
		[ReadOnly(true)]
		[PropertyMapName("UserReceiveDate")]
		public DateTime UserReceiveDate
		{
			get
			{
				return _UserReceivedDate;
			}
			set
			{
				if (_UserReceivedDate != value)
				{
					_UserReceivedDate = value;
					this.RaiseChanged("UserReceiveDate");
					this.RaiseChanged("UserReceiveDateString");
					this.RaiseChanged("UserReceiveTimeString");
					this.RaiseChanged("UserReceiveDateTimeString");
				}
			}
		}
		/// <summary>
		/// Gets User Receive Date String.
		/// </summary>
		[Category("User")]
		[Description("Gets User Receive Date String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string UserReceiveDateString
		{
			get
			{
				var ret = (this.UserReceiveDate == DateTime.MinValue) ? "" : this.UserReceiveDate.ToThaiDateTimeString("dd/MM/yyyy");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets User Receive Time String.
		/// </summary>
		[Category("User")]
		[Description("Gets User Receive Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string UserReceiveTimeString
		{
			get
			{
				var ret = (this.UserReceiveDate == DateTime.MinValue) ? "" : this.UserReceiveDate.ToThaiTimeString();
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets User Receive Date Time String.
		/// </summary>
		[Category("User")]
		[Description("Gets User Receive Date Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string UserReceiveDateTimeString
		{
			get
			{
				var ret = (this.UserReceiveDate == DateTime.MinValue) ? "" : this.UserReceiveDate.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
				return ret;
			}
			set { }
		}

		#endregion

		#region Sold

		/// <summary>
		/// Gets or sets SoldBy (UserId)
		/// </summary>
		[Category("Sold")]
		[Description("Gets or sets SoldBy (UserId)")]
		[ReadOnly(true)]
		[MaxLength(10)]
		[PropertyMapName("SoldBy")]
		public virtual string SoldBy
		{
			get
			{
				return _SoldBy;
			}
			set
			{
				if (_SoldBy != value)
				{
					_SoldBy = value;
					this.RaiseChanged("UserId");
				}
			}
		}
		/// <summary>
		/// Gets or sets Sold User Full Name EN.
		/// </summary>
		[Category("Sold")]
		[Description("Gets or sets Sold User Full Name EN.")]
		[ReadOnly(true)]
		[MaxLength(150)]
		[PropertyMapName("SoldByFullNameEN")]
		public virtual string SoldByFullNameEN
		{
			get
			{
				return _SoldByFullNameEN;
			}
			set
			{
				if (_SoldByFullNameEN != value)
				{
					_SoldByFullNameEN = value;
					this.RaiseChanged("SoldByFullNameEN");
				}
			}
		}
		/// <summary>
		/// Gets or sets Sold User Full Name TH.
		/// </summary>
		[Category("Sold")]
		[Description("Gets or sets Sold User Full Name TH.")]
		[ReadOnly(true)]
		[MaxLength(150)]
		[PropertyMapName("SoldByFullNameTH")]
		public virtual string SoldByFullNameTH
		{
			get
			{
				return _SoldByFullNameTH;
			}
			set
			{
				if (_SoldByFullNameTH != value)
				{
					_SoldByFullNameTH = value;
					this.RaiseChanged("SoldByFullNameTH");
				}
			}
		}
		/// <summary>
		/// Gets or sets Sold Date.
		/// </summary>
		[Category("Sold")]
		[Description(" Gets or sets Sold Date")]
		[ReadOnly(true)]
		[PropertyMapName("SoldDate")]
		public DateTime SoldDate
		{
			get
			{
				return _SoldDate;
			}
			set
			{
				if (_SoldDate != value)
				{
					_SoldDate = value;
					this.RaiseChanged("SoldDate");
					this.RaiseChanged("SoldDateString");
					this.RaiseChanged("SoldTimeString");
					this.RaiseChanged("SoldDateTimeString");
				}
			}
		}
		/// <summary>
		/// Gets Sold Date String.
		/// </summary>
		[Category("Sold")]
		[Description("Gets Sold Date String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string SoldDateString
		{
			get
			{
				var ret = (this.SoldDate == DateTime.MinValue) ? "" : this.SoldDate.ToThaiDateTimeString("dd/MM/yyyy");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Sold Time String.
		/// </summary>
		[Category("Sold")]
		[Description("Gets Sold Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string SoldTimeString
		{
			get
			{
				var ret = (this.SoldDate == DateTime.MinValue) ? "" : this.SoldDate.ToThaiTimeString();
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Sold Date Time String.
		/// </summary>
		[Category("Sold")]
		[Description("Gets Sold Date Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string SoldDateTimeString
		{
			get
			{
				var ret = (this.SoldDate == DateTime.MinValue) ? "" : this.SoldDate.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
				return ret;
			}
			set { }
		}

		#endregion

		#region Coupon

		/// <summary>
		/// Gets or sets coupon book id.
		/// </summary>
		[Category("Coupon")]
		[Description("Gets or sets coupon book id.")]
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
		/// <summary>
		/// Gets or sets number of coupon type.
		/// </summary>
		[Category("Coupon")]
		[Description("Gets or sets number of coupon type.")]
		[ReadOnly(true)]
		[PropertyMapName("CouponType")]
		public CouponType CouponType
		{
			get { return _CouponType; }
			set
			{
				if (_CouponType != value)
				{
					_CouponType = value;
					// Raise event.
					this.RaiseChanged("CouponType");
					this.RaiseChanged("CouponTypeString");
				}
			}
		}
		/// <summary>
		/// Gets Coupon Type String.
		/// </summary>
		[Category("Coupon")]
		[Description("Gets Coupon Type String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string CouponTypeString
		{
			get
			{
				if (CouponType == CouponType.BHT35)
					return "35";
				else if (CouponType == CouponType.BHT80)
					return "80";
				else return "N/A"; // N/A
			}
			set { }
		}
		/// <summary>
		/// Gets or sets number of coupon price.
		/// </summary>
		[Category("Coupon")]
		[Description("Gets or sets number of coupon price.")]
		[ReadOnly(true)]
		[PropertyMapName("Price")]
		public decimal Price
		{
			get { return _Price; }
			set
			{
				if (_Price != value)
				{
					_Price = value;
					// Raise event.
					this.RaiseChanged("Price");
				}
			}
		}

		#endregion

		#region Status (DC)

		/// <summary>
		/// Gets or sets Status (1 = Sync, 0 = Unsync, etc..)
		/// </summary>
		[Category("DataCenter")]
		[Description("Gets or sets Status (1 = Sync, 0 = Unsync, etc..)")]
		[ReadOnly(true)]
		[PropertyMapName("Status", typeof(TSBCouponTransaction))]
		public int Status
		{
			get
			{
				return _Status;
			}
			set
			{
				if (_Status != value)
				{
					_Status = value;
					this.RaiseChanged("Status");
				}
			}
		}
		/// <summary>
		/// Gets or sets LastUpdated (Sync to DC).
		/// </summary>
		[Category("DataCenter")]
		[Description("Gets or sets LastUpdated (Sync to DC).")]
		[ReadOnly(true)]
		[PropertyMapName("LastUpdate", typeof(TSBCouponTransaction))]
		public DateTime LastUpdate
		{
			get { return _LastUpdate; }
			set
			{
				if (_LastUpdate != value)
				{
					_LastUpdate = value;
					this.RaiseChanged("LastUpdate");
				}
			}
		}

		#endregion

		#endregion

		#region Internal Class

		/// <summary>
		/// The internal FKs class for query data.
		/// </summary>
		public class FKs : TSBCouponTransaction, IFKs<TSBCouponTransaction>
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
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets Active TSB Coupon transactions (all status).
		/// </summary>
		/// <returns>
		/// Returns Current Active TSB Coupon transactions. If not found returns null.
		/// </returns>
		public static NDbResult<List<TSBCouponTransaction>> GetTSBCouponTransactions()
		{
			var result = new NDbResult<List<TSBCouponTransaction>>();
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
			result = GetTSBCouponTransactions(tsb);
			return result;
		}
		/// <summary>
		/// Gets TSB Coupon transactions (all status).
		/// </summary>
		/// <param name="tsb">The target TSB to get coupon transaction.</param>
		/// <returns>Returns TSB Coupon transactions. If TSB not found returns null.</returns>
		public static NDbResult<List<TSBCouponTransaction>> GetTSBCouponTransactions(TSB tsb)
		{
			var result = new NDbResult<List<TSBCouponTransaction>>();
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
					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM TSBCouponTransactionView ";
					cmd += " WHERE TSBId = ? ";
					cmd += "   AND FinishFlag = 1 ";

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
		/// Save Transaction.
		/// </summary>
		/// <param name="value">The transaction instance.</param>
		/// <returns>Returns Save TSB Coupon transactions.</returns>
		public static NDbResult<TSBCouponTransaction> SaveTransaction(TSBCouponTransaction value)
		{
			var result = new NDbResult<TSBCouponTransaction>();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			if (null == value)
			{
				result.ParameterIsNull();
				return result;
			}
			if (value.TransactionDate == DateTime.MinValue)
			{
				value.TransactionDate = DateTime.Now;
			}
			return TSBCouponTransaction.Save(value);
		}
		/// <summary>
		/// Save Transactions.
		/// </summary>
		/// <param name="values">The List of transaction instance.</param>
		/// <returns>Returns NDbResult.</returns>
		public static NDbResult SaveTransactions(
			List<TSBCouponTransaction> values)
		{
			var result = new NDbResult();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			if (null == values)
			{
				result.ParameterIsNull();
				return result;
			}

			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					db.BeginTransaction();
					values.ForEach(value =>
					{
						SaveTransaction(value);
					});
					db.Commit();
				}
				catch (Exception ex)
				{
					med.Err(ex);
					result.Error(ex);
				}

				return result;
			}
		}

		public static NDbResult SyncTransaction(TSBCouponTransaction value)
		{
			var result = new NDbResult();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			if (null == value)
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
					cmd += "  FROM TSBCouponTransactionView ";
					cmd += " WHERE TSBId = ? ";
					cmd += "   AND CouponId = ? ";

					var ret = NQuery.Query<FKs>(cmd, 
						value.TSBId, value.CouponId).FirstOrDefault().ToModel();
					if (null != ret)
					{
						// exist so set id for update.
						value.TransactionId = ret.TransactionId;
					}

					return TSBCouponTransaction.Save(value);
				}
				catch (Exception ex)
				{
					med.Err(ex);
					result.Error(ex);
				}
				return result;
			}
		}

		public static NDbResult SyncTransactions(List<TSBCouponTransaction> values)
		{
			var result = new NDbResult();
			SQLiteConnection db = Default;
			if (null == db)
			{
				result.DbConenctFailed();
				return result;
			}
			if (null == values)
			{
				result.ParameterIsNull();
				return result;
			}

			lock (sync)
			{
				MethodBase med = MethodBase.GetCurrentMethod();
				try
				{
					db.BeginTransaction();
					foreach (var r in values)
					{
						SyncTransaction(r);
					}
					db.Commit();
					result.Success();
				}
				catch (Exception ex)
				{
					med.Err(ex);
					result.Error(ex);
					db.Rollback();
				}
				return result;
			}
		}

		#endregion
	}

	#endregion
}
