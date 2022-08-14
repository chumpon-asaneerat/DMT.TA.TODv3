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
	#region UserCreditHistory (For Query only)

	/// <summary>
	/// The UserCreditHistory Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("UserCreditHistory")]
	public class UserCreditHistory : NTable<UserCreditHistory>
	{
		#region Internal Variables

		private int _UserCreditId = 0;

		private string _TransDate = string.Empty;
		private string _BagCreateDate = string.Empty;
		private string _BagNo = string.Empty;
		private string _BeltNo = string.Empty;
		// for UserCreditBalance
		private DateTime? _ReceivedDate = new DateTime?();
		private bool? _Canceled = new bool?();
		private DateTime? _CancelDate = new DateTime?();
		private string _CancelUserId = string.Empty;
		private string _CancelFullNameEN = string.Empty;
		private string _CancelFullNameTH = string.Empty;
		private string _RevenueId = string.Empty;
		private string _RevenueBagNo = string.Empty;
		private string _RevenueBeltNo = string.Empty;


		private string _TSBId = string.Empty;
		private string _TSBNameEN = string.Empty;
		private string _TSBNameTH = string.Empty;

		private string _PlazaGroupId = string.Empty;
		private string _PlazaGroupNameEN = string.Empty;
		private string _PlazaGroupNameTH = string.Empty;

		private int? _ShiftId = new int?();
		private string _ShiftNameTH = string.Empty;
		private string _ShiftNameEN = string.Empty;

		private string _UserId = string.Empty;
		private string _FullNameEN = string.Empty;
		private string _FullNameTH = string.Empty;

		// Borrows Coin/Bill (Amount)
		private decimal _BorrowAmtST25 = 0;
		private decimal _BorrowAmtST50 = 0;
		private decimal _BorrowAmtBHT1 = 0;
		private decimal _BorrowAmtBHT2 = 0;
		private decimal _BorrowAmtBHT5 = 0;
		private decimal _BorrowAmtBHT10 = 0;
		private decimal _BorrowAmtBHT20 = 0;
		private decimal _BorrowAmtBHT50 = 0;
		private decimal _BorrowAmtBHT100 = 0;
		private decimal _BorrowAmtBHT500 = 0;
		private decimal _BorrowAmtBHT1000 = 0;

		private decimal _BorrowBHTTotal = decimal.Zero;

		// Returns Coin/Bill (Amount)
		private decimal _ReturnAmtST25 = 0;
		private decimal _ReturnAmtST50 = 0;
		private decimal _ReturnAmtBHT1 = 0;
		private decimal _ReturnAmtBHT2 = 0;
		private decimal _ReturnAmtBHT5 = 0;
		private decimal _ReturnAmtBHT10 = 0;
		private decimal _ReturnAmtBHT20 = 0;
		private decimal _ReturnAmtBHT50 = 0;
		private decimal _ReturnAmtBHT100 = 0;
		private decimal _ReturnAmtBHT500 = 0;
		private decimal _ReturnAmtBHT1000 = 0;

		private decimal _ReturnBHTTotal = decimal.Zero;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public UserCreditHistory() : base() { }

		#endregion

		#region Public Properties

		#region Common Key, Date, BagNo, BeltNo

		/// <summary>
		/// Gets or sets User Credit Id
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets User Credit Id.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("UserCreditId")]
		public virtual int UserCreditId
		{
			get
			{
				return _UserCreditId;
			}
			set
			{
				if (_UserCreditId != value)
				{
					_UserCreditId = value;
					this.RaiseChanged("UserCreditId");
				}
			}
		}
		/// <summary>
		/// Gets or sets Bag Create Date (date part only in string)
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Bag Create Date (date part only in string).")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("BagCreateDate")]
		public virtual string BagCreateDate
		{
			get
			{
				return _BagCreateDate;
			}
			set
			{
				if (_BagCreateDate != value)
				{
					_BagCreateDate = value;
					this.RaiseChanged("BagCreateDate");
				}
			}
		}
		/// <summary>
		/// Gets or sets Bag No
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Bag No.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("BagNo")]
		public virtual string BagNo
		{
			get
			{
				return _BagNo;
			}
			set
			{
				if (_BagNo != value)
				{
					_BagNo = value;
					this.RaiseChanged("BagNo");
				}
			}
		}
		/// <summary>
		/// Gets or sets Belt No
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Belt No.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("BeltNo")]
		public virtual string BeltNo
		{
			get
			{
				return _BeltNo;
			}
			set
			{
				if (_BeltNo != value)
				{
					_BeltNo = value;
					this.RaiseChanged("BeltNo");
				}
			}
		}
		/// <summary>
		/// Gets or sets Transaction Date (date part only in string)
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Transaction Date (date part only in string).")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("TransDate")]
		public virtual string TransDate
		{
			get
			{
				return _TransDate;
			}
			set
			{
				if (_TransDate != value)
				{
					_TransDate = value;
					this.RaiseChanged("TransDate");
				}
			}
		}

		#endregion

		#region ReceivedDate, Cancled, etc from user credit balance

		/// <summary>
		/// Gets or sets Received Date.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Received Date.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("ReceivedDate")]
		public virtual DateTime? ReceivedDate
		{
			get { return _ReceivedDate; }
			set
			{
				if (_ReceivedDate != value)
				{
					_ReceivedDate = value;
					// Raise event.
					this.RaiseChanged("ReceivedDate");
					this.RaiseChanged("ReceivedDateString");
					this.RaiseChanged("ReceivedDateTimeString");
				}
			}
		}
		/// <summary>
		/// Gets Received Date String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Received Date String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string ReceivedDateString
		{
			get
			{
				var ret = (!this._ReceivedDate.HasValue || this._ReceivedDate.Value == DateTime.MinValue) ?
					"" : this._ReceivedDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Received DateTime String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Received DateTime String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string ReceivedDateTimeString
		{
			get
			{
				var ret = (!this._ReceivedDate.HasValue || this._ReceivedDate.Value == DateTime.MinValue) ?
					"" : this._ReceivedDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
				return ret;
			}
			set { }
		}
		[Category("Cancel")]
		[Description("Gets or sets is cancel user credit.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("Canceled")]
		public virtual bool? Canceled
		{
			get { return _Canceled; }
			set
			{
				if (_Canceled != value)
				{
					_Canceled = value;
					this.RaiseChanged("Canceled");
				}
			}
		}
		/// <summary>
		/// Gets or sets Cancel User Id
		/// </summary>
		[Category("Cancel")]
		[Description("Gets or sets Cancel User Id.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("CancelUserId")]
		public virtual string CancelUserId
		{
			get
			{
				return _CancelUserId;
			}
			set
			{
				if (_CancelUserId != value)
				{
					_CancelUserId = value;
					this.RaiseChanged("CancelUserId");
				}
			}
		}
		/// <summary>
		/// Gets or sets Cancel User Full Name EN
		/// </summary>
		[Category("Cancel")]
		[Description("Gets or sets Cancel User Full Name EN.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("CancelFullNameEN")]
		public virtual string CancelFullNameEN
		{
			get
			{
				return _CancelFullNameEN;
			}
			set
			{
				if (_CancelFullNameEN != value)
				{
					_CancelFullNameEN = value;
					this.RaiseChanged("CancelFullNameEN");
				}
			}
		}
		/// <summary>
		/// Gets or sets Cancel User Full Name TH
		/// </summary>
		[Category("Cancel")]
		[Description("Gets or sets Cancel User Full Name TH.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("CancelFullNameTH")]
		public virtual string CancelFullNameTH
		{
			get
			{
				return _CancelFullNameTH;
			}
			set
			{
				if (_CancelFullNameTH != value)
				{
					_CancelFullNameTH = value;
					this.RaiseChanged("CancelFullNameTH");
				}
			}
		}
		/// <summary>
		/// Gets or sets Cancel Date.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Cancel Date.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("CancelDate")]
		public virtual DateTime? CancelDate
		{
			get { return _CancelDate; }
			set
			{
				if (_CancelDate != value)
				{
					_CancelDate = value;
					// Raise event.
					this.RaiseChanged("CancelDate");
					this.RaiseChanged("CancelDateString");
					this.RaiseChanged("CancelDateTimeString");
				}
			}
		}
		/// <summary>
		/// Gets Cancel Date String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Cancel Date String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string CancelDateString
		{
			get
			{
				var ret = (!this._CancelDate.HasValue || this._CancelDate.Value == DateTime.MinValue) ?
					"" : this._CancelDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Cancel DateTime String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Cancel DateTime String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string CancelDateTimeString
		{
			get
			{
				var ret = (!this._CancelDate.HasValue || this._CancelDate.Value == DateTime.MinValue) ?
					"" : this._CancelDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets or sets Revenue Id.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Revenue Id.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("RevenueId")]
		public virtual string RevenueId
		{
			get { return _RevenueId; }
			set
			{
				if (_RevenueId != value)
				{
					_RevenueId = value;
					// Raise event.
					this.RaiseChanged("RevenueId");
				}
			}
		}
		/// <summary>
		/// Gets or sets Revenue Bag Number.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Revenue Bag Number.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("RevenueBagNo")]
		public virtual string RevenueBagNo
		{
			get { return _RevenueBagNo; }
			set
			{
				if (_RevenueBagNo != value)
				{
					_RevenueBagNo = value;
					// Raise event.
					this.RaiseChanged("RevenueBagNo");
				}
			}
		}
		/// <summary>
		/// Gets or sets Revenue Belt Number.
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Revenue Belt Number.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("RevenueBeltNo")]
		public virtual string RevenueBeltNo
		{
			get { return _RevenueBeltNo; }
			set
			{
				if (_RevenueBeltNo != value)
				{
					_RevenueBeltNo = value;
					// Raise event.
					this.RaiseChanged("RevenueBeltNo");
				}
			}
		}

		#endregion

		#region Special property for report binding (Bag/Belt No)

		[Category("Common")]
		[Description("Gets Used Bag No.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string UsedBagNo
		{
			get 
			{
				if (!string.IsNullOrWhiteSpace(this.RevenueBagNo))
					return this.RevenueBagNo;
				else return this.BagNo;
			}
			set { }
		}

		[Category("Common")]
		[Description("Gets Used Belt No.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string UsedBeltNo
		{
			get 
			{
				if (!string.IsNullOrWhiteSpace(this.RevenueBeltNo))
					return this.RevenueBeltNo;
				else return this.BeltNo;
			}
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
		public virtual string TSBId
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

		#region PlazaGroup

		/// <summary>
		/// Gets or sets Plaza Group Id.
		/// </summary>
		[Category("Plaza Group")]
		[Description("Gets or sets Plaza Group Id.")]
		[ReadOnly(true)]
		[Ignore]
		[MaxLength(10)]
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
		/// <summary>
		/// Gets or sets Plaza Group Name EN.
		/// </summary>
		[Category("Plaza Group")]
		[Description("Gets or sets Plaza Group Name EN.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("PlazaGroupNameEN")]
		public virtual string PlazaGroupNameEN
		{
			get
			{
				return _PlazaGroupNameEN;
			}
			set
			{
				if (_PlazaGroupNameEN != value)
				{
					_PlazaGroupNameEN = value;
					this.RaiseChanged("PlazaGroupNameEN");
				}
			}
		}
		/// <summary>
		/// Gets or sets Plaza Group Name TH.
		/// </summary>
		[Category("Plaza Group")]
		[Description("Gets or sets Plaza Group Name TH.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("PlazaGroupNameTH")]
		public virtual string PlazaGroupNameTH
		{
			get
			{
				return _PlazaGroupNameTH;
			}
			set
			{
				if (_PlazaGroupNameTH != value)
				{
					_PlazaGroupNameTH = value;
					this.RaiseChanged("PlazaGroupNameTH");
				}
			}
		}

		#endregion

		#region Shift (From user credit balance)

		/// <summary>
		/// Gets or sets Shift Id.
		/// </summary>
		[Category("Shift")]
		[Description("Gets or sets Shift Id.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("ShiftId")]
		public virtual int? ShiftId
		{
			get
			{
				return _ShiftId;
			}
			set
			{
				if (_ShiftId != value)
				{
					_ShiftId = value;
					this.RaiseChanged("ShiftId");
				}
			}
		}
		/// <summary>
		/// Gets or sets Shift Name EN.
		/// </summary>
		[Category("Shift")]
		[Description("Gets or sets Shift Name EN.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("ShiftNameEN")]
		public virtual string ShiftNameEN
		{
			get
			{
				return _ShiftNameEN;
			}
			set
			{
				if (_ShiftNameEN != value)
				{
					_ShiftNameEN = value;
					this.RaiseChanged("ShiftNameEN");
				}
			}
		}
		/// <summary>
		/// Gets or sets Shift Name TH.
		/// </summary>
		[Category("Shift")]
		[Description("Gets or sets Shift Name TH.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("ShiftNameTH")]
		public virtual string ShiftNameTH
		{
			get
			{
				return _ShiftNameTH;
			}
			set
			{
				if (_ShiftNameTH != value)
				{
					_ShiftNameTH = value;
					this.RaiseChanged("ShiftNameTH");
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

		#region Borrow Coin/Bill (Amount)

		/// <summary>
		/// Gets or sets Borrow amount of .25 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of .25 baht coin.")]
		[PropertyMapName("BorrowST25")]
		[Ignore]
		[PropertyOrder(21)]
		public virtual decimal BorrowST25
		{
			get { return _BorrowAmtST25; }
			set
			{
				if (_BorrowAmtST25 != value)
				{
					_BorrowAmtST25 = value;
					// Raise event.
					this.RaiseChanged("BorrowST25");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of .50 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of .50 baht coin.")]
		[PropertyMapName("BorrowST50")]
		[Ignore]
		[PropertyOrder(22)]
		public virtual decimal BorrowST50
		{
			get { return _BorrowAmtST50; }
			set
			{
				if (_BorrowAmtST50 != value)
				{
					_BorrowAmtST50 = value;
					// Raise event.
					this.RaiseChanged("BorrowST50");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 1 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 1 baht coin.")]
		[PropertyMapName("BorrowBHT1")]
		[Ignore]
		[PropertyOrder(23)]
		public virtual decimal BorrowBHT1
		{
			get { return _BorrowAmtBHT1; }
			set
			{
				if (_BorrowAmtBHT1 != value)
				{
					_BorrowAmtBHT1 = value;
					// Raise event.
					this.RaiseChanged("BorrowBHT1");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 2 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 2 baht coin.")]
		[PropertyMapName("BorrowBHT2")]
		[Ignore]
		[PropertyOrder(24)]
		public virtual decimal BorrowBHT2
		{
			get { return _BorrowAmtBHT2; }
			set
			{
				if (_BorrowAmtBHT2 != value)
				{
					_BorrowAmtBHT2 = value;
					// Raise event.
					this.RaiseChanged("BorrowBHT2");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 5 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 5 baht coin.")]
		[PropertyMapName("BorrowBHT5")]
		[Ignore]
		[PropertyOrder(25)]
		public virtual decimal BorrowBHT5
		{
			get { return _BorrowAmtBHT5; }
			set
			{
				if (_BorrowAmtBHT5 != value)
				{
					_BorrowAmtBHT5 = value;
					// Raise event.
					this.RaiseChanged("BorrowBHT5");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 10 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 10 baht coin.")]
		[PropertyMapName("BorrowBHT10")]
		[Ignore]
		[PropertyOrder(26)]
		public virtual decimal BorrowBHT10
		{
			get { return _BorrowAmtBHT10; }
			set
			{
				if (_BorrowAmtBHT10 != value)
				{
					_BorrowAmtBHT10 = value;
					// Raise event.
					this.RaiseChanged("BorrowBHT10");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 20 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 20 baht bill.")]
		[PropertyMapName("BorrowBHT20")]
		[Ignore]
		[PropertyOrder(27)]
		public virtual decimal BorrowBHT20
		{
			get { return _BorrowAmtBHT20; }
			set
			{
				if (_BorrowAmtBHT20 != value)
				{
					_BorrowAmtBHT20 = value;
					// Raise event.
					this.RaiseChanged("BorrowBHT20");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 50 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 50 baht bill.")]
		[PropertyMapName("BorrowBHT50")]
		[Ignore]
		[PropertyOrder(28)]
		public virtual decimal BorrowBHT50
		{
			get { return _BorrowAmtBHT50; }
			set
			{
				if (_BorrowAmtBHT50 != value)
				{
					_BorrowAmtBHT50 = value;
					// Raise event.
					this.RaiseChanged("BorrowBHT50");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 100 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 100 baht bill.")]
		[PropertyMapName("BorrowBHT100")]
		[Ignore]
		[PropertyOrder(29)]
		public virtual decimal BorrowBHT100
		{
			get { return _BorrowAmtBHT100; }
			set
			{
				if (_BorrowAmtBHT100 != value)
				{
					_BorrowAmtBHT100 = value;
					// Raise event.
					this.RaiseChanged("BorrowBHT100");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 500 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 500 baht bill.")]
		[PropertyMapName("BorrowBHT500")]
		[Ignore]
		[PropertyOrder(30)]
		public virtual decimal BorrowBHT500
		{
			get { return _BorrowAmtBHT500; }
			set
			{
				if (_BorrowAmtBHT500 != value)
				{
					_BorrowAmtBHT500 = value;
					// Raise event.
					this.RaiseChanged("BorrowBHT500");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 1000 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 1000 baht bill.")]
		[PropertyMapName("BorrowBHT1000")]
		[Ignore]
		[PropertyOrder(31)]
		public virtual decimal BorrowBHT1000
		{
			get { return _BorrowAmtBHT1000; }
			set
			{
				if (_BorrowAmtBHT1000 != value)
				{
					_BorrowAmtBHT1000 = value;
					// Raise event.
					this.RaiseChanged("BorrowBHT1000");
				}
			}
		}

		#endregion

		#region Borrow Coin/Bill (Summary)

		/// <summary>
		/// Gets or sets Borrow total (coin/bill) value in baht.
		/// </summary>
		[Category("Coin/Bill (Summary)")]
		[Description("Gets or sets Borrow total (coin/bill) value in baht.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("BorrowBHTTotal")]
		public virtual decimal BorrowBHTTotal
		{
			get { return _BorrowBHTTotal; }
			set
			{
				if (_BorrowBHTTotal != value)
				{
					_BorrowBHTTotal = value;
					// Raise event.
					this.RaiseChanged("BorrowBHTTotal");
				}
			}
		}

		#endregion

		#region Return Coin/Bill (Amount)

		/// <summary>
		/// Gets or sets Return amount of .25 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of .25 baht coin.")]
		[PropertyMapName("ReturnST25")]
		[Ignore]
		[PropertyOrder(21)]
		public virtual decimal ReturnST25
		{
			get { return _ReturnAmtST25; }
			set
			{
				if (_ReturnAmtST25 != value)
				{
					_ReturnAmtST25 = value;
					// Raise event.
					this.RaiseChanged("ReturnST25");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of .50 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of .50 baht coin.")]
		[PropertyMapName("ReturnST50")]
		[Ignore]
		[PropertyOrder(22)]
		public virtual decimal ReturnST50
		{
			get { return _ReturnAmtST50; }
			set
			{
				if (_ReturnAmtST50 != value)
				{
					_ReturnAmtST50 = value;
					// Raise event.
					this.RaiseChanged("ReturnST50");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 1 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 1 baht coin.")]
		[PropertyMapName("ReturnBHT1")]
		[Ignore]
		[PropertyOrder(23)]
		public virtual decimal ReturnBHT1
		{
			get { return _ReturnAmtBHT1; }
			set
			{
				if (_ReturnAmtBHT1 != value)
				{
					_ReturnAmtBHT1 = value;
					// Raise event.
					this.RaiseChanged("ReturnBHT1");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 2 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 2 baht coin.")]
		[PropertyMapName("ReturnBHT2")]
		[Ignore]
		[PropertyOrder(24)]
		public virtual decimal ReturnBHT2
		{
			get { return _ReturnAmtBHT2; }
			set
			{
				if (_ReturnAmtBHT2 != value)
				{
					_ReturnAmtBHT2 = value;
					// Raise event.
					this.RaiseChanged("ReturnBHT2");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 5 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 5 baht coin.")]
		[PropertyMapName("ReturnBHT5")]
		[Ignore]
		[PropertyOrder(25)]
		public virtual decimal ReturnBHT5
		{
			get { return _ReturnAmtBHT5; }
			set
			{
				if (_ReturnAmtBHT5 != value)
				{
					_ReturnAmtBHT5 = value;
					// Raise event.
					this.RaiseChanged("ReturnBHT5");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 10 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 10 baht coin.")]
		[PropertyMapName("ReturnBHT10")]
		[Ignore]
		[PropertyOrder(26)]
		public virtual decimal ReturnBHT10
		{
			get { return _ReturnAmtBHT10; }
			set
			{
				if (_ReturnAmtBHT10 != value)
				{
					_ReturnAmtBHT10 = value;
					// Raise event.
					this.RaiseChanged("ReturnBHT10");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 20 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 20 baht bill.")]
		[PropertyMapName("ReturnBHT20")]
		[Ignore]
		[PropertyOrder(27)]
		public virtual decimal ReturnBHT20
		{
			get { return _ReturnAmtBHT20; }
			set
			{
				if (_ReturnAmtBHT20 != value)
				{
					_ReturnAmtBHT20 = value;
					// Raise event.
					this.RaiseChanged("ReturnBHT20");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 50 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 50 baht bill.")]
		[PropertyMapName("ReturnBHT50")]
		[Ignore]
		[PropertyOrder(28)]
		public virtual decimal ReturnBHT50
		{
			get { return _ReturnAmtBHT50; }
			set
			{
				if (_ReturnAmtBHT50 != value)
				{
					_ReturnAmtBHT50 = value;
					// Raise event.
					this.RaiseChanged("ReturnBHT50");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 100 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 100 baht bill.")]
		[PropertyMapName("ReturnBHT100")]
		[Ignore]
		[PropertyOrder(29)]
		public virtual decimal ReturnBHT100
		{
			get { return _ReturnAmtBHT100; }
			set
			{
				if (_ReturnAmtBHT100 != value)
				{
					_ReturnAmtBHT100 = value;
					// Raise event.
					this.RaiseChanged("ReturnBHT100");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 500 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 500 baht bill.")]
		[PropertyMapName("ReturnBHT500")]
		[Ignore]
		[PropertyOrder(30)]
		public virtual decimal ReturnBHT500
		{
			get { return _ReturnAmtBHT500; }
			set
			{
				if (_ReturnAmtBHT500 != value)
				{
					_ReturnAmtBHT500 = value;
					// Raise event.
					this.RaiseChanged("ReturnBHT500");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 1000 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 1000 baht bill.")]
		[PropertyMapName("ReturnBHT1000")]
		[Ignore]
		[PropertyOrder(31)]
		public virtual decimal ReturnBHT1000
		{
			get { return _ReturnAmtBHT1000; }
			set
			{
				if (_ReturnAmtBHT1000 != value)
				{
					_ReturnAmtBHT1000 = value;
					// Raise event.
					this.RaiseChanged("ReturnBHT1000");
				}
			}
		}

		#endregion

		#region Return Coin/Bill (Summary)

		/// <summary>
		/// Gets or sets Return total (coin/bill) value in baht.
		/// </summary>
		[Category("Coin/Bill (Summary)")]
		[Description("Gets or sets Return total (coin/bill) value in baht.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("ReturnBHTTotal")]
		public virtual decimal ReturnBHTTotal
		{
			get { return _ReturnBHTTotal; }
			set
			{
				if (_ReturnBHTTotal != value)
				{
					_ReturnBHTTotal = value;
					// Raise event.
					this.RaiseChanged("ReturnBHTTotal");
				}
			}
		}

		#endregion

		#endregion

		#region Internal Class

		/// <summary>
		/// The internal FKs class for query data.
		/// </summary>
		public class FKs : UserCreditHistory, IFKs<UserCreditHistory>
		{
			#region Common Key, Date, BagNo, BeltNo

			/// <summary>
			/// Gets or sets User Credit Id
			/// </summary>
			[PropertyMapName("UserCreditId")]
			public override int UserCreditId
			{
				get { return base.UserCreditId; }
				set { base.UserCreditId = value; }
			}
			/// <summary>
			/// Gets or sets Bag Create Date  (date part only in string)
			/// </summary>
			[MaxLength(20)]
			[PropertyMapName("BagCreateDate")]
			public override string BagCreateDate
			{
				get { return base.BagCreateDate; }
				set { base.BagCreateDate = value; }
			}
			/// <summary>
			/// Gets or sets Bag No
			/// </summary>
			[MaxLength(30)]
			[PropertyMapName("BagNo")]
			public override string BagNo
			{
				get { return base.BagNo; }
				set { base.BagNo = value; }
			}
			/// <summary>
			/// Gets or sets Belt No
			/// </summary>
			[MaxLength(30)]
			[PropertyMapName("BeltNo")]
			public override string BeltNo
			{
				get { return base.BeltNo; }
				set { base.BeltNo = value; }
			}
			/// <summary>
			/// Gets or sets Transaction Date  (date part only in string)
			/// </summary>
			[MaxLength(20)]
			[PropertyMapName("TransDate")]
			public override string TransDate
			{
				get { return base.TransDate; }
				set { base.TransDate = value; }
			}

			#endregion

			#region ReceivedDate, Cancled, etc from user credit balance

			[Indexed]
			[PropertyMapName("ReceivedDate")]
			public override DateTime? ReceivedDate
			{
				get { return base.ReceivedDate; }
				set { base.ReceivedDate = value; }
			}
			[PropertyMapName("Canceled")]
			public override bool? Canceled
			{
				get { return base.Canceled; }
				set { base.Canceled = value; }
			}
			[MaxLength(10)]
			[PropertyMapName("CancelUserId")]
			public override string CancelUserId
			{
				get { return base.CancelUserId; }
				set { base.CancelUserId = value; }
			}
			[MaxLength(150)]
			[PropertyMapName("CancelFullNameEN")]
			public override string CancelFullNameEN
			{
				get { return base.CancelFullNameEN; }
				set { base.CancelFullNameEN = value; }
			}
			[MaxLength(150)]
			[PropertyMapName("CancelFullNameTH")]
			public override string CancelFullNameTH
			{
				get { return base.CancelFullNameTH; }
				set { base.CancelFullNameTH = value; }
			}
			[ReadOnly(true)]
			[Indexed]
			[PropertyMapName("CancelDate")]
			public override DateTime? CancelDate
			{
				get { return base.CancelDate; }
				set { base.CancelDate = value; }
			}
			[MaxLength(20)]
			[PropertyMapName("RevenueId")]
			public override string RevenueId
			{
				get { return base.RevenueId; }
				set { base.RevenueId = value; }
			}
			[MaxLength(10)]
			[PropertyMapName("RevenueBagNo")]
			public override string RevenueBagNo
			{
				get { return base.RevenueBagNo; }
				set { base.RevenueBagNo = value; }
			}
			[MaxLength(20)]
			[PropertyMapName("RevenueBeltNo")]
			public override string RevenueBeltNo
			{
				get { return base._RevenueBeltNo; }
				set { base._RevenueBeltNo = value; }
			}

			#endregion

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

			#region PlazaGroup

			/// <summary>
			/// Gets or sets Plaza Group Id.
			/// </summary>
			[MaxLength(10)]
			[PropertyMapName("PlazaGroupId")]
			public override string PlazaGroupId
			{
				get { return base.PlazaGroupId; }
				set { base.PlazaGroupId = value; }
			}
			/// <summary>
			/// Gets or sets Plaza Group Name EN.
			/// </summary>
			[MaxLength(100)]
			[PropertyMapName("PlazaGroupNameEN")]
			public override string PlazaGroupNameEN
			{
				get { return base.PlazaGroupNameEN; }
				set { base.PlazaGroupNameEN = value; }
			}
			/// <summary>
			/// Gets or sets Plaza Group Name TH.
			/// </summary>
			[MaxLength(100)]
			[PropertyMapName("PlazaGroupNameTH")]
			public override string PlazaGroupNameTH
			{
				get { return base.PlazaGroupNameTH; }
				set { base.PlazaGroupNameTH = value; }
			}

			#endregion

			#region Shift (From user credit balance)

			//[Indexed]
			[PropertyMapName("ShiftId")]
			public override int? ShiftId
			{
				get { return base.ShiftId; }
				set { base.ShiftId = value; }
			}
			/// <summary>
			/// Gets or sets Shift Name TH.
			/// </summary>
			[MaxLength(50)]
			[PropertyMapName("ShiftNameTH")]
			public override string ShiftNameTH
			{
				get
				{
					if (!this.ShiftId.HasValue)
						return "ไม่ระบุ";
					return base.ShiftNameTH;
				}
				set { base.ShiftNameTH = value; }
			}
			/// <summary>
			/// Gets or sets Shift Name EN.
			/// </summary>
			[MaxLength(50)]
			[PropertyMapName("ShiftNameEN")]
			public override string ShiftNameEN
			{
				get
				{
					if (!this.ShiftId.HasValue)
						return "[None]";
					return base.ShiftNameEN;
				}
				set { base.ShiftNameEN = value; }
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

			#region Borrow Coin/Bill (Amount)

			/// <summary>
			/// Gets or sets Borrow amount of .25 baht coin.
			/// </summary>
			[PropertyMapName("BorrowST25")]
			public override decimal BorrowST25
			{
				get { return base.BorrowST25; }
				set { base.BorrowST25 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of .50 baht coin.
			/// </summary>
			[PropertyMapName("BorrowST50")]
			public override decimal BorrowST50
			{
				get { return base.BorrowST50; }
				set { base.BorrowST50 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 1 baht coin.
			/// </summary>
			[PropertyMapName("BorrowBHT1")]
			public override decimal BorrowBHT1
			{
				get { return base.BorrowBHT1; }
				set { base.BorrowBHT1 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 2 baht coin.
			/// </summary>
			[PropertyMapName("BorrowBHT2")]
			public override decimal BorrowBHT2
			{
				get { return base.BorrowBHT2; }
				set { base.BorrowBHT2 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 5 baht coin.
			/// </summary>
			[PropertyMapName("BorrowBHT5")]
			public override decimal BorrowBHT5
			{
				get { return base.BorrowBHT5; }
				set { base.BorrowBHT5 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 10 baht coin.
			/// </summary>
			[PropertyMapName("BorrowBHT10")]
			public override decimal BorrowBHT10
			{
				get { return base.BorrowBHT10; }
				set { base.BorrowBHT10 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 20 baht bill.
			/// </summary>
			[PropertyMapName("BorrowBHT20")]
			public override decimal BorrowBHT20
			{
				get { return base.BorrowBHT20; }
				set { base.BorrowBHT20 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 50 baht bill.
			/// </summary>
			[PropertyMapName("BorrowBHT50")]
			public override decimal BorrowBHT50
			{
				get { return base.BorrowBHT50; }
				set { base.BorrowBHT50 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 100 baht bill.
			/// </summary>
			[PropertyMapName("BorrowBHT100")]
			public override decimal BorrowBHT100
			{
				get { return base.BorrowBHT100; }
				set { base.BorrowBHT100 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 500 baht bill.
			/// </summary>
			[PropertyMapName("BorrowBHT500")]
			public override decimal BorrowBHT500
			{
				get { return base.BorrowBHT500; }
				set { base.BorrowBHT500 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 1000 baht bill.
			/// </summary>
			[PropertyMapName("BorrowBHT1000")]
			public override decimal BorrowBHT1000
			{
				get { return base.BorrowBHT1000; }
				set { base.BorrowBHT1000 = value; }
			}

			#endregion

			#region Borrow Coin/Bill (Summary)

			/// <summary>
			/// Gets or sets Borrow total (coin/bill) value in baht.
			/// </summary>
			[PropertyMapName("BorrowBHTTotal")]
			public override decimal BorrowBHTTotal
			{
				get { return base.BorrowBHTTotal; }
				set { base.BorrowBHTTotal = value; }
			}

			#endregion

			#region Return Coin/Bill (Amount)

			/// <summary>
			/// Gets or sets Return amount of .25 baht coin.
			/// </summary>
			[PropertyMapName("ReturnST25")]
			public override decimal ReturnST25
			{
				get { return base.ReturnST25; }
				set { base.ReturnST25 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of .50 baht coin.
			/// </summary>
			[PropertyMapName("ReturnST50")]
			public override decimal ReturnST50
			{
				get { return base.ReturnST50; }
				set { base.ReturnST50 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 1 baht coin.
			/// </summary>
			[PropertyMapName("ReturnBHT1")]
			public override decimal ReturnBHT1
			{
				get { return base.ReturnBHT1; }
				set { base.ReturnBHT1 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 2 baht coin.
			/// </summary>
			[PropertyMapName("ReturnBHT2")]
			public override decimal ReturnBHT2
			{
				get { return base.ReturnBHT2; }
				set { base.ReturnBHT2 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 5 baht coin.
			/// </summary>
			[PropertyMapName("ReturnBHT5")]
			public override decimal ReturnBHT5
			{
				get { return base.ReturnBHT5; }
				set { base.ReturnBHT5 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 10 baht coin.
			/// </summary>
			[PropertyMapName("ReturnBHT10")]
			public override decimal ReturnBHT10
			{
				get { return base.ReturnBHT10; }
				set { base.ReturnBHT10 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 20 baht bill.
			/// </summary>
			[PropertyMapName("ReturnBHT20")]
			public override decimal ReturnBHT20
			{
				get { return base.ReturnBHT20; }
				set { base.ReturnBHT20 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 50 baht bill.
			/// </summary>
			[PropertyMapName("ReturnBHT50")]
			public override decimal ReturnBHT50
			{
				get { return base.ReturnBHT50; }
				set { base.ReturnBHT50 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 100 baht bill.
			/// </summary>
			[PropertyMapName("ReturnBHT100")]
			public override decimal ReturnBHT100
			{
				get { return base.ReturnBHT100; }
				set { base.ReturnBHT100 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 500 baht bill.
			/// </summary>
			[PropertyMapName("ReturnBHT500")]
			public override decimal ReturnBHT500
			{
				get { return base.ReturnBHT500; }
				set { base.ReturnBHT500 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 1000 baht bill.
			/// </summary>
			[PropertyMapName("ReturnBHT1000")]
			public override decimal ReturnBHT1000
			{
				get { return base.ReturnBHT1000; }
				set { base.ReturnBHT1000 = value; }
			}

			#endregion

			#region Return Coin/Bill (Summary)

			/// <summary>
			/// Gets or sets Return total (coin/bill) value in baht.
			/// </summary>
			[PropertyMapName("ReturnBHTTotal")]
			public override decimal ReturnBHTTotal
			{
				get { return base.ReturnBHTTotal; }
				set { base.ReturnBHTTotal = value; }
			}

			#endregion
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Get UserCredit Histories.
		/// </summary>
		/// <param name="dt">The target date.</param>
		/// <returns>Returns NDbResult.</returns>
		public static NDbResult<List<UserCreditHistory>> GetUserCreditHistories(DateTime dt)
		{
			var result = new NDbResult<List<UserCreditHistory>>();
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
			result = GetUserCreditHistories(tsb, dt);
			return result;
		}
		/// <summary>
		/// Get UserCredit Histories.
		/// </summary>
		/// <param name="tsb">The TSB instance.</param>
		/// <param name="dt">The target date.</param>
		/// <returns>Returns NDbResult.</returns>
		public static NDbResult<List<UserCreditHistory>> GetUserCreditHistories(TSB tsb, DateTime dt)
		{
			var result = new NDbResult<List<UserCreditHistory>>();
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
					  FROM UserCreditHistoryView
					 WHERE TSBId = ? 
					   AND TransDate = ? 
					   AND (Canceled IS NULL OR Canceled <> 1) ";

					string dStr = dt.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					var rets = NQuery.Query<FKs>(cmd,
						tsb.TSBId, dStr).ToList();
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
