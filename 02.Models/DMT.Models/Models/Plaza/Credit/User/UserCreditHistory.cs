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
		private string _BagCreateDate = string.Empty;
		private string _BagNo = string.Empty;
		private string _BeltNo = string.Empty;

		private string _TSBId = string.Empty;
		private string _TSBNameEN = string.Empty;
		private string _TSBNameTH = string.Empty;

		private string _PlazaGroupId = string.Empty;
		private string _PlazaGroupNameEN = string.Empty;
		private string _PlazaGroupNameTH = string.Empty;
		private string _Direction = string.Empty;

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
		/// Gets or sets Bag Create Date (string)
		/// </summary>
		[Category("Common")]
		[Description("Gets or sets Bag Create Date (string).")]
		[ReadOnly(true)]
		[Ignore]
		[MaxLength(30)]
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
		[MaxLength(30)]
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
		[MaxLength(30)]
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
		/// <summary>
		/// Gets or sets Direction.
		/// </summary>
		[Category("Plaza Group")]
		[Description("Gets or sets Direction.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("Direction")]
		public virtual string Direction
		{
			get
			{
				return _Direction;
			}
			set
			{
				if (_Direction != value)
				{
					_Direction = value;
					this.RaiseChanged("Direction");
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
		[PropertyMapName("BorrowAmountST25")]
		[Ignore]
		[PropertyOrder(21)]
		public virtual decimal BorrowAmountST25
		{
			get { return _BorrowAmtST25; }
			set
			{
				if (_BorrowAmtST25 != value)
				{
					_BorrowAmtST25 = value;
					// Raise event.
					this.RaiseChanged("BorrowAmountST25");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of .50 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of .50 baht coin.")]
		[PropertyMapName("BorrowAmountST50")]
		[Ignore]
		[PropertyOrder(22)]
		public virtual decimal BorrowAmountST50
		{
			get { return _BorrowAmtST50; }
			set
			{
				if (_BorrowAmtST50 != value)
				{
					_BorrowAmtST50 = value;
					// Raise event.
					this.RaiseChanged("BorrowAmountST50");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 1 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 1 baht coin.")]
		[PropertyMapName("BorrowAmountBHT1")]
		[Ignore]
		[PropertyOrder(23)]
		public virtual decimal BorrowAmountBHT1
		{
			get { return _BorrowAmtBHT1; }
			set
			{
				if (_BorrowAmtBHT1 != value)
				{
					_BorrowAmtBHT1 = value;
					// Raise event.
					this.RaiseChanged("BorrowAmountBHT1");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 2 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 2 baht coin.")]
		[PropertyMapName("BorrowAmountBHT2")]
		[Ignore]
		[PropertyOrder(24)]
		public virtual decimal BorrowAmountBHT2
		{
			get { return _BorrowAmtBHT2; }
			set
			{
				if (_BorrowAmtBHT2 != value)
				{
					_BorrowAmtBHT2 = value;
					// Raise event.
					this.RaiseChanged("BorrowAmountBHT2");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 5 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 5 baht coin.")]
		[PropertyMapName("BorrowAmountBHT5")]
		[Ignore]
		[PropertyOrder(25)]
		public virtual decimal BorrowAmountBHT5
		{
			get { return _BorrowAmtBHT5; }
			set
			{
				if (_BorrowAmtBHT5 != value)
				{
					_BorrowAmtBHT5 = value;
					// Raise event.
					this.RaiseChanged("BorrowAmountBHT5");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 10 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 10 baht coin.")]
		[PropertyMapName("BorrowAmountBHT10")]
		[Ignore]
		[PropertyOrder(26)]
		public virtual decimal BorrowAmountBHT10
		{
			get { return _BorrowAmtBHT10; }
			set
			{
				if (_BorrowAmtBHT10 != value)
				{
					_BorrowAmtBHT10 = value;
					// Raise event.
					this.RaiseChanged("BorrowAmountBHT10");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 20 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 20 baht bill.")]
		[PropertyMapName("BorrowAmountBHT20")]
		[Ignore]
		[PropertyOrder(27)]
		public virtual decimal BorrowAmountBHT20
		{
			get { return _BorrowAmtBHT20; }
			set
			{
				if (_BorrowAmtBHT20 != value)
				{
					_BorrowAmtBHT20 = value;
					// Raise event.
					this.RaiseChanged("BorrowAmountBHT20");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 50 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 50 baht bill.")]
		[PropertyMapName("BorrowAmountBHT50")]
		[Ignore]
		[PropertyOrder(28)]
		public virtual decimal BorrowAmountBHT50
		{
			get { return _BorrowAmtBHT50; }
			set
			{
				if (_BorrowAmtBHT50 != value)
				{
					_BorrowAmtBHT50 = value;
					// Raise event.
					this.RaiseChanged("BorrowAmountBHT50");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 100 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 100 baht bill.")]
		[PropertyMapName("BorrowAmountBHT100")]
		[Ignore]
		[PropertyOrder(29)]
		public virtual decimal BorrowAmountBHT100
		{
			get { return _BorrowAmtBHT100; }
			set
			{
				if (_BorrowAmtBHT100 != value)
				{
					_BorrowAmtBHT100 = value;
					// Raise event.
					this.RaiseChanged("BorrowAmountBHT100");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 500 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 500 baht bill.")]
		[PropertyMapName("BorrowAmountBHT500")]
		[Ignore]
		[PropertyOrder(30)]
		public virtual decimal BorrowAmountBHT500
		{
			get { return _BorrowAmtBHT500; }
			set
			{
				if (_BorrowAmtBHT500 != value)
				{
					_BorrowAmtBHT500 = value;
					// Raise event.
					this.RaiseChanged("BorrowAmountBHT500");
				}
			}
		}
		/// <summary>
		/// Gets or sets Borrow amount of 1000 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Borrow amount of 1000 baht bill.")]
		[PropertyMapName("BorrowAmountBHT1000")]
		[Ignore]
		[PropertyOrder(31)]
		public virtual decimal BorrowAmountBHT1000
		{
			get { return _BorrowAmtBHT1000; }
			set
			{
				if (_BorrowAmtBHT1000 != value)
				{
					_BorrowAmtBHT1000 = value;
					// Raise event.
					this.RaiseChanged("BorrowAmountBHT1000");
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
		[PropertyMapName("ReturnAmountST25")]
		[Ignore]
		[PropertyOrder(21)]
		public virtual decimal ReturnAmountST25
		{
			get { return _ReturnAmtST25; }
			set
			{
				if (_ReturnAmtST25 != value)
				{
					_ReturnAmtST25 = value;
					// Raise event.
					this.RaiseChanged("ReturnAmountST25");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of .50 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of .50 baht coin.")]
		[PropertyMapName("ReturnAmountST50")]
		[Ignore]
		[PropertyOrder(22)]
		public virtual decimal ReturnAmountST50
		{
			get { return _ReturnAmtST50; }
			set
			{
				if (_ReturnAmtST50 != value)
				{
					_ReturnAmtST50 = value;
					// Raise event.
					this.RaiseChanged("ReturnAmountST50");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 1 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 1 baht coin.")]
		[PropertyMapName("ReturnAmountBHT1")]
		[Ignore]
		[PropertyOrder(23)]
		public virtual decimal ReturnAmountBHT1
		{
			get { return _ReturnAmtBHT1; }
			set
			{
				if (_ReturnAmtBHT1 != value)
				{
					_ReturnAmtBHT1 = value;
					// Raise event.
					this.RaiseChanged("ReturnAmountBHT1");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 2 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 2 baht coin.")]
		[PropertyMapName("ReturnAmountBHT2")]
		[Ignore]
		[PropertyOrder(24)]
		public virtual decimal ReturnAmountBHT2
		{
			get { return _ReturnAmtBHT2; }
			set
			{
				if (_ReturnAmtBHT2 != value)
				{
					_ReturnAmtBHT2 = value;
					// Raise event.
					this.RaiseChanged("ReturnAmountBHT2");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 5 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 5 baht coin.")]
		[PropertyMapName("ReturnAmountBHT5")]
		[Ignore]
		[PropertyOrder(25)]
		public virtual decimal ReturnAmountBHT5
		{
			get { return _ReturnAmtBHT5; }
			set
			{
				if (_ReturnAmtBHT5 != value)
				{
					_ReturnAmtBHT5 = value;
					// Raise event.
					this.RaiseChanged("ReturnAmountBHT5");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 10 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 10 baht coin.")]
		[PropertyMapName("ReturnAmountBHT10")]
		[Ignore]
		[PropertyOrder(26)]
		public virtual decimal ReturnAmountBHT10
		{
			get { return _ReturnAmtBHT10; }
			set
			{
				if (_ReturnAmtBHT10 != value)
				{
					_ReturnAmtBHT10 = value;
					// Raise event.
					this.RaiseChanged("ReturnAmountBHT10");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 20 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 20 baht bill.")]
		[PropertyMapName("ReturnAmountBHT20")]
		[Ignore]
		[PropertyOrder(27)]
		public virtual decimal ReturnAmountBHT20
		{
			get { return _ReturnAmtBHT20; }
			set
			{
				if (_ReturnAmtBHT20 != value)
				{
					_ReturnAmtBHT20 = value;
					// Raise event.
					this.RaiseChanged("ReturnAmountBHT20");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 50 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 50 baht bill.")]
		[PropertyMapName("ReturnAmountBHT50")]
		[Ignore]
		[PropertyOrder(28)]
		public virtual decimal ReturnAmountBHT50
		{
			get { return _ReturnAmtBHT50; }
			set
			{
				if (_ReturnAmtBHT50 != value)
				{
					_ReturnAmtBHT50 = value;
					// Raise event.
					this.RaiseChanged("ReturnAmountBHT50");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 100 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 100 baht bill.")]
		[PropertyMapName("ReturnAmountBHT100")]
		[Ignore]
		[PropertyOrder(29)]
		public virtual decimal ReturnAmountBHT100
		{
			get { return _ReturnAmtBHT100; }
			set
			{
				if (_ReturnAmtBHT100 != value)
				{
					_ReturnAmtBHT100 = value;
					// Raise event.
					this.RaiseChanged("ReturnAmountBHT100");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 500 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 500 baht bill.")]
		[PropertyMapName("ReturnAmountBHT500")]
		[Ignore]
		[PropertyOrder(30)]
		public virtual decimal ReturnAmountBHT500
		{
			get { return _ReturnAmtBHT500; }
			set
			{
				if (_ReturnAmtBHT500 != value)
				{
					_ReturnAmtBHT500 = value;
					// Raise event.
					this.RaiseChanged("ReturnAmountBHT500");
				}
			}
		}
		/// <summary>
		/// Gets or sets Return amount of 1000 baht bill.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets Return amount of 1000 baht bill.")]
		[PropertyMapName("ReturnAmountBHT1000")]
		[Ignore]
		[PropertyOrder(31)]
		public virtual decimal ReturnAmountBHT1000
		{
			get { return _ReturnAmtBHT1000; }
			set
			{
				if (_ReturnAmtBHT1000 != value)
				{
					_ReturnAmtBHT1000 = value;
					// Raise event.
					this.RaiseChanged("ReturnAmountBHT1000");
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
			/// Gets or sets Bag Create Date (string)
			/// </summary>
			[MaxLength(30)]
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
			/// <summary>
			/// Gets or sets Direction.
			/// </summary>
			[MaxLength(10)]
			[PropertyMapName("Direction")]
			public override string Direction
			{
				get { return base.Direction; }
				set { base.Direction = value; }
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
			[PropertyMapName("BorrowAmountST25")]
			public override decimal BorrowAmountST25
			{
				get { return base.BorrowAmountST25; }
				set { base.BorrowAmountST25 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of .50 baht coin.
			/// </summary>
			[PropertyMapName("BorrowAmountST50")]
			public override decimal BorrowAmountST50
			{
				get { return base.BorrowAmountST50; }
				set { base.BorrowAmountST50 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 1 baht coin.
			/// </summary>
			[PropertyMapName("BorrowAmountBHT1")]
			public override decimal BorrowAmountBHT1
			{
				get { return base.BorrowAmountBHT1; }
				set { base.BorrowAmountBHT1 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 2 baht coin.
			/// </summary>
			[PropertyMapName("BorrowAmountBHT2")]
			public override decimal BorrowAmountBHT2
			{
				get { return base.BorrowAmountBHT2; }
				set { base.BorrowAmountBHT2 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 5 baht coin.
			/// </summary>
			[PropertyMapName("BorrowAmountBHT5")]
			public override decimal BorrowAmountBHT5
			{
				get { return base.BorrowAmountBHT5; }
				set { base.BorrowAmountBHT5 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 10 baht coin.
			/// </summary>
			[PropertyMapName("BorrowAmountBHT10")]
			public override decimal BorrowAmountBHT10
			{
				get { return base.BorrowAmountBHT10; }
				set { base.BorrowAmountBHT10 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 20 baht bill.
			/// </summary>
			[PropertyMapName("BorrowAmountBHT20")]
			public override decimal BorrowAmountBHT20
			{
				get { return base.BorrowAmountBHT20; }
				set { base.BorrowAmountBHT20 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 50 baht bill.
			/// </summary>
			[PropertyMapName("BorrowAmountBHT50")]
			public override decimal BorrowAmountBHT50
			{
				get { return base.BorrowAmountBHT50; }
				set { base.BorrowAmountBHT50 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 100 baht bill.
			/// </summary>
			[PropertyMapName("BorrowAmountBHT100")]
			public override decimal BorrowAmountBHT100
			{
				get { return base.BorrowAmountBHT100; }
				set { base.BorrowAmountBHT100 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 500 baht bill.
			/// </summary>
			[PropertyMapName("BorrowAmountBHT500")]
			public override decimal BorrowAmountBHT500
			{
				get { return base.BorrowAmountBHT500; }
				set { base.BorrowAmountBHT500 = value; }
			}
			/// <summary>
			/// Gets or sets Borrow amount of 1000 baht bill.
			/// </summary>
			[PropertyMapName("BorrowAmountBHT1000")]
			public override decimal BorrowAmountBHT1000
			{
				get { return base.BorrowAmountBHT1000; }
				set { base.BorrowAmountBHT1000 = value; }
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
			[PropertyMapName("ReturnAmountST25")]
			public override decimal ReturnAmountST25
			{
				get { return base.ReturnAmountST25; }
				set { base.ReturnAmountST25 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of .50 baht coin.
			/// </summary>
			[PropertyMapName("ReturnAmountST50")]
			public override decimal ReturnAmountST50
			{
				get { return base.ReturnAmountST50; }
				set { base.ReturnAmountST50 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 1 baht coin.
			/// </summary>
			[PropertyMapName("ReturnAmountBHT1")]
			public override decimal ReturnAmountBHT1
			{
				get { return base.ReturnAmountBHT1; }
				set { base.ReturnAmountBHT1 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 2 baht coin.
			/// </summary>
			[PropertyMapName("ReturnAmountBHT2")]
			public override decimal ReturnAmountBHT2
			{
				get { return base.ReturnAmountBHT2; }
				set { base.ReturnAmountBHT2 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 5 baht coin.
			/// </summary>
			[PropertyMapName("ReturnAmountBHT5")]
			public override decimal ReturnAmountBHT5
			{
				get { return base.ReturnAmountBHT5; }
				set { base.ReturnAmountBHT5 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 10 baht coin.
			/// </summary>
			[PropertyMapName("ReturnAmountBHT10")]
			public override decimal ReturnAmountBHT10
			{
				get { return base.ReturnAmountBHT10; }
				set { base.ReturnAmountBHT10 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 20 baht bill.
			/// </summary>
			[PropertyMapName("ReturnAmountBHT20")]
			public override decimal ReturnAmountBHT20
			{
				get { return base.ReturnAmountBHT20; }
				set { base.ReturnAmountBHT20 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 50 baht bill.
			/// </summary>
			[PropertyMapName("ReturnAmountBHT50")]
			public override decimal ReturnAmountBHT50
			{
				get { return base.ReturnAmountBHT50; }
				set { base.ReturnAmountBHT50 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 100 baht bill.
			/// </summary>
			[PropertyMapName("ReturnAmountBHT100")]
			public override decimal ReturnAmountBHT100
			{
				get { return base.ReturnAmountBHT100; }
				set { base.ReturnAmountBHT100 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 500 baht bill.
			/// </summary>
			[PropertyMapName("ReturnAmountBHT500")]
			public override decimal ReturnAmountBHT500
			{
				get { return base.ReturnAmountBHT500; }
				set { base.ReturnAmountBHT500 = value; }
			}
			/// <summary>
			/// Gets or sets Return amount of 1000 baht bill.
			/// </summary>
			[PropertyMapName("ReturnAmountBHT1000")]
			public override decimal ReturnAmountBHT1000
			{
				get { return base.ReturnAmountBHT1000; }
				set { base.ReturnAmountBHT1000 = value; }
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
					   AND BagCreateDate = ? ";

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
