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

        #region Private Methods

        private void CalcBorrowTotalAmount()
        {
            decimal total = 0;
            total += _BorrowAmtST25;
            total += _BorrowAmtST50;
            total += _BorrowAmtBHT1;
            total += _BorrowAmtBHT2;
            total += _BorrowAmtBHT5;
            total += _BorrowAmtBHT10;
            total += _BorrowAmtBHT20;
            total += _BorrowAmtBHT50;
            total += _BorrowAmtBHT100;
            total += _BorrowAmtBHT500;
            total += _BorrowAmtBHT1000;

            _BorrowBHTTotal = total;
            // Raise event.
            this.RaiseChanged("BorrowBHTTotal");
        }
        private void CalcReturnTotalAmount()
        {
            decimal total = 0;
            total += _ReturnAmtST25;
            total += _ReturnAmtST50;
            total += _ReturnAmtBHT1;
            total += _ReturnAmtBHT2;
            total += _ReturnAmtBHT5;
            total += _ReturnAmtBHT10;
            total += _ReturnAmtBHT20;
            total += _ReturnAmtBHT50;
            total += _ReturnAmtBHT100;
            total += _ReturnAmtBHT500;
            total += _ReturnAmtBHT1000;

            _ReturnBHTTotal = total;
            // Raise event.
            this.RaiseChanged("ReturnBHTTotal");
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

        #region PlazaGroup

        /// <summary>
        /// Gets or sets Plaza Group Id.
        /// </summary>
        [Category("Plaza Group")]
        [Description("Gets or sets Plaza Group Id.")]
        [ReadOnly(true)]
        [MaxLength(10)]
        [PropertyMapName("PlazaGroupId")]
        public string PlazaGroupId
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

                    CalcBorrowTotalAmount();
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

                    CalcBorrowTotalAmount();
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

                    CalcBorrowTotalAmount();
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

                    CalcBorrowTotalAmount();
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

                    CalcBorrowTotalAmount();
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

                    CalcBorrowTotalAmount();
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

                    CalcBorrowTotalAmount();
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

                    CalcBorrowTotalAmount();
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

                    CalcBorrowTotalAmount();
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

                    CalcBorrowTotalAmount();
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

                    CalcBorrowTotalAmount();
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
        [JsonIgnore]
        [Ignore]
        [PropertyMapName("BorrowBHTTotal")]
        public decimal BorrowBHTTotal
        {
            get { return _BorrowBHTTotal; }
            set { }
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

                    CalcReturnTotalAmount();
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

                    CalcReturnTotalAmount();
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

                    CalcReturnTotalAmount();
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

                    CalcReturnTotalAmount();
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

                    CalcReturnTotalAmount();
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

                    CalcReturnTotalAmount();
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

                    CalcReturnTotalAmount();
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

                    CalcReturnTotalAmount();
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

                    CalcReturnTotalAmount();
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

                    CalcReturnTotalAmount();
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

                    CalcReturnTotalAmount();
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
        [JsonIgnore]
        [Ignore]
        [PropertyMapName("ReturnBHTTotal")]
        public decimal ReturnBHTTotal
        {
            get { return _ReturnBHTTotal; }
            set { }
        }

        #endregion

        #endregion

        #region Internal Class

        /// <summary>
        /// The internal FKs class for query data.
        /// </summary>
        public class FKs : UserCreditHistory, IFKs<UserCreditHistory>
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

        #endregion
    }

    #endregion
}
