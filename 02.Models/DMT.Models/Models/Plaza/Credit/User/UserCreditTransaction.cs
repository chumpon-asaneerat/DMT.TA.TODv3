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
    #region UserCreditTransaction

    /// <summary>
    /// The UserCreditTransaction Data Model class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("UserCreditTransaction")]
    public class UserCreditTransaction : NTable<UserCreditTransaction>
    {
        #region Enum

        /// <summary>
        /// The User Credit Transaction Types enum.
        /// </summary>
        public enum TransactionTypes
        {
            /// <summary>
            /// Borrow
            /// </summary>
            Borrow = 1,
            /// <summary>
            /// Return
            /// </summary>
            Return = 2,
            /// <summary>
            /// Undo - Borrow
            /// </summary>
            UndoBorrow = 3,
            /// <summary>
            /// Undo - Return
            /// </summary>
            UndoReturn = 4
        }

        #endregion

        #region Internal Variables

        // For Runtime Used
        private string _description = string.Empty;
        private bool _hasRemark = false;

        private int _TransactionId = 0;
        private DateTime? _TransactionDate = new DateTime?();
        private TransactionTypes _TransactionType = TransactionTypes.Borrow;

        private int _RefId = 0; // for undo.

        private int _UserCreditId = 0;

        private string _TSBId = string.Empty;
        private string _TSBNameEN = string.Empty;
        private string _TSBNameTH = string.Empty;

        private string _PlazaGroupId = string.Empty;
        private string _PlazaGroupNameEN = string.Empty;
        private string _PlazaGroupNameTH = string.Empty;
        private string _Direction = string.Empty;

        private int? _ShiftId = new int?();
        private string _ShiftNameTH = string.Empty;
        private string _ShiftNameEN = string.Empty;

        private string _UserId = string.Empty;
        private string _FullNameEN = string.Empty;
        private string _FullNameTH = string.Empty;

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
        private string _Remark = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserCreditTransaction() : base() { }

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
        /// Gets  Remark Visibility.
        /// </summary>
        [Category("Runtime")]
        [Description("Gets Remark Visibility.")]
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
        [NotNull]
        [Indexed]
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
        /// <summary>
        /// Gets Transaction Type in string.
        /// </summary>
        [Category("Common")]
        [Description("Gets Transaction Type in string.")]
        [Ignore]
        [JsonIgnore]
        public string TransactionTypeString
        {
            get 
            {
                string str = string.Empty;
                if (_TransactionType == TransactionTypes.Borrow) str = "ยืมเงิน";
                else if (_TransactionType == TransactionTypes.Return) str = "คืนเงิน";
                else if (_TransactionType == TransactionTypes.UndoBorrow) str = "ยกเเลิกการยืมเงิน";
                else if (_TransactionType == TransactionTypes.UndoReturn) str = "ยกเเลิกการคืนเงิน";
                return str;
            }
            set { }
        }

        /// <summary>
        /// Gets or sets RefId
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets RefId")]
        [Indexed]
        [ReadOnly(true)]
        [PropertyMapName("RefId")]
        public int RefId
        {
            get
            {
                return _RefId;
            }
            set
            {
                if (_RefId != value)
                {
                    _RefId = value;
                    this.RaiseChanged("RefId");
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

        #endregion

        #region PlazaGroup

        /// <summary>
        /// Gets or sets Plaza Group Id.
        /// </summary>
        [Category("Plaza Group")]
        [Description("Gets or sets Plaza Group Id.")]
        [ReadOnly(true)]
        [NotNull]
        [Indexed]
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
        [NotNull]
        [Indexed]
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

        #region UserCredit

        /// <summary>
        /// Gets or sets UserCreditId
        /// </summary>
        [Category("UserCredit")]
        [Description("Gets or sets UserCreditId")]
        [ReadOnly(true)]
        [Indexed]
        [PropertyMapName("UserCreditId")]
        public int UserCreditId
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

        #endregion

        //TODO: Need to Create column to keep current Chift

        #region Supervisor - Need to Create column to keep current Chift

        /// <summary>
        /// Gets or sets SupervisorId.
        /// </summary>
        [Ignore]
        [JsonIgnore]
        public string SupervisorId { get; set; }
        /// <summary>
        /// Gets or sets Supervisor Name EN.
        /// </summary>
        [Ignore]
        [JsonIgnore]
        public string SupervisorNameEN { get; set; }
        /// <summary>
        /// Gets or sets Supervisor Name TH.
        /// </summary>
        [Ignore]
        [JsonIgnore]
        public string SupervisorNameTH { get; set; }

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

        #endregion

        #region Internal Class

        /// <summary>
        /// The internal FKs class for query data.
        /// </summary>
        public class FKs : UserCreditTransaction, IFKs<UserCreditTransaction>
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

            #region PlazaGroup

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
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets Active TSB User Credit transactions.
        /// </summary>
        /// <returns>
        /// Returns Current Active TSB User Credit transactions.
        /// If not found returns null.
        /// </returns>
        public static NDbResult<List<UserCreditTransaction>> GetUserCreditTransactions()
        {
            var result = new NDbResult<List<UserCreditTransaction>>();
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
            result = GetUserCreditTransactions(tsb);
            return result;
        }
        /// <summary>
        /// Gets User Credit transactions.
        /// </summary>
        /// <param name="tsb">The target TSB to get transactions.</param>
        /// <returns>
        /// Returns User Credit transactions. If TSB not found returns null.
        /// </returns>
        public static NDbResult<List<UserCreditTransaction>> GetUserCreditTransactions(TSB tsb)
        {
            var result = new NDbResult<List<UserCreditTransaction>>();
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
                    cmd += "  FROM UserCreditTransactionView ";
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
        /// Gets User Credit transactions (only Borrow/Return not include undo (Borrow/Return)).
        /// </summary>
        /// <param name="tsb">The target TSB to get transactions.</param>
        /// <param name="user">The target User to get transactions.</param>
        /// <param name="begin">The Begin Date Time.</param>
        /// <param name="end">The End Date Time.</param>
        /// <returns>
        /// Returns User Credit transactions. If TSB not found returns null.
        /// </returns>
        public static NDbResult<List<UserCreditTransaction>> GetUserCreditTransactions(TSB tsb,
            User user, DateTime begin, DateTime end)
        {
            var result = new NDbResult<List<UserCreditTransaction>>();
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
                    if (null != user)
                    {
                        string cmd = string.Empty;
                        cmd += "SELECT * ";
                        cmd += "  FROM UserCreditTransactionView ";
                        cmd += " WHERE TSBId = ? ";
                        cmd += "   AND UserId = ? ";
                        cmd += "   AND TransactionDate >= ? ";
                        cmd += "   AND TransactionDate < ? ";
                        cmd += "   AND (TransactionType = 1 OR TransactionType = 2)";

                        var rets = NQuery.Query<FKs>(cmd, tsb.TSBId, user.UserId, begin, end).ToList();
                        var results = rets.ToModels();
                        result.Success(results);
                    }
                    else
                    {
                        string cmd = string.Empty;
                        cmd += "SELECT * ";
                        cmd += "  FROM UserCreditTransactionView ";
                        cmd += " WHERE TSBId = ? ";
                        cmd += "   AND TransactionDate >= ? ";
                        cmd += "   AND TransactionDate < ? ";
                        cmd += "   AND (TransactionType = 1 OR TransactionType = 2)";

                        var rets = NQuery.Query<FKs>(cmd, tsb.TSBId, begin, end).ToList();
                        var results = rets.ToModels();
                        result.Success(results);
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
        /// <summary>
        /// Save Transaction.
        /// </summary>
        /// <param name="value">The User Credit Transaction instance.</param>
        /// <returns>Returns save transaction.</returns>
        public static NDbResult<UserCreditTransaction> SaveUserCreditTransaction(
            UserCreditTransaction value)
        {
            var result = new NDbResult<UserCreditTransaction>();
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
            else
            {
                if (!value.TransactionDate.HasValue || value.TransactionDate.Value == DateTime.MinValue)
                {
                    value.TransactionDate = DateTime.Now;
                }
                result = Save(value);
            }
            return result;
        }

        #endregion
    }

    #endregion
}
