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
	#region TSBCreditBalance (For Query only)

	/// <summary>
	/// The TSBCreditBalance Data Model class.
	/// </summary>
	[TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
	[Serializable]
	[JsonObject(MemberSerialization.OptOut)]
	//[Table("TSBCreditBalance")]
	public class TSBCreditBalance : NTable<TSBCreditBalance>
	{
		#region Internal Variables

		// For Runtime Used
		private string _description = string.Empty;
		private bool _hasRemark = false;

		private string _TSBId = string.Empty;
		private string _TSBNameEN = string.Empty;
		private string _TSBNameTH = string.Empty;
		// วงเงินอนุมัติ เป็นวงเงินที่ บ/ช กำหนดให้แต่ละด่าน เป็นค่าสูงสุดที่แต่ละด่านจะมีได้ โดยยอดนี้จะต้อง มากกว่าหรือเท่ากับ ยอดรวม + เงินยืมเพิ่ม
		private decimal _MaxCredit = decimal.Zero;
		private decimal _LowLimitST25 = decimal.Zero;
		private decimal _LowLimitST50 = decimal.Zero;
		private decimal _LowLimitBHT1 = decimal.Zero;
		private decimal _LowLimitBHT2 = decimal.Zero;
		private decimal _LowLimitBHT5 = decimal.Zero;
		private decimal _LowLimitBHT10 = decimal.Zero;
		private decimal _LowLimitBHT20 = decimal.Zero;
		private decimal _LowLimitBHT50 = decimal.Zero;
		private decimal _LowLimitBHT100 = decimal.Zero;
		private decimal _LowLimitBHT500 = decimal.Zero;
		private decimal _LowLimitBHT1000 = decimal.Zero;

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

		private decimal _UserBHTTotal = decimal.Zero;

		// Summary
		// วงเงินขอเพิ่ม เป็นเงินที่ ขอเพิ่มไปยัง บ/ช โดย เมื่อรวมกับยอดรวม ต้องไม่เกิน ยอดวงเงินอนุมัติ
		private decimal _AdditionalBHTTotal = decimal.Zero;
		// เงินยืมเพิ่ม ไม่จำกัด เพราะต้องคืน เท่ากับที่ยืมมา
		private decimal _BorrowBHTTotal = decimal.Zero;
		// เงินขอแลกเปลี่ยน 
		private decimal _ExchangeBHTTotal = decimal.Zero;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public TSBCreditBalance() : base() { }

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
		[PropertyMapName("MaxCredit")]
		[ReadOnly(true)]
		[Ignore]
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

		#region Credit (Low Limit)

		/// <summary>
		/// Gets or sets Low Limit for ST25.
		/// </summary>
		[Category("Credits")]
		[Description("Gets or sets Low Limit for ST25.")]
		[PropertyMapName("LowLimitST25")]
		[ReadOnly(true)]
		[Ignore]
		public virtual decimal LowLimitST25
		{
			get
			{
				return _LowLimitST25;
			}
			set
			{
				if (value < decimal.Zero) return;
				if (_LowLimitST25 != value)
				{
					_LowLimitST25 = value;
					this.RaiseChanged("LowLimitST25");
				}
			}
		}
		/// <summary>
		/// Gets or sets Low Limit for ST50.
		/// </summary>
		[Category("Credits")]
		[Description("Gets or sets Low Limit for ST50.")]
		[PropertyMapName("LowLimitST50")]
		[ReadOnly(true)]
		[Ignore]
		public virtual decimal LowLimitST50
		{
			get
			{
				return _LowLimitST50;
			}
			set
			{
				if (value < decimal.Zero) return;
				if (_LowLimitST50 != value)
				{
					_LowLimitST50 = value;
					this.RaiseChanged("LowLimitST50");
				}
			}
		}
		/// <summary>
		/// Gets or sets Low Limit for BHT1.
		/// </summary>
		[Category("Credits")]
		[Description("Gets or sets Low Limit for BHT1.")]
		[PropertyMapName("LowLimitBHT1")]
		[ReadOnly(true)]
		[Ignore]
		public virtual decimal LowLimitBHT1
		{
			get
			{
				return _LowLimitBHT1;
			}
			set
			{
				if (value < decimal.Zero) return;
				if (_LowLimitBHT1 != value)
				{
					_LowLimitBHT1 = value;
					this.RaiseChanged("LowLimitBHT1");
				}
			}
		}
		/// <summary>
		/// Gets or sets Low Limit for BHT2.
		/// </summary>
		[Category("Credits")]
		[Description("Gets or sets Low Limit for BHT2.")]
		[PropertyMapName("LowLimitBHT2")]
		[ReadOnly(true)]
		[Ignore]
		public virtual decimal LowLimitBHT2
		{
			get
			{
				return _LowLimitBHT2;
			}
			set
			{
				if (value < decimal.Zero) return;
				if (_LowLimitBHT2 != value)
				{
					_LowLimitBHT2 = value;
					this.RaiseChanged("LowLimitBHT2");
				}
			}
		}
		/// <summary>
		/// Gets or sets Low Limit for BHT5.
		/// </summary>
		[Category("Credits")]
		[Description("Gets or sets Low Limit for BHT5.")]
		[PropertyMapName("LowLimitBHT5")]
		[ReadOnly(true)]
		[Ignore]
		public virtual decimal LowLimitBHT5
		{
			get
			{
				return _LowLimitBHT5;
			}
			set
			{
				if (value < decimal.Zero) return;
				if (_LowLimitBHT5 != value)
				{
					_LowLimitBHT5 = value;
					this.RaiseChanged("LowLimitBHT5");
				}
			}
		}
		/// <summary>
		/// Gets or sets Low Limit for BHT10.
		/// </summary>
		[Category("Credits")]
		[Description("Gets or sets Low Limit for BHT10.")]
		[PropertyMapName("LowLimitBHT10")]
		[ReadOnly(true)]
		[Ignore]
		public virtual decimal LowLimitBHT10
		{
			get
			{
				return _LowLimitBHT10;
			}
			set
			{
				if (value < decimal.Zero) return;
				if (_LowLimitBHT10 != value)
				{
					_LowLimitBHT10 = value;
					this.RaiseChanged("LowLimitBHT10");
				}
			}
		}
		/// <summary>
		/// Gets or sets Low Limit for BHT20.
		/// </summary>
		[Category("Credits")]
		[Description("Gets or sets Low Limit for BHT20.")]
		[PropertyMapName("LowLimitBHT20")]
		[ReadOnly(true)]
		[Ignore]
		public virtual decimal LowLimitBHT20
		{
			get
			{
				return _LowLimitBHT20;
			}
			set
			{
				if (value < decimal.Zero) return;
				if (_LowLimitBHT20 != value)
				{
					_LowLimitBHT20 = value;
					this.RaiseChanged("LowLimitBHT20");
				}
			}
		}
		/// <summary>
		/// Gets or sets Low Limit for BHT50.
		/// </summary>
		[Category("Credits")]
		[Description("Gets or sets Low Limit for BHT50.")]
		[PropertyMapName("LowLimitBHT50")]
		[ReadOnly(true)]
		[Ignore]
		public virtual decimal LowLimitBHT50
		{
			get
			{
				return _LowLimitBHT50;
			}
			set
			{
				if (value < decimal.Zero) return;
				if (_LowLimitBHT50 != value)
				{
					_LowLimitBHT50 = value;
					this.RaiseChanged("LowLimitBHT50");
				}
			}
		}
		/// <summary>
		/// Gets or sets Low Limit for BHT100.
		/// </summary>
		[Category("Credits")]
		[Description("Gets or sets Low Limit for BHT100.")]
		[PropertyMapName("LowLimitBHT100")]
		[ReadOnly(true)]
		[Ignore]
		public virtual decimal LowLimitBHT100
		{
			get
			{
				return _LowLimitBHT100;
			}
			set
			{
				if (value < decimal.Zero) return;
				if (_LowLimitBHT100 != value)
				{
					_LowLimitBHT100 = value;
					this.RaiseChanged("LowLimitBHT100");
				}
			}
		}
		/// <summary>
		/// Gets or sets Low Limit for BHT500.
		/// </summary>
		[Category("Credits")]
		[Description("Gets or sets Low Limit for BHT500.")]
		[PropertyMapName("LowLimitBHT500")]
		[ReadOnly(true)]
		[Ignore]
		public virtual decimal LowLimitBHT500
		{
			get
			{
				return _LowLimitBHT500;
			}
			set
			{
				if (value < decimal.Zero) return;
				if (_LowLimitBHT500 != value)
				{
					_LowLimitBHT500 = value;
					this.RaiseChanged("LowLimitBHT500");
				}
			}
		}
		/// <summary>
		/// Gets or sets Low Limit for BHT1000.
		/// </summary>
		[Category("Credits")]
		[Description("Gets or sets Low Limit for BHT1.")]
		[PropertyMapName("LowLimitBHT1000")]
		[ReadOnly(true)]
		[Ignore]
		public virtual decimal LowLimitBHT1000
		{
			get
			{
				return _LowLimitBHT1000;
			}
			set
			{
				if (value < decimal.Zero) return;
				if (_LowLimitBHT1000 != value)
				{
					_LowLimitBHT1000 = value;
					this.RaiseChanged("LowLimitBHT1000");
				}
			}
		}

		#endregion

		#endregion

		#region Coin/Bill (Amount)

		/// <summary>
		/// Gets or sets amount of .25 baht coin.
		/// </summary>
		[Category("Coin/Bill (Amount)")]
		[Description("Gets or sets amount of .25 baht coin.")]
		[PropertyMapName("AmountST25")]
		[Ignore]
		[PropertyOrder(21)]
		public virtual decimal AmountST25
		{
			get { return _AmtST25; }
			set
			{
				if (_AmtST25 != value)
				{
					_AmtST25 = value;
					// Raise event.
					this.RaiseChanged("AmountST25");
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
		[Ignore]
		[PropertyOrder(22)]
		public virtual decimal AmountST50
		{
			get { return _AmtST50; }
			set
			{
				if (_AmtST50 != value)
				{
					_AmtST50 = value;
					// Raise event.
					this.RaiseChanged("AmountST50");
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
		[Ignore]
		[PropertyOrder(23)]
		public virtual decimal AmountBHT1
		{
			get { return _AmtBHT1; }
			set
			{
				if (_AmtBHT1 != value)
				{
					_AmtBHT1 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT1");
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
		[Ignore]
		[PropertyOrder(24)]
		public virtual decimal AmountBHT2
		{
			get { return _AmtBHT2; }
			set
			{
				if (_AmtBHT2 != value)
				{
					_AmtBHT2 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT2");
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
		[Ignore]
		[PropertyOrder(25)]
		public virtual decimal AmountBHT5
		{
			get { return _AmtBHT5; }
			set
			{
				if (_AmtBHT5 != value)
				{
					_AmtBHT5 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT5");
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
		[Ignore]
		[PropertyOrder(26)]
		public virtual decimal AmountBHT10
		{
			get { return _AmtBHT10; }
			set
			{
				if (_AmtBHT10 != value)
				{
					_AmtBHT10 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT10");
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
		[Ignore]
		[PropertyOrder(27)]
		public virtual decimal AmountBHT20
		{
			get { return _AmtBHT20; }
			set
			{
				if (_AmtBHT20 != value)
				{
					_AmtBHT20 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT20");
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
		[Ignore]
		[PropertyOrder(28)]
		public virtual decimal AmountBHT50
		{
			get { return _AmtBHT50; }
			set
			{
				if (_AmtBHT50 != value)
				{
					_AmtBHT50 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT50");
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
		[Ignore]
		[PropertyOrder(29)]
		public virtual decimal AmountBHT100
		{
			get { return _AmtBHT100; }
			set
			{
				if (_AmtBHT100 != value)
				{
					_AmtBHT100 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT100");
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
		[Ignore]
		[PropertyOrder(30)]
		public virtual decimal AmountBHT500
		{
			get { return _AmtBHT500; }
			set
			{
				if (_AmtBHT500 != value)
				{
					_AmtBHT500 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT500");
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
		[Ignore]
		[PropertyOrder(31)]
		public virtual decimal AmountBHT1000
		{
			get { return _AmtBHT1000; }
			set
			{
				if (_AmtBHT1000 != value)
				{
					_AmtBHT1000 = value;
					// Raise event.
					this.RaiseChanged("AmountBHT1000");
					this.RaiseChanged("IsValidBHT1000");
					this.RaiseChanged("BHT1000Foreground");

					CalcTotalAmount();
				}
			}
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

		// TODO: TSBCreditBalance Check CreditFlowBHTTotal/BHTTotal
		// TODO: TSBCreditBalance Check ExchangeBHTTotal/BorrowBHTTotal/AdditionalBHTTotal/UserBHTTotal (may be used join to TSBExchange)

		#region Summary

		/// <summary>
		/// Gets or sets total (coin/bill) value in baht.
		/// </summary>
		[Category("Summary")]
		[Description("Gets or sets total (coin/bill) value in baht.")]
		[ReadOnly(true)]
		[JsonIgnore]
		[Ignore]
		[PropertyMapName("BHTTotal")]
		public decimal BHTTotal
		{
			get { return _BHTTotal; }
			set { }
		}
		/// <summary>
		/// Gets or sets users borrow in baht.
		/// </summary>
		[Category("Summary")]
		[Description("Gets or sets users borrow/return in baht.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("UserBHTTotal")]
		public virtual decimal UserBHTTotal
		{
			get { return _UserBHTTotal; }
			set
			{
				if (_UserBHTTotal != value)
				{
					_UserBHTTotal = value;
					// Raise event.
					this.RaiseChanged("UserBHTTotal");
					this.RaiseChanged("CreditFlowBHTTotal");
					this.RaiseChanged("GrandBHTTotal");
				}
			}
		}
		/// <summary>
		/// Gets or sets exchange in baht.
		/// </summary>
		[Category("Summary")]
		[Description("Gets or sets exchange in baht.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("ExchangeBHTTotal")]
		public virtual decimal ExchangeBHTTotal
		{
			get { return _ExchangeBHTTotal; }
			set
			{
				if (_ExchangeBHTTotal != value)
				{
					_ExchangeBHTTotal = value;
					// Raise event.
					this.RaiseChanged("ExchangeBHTTotal");
					this.RaiseChanged("CreditFlowBHTTotal");
					this.RaiseChanged("GrandBHTTotal");
				}
			}
		}
		/// <summary>
		/// Gets or sets borrow in baht.
		/// </summary>
		[Category("Summary")]
		[Description("Gets or sets borrow/return in baht.")]
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
					this.RaiseChanged("CreditFlowBHTTotal");
					this.RaiseChanged("GrandBHTTotal");
				}
			}
		}
		/// <summary>
		/// Gets or sets additional borrow in baht.
		/// </summary>
		[Category("Summary")]
		[Description("Gets or sets additional borrow/return in baht.")]
		[ReadOnly(true)]
		[Ignore]
		[PropertyMapName("AdditionalBHTTotal")]
		public virtual decimal AdditionalBHTTotal
		{
			get { return _AdditionalBHTTotal; }
			set
			{
				if (_AdditionalBHTTotal != value)
				{
					_AdditionalBHTTotal = value;
					// Raise event.
					this.RaiseChanged("AdditionalBHTTotal");
					this.RaiseChanged("CreditFlowBHTTotal");
					this.RaiseChanged("GrandBHTTotal");
				}
			}
		}
		/// <summary>
		/// Gets or sets total (credit flow) value in baht.
		/// </summary>
		[Category("Summary")]
		[Description("Gets or sets total (credit flow) value in baht.")]
		[ReadOnly(true)]
		[Ignore]
		[JsonIgnore]
		[PropertyMapName("CreditFlowBHTTotal")]
		public decimal CreditFlowBHTTotal
		{
			// Need to check calculation
			get { return /*_AdditionalBHTTotal + */_BHTTotal + _UserBHTTotal; }
			set { }
		}
		/// <summary>
		/// Gets or sets grand total value in baht.
		/// </summary>
		[Category("Summary")]
		[Description("Gets or sets grand total value in baht.")]
		[ReadOnly(true)]
		[Ignore]
		[JsonIgnore]
		[PropertyMapName("GrandBHTTotal")]
		public decimal GrandBHTTotal
		{
			// Need to check calculation
			get 
			{ 
				return /*_AdditionalBHTTotal + */_BHTTotal + _UserBHTTotal; 
			}
			set { }
		}

		#endregion

		#endregion

		#region Internal Class

		/// <summary>
		/// The internal FKs class for query data.
		/// </summary>
		public class FKs : TSBCreditBalance, IFKs<TSBCreditBalance>
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

			#region Credit (Low Limit)

			/// <summary>
			/// Gets or sets amount LowLimitST25
			/// </summary>
			[PropertyMapName("LowLimitST25")]
			public override decimal LowLimitST25
			{
				get { return base.LowLimitST25; }
				set { base.LowLimitST25 = value; }
			}
			/// <summary>
			/// Gets or sets amount LowLimitST50
			/// </summary>
			[PropertyMapName("LowLimitST50")]
			public override decimal LowLimitST50
			{
				get { return base.LowLimitST50; }
				set { base.LowLimitST50 = value; }
			}
			/// <summary>
			/// Gets or sets amount LowLimitBHT1
			/// </summary>
			[PropertyMapName("LowLimitBHT1")]
			public override decimal LowLimitBHT1
			{
				get { return base.LowLimitBHT1; }
				set { base.LowLimitBHT1 = value; }
			}
			/// <summary>
			/// Gets or sets amount LowLimitBHT2
			/// </summary>
			[PropertyMapName("LowLimitBHT2")]
			public override decimal LowLimitBHT2
			{
				get { return base.LowLimitBHT2; }
				set { base.LowLimitBHT2 = value; }
			}
			/// <summary>
			/// Gets or sets amount LowLimitBHT5
			/// </summary>
			[PropertyMapName("LowLimitBHT5")]
			public override decimal LowLimitBHT5
			{
				get { return base.LowLimitBHT5; }
				set { base.LowLimitBHT5 = value; }
			}
			/// <summary>
			/// Gets or sets amount LowLimitBHT10
			/// </summary>
			[PropertyMapName("LowLimitBHT10")]
			public override decimal LowLimitBHT10
			{
				get { return base.LowLimitBHT10; }
				set { base.LowLimitBHT10 = value; }
			}
			/// <summary>
			/// Gets or sets amount LowLimitBHT20
			/// </summary>
			[PropertyMapName("LowLimitBHT20")]
			public override decimal LowLimitBHT20
			{
				get { return base.LowLimitBHT20; }
				set { base.LowLimitBHT20 = value; }
			}
			/// <summary>
			/// Gets or sets amount LowLimitBHT50
			/// </summary>
			[PropertyMapName("LowLimitBHT50")]
			public override decimal LowLimitBHT50
			{
				get { return base.LowLimitBHT50; }
				set { base.LowLimitBHT50 = value; }
			}
			/// <summary>
			/// Gets or sets amount LowLimitBHT100
			/// </summary>
			[PropertyMapName("LowLimitBHT100")]
			public override decimal LowLimitBHT100
			{
				get { return base.LowLimitBHT100; }
				set { base.LowLimitBHT100 = value; }
			}
			/// <summary>
			/// Gets or sets amount LowLimitBHT500
			/// </summary>
			[PropertyMapName("LowLimitBHT500")]
			public override decimal LowLimitBHT500
			{
				get { return base.LowLimitBHT500; }
				set { base.LowLimitBHT500 = value; }
			}
			/// <summary>
			/// Gets or sets amount LowLimitBHT1000
			/// </summary>
			[PropertyMapName("LowLimitBHT1000")]
			public override decimal LowLimitBHT1000
			{
				get { return base.LowLimitBHT1000; }
				set { base.LowLimitBHT1000 = value; }
			}

			#endregion

			#endregion

			#region Coin/Bill (Amount)

			/// <summary>
			/// Gets or sets amount of .25 baht coin.
			/// </summary>
			[PropertyMapName("AmountST25")]
			public override decimal AmountST25
			{
				get { return base.AmountST25; }
				set { base.AmountST25 = value; }
			}
			/// <summary>
			/// Gets or sets amount of .50 baht coin.
			/// </summary>
			[PropertyMapName("AmountST50")]
			public override decimal AmountST50
			{
				get { return base.AmountST50; }
				set { base.AmountST50 = value; }
			}
			/// <summary>
			/// Gets or sets amount of 1 baht coin.
			/// </summary>
			[PropertyMapName("AmountBHT1")]
			public override decimal AmountBHT1
			{
				get { return base.AmountBHT1; }
				set { base.AmountBHT1 = value; }
			}
			/// <summary>
			/// Gets or sets amount of 2 baht coin.
			/// </summary>
			[PropertyMapName("AmountBHT2")]
			public override decimal AmountBHT2
			{
				get { return base.AmountBHT2; }
				set { base.AmountBHT2 = value; }
			}
			/// <summary>
			/// Gets or sets amount of 5 baht coin.
			/// </summary>
			[PropertyMapName("AmountBHT5")]
			public override decimal AmountBHT5
			{
				get { return base.AmountBHT5; }
				set { base.AmountBHT5 = value; }
			}
			/// <summary>
			/// Gets or sets amount of 10 baht coin.
			/// </summary>
			[PropertyMapName("AmountBHT10")]
			public override decimal AmountBHT10
			{
				get { return base.AmountBHT10; }
				set { base.AmountBHT10 = value; }
			}
			/// <summary>
			/// Gets or sets amount of 20 baht bill.
			/// </summary>
			[PropertyMapName("AmountBHT20")]
			public override decimal AmountBHT20
			{
				get { return base.AmountBHT20; }
				set { base.AmountBHT20 = value; }
			}
			/// <summary>
			/// Gets or sets amount of 50 baht bill.
			/// </summary>
			[PropertyMapName("AmountBHT50")]
			public override decimal AmountBHT50
			{
				get { return base.AmountBHT50; }
				set { base.AmountBHT50 = value; }
			}
			/// <summary>
			/// Gets or sets amount of 100 baht bill.
			/// </summary>
			[PropertyMapName("AmountBHT100")]
			public override decimal AmountBHT100
			{
				get { return base.AmountBHT100; }
				set { base.AmountBHT100 = value; }
			}
			/// <summary>
			/// Gets or sets amount of 500 baht bill.
			/// </summary>
			[PropertyMapName("AmountBHT500")]
			public override decimal AmountBHT500
			{
				get { return base.AmountBHT500; }
				set { base.AmountBHT500 = value; }
			}
			/// <summary>
			/// Gets or sets amount of 1000 baht bill.
			/// </summary>
			[PropertyMapName("AmountBHT1000")]
			public override decimal AmountBHT1000
			{
				get { return base.AmountBHT1000; }
				set { base.AmountBHT1000 = value; }
			}

			#endregion

			#region UserBHTTotal

			/// <summary>
			/// Gets or sets users borrow in baht.
			/// </summary>
			[PropertyMapName("UserBHTTotal")]
			public override decimal UserBHTTotal
			{
				get { return base.UserBHTTotal; }
				set { base.UserBHTTotal = value; }
			}

			#endregion

			#region Exchange/Borrow/Additional/User(borrow/returns)

			/// <summary>
			/// Gets or sets exchange in baht.
			/// </summary>
			[PropertyMapName("ExchangeBHTTotal")]
			public override decimal ExchangeBHTTotal
			{
				get { return base.ExchangeBHTTotal; }
				set { base.ExchangeBHTTotal = value; }
			}
			/// <summary>
			/// Gets or sets borrow in baht.
			/// </summary>
			[PropertyMapName("BorrowBHTTotal")]
			public override decimal BorrowBHTTotal
			{
				get { return base.BorrowBHTTotal; }
				set { base.BorrowBHTTotal = value; }
			}
			/// <summary>
			/// Gets or sets additional in baht.
			/// </summary>
			[PropertyMapName("AdditionalBHTTotal")]
			public override decimal AdditionalBHTTotal
			{
				get { return base.AdditionalBHTTotal; }
				set { base.AdditionalBHTTotal = value; }
			}

			#endregion
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets Active TSB Credit balance.
		/// </summary>
		/// <returns>Returns Current Active TSB Credit balance. If not found returns null.</returns>
		public static NDbResult<TSBCreditBalance> GetCurrent()
		{
			var result = new NDbResult<TSBCreditBalance>();
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
			result = GetCurrent(tsb);
			return result;
		}
		/// <summary>
		/// Gets TSB Credit Balance.
		/// </summary>
		/// <param name="tsb">The target TSB to get balance.</param>
		/// <returns>Returns TSB Credit balance. If TSB not found returns null.</returns>
		public static NDbResult<TSBCreditBalance> GetCurrent(TSB tsb)
		{
			var result = new NDbResult<TSBCreditBalance>();
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
					  FROM TSBCreditSummaryView
					 WHERE TSBId = ? ";
					var ret = NQuery.Query<FKs>(cmd, tsb.TSBId).FirstOrDefault();
					var data = ret.ToModel();
					result.Success(data);
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
		/// Gets All TSB Credit Balance.
		/// </summary>
		/// <returns>Returns List fo all TSB Credit balance.</returns>
		public static NDbResult<List<TSBCreditBalance>> GetTSBCreditBalances()
		{
			var result = new NDbResult<List<TSBCreditBalance>>();
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
					  FROM TSBCreditSummaryView ";
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
