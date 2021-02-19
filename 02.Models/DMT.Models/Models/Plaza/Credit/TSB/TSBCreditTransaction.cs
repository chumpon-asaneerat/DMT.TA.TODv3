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
    //TODO: ค่า max และ limit ต้องมี table เพิ่ม

    #region TSBCreditTransaction

    /// <summary>
    /// The TSBCreditTransaction Data Model class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("TSBCreditTransaction")]
    public class TSBCreditTransaction : NTable<TSBCreditTransaction>
    {
        #region Enum

        // Initialize (0) -> ยอดตั้งต้น (+)
        // Received (1) -> กรณีรับเงินจาก บ/ช (+)
        // Exchange (2) -> กรณีแลกเงิน เกิดขึ้นตอนรับเงินจาก บ/ช ซึ่งต้องทำคืนเงิน ในจำนวนเท่า ๆ กันด้วย (-)
        // Returns (3) -> กรณีคืนเงินกลับ บ/ช (-)
        // ReplaceOut (11) -> เงินแลกเปลี่ยนภายใน รับเขา (+)
        // ReplaceIn (12)-> เงินแลกเปลี่ยนภายใน จ่ายออก (-)

        /// <summary>
        /// The TSB Credit Transaction Type enum.
        /// </summary>
        public enum TransactionTypes : int
        {
            /// <summary>
            /// Initial credit.
            /// </summary>
            Initial = 0,
            /// <summary>
            /// received from account after account approve and plaza received it.
            /// </summary>
            Received = 1,
            /// <summary>
            /// exchange to account after account approve and plaza received it.
            /// </summary>
            Exchange = 2,
            /// <summary>
            /// return to account after plaza no longer need or reach due date.
            /// </summary>
            Returns = 3,
            /// <summary>
            /// Internal Replace (Takeout from TSB)
            /// </summary>
            ReplaceOut = 11,
            /// <summary>
            /// Internal Replace (Replace in TSB)
            /// </summary>
            ReplaceIn = 12
        }

        #endregion

        #region Internal Variables

        private int _TransactionId = 0;
        private Guid? _GroupId = new Guid?();
        private DateTime? _TransactionDate = new DateTime?();
        private TransactionTypes _TransactionType = TransactionTypes.Initial;

        // TSB
        private string _TSBId = string.Empty;
        private string _TSBNameEN = string.Empty;
        private string _TSBNameTH = string.Empty;

        // Plaza Group
        private string _PlazaGroupId = string.Empty;
        private string _PlazaGroupNameEN = string.Empty;
        private string _PlazaGroupNameTH = string.Empty;

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

        // Supervisor
        private string _SupervisorId = string.Empty;
        private string _SupervisorNameEN = string.Empty;
        private string _SupervisorNameTH = string.Empty;

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

        private bool _hasRemark = false;
        private string _Remark = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBCreditTransaction() : base() { }

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

        // TODO: TSBCreditBalance Check GroupId (used for what??? may be for clear data)

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
        /// Gets or sets Transaction GroupId
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets Transaction GroupId")]
        [ReadOnly(true)]
        [PropertyMapName("GroupId")]
        public Guid? GroupId
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
        /// Gets or sets Transaction Date.
        /// </summary>
        [Category("Common")]
        [Description(" Gets or sets Transaction Date")]
        [NotNull]
        [Indexed]
        [ReadOnly(true)]
        [PropertyMapName("TransactionDate")]
        public DateTime? TransactionDate
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
                var ret = (!this._TransactionDate.HasValue || this._TransactionDate.Value == DateTime.MinValue) ?
                    "" : this._TransactionDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
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
                var ret = (!this._TransactionDate.HasValue || this._TransactionDate.Value == DateTime.MinValue) ?
                    "" : this._TransactionDate.Value.ToThaiTimeString();
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
                var ret = (!this._TransactionDate.HasValue || this._TransactionDate.Value == DateTime.MinValue) ?
                    "" : this._TransactionDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
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
        [NotNull]
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

        #endregion

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
        [NotNull]
        [Indexed]
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
        [PropertyOrder(50)]
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

        #region PlazaGroup

        /// <summary>
        /// Gets or sets Plaza Group Id.
        /// </summary>
        [Category("Plaza Group")]
        [Description("Gets or sets Plaza Group Id.")]
        [ReadOnly(true)]
        [Ignore]
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

        #region Supervisor

        /// <summary>
        /// Gets or sets Supervisor Id
        /// </summary>
        [Category("Supervisor")]
        [Description("Gets or sets Supervisor Id.")]
        [ReadOnly(true)]
        [NotNull]
        [Indexed]
        [MaxLength(10)]
        [PropertyMapName("SupervisorId")]
        public string SupervisorId
        {
            get
            {
                return _SupervisorId;
            }
            set
            {
                if (_SupervisorId != value)
                {
                    _SupervisorId = value;
                    this.RaiseChanged("SupervisorId");
                }
            }
        }
        /// <summary>
        /// Gets or sets Supervisor Name EN.
        /// </summary>
        [Category("Supervisor")]
        [Description("Gets or sets Supervisor Name EN.")]
        [ReadOnly(true)]
        [MaxLength(150)]
        [PropertyMapName("Supervisor Name EN")]
        public virtual string SupervisorNameEN
        {
            get
            {
                return _SupervisorNameEN;
            }
            set
            {
                if (_SupervisorNameEN != value)
                {
                    _SupervisorNameEN = value;
                    this.RaiseChanged("SupervisorNameEN");
                }
            }
        }
        /// <summary>
        /// Gets or sets Supervisor Name TH.
        /// </summary>
        [Category("Supervisor")]
        [Description("Gets or sets Supervisor Name TH.")]
        [ReadOnly(true)]
        [MaxLength(150)]
        [PropertyMapName("SupervisorNameTH")]
        public virtual string SupervisorNameTH
        {
            get
            {
                return _SupervisorNameTH;
            }
            set
            {
                if (_SupervisorNameTH != value)
                {
                    _SupervisorNameTH = value;
                    this.RaiseChanged("SupervisorNameTH");
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
                if (value < decimal.Zero) return;
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

        #region Coin/Bill (Summary)

        /// <summary>
        /// Gets or sets total value in baht.
        /// </summary>
        [Category("Coin/Bill (Summary)")]
        [Description("Gets or sets total value in baht.")]
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
                }
            }
        }

        #endregion

        #endregion

        #region Internal Class

        /// <summary>
        /// The internal FKs class for query data.
        /// </summary>
        public class FKs : TSBCreditTransaction, IFKs<TSBCreditTransaction>
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

            #region PlazaGroup

            /// <summary>
            /// Gets or sets Plaza Group Id.
            /// </summary>
            [MaxLength(10)]
            [PropertyMapName("PlazaGroupNameEN")]
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
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets Active TSB Credit transactions.
        /// </summary>
        /// <returns>Returns Current Active TSB Credit transactions. If not found returns null.</returns>
        public static NDbResult<List<TSBCreditTransaction>> GetTransactions()
        {
            var result = new NDbResult<List<TSBCreditTransaction>>();
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
        /// Gets TSB Credit transactions.
        /// </summary>
        /// <param name="tsb">The target TSB to get transactions.</param>
        /// <returns>Returns TSB Credit transactions. If TSB not found returns null.</returns>
        public static NDbResult<List<TSBCreditTransaction>> GetTransactions(TSB tsb)
        {
            var result = new NDbResult<List<TSBCreditTransaction>>();
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
                    cmd += "  FROM TSBCreditTransactionView ";
                    cmd += " WHERE TSBId = ? ";

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
        /// Gets Initial Transaction (Active TSB).
        /// </summary>
        /// <returns>Returns Initial TSBCreditTransaction instance.</returns>
        public static NDbResult<TSBCreditTransaction> GetInitialTransaction()
        {
            var result = new NDbResult<TSBCreditTransaction>();
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
            result = GetInitialTransaction(tsb);
            return result;
        }
        /// <summary>
        /// Gets Initial Transaction of target TSB.
        /// </summary>
        /// <param name="tsb">The targe TSB instance.</param>
        /// <returns>Returns Initial TSBCreditTransaction instance.</returns>
        public static NDbResult<TSBCreditTransaction> GetInitialTransaction(TSB tsb)
        {
            var result = new NDbResult<TSBCreditTransaction>();
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
                    cmd += "  FROM TSBCreditTransactionView ";
                    cmd += " WHERE TSBId = ? ";
                    cmd += "   AND TransactionType = ? ";

                    var ret = NQuery.Query<FKs>(cmd,
                        tsb.TSBId, TransactionTypes.Initial).FirstOrDefault();
                    TSBCreditTransaction inst;
                    if (null == ret)
                    {
                        inst = Create();
                        tsb.AssignTo(inst);
                        inst.TransactionType = TransactionTypes.Initial;
                    }
                    else
                    {
                        inst = ret.ToModel();
                    }
                    result.Success(inst);
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
        /// Gets Replace Transaction of target TSB.
        /// </summary>
        /// <param name="value">The Date of Transaction.</param>
        /// <returns>Returns Initial TSBCreditTransaction instance.</returns>
        public static NDbResult<List<TSBCreditTransaction>> GetReplaceTransactions(DateTime value)
        {
            var result = new NDbResult<List<TSBCreditTransaction>>();
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
            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * ";
                    cmd += "  FROM TSBCreditTransactionView ";
                    cmd += " WHERE TSBId = ? ";
                    cmd += "   AND TransactionDate >= ? ";
                    cmd += "   AND TransactionDate <= ? ";
                    cmd += "   AND TransactionType = ? ";

                    var rets = NQuery.Query<FKs>(cmd,
                        tsb.TSBId,
                        value.Date, value.Date.AddDays(1).AddMilliseconds(-1),
                        TransactionTypes.ReplaceOut).ToList();
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
        /// <returns>Returns save transaction instance.</returns>
        public static NDbResult<TSBCreditTransaction> SaveTransaction(TSBCreditTransaction value)
        {
            var result = new NDbResult<TSBCreditTransaction>();
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
            if (!value.TransactionDate.HasValue || value.TransactionDate.Value == DateTime.MinValue)
            {
                value.TransactionDate = DateTime.Now;
            }
            result = Save(value);
            return result;
        }

        #endregion
    }

    #endregion
}
