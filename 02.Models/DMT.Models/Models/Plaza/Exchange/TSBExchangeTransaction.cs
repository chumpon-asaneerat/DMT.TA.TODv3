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
	#region TSBExchangeTransaction

	/// <summary>
	/// The TSBExchangeTransaction Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("TSBExchangeTransaction")]
	public class TSBExchangeTransaction : NTable<TSBExchangeTransaction>
	{
		#region Enum

		/// <summary>
		/// The TSB Exchange Transaction Types enum.
		/// </summary>
		public enum TransactionTypes : int
		{
			/// <summary>
			/// None (for filter only).
			/// </summary>
			None = -1,
			/// <summary>
			/// Request.
			/// </summary>
			Request = 1,
			/// <summary>
			/// Canceled (unused).
			/// </summary>
			Canceled = 2,
			/// <summary>
			/// Approve.
			/// </summary>
			Approve = 3,
			/// <summary>
			/// Reject (unused).
			/// </summary>
			Reject = 4,
			/// <summary>
			/// Received.
			/// </summary>
			Received = 5,
			/// <summary>
			/// Exchange (send back to account dept.)
			/// </summary>
			Exchange = 6,
			/// <summary>
			/// Return from plaza but account not update status.
			/// </summary>
			Return = 7,
			/// <summary>
			/// Completed (reserved).
			/// </summary>
			Completed = 9
		}
		/// <summary>
		/// The Finished Flags enum.
		/// </summary>
		public enum FinishedFlags : int
		{
			/// <summary>
			/// Avaliable.
			/// </summary>
			Avaliable = 0,
			/// <summary>
			/// Completed.
			/// </summary>
			Completed = 1
		}

		#endregion

		#region Internal Variables

		private int _TransactionId = 0;
		private DateTime _TransactionDate = DateTime.MinValue;
		private TransactionTypes _TransactionType = TransactionTypes.Request;

		private Guid _GroupId = Guid.Empty; // Exchange group Id.
											// TSB
		private string _TSBId = string.Empty;
		private string _TSBNameEN = string.Empty;
		private string _TSBNameTH = string.Empty;
		// วงเงินอนุมัติ เป็นวงเงินที่ บ/ช กำหนดให้แต่ละด่าน เป็นค่าสูงสุดที่แต่ละด่านจะมีได้ โดยยอดนี้จะต้อง มากกว่าหรือเท่ากับ ยอดรวม + เงินยืมเพิ่ม
		private decimal _MaxCredit = decimal.Zero;

		// User
		private string _UserId = string.Empty;
		private string _FullNameEN = string.Empty;
		private string _FullNameTH = string.Empty;
		private DateTime _UserReceivedDate = DateTime.MinValue;

		// Coin/Bill (Amount)
		private decimal _AmtST25 = 0;
		private decimal _AmtST50 = 0;
		private decimal _AmtBHT1 = 0;
		private decimal _AmtBHT2 = 0;
		private decimal _AmtBHT5 = 0;
		private decimal _AmtBHT10 = 0;
		private decimal _AmtBHT20 = 0;
		private decimal _AmtBHT50 = 0;
		private decimal _AmtBHT100 = 0;
		private decimal _AmtBHT500 = 0;
		private decimal _AmtBHT1000 = 0;

		private decimal _BHTTotal = decimal.Zero;

		// วงเงินขอเพิ่ม เป็นเงินที่ ขอเพิ่มไปยัง บ/ช โดย เมื่อรวมกับยอดรวม ต้องไม่เกิน ยอดวงเงินอนุมัติ
		private decimal _AdditionalBHT = decimal.Zero;
		// เงินยืมเพิ่ม ไม่จำกัด เพราะต้องคืน เท่ากับที่ยืมมา
		private decimal _BorrowBHT = decimal.Zero;
		// เงินขอแลกเปลี่ยน 
		private decimal _ExchangeBHT = decimal.Zero;

		private DateTime? _PeriodBegin = new DateTime?();
		private DateTime? _PeriodEnd = new DateTime?();

		private bool _hasRemark = false;

		private string _Remark = string.Empty;

		private FinishedFlags _FinishFlag = FinishedFlags.Avaliable;

		private int _Status = 0;
		private DateTime _LastUpdate = DateTime.MinValue;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public TSBExchangeTransaction() : base() { }

		#endregion

		#region Private Methods

		private void CalcTotalAmount()
		{
			decimal total = 0;
			total += _AmtST25;
			total += _AmtST50;
			total += _AmtBHT1;
			total += _AmtBHT2;
			total += _AmtBHT5;
			total += _AmtBHT10;
			total += _AmtBHT20;
			total += _AmtBHT50;
			total += _AmtBHT100;
			total += _AmtBHT500;
			total += _AmtBHT1000;

			_BHTTotal = total;
			// Raise event.
			this.RaiseChanged("BHTTotal");
		}

		#endregion

		#region Public Properties

		#region Runtime

		/// <summary>
		/// Gets or sets Description (Runtime).
		/// </summary>
		[Category("Runtime")]
		[Description("Gets or sets Description (Runtime).")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string Description { get; set; }
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
		/// Gets or sets Remark Visibility.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets or sets Remark Visibility.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyMapName("RemarkVisibility")]
		public System.Windows.Visibility RemarkVisibility
		{
			get { return (_hasRemark) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; }
			set { }
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
				}
			}
		}
		/// <summary>
		/// Gets or sets Exchange GroupId
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Exchange GroupId")]
		[ReadOnly(true)]
		[PropertyMapName("GroupId")]
		public Guid GroupId
		{
			get
			{
				return _GroupId;
			}
			set
			{
				if (_GroupId != value)
				{
					_GroupId = value;
					this.RaiseChanged("GroupId");
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

		#region Valid Colors

		/// <summary>
		/// Gets Foreground color for ST25.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets Foreground color for ST25.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public SolidColorBrush ST25Foreground
		{
			get { return (IsValidST25) ? BlackForeground : RedForeground; }
			set { }
		}
		/// <summary>
		/// Gets Foreground color for ST50.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets Foreground color for ST50.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public SolidColorBrush ST50Foreground
		{
			get { return (IsValidST50) ? BlackForeground : RedForeground; }
			set { }
		}
		/// <summary>
		/// Gets Foreground color for BHT1.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets Foreground color for BHT1.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public SolidColorBrush BHT1Foreground
		{
			get { return (IsValidBHT1) ? BlackForeground : RedForeground; }
			set { }
		}
		/// <summary>
		/// Gets Foreground color for BHT2.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets Foreground color for BHT2.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public SolidColorBrush BHT2Foreground
		{
			get { return (IsValidBHT2) ? BlackForeground : RedForeground; }
			set { }
		}
		/// <summary>
		/// Gets Foreground color for BHT5.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets Foreground color for BHT5.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public SolidColorBrush BHT5Foreground
		{
			get { return (IsValidBHT5) ? BlackForeground : RedForeground; }
			set { }
		}
		/// <summary>
		/// Gets Foreground color for BHT10.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets Foreground color for BHT10.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public SolidColorBrush BHT10Foreground
		{
			get { return (IsValidBHT10) ? BlackForeground : RedForeground; }
			set { }
		}
		/// <summary>
		/// Gets Foreground color for BHT20.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets Foreground color for BHT20.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public SolidColorBrush BHT20Foreground
		{
			get { return (IsValidBHT20) ? BlackForeground : RedForeground; }
			set { }
		}
		/// <summary>
		/// Gets Foreground color for BHT50.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets Foreground color for BHT50.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public SolidColorBrush BHT50Foreground
		{
			get { return (IsValidBHT50) ? BlackForeground : RedForeground; }
			set { }
		}
		/// <summary>
		/// Gets Foreground color for BHT100.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets Foreground color for BHT100.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public SolidColorBrush BHT100Foreground
		{
			get { return (IsValidBHT100) ? BlackForeground : RedForeground; }
			set { }
		}
		/// <summary>
		/// Gets Foreground color for BHT500.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets Foreground color for BHT500.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public SolidColorBrush BHT500Foreground
		{
			get { return (IsValidBHT500) ? BlackForeground : RedForeground; }
			set { }
		}
		/// <summary>
		/// Gets Foreground color for BHT1000.
		/// </summary>
		[Category("Runtime")]
		[Description("Gets Foreground color for BHT1000.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public SolidColorBrush BHT1000Foreground
		{
			get { return (IsValidBHT1000) ? BlackForeground : RedForeground; }
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
		/// <summary>
		/// Gets or sets amount TSB Max BHT.
		/// </summary>
		[Category("TSB")]
		[Description("Gets or sets Max TSB Credit.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyOrder(50)]
		[PropertyMapName("MaxCredit")]
		public virtual decimal MaxCredit
		{
			get { return _MaxCredit; }
			set
			{
				if (_MaxCredit != value)
				{
					_MaxCredit = value;
					// Raise event.
					this.RaiseChanged("MaxCredit");
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

		#region Coin/Bill (Amount)

		/// <summary>
		/// Gets or sets amount of .25 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets amount of .25 baht coin.")]
		[PropertyMapName("AmountST25")]
		[PropertyOrder(21)]
		public virtual decimal AmountST25
		{
			get { return _AmtST25; }
			set
			{
				if (value < decimal.Zero) return;
				if (_AmtST25 != value)
				{
					_AmtST25 = value;
					// Raise event.
					this.RaiseChanged("AmountST25");
					this.RaiseChanged("CountST25");
					this.RaiseChanged("IsValidST25");

					CalcTotalAmount();
				}
			}
		}
		/// <summary>
		/// Gets or sets amount of .50 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets amount of .50 baht coin.")]
		[PropertyMapName("AmountST50")]
		[PropertyOrder(22)]
		public virtual decimal AmountST50
		{
			get { return _AmtST50; }
			set
			{
				if (value < decimal.Zero) return;
				if (_AmtST50 != value)
				{
					_AmtST50 = value;
					// Raise event.
					this.RaiseChanged("AmountST50");
					this.RaiseChanged("CountST50");
					this.RaiseChanged("IsValidST50");
					this.RaiseChanged("ST50Foreground");

					CalcTotalAmount();
				}
			}
		}
		/// <summary>
		/// Gets or sets amount of 1 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets amount of 1 baht coin.")]
		[PropertyMapName("AmountBHT1")]
		[PropertyOrder(23)]
		public virtual decimal AmountBHT1
		{
			get { return _AmtBHT1; }
			set
			{
				if (value < decimal.Zero) return;
				if (_AmtBHT1 != value)
				{
					_AmtBHT1 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT1");
					this.RaiseChanged("CountBHT1");
					this.RaiseChanged("IsValidBHT1");
					this.RaiseChanged("BHT1Foreground");

					CalcTotalAmount();
				}
			}
		}
		/// <summary>
		/// Gets or sets amount of 2 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets amount of 2 baht coin.")]
		[PropertyMapName("AmountBHT2")]
		[PropertyOrder(24)]
		public virtual decimal AmountBHT2
		{
			get { return _AmtBHT2; }
			set
			{
				if (value < decimal.Zero) return;
				if (_AmtBHT2 != value)
				{
					_AmtBHT2 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT2");
					this.RaiseChanged("CountBHT2");
					this.RaiseChanged("IsValidBHT2");
					this.RaiseChanged("BHT2Foreground");

					CalcTotalAmount();
				}
			}
		}
		/// <summary>
		/// Gets or sets amount of 5 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets amount of 5 baht coin.")]
		[PropertyMapName("AmountBHT5")]
		[PropertyOrder(25)]
		public virtual decimal AmountBHT5
		{
			get { return _AmtBHT5; }
			set
			{
				if (value < decimal.Zero) return;
				if (_AmtBHT5 != value)
				{
					_AmtBHT5 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT5");
					this.RaiseChanged("CountBHT5");
					this.RaiseChanged("IsValidBHT5");
					this.RaiseChanged("BHT5Foreground");

					CalcTotalAmount();
				}
			}
		}
		/// <summary>
		/// Gets or sets amount of 10 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets amount of 10 baht coin.")]
		[PropertyMapName("AmountBHT10")]
		[PropertyOrder(26)]
		public virtual decimal AmountBHT10
		{
			get { return _AmtBHT10; }
			set
			{
				if (value < decimal.Zero) return;
				if (_AmtBHT10 != value)
				{
					_AmtBHT10 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT10");
					this.RaiseChanged("CountBHT10");
					this.RaiseChanged("IsValidBHT10");
					this.RaiseChanged("BHT10Foreground");

					CalcTotalAmount();
				}
			}
		}
		/// <summary>
		/// Gets or sets amount of 20 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets amount of 20 baht bill.")]
		[PropertyMapName("AmountBHT20")]
		[PropertyOrder(27)]
		public virtual decimal AmountBHT20
		{
			get { return _AmtBHT20; }
			set
			{
				if (_AmtBHT20 != value)
				{
					if (value < decimal.Zero) return;
					_AmtBHT20 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT20");
					this.RaiseChanged("CountBHT20");
					this.RaiseChanged("IsValidBHT20");
					this.RaiseChanged("BHT20Foreground");

					CalcTotalAmount();
				}
			}
		}
		/// <summary>
		/// Gets or sets amount of 50 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets amount of 50 baht bill.")]
		[PropertyMapName("AmountBHT50")]
		[PropertyOrder(28)]
		public virtual decimal AmountBHT50
		{
			get { return _AmtBHT50; }
			set
			{
				if (value < decimal.Zero) return;
				if (_AmtBHT50 != value)
				{
					_AmtBHT50 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT50");
					this.RaiseChanged("CountBHT50");
					this.RaiseChanged("IsValidBHT50");
					this.RaiseChanged("BHT50Foreground");

					CalcTotalAmount();
				}
			}
		}
		/// <summary>
		/// Gets or sets amount of 100 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets amount of 100 baht bill.")]
		[PropertyMapName("AmountBHT100")]
		[PropertyOrder(29)]
		public virtual decimal AmountBHT100
		{
			get { return _AmtBHT100; }
			set
			{
				if (value < decimal.Zero) return;
				if (_AmtBHT100 != value)
				{
					_AmtBHT100 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT100");
					this.RaiseChanged("CountBHT100");
					this.RaiseChanged("IsValidBHT100");
					this.RaiseChanged("BHT100Foreground");

					CalcTotalAmount();
				}
			}
		}
		/// <summary>
		/// Gets or sets amount of 500 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets amount of 500 baht bill.")]
		[PropertyMapName("AmountBHT500")]
		[PropertyOrder(30)]
		public virtual decimal AmountBHT500
		{
			get { return _AmtBHT500; }
			set
			{
				if (value < decimal.Zero) return;
				if (_AmtBHT500 != value)
				{
					_AmtBHT500 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT500");
					this.RaiseChanged("CountBHT500");
					this.RaiseChanged("IsValidBHT500");
					this.RaiseChanged("BHT500Foreground");

					CalcTotalAmount();
				}
			}
		}
		/// <summary>
		/// Gets or sets amount of 1000 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets amount of 1000 baht bill.")]
		[PropertyMapName("AmountBHT1000")]
		[PropertyOrder(31)]
		public virtual decimal AmountBHT1000
		{
			get { return _AmtBHT1000; }
			set
			{
				if (value < decimal.Zero) return;
				if (_AmtBHT1000 != value)
				{
					_AmtBHT1000 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT1000");
					this.RaiseChanged("CountBHT1000");
					this.RaiseChanged("IsValidBHT1000");
					this.RaiseChanged("BHT1000Foreground");

					CalcTotalAmount();
				}
			}
		}

		#endregion

		#region Coin/Bill (Count)

		/// <summary>
		/// Gets or sets amount of .25 baht coin.
		/// </summary>
		[Category("Coin/Bill (Count)")]
		[Description("Gets or sets count of .25 baht coin.")]
		[PropertyMapName("CountST25")]
		[Ignore]
		[JsonIgnore]
		[PropertyOrder(21)]
		public virtual int CountST25
		{
			get { return Convert.ToInt32(_AmtST25 / (decimal)0.25); }
			set { }
		}
		/// <summary>
		/// Gets or sets count of .50 baht coin.
		/// </summary>
		[Category("Coin/Bill (Count)")]
		[Description("Gets or sets count of .50 baht coin.")]
		[PropertyMapName("CountST50")]
		[Ignore]
		[JsonIgnore]
		[PropertyOrder(22)]
		public virtual int CountST50
		{
			get { return Convert.ToInt32(_AmtST50 / (decimal)0.50); }
			set { }
		}
		/// <summary>
		/// Gets or sets count of 1 baht coin.
		/// </summary>
		[Category("Coin/Bill (Count)")]
		[Description("Gets or sets count of 1 baht coin.")]
		[PropertyMapName("CountBHT1")]
		[Ignore]
		[JsonIgnore]
		[PropertyOrder(23)]
		public virtual int CountBHT1
		{
			get { return Convert.ToInt32(_AmtBHT1); }
			set { }
		}
		/// <summary>
		/// Gets or sets count of 2 baht coin.
		/// </summary>
		[Category("Coin/Bill (Count)")]
		[Description("Gets or sets count of 2 baht coin.")]
		[PropertyMapName("CountBHT2")]
		[Ignore]
		[JsonIgnore]
		[PropertyOrder(24)]
		public virtual int CountBHT2
		{
			get { return Convert.ToInt32(_AmtBHT2 / 2); }
			set { }
		}
		/// <summary>
		/// Gets or sets count of 5 baht coin.
		/// </summary>
		[Category("Coin/Bill (Count)")]
		[Description("Gets or sets count of 5 baht coin.")]
		[PropertyMapName("CountBHT5")]
		[Ignore]
		[JsonIgnore]
		[PropertyOrder(25)]
		public virtual int CountBHT5
		{
			get { return Convert.ToInt32(_AmtBHT5 / 5); }
			set { }
		}
		/// <summary>
		/// Gets or sets count of 10 baht coin.
		/// </summary>
		[Category("Coin/Bill (Count)")]
		[Description("Gets or sets count of 10 baht coin.")]
		[PropertyMapName("CountBHT10")]
		[Ignore]
		[JsonIgnore]
		[PropertyOrder(26)]
		public virtual int CountBHT10
		{
			get { return Convert.ToInt32(_AmtBHT10 / 10); }
			set { }
		}
		/// <summary>
		/// Gets or sets count of 20 baht bill.
		/// </summary>
		[Category("Coin/Bill (Count)")]
		[Description("Gets or sets count of 20 baht bill.")]
		[PropertyMapName("CountBHT20")]
		[Ignore]
		[JsonIgnore]
		[PropertyOrder(27)]
		public virtual int CountBHT20
		{
			get { return Convert.ToInt32(_AmtBHT20 / 20); }
			set { }
		}
		/// <summary>
		/// Gets or sets count of 50 baht bill.
		/// </summary>
		[Category("Coin/Bill (Count)")]
		[Description("Gets or sets count of 50 baht bill.")]
		[PropertyMapName("CountBHT50")]
		[Ignore]
		[JsonIgnore]
		[PropertyOrder(28)]
		public virtual int CountBHT50
		{
			get { return Convert.ToInt32(_AmtBHT50 / 50); }
			set { }
		}
		/// <summary>
		/// Gets or sets count of 100 baht bill.
		/// </summary>
		[Category("Coin/Bill (Count)")]
		[Description("Gets or sets count of 100 baht bill.")]
		[PropertyMapName("CountBHT100")]
		[Ignore]
		[JsonIgnore]
		[PropertyOrder(29)]
		public virtual int CountBHT100
		{
			get { return Convert.ToInt32(_AmtBHT100/ 100); }
			set { }
		}
		/// <summary>
		/// Gets or sets count of 500 baht bill.
		/// </summary>
		[Category("Coin/Bill (Count)")]
		[Description("Gets or sets count of 500 baht bill.")]
		[PropertyMapName("CountBHT500")]
		[Ignore]
		[JsonIgnore]
		[PropertyOrder(30)]
		public virtual int CountBHT500
		{
			get { return Convert.ToInt32(_AmtBHT500 / 500); }
			set { }
		}
		/// <summary>
		/// Gets or sets amount of 1000 baht bill.
		/// </summary>
		[Category("Coin/Bill (Count)")]
		[Description("Gets or sets amount of 1000 baht bill.")]
		[PropertyMapName("CountBHT1000")]
		[Ignore]
		[JsonIgnore]
		[PropertyOrder(31)]
		public virtual int CountBHT1000
		{
			get { return Convert.ToInt32(_AmtBHT1000 / 1000); }
			set { }
		}

		#endregion

		#region Coin/Bill (IsValid)

		/// <summary>
		/// Gets amount is exact match .25 baht coin.
		/// </summary>
		[Category("Coin/Bill (IsValid)")]
		[Description("Gets amount is exact match .25 baht coin.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyOrder(32)]
		public virtual bool IsValidST25
		{
			get { return (_AmtST25 % (decimal).25) == 0; }
			set { }
		}
		/// <summary>
		/// Gets amount is exact match .50 baht coin.
		/// </summary>
		[Category("Coin/Bill (IsValid)")]
		[Description("Gets amount is exact match .50 baht coin.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyOrder(33)]
		public virtual bool IsValidST50
		{
			get { return (_AmtST50 % (decimal).5) == 0; }
			set { }
		}
		/// <summary>
		/// Gets amount is exact match 1 baht coin.
		/// </summary>
		[Category("Coin/Bill (IsValid)")]
		[Description("Gets amount is exact match 1 baht coin.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyOrder(34)]
		public virtual bool IsValidBHT1
		{
			get { return (_AmtBHT1 % 1) == 0; }
			set { }
		}
		/// <summary>
		/// Gets amount is exact match 2 baht coin.
		/// </summary>
		[Category("Coin/Bill (IsValid)")]
		[Description("Gets amount is exact match 2 baht coin.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyOrder(35)]
		public virtual bool IsValidBHT2
		{
			get { return (_AmtBHT2 % 2) == 0; }
			set { }
		}
		/// <summary>
		/// Gets amount is exact match 5 baht coin.
		/// </summary>
		[Category("Coin/Bill (IsValid)")]
		[Description("Gets amount is exact match 5 baht coin.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyOrder(36)]
		public virtual bool IsValidBHT5
		{
			get { return (_AmtBHT5 % 5) == 0; }
			set { }
		}
		/// <summary>
		/// Gets amount is exact match 10 baht coin.
		/// </summary>
		[Category("Coin/Bill (IsValid)")]
		[Description("Gets amount is exact match 10 baht coin.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyOrder(37)]
		public virtual bool IsValidBHT10
		{
			get { return (_AmtBHT10 % 10) == 0; }
			set { }
		}
		/// <summary>
		/// Gets amount is exact match 20 baht bill.
		/// </summary>
		[Category("Coin/Bill (IsValid)")]
		[Description("Gets amount is exact match 20 baht bill.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyOrder(38)]
		public virtual bool IsValidBHT20
		{
			get { return (_AmtBHT20 % 20) == 0; }
			set { }
		}
		/// <summary>
		/// Gets amount is exact match 50 baht bill.
		/// </summary>
		[Category("Coin/Bill (IsValid)")]
		[Description("Gets amount is exact match 50 baht bill.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyOrder(39)]
		public virtual bool IsValidBHT50
		{
			get { return (_AmtBHT50 % 50) == 0; }
			set { }
		}
		/// <summary>
		/// Gets amount is exact match 100 baht bill.
		/// </summary>
		[Category("Coin/Bill (IsValid)")]
		[Description("Gets amount is exact match 100 baht bill.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyOrder(40)]
		public virtual bool IsValidBHT100
		{
			get { return (_AmtBHT100 % 100) == 0; }
			set { }
		}
		/// <summary>
		/// Gets amount is exact match 500 baht bill.
		/// </summary>
		[Category("Coin/Bill (IsValid)")]
		[Description("Gets amount is exact match 500 baht bill.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyOrder(41)]
		public virtual bool IsValidBHT500
		{
			get { return (_AmtBHT500 % 500) == 0; }
			set { }
		}
		/// <summary>
		/// Gets amount is exact match 1000 baht bill.
		/// </summary>
		[Category("Coin/Bill (IsValid)")]
		[Description("Gets amount is exact match 1000 baht bill.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyOrder(41)]
		public virtual bool IsValidBHT1000
		{
			get { return (_AmtBHT1000 % 1000) == 0; }
			set { }
		}

		#endregion

		#region Coin/Bill (Summary)

		/// <summary>
		/// Gets or sets total value in baht.
		/// </summary>
		[Category("Coin/Bill (Summary)")]
		[Description("Gets or sets total value in baht.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		//[PropertyMapName("BHTTotal")]
		public decimal BHTTotal
		{
			get { return _BHTTotal; }
			set { }
		}

		#endregion

		#region Exchange/Borrow/Additional

		/// <summary>
		/// Gets or sets amount Exchange BHT.
		/// </summary>
		[Category("Summary (Amount)")]
		[Description("Gets or sets amount Exchange BHT.")]
		[PropertyMapName("ExchangeBHT")]
		[PropertyOrder(51)]
		public virtual decimal ExchangeBHT
		{
			get { return _ExchangeBHT; }
			set
			{
				if (_ExchangeBHT != value)
				{
					_ExchangeBHT = value;
					// Raise event.
					this.RaiseChanged("ExchangeBHT");
					this.RaiseChanged("GrandTotalBHT");
				}
			}
		}
		/// <summary>
		/// Gets or sets amount Borrow BHT.
		/// </summary>
		[Category("Summary (Amount)")]
		[Description("Gets or sets amount Borrow BHT.")]
		[PropertyMapName("BorrowBHT")]
		[PropertyOrder(52)]
		public virtual decimal BorrowBHT
		{
			get { return _BorrowBHT; }
			set
			{
				if (_BorrowBHT != value)
				{
					_BorrowBHT = value;
					// Raise event.
					this.RaiseChanged("BorrowBHT");
					this.RaiseChanged("GrandTotalBHT");
				}
			}
		}
		/// <summary>
		/// Gets or sets amount Additional BHT.
		/// </summary>
		[Category("Summary (Amount)")]
		[Description("Gets or sets amount Additional BHT.")]
		[PropertyMapName("AdditionalBHT")]
		[PropertyOrder(53)]
		public virtual decimal AdditionalBHT
		{
			get { return _AdditionalBHT; }
			set
			{
				if (_AdditionalBHT != value)
				{
					_AdditionalBHT = value;
					// Raise event.
					this.RaiseChanged("AdditionalBHT");
					this.RaiseChanged("GrandTotalBHT");
				}
			}
		}

		/// <summary>
		/// Gets or sets Grand Total in baht.
		/// </summary>
		[Category("Summary (Amount)")]
		[Description("Gets or sets Grand Total in baht.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		//[PropertyMapName("GrandTotalBHT")]
		public decimal GrandTotalBHT
		{
			get
			{
				return _ExchangeBHT + _BorrowBHT + _AdditionalBHT;
			}
			set { }
		}

		#endregion

		#region Period

		/// <summary>
		/// Gets or sets Period Begin Date.
		/// </summary>
		[Category("Period")]
		[Description(" Gets or sets Period Begin Date")]
		[ReadOnly(true)]
		[PropertyMapName("PeriodBegin")]
		public virtual DateTime? PeriodBegin
		{
			get
			{
				return _PeriodBegin;
			}
			set
			{
				if (_PeriodBegin != value)
				{
					_PeriodBegin = value;
					this.RaiseChanged("PeriodBegin");
					this.RaiseChanged("PeriodBeginDateString");
					this.RaiseChanged("PeriodBeginTimeString");
					this.RaiseChanged("PeriodBeginDateTimeString");
				}
			}
		}
		/// <summary>
		/// Gets Period Begin Date String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Period Begin Date String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string PeriodBeginDateString
		{
			get
			{
				var ret = (!this.PeriodBegin.HasValue) ? "" : this.PeriodBegin.Value.ToThaiDateTimeString("dd/MM/yyyy");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Period Begin Time String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Period Begin Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string PeriodBeginTimeString
		{
			get
			{
				var ret = (!this.PeriodBegin.HasValue) ? "" : this.PeriodBegin.Value.ToThaiTimeString();
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Period Begin Date Time String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Period Begin Date Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string PeriodBeginDateTimeString
		{
			get
			{
				var ret = (!this.PeriodBegin.HasValue) ? "" : this.PeriodBegin.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
				return ret;
			}
			set { }
		}

		/// <summary>
		/// Gets or sets Period End Date.
		/// </summary>
		[Category("Period")]
		[Description(" Gets or sets Period End Date")]
		[ReadOnly(true)]
		[PropertyMapName("PeriodEnd")]
		public virtual DateTime? PeriodEnd
		{
			get
			{
				return _PeriodEnd;
			}
			set
			{
				if (_PeriodEnd != value)
				{
					_PeriodEnd = value;
					this.RaiseChanged("PeriodEnd");
					this.RaiseChanged("PeriodEndDateString");
					this.RaiseChanged("PeriodEndTimeString");
					this.RaiseChanged("PeriodEndDateTimeString");
				}
			}
		}
		/// <summary>
		/// Gets Period End Date String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Period End Date String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string PeriodEndDateString
		{
			get
			{
				var ret = (!this.PeriodEnd.HasValue) ? "" : this.PeriodEnd.Value.ToThaiDateTimeString("dd/MM/yyyy");
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Period End Time String.
		/// </summary>
		[Category("Period")]
		[Description("Gets Period End Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string PeriodEndTimeString
		{
			get
			{
				var ret = (!this.PeriodEnd.HasValue) ? "" : this.PeriodEnd.Value.ToThaiTimeString();
				return ret;
			}
			set { }
		}
		/// <summary>
		/// Gets Period End Date Time String.
		/// </summary>
		[Category("Common")]
		[Description("Gets Period End Date Time String.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		public string PeriodEndDateTimeString
		{
			get
			{
				var ret = (!this.PeriodEnd.HasValue) ? "" : this.PeriodEnd.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
				return ret;
			}
			set { }
		}

		#endregion

		#region Remark

		/// <summary>
		/// Gets or sets  Remark.
		/// </summary>
		[Category("Remark")]
		[Description("Gets or sets  Remark.")]
		[MaxLength(255)]
		[PropertyMapName("Remark")]
		public string Remark
		{
			get { return _Remark; }
			set
			{
				if (_Remark != value)
				{
					_Remark = value;
					// Raise event.
					this.RaiseChanged("Remark");
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
		[PropertyMapName("Status", typeof(TSBExchangeTransaction))]
		[PropertyOrder(10001)]
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
		[PropertyMapName("LastUpdate", typeof(TSBExchangeTransaction))]
		[PropertyOrder(10002)]
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
		public class FKs : TSBExchangeTransaction, IFKs<TSBExchangeTransaction>
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
			/// <summary>
			/// Gets or sets amount TSB Max BHT.
			/// </summary>
			[PropertyMapName("MaxCredit")]
			public override decimal MaxCredit
			{
				get { return base.MaxCredit; }
				set { base.MaxCredit = value; }
			}

			#endregion
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets TSB Exchange Transactions (Active TSB).
		/// </summary>
		/// <returns>Returns List of TSB Exchange Transactions.</returns>
		public static NDbResult<List<TSBExchangeTransaction>> GetTransactions()
		{
			var result = new NDbResult<List<TSBExchangeTransaction>>();
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
			result = GetTransactions(tsb);
			return result;
		}
		/// <summary>
		/// Gets TSB Exchange Transactions.
		/// </summary>
		/// <param name="tsb">The TSB instance.</param>
		/// <returns>Returns List of TSB Exchange Transactions.</returns>
		public static NDbResult<List<TSBExchangeTransaction>> GetTransactions(TSB tsb)
		{
			var result = new NDbResult<List<TSBExchangeTransaction>>();
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
			result = GetTransactions(tsb, Guid.Empty);
			return result;
		}
		/// <summary>
		/// Gets TSB Exchange Transactions by group Id.
		/// </summary>
		/// <param name="tsb">The TSB instance.</param>
		/// <param name="groupId">The Group Id.</param>
		/// <returns>Returns List of TSB Exchange Transactions.</returns>
		public static NDbResult<List<TSBExchangeTransaction>> GetTransactions(TSB tsb, Guid groupId)
		{
			var result = new NDbResult<List<TSBExchangeTransaction>>();
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
					List<FKs> rets;

					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM TSBExchangeTransactionView ";
					cmd += " WHERE TSBId = ? ";
					if (groupId != Guid.Empty)
					{
						cmd += "   AND GroupId = ? ";
						//cmd += "   AND FinishFlag = 1 ";

						rets = NQuery.Query<FKs>(cmd, tsb.TSBId, groupId).ToList();
					}
					else
					{
						rets = NQuery.Query<FKs>(cmd, tsb.TSBId).ToList();
					}

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
		/// Gets TSB Exchange Transactions by group Id and state.
		/// </summary>
		/// <param name="tsb">The TSB instance.</param>
		/// <param name="groupId">The Group Id.</param>
		/// <param name="type">The Transaction Type.</param>
		/// <returns>Returns List of TSB Exchange Transactions.</returns>
		public static NDbResult<TSBExchangeTransaction> GetTransaction(TSB tsb, Guid groupId,
			TSBExchangeTransaction.TransactionTypes type)
		{
			var result = new NDbResult<TSBExchangeTransaction>();
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
					FKs ret;

					string cmd = string.Empty;
					cmd += "SELECT * ";
					cmd += "  FROM TSBExchangeTransactionView ";
					cmd += " WHERE TSBId = ? ";
					//cmd += "   AND FinishFlag = 1 ";
					if (groupId != Guid.Empty)
					{
						cmd += "   AND GroupId = ? ";
						if (type == TSBExchangeTransaction.TransactionTypes.None)
						{
							ret = NQuery.Query<FKs>(cmd, tsb.TSBId, groupId).FirstOrDefault();
						}
						else
						{
							cmd += "   AND TransactionType = ? ";
							ret = NQuery.Query<FKs>(cmd, tsb.TSBId, groupId, type).FirstOrDefault();
						}
					}
					else
					{
						if (type == TSBExchangeTransaction.TransactionTypes.None)
						{
							ret = NQuery.Query<FKs>(cmd, tsb.TSBId).FirstOrDefault();
						}
						else
						{
							cmd += "   AND TransactionType = ? ";
							ret = NQuery.Query<FKs>(cmd, tsb.TSBId, type).FirstOrDefault();
						}
					}

					var val = (null != ret) ? ret.ToModel() : null;
					result.Success(val);
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
		/// <param name="value">The TSBExchangeTransaction instance.</param>
		/// <returns>Returns save TSBExchangeTransaction instance.</returns>
		public static NDbResult<TSBExchangeTransaction> SaveTransaction(
			TSBExchangeTransaction value)
		{
			var result = new NDbResult<TSBExchangeTransaction>();
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
			result = Save(value);
			return result;
		}

		#endregion
	}

	#endregion
}
