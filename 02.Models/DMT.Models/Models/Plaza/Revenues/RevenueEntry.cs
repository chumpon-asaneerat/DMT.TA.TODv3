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
    #region Revenue Entry

    /// <summary>
    /// The RevenueEntry class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("RevenueEntry")]
    public class RevenueEntry : NTable<RevenueEntry>
    {
        #region Intenral Variables

        private Guid _PKId = Guid.NewGuid();
        private DateTime? _EntryDate = new DateTime?();
        private DateTime? _RevenueDate = new DateTime?();

        private string _RevenueId = string.Empty;
        private string _BagNo = string.Empty;
        private string _BeltNo = string.Empty;

        private bool _IsHistorical = false;

        private string _Lanes = string.Empty;
        private string _PlazaNames = string.Empty;

        private DateTime? _ShiftBegin = new DateTime?();
        private DateTime? _ShiftEnd = new DateTime?();

        private string _TSBId = string.Empty;
        private string _TSBNameEN = string.Empty;
        private string _TSBNameTH = string.Empty;

        private string _PlazaGroupId = string.Empty;
        private string _PlazaGroupNameEN = string.Empty;
        private string _PlazaGroupNameTH = string.Empty;
        private string _Direction = string.Empty;

        private int _ShiftId = 0;
        private string _ShiftNameTH = string.Empty;
        private string _ShiftNameEN = string.Empty;

        private string _UserId = string.Empty;
        private string _CollectorNameEN = string.Empty;
        private string _CollectorNameTH = string.Empty;

        private string _SupervisorId = string.Empty;
        private string _SupervisorNameEN = string.Empty;
        private string _SupervisorNameTH = string.Empty;

        // Traffic
        private int _TrafficST25 = 0;
        private int _TrafficST50 = 0;
        private int _TrafficBHT1 = 0;
        private int _TrafficBHT2 = 0;
        private int _TrafficBHT5 = 0;
        private int _TrafficBHT10 = 0;
        private int _TrafficBHT20 = 0;
        private int _TrafficBHT50 = 0;
        private int _TrafficBHT100 = 0;
        private int _TrafficBHT500 = 0;
        private int _TrafficBHT1000 = 0;
        private decimal _TrafficBHTTotal = decimal.Zero;
        private string _TrafficRemark = "";
        // Other
        private decimal _OtherBHTTotal = decimal.Zero;
        private string _OtherRemark = "";
        // NonRevenue
        private decimal _NonRevenueBHTTotal = decimal.Zero;
        private string _NonRevenueRemark = "";

        // Coupon Usage
        private int _CouponUsageBHT30 = 0;
        private int _CouponUsageBHT35 = 0;
        private int _CouponUsageBHT60 = 0;
        private int _CouponUsageBHT70 = 0;
        private int _CouponUsageBHT80 = 0;
        // Free Pass
        private int _FreePassUsageClassA = 0;
        private int _FreePassUsageOther = 0;
        // Coupon Sold
        private int _CouponSoldBHT35 = 0;
        private int _CouponSoldBHT80 = 0;
        private decimal _CouponSoldBHT35Factor = 665;
        private decimal _CouponSoldBHT80Factor = 1520;
        private decimal _CouponSoldBHT35Total = decimal.Zero;
        private decimal _CouponSoldBHT80Total = decimal.Zero;
        private decimal _CouponSoldBHTTotal = decimal.Zero;

        private int _Status = 0;
        private DateTime? _LastUpdate = new DateTime?();

        private int? _ToTAServer = new int?();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RevenueEntry() : base()
        {
        }

        #endregion

        #region Private Methods

        private void CalcTrafficTotal()
        {
            decimal total = 0;
            total += Convert.ToDecimal(_TrafficST25 * (decimal).25);
            total += Convert.ToDecimal(_TrafficST50 * (decimal).50);
            total += _TrafficBHT1 * 1;
            total += _TrafficBHT2 * 2;
            total += _TrafficBHT5 * 5;
            total += _TrafficBHT10 * 10;
            total += _TrafficBHT20 * 20;
            total += _TrafficBHT50 * 50;
            total += _TrafficBHT100 * 100;
            total += _TrafficBHT500 * 500;
            total += _TrafficBHT1000 * 1000;

            _TrafficBHTTotal = total;
            // Raise event.
            this.RaiseChanged("TrafficBHTTotal");
        }

        private void CalcCouponSoldTotal()
        {
            // Raise event.
            RaiseChanged("CntTotal");

            _CouponSoldBHT35Total = Convert.ToDecimal(_CouponSoldBHT35 * _CouponSoldBHT35Factor);
            this.RaiseChanged("CouponSoldBHT35Total");

            _CouponSoldBHT80Total = Convert.ToDecimal(_CouponSoldBHT80 * _CouponSoldBHT80Factor);
            this.RaiseChanged("CouponSoldBHT80Total");

            decimal total = 0;
            total += _CouponSoldBHT35Total;
            total += _CouponSoldBHT80Total;

            _CouponSoldBHTTotal = total;
            // Raise event.
            this.RaiseChanged("CouponSoldBHTTotal");
        }

        #endregion

        #region Public Properties

        #region Common

        /// <summary>
        /// Gets or sets PKId
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets PKId")]
        [ReadOnly(true)]
        [PrimaryKey]
        [PropertyMapName("PKId")]
        public Guid PKId
        {
            get
            {
                return _PKId;
            }
            set
            {
                if (_PKId != value)
                {
                    _PKId = value;
                    this.RaiseChanged("PKId");
                }
            }
        }
        /// <summary>
        /// Gets or sets Entry Date.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets or sets Entry Date.")]
        [NotNull]
        [Indexed]
        //[ReadOnly(true)]
        [PropertyMapName("EntryDate")]
        public DateTime? EntryDate
        {
            get { return _EntryDate; }
            set
            {
                if (_EntryDate != value)
                {
                    _EntryDate = value;
                    // Raise event.
                    this.RaiseChanged("EntryDate");
                }
            }
        }
        /// <summary>
        /// Gets Entry Date String.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets Entry Date String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string EntryDateString
        {
            get
            {
                var ret = (!this._EntryDate.HasValue || this._EntryDate.Value == DateTime.MinValue) ?
                    "" : this._EntryDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets Entry DateTime String.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets Entry DateTime String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string EntryDateTimeString
        {
            get
            {
                var ret = (!this._EntryDate.HasValue || this._EntryDate.Value == DateTime.MinValue) ?
                    "" : this._EntryDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets or sets Revenue Date.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets or sets Revenue Date.")]
        [NotNull]
        [Indexed]
        //[ReadOnly(true)]
        [PropertyMapName("RevenueDate")]
        public DateTime? RevenueDate
        {
            get { return _RevenueDate; }
            set
            {
                if (_RevenueDate != value)
                {
                    _RevenueDate = value;
                    // Raise event.
                    this.RaiseChanged("RevenueDate");
                    this.RaiseChanged("RevenueDateString");
                    this.RaiseChanged("RevenueDateTimeString");
                }
            }
        }
        /// <summary>
        /// Gets Revenue Date String.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets Revenue Date String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string RevenueDateString
        {
            get
            {
                var ret = (!this._RevenueDate.HasValue || this._RevenueDate.Value == DateTime.MinValue) ?
                    "" : this._RevenueDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets Revenue DateTime String.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets Revenue DateTime String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string RevenueDateTimeString
        {
            get
            {
                var ret = (!this._RevenueDate.HasValue || this._RevenueDate.Value == DateTime.MinValue) ?
                    "" : this._RevenueDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets or sets RevenueId.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets or sets RevenueId.")]
        //[ReadOnly(true)]
        [NotNull]
        [Indexed]
        [MaxLength(20)]
        [PropertyMapName("RevenueId")]
        public string RevenueId
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
        /// Gets or sets Bag Number.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets or sets Bag Number.")]
        //[ReadOnly(true)]
        [NotNull]
        [Indexed]
        [MaxLength(10)]
        [PropertyMapName("BagNo")]
        public string BagNo
        {
            get { return _BagNo; }
            set
            {
                if (_BagNo != value)
                {
                    _BagNo = value;
                    // Raise event.
                    this.RaiseChanged("BagNo");
                }
            }
        }
        /// <summary>
        /// Gets or sets Belt Number.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets or sets Belt Number.")]
        //[ReadOnly(true)]
        [NotNull]
        [Indexed]
        [MaxLength(20)]
        [PropertyMapName("BeltNo")]
        public string BeltNo
        {
            get { return _BeltNo; }
            set
            {
                if (_BeltNo != value)
                {
                    _BeltNo = value;
                    // Raise event.
                    this.RaiseChanged("BeltNo");
                }
            }
        }
        /// <summary>
        /// Gets or sets Is Historical.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets or sets Is Historical.")]
        [ReadOnly(true)]
        [NotNull]
        [PropertyMapName("IsHistorical")]
        public bool IsHistorical
        {
            get { return _IsHistorical; }
            set
            {
                if (_IsHistorical != value)
                {
                    _IsHistorical = value;
                    // Raise event.
                    this.RaiseChanged("IsHistorical");
                    this.RaiseChanged("IsHistoricalDisplayText");
                }
            }
        }
        /// <summary>
        /// Gets Is Historical Display Text.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets Is Historical Display Text.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string IsHistoricalDisplayText
        {
            get
            {
                return (this.IsHistorical) ? "ป้อนย้อนหลัง" : "";
            }
            set { }
        }
        /// <summary>
        /// Gets or sets Lane Lists.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets or sets Lane Lists.")]
        //[ReadOnly(true)]
        [MaxLength(100)]
        [PropertyMapName("Lanes")]
        public string Lanes
        {
            get { return _Lanes; }
            set
            {
                if (_Lanes != value)
                {
                    _Lanes = value;
                    // Raise event.
                    this.RaiseChanged("Lanes");
                }
            }
        }
        /// <summary>
        /// Gets or sets Plaza Names Lists.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets or sets Plaza Names Lists.")]
        //[ReadOnly(true)]
        [MaxLength(100)]
        [PropertyMapName("PlazaNames")]
        public string PlazaNames
        {
            get { return _PlazaNames; }
            set
            {
                if (_PlazaNames != value)
                {
                    _PlazaNames = value;
                    // Raise event.
                    this.RaiseChanged("PlazaNames");
                }
            }
        }
        /// <summary>
        /// Gets or sets Shift Begin.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets or sets Shift Begin.")]
        //[ReadOnly(true)]
        [NotNull]
        [PropertyMapName("ShiftBegin")]
        public DateTime? ShiftBegin
        {
            get { return _ShiftBegin; }
            set
            {
                if (_ShiftBegin != value)
                {
                    _ShiftBegin = value;
                    // Raise event.
                    this.RaiseChanged("ShiftBegin");
                }
            }
        }
        /// <summary>
        /// Gets or sets Shift End.
        /// </summary>
        [Category("Revenue")]
        [Description(" Gets or sets Shift End.")]
        [NotNull]
        //[ReadOnly(true)]
        [PropertyMapName("ShiftEnd")]
        public DateTime? ShiftEnd
        {
            get { return _ShiftEnd; }
            set
            {
                if (_ShiftEnd != value)
                {
                    _ShiftEnd = value;
                    // Raise event.
                    this.RaiseChanged("ShiftEnd");
                }
            }
        }
        /// <summary>
        /// Gets Shift Begin Date String.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets Shift Begin Date String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string ShiftBeginDateString
        {
            get
            {
                var ret = (!this._ShiftBegin.HasValue || this._ShiftBegin.Value == DateTime.MinValue) ?
                    "" : this._ShiftBegin.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets Shift End Date String.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets Shift End Date String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string ShiftEndDateString
        {
            get
            {
                var ret = (!this._ShiftEnd.HasValue || this._ShiftEnd.Value == DateTime.MinValue) ?
                    "" : this._ShiftEnd.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets Shift Begin Time String.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets Shift Begin Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string ShiftBeginTimeString
        {
            get
            {
                var ret = (!this._ShiftBegin.HasValue || this._ShiftBegin.Value == DateTime.MinValue) ?
                    "" : this._ShiftBegin.Value.ToThaiTimeString();
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets Shift End Time String.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets Shift End Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string ShiftEndTimeString
        {
            get
            {
                var ret = (!this._ShiftEnd.HasValue || this._ShiftEnd.Value == DateTime.MinValue) ?
                    "" : this._ShiftEnd.Value.ToThaiTimeString();
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets Shift Begin DateTime String.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets Shift Begin DateTime String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string ShiftBeginDateTimeString
        {
            get
            {
                var ret = (!this._ShiftBegin.HasValue || this._ShiftBegin.Value == DateTime.MinValue) ?
                    "" : this._ShiftBegin.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets Shift End DateTime String.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets Shift End DateTime String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string ShiftEndDateTimeString
        {
            get
            {
                var ret = (!this._ShiftEnd.HasValue || this._ShiftEnd.Value == DateTime.MinValue) ?
                    "" : this._ShiftEnd.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
                return ret;
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

        #region Shift

        /// <summary>
        /// Gets or sets Shift Id.
        /// </summary>
        [Category("Shift")]
        [Description("Gets or sets Shift Id.")]
        [ReadOnly(true)]
        [NotNull]
        [Indexed]
        [PropertyMapName("ShiftId")]
        public int ShiftId
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
        /// Gets or sets Collector Name EN.
        /// </summary>
        [Category("User")]
        [ReadOnly(true)]
        [MaxLength(150)]
        [Description("Gets or sets Collector Name EN.")]
        [PropertyMapName("CollectorNameEN")]
        public virtual string CollectorNameEN
        {
            get { return _CollectorNameEN; }
            set
            {
                if (null != _CollectorNameEN)
                {
                    _CollectorNameEN = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets Collector Name TH.
        /// </summary>
        [Category("User")]
        [Description("Gets or sets Collector Name TH.")]
        [MaxLength(150)]
        [PropertyMapName("CollectorNameTH")]
        public virtual string CollectorNameTH
        {
            get { return _CollectorNameTH; }
            set
            {
                if (null != _CollectorNameTH)
                {
                    _CollectorNameTH = value;
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

        #region Traffic

        /// <summary>
        /// Gets or sets number of .25 baht coin.
        /// </summary>
        [Category("Traffic")]
        [Description("Gets or sets number of .25 baht coin.")]
        [PropertyMapName("TrafficST25")]
        public int TrafficST25
        {
            get { return _TrafficST25; }
            set
            {
                if (value < 0) return;
                if (_TrafficST25 != value)
                {
                    _TrafficST25 = value;
                    CalcTrafficTotal();
                    // Raise event.
                    this.RaiseChanged("TrafficST25");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of .50 baht coin.
        /// </summary>
        [Category("Traffic")]
        [Description("Gets or sets number of .50 baht coin.")]
        [PropertyMapName("TrafficST50")]
        public int TrafficST50
        {
            get { return _TrafficST50; }
            set
            {
                if (value < 0) return;
                if (_TrafficST50 != value)
                {
                    _TrafficST50 = value;
                    CalcTrafficTotal();
                    // Raise event.
                    this.RaiseChanged("TrafficST50");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 1 baht coin.
        /// </summary>
        [Category("Traffic")]
        [Description("Gets or sets number of 1 baht coin.")]
        [PropertyMapName("TrafficBHT1")]
        public int TrafficBHT1
        {
            get { return _TrafficBHT1; }
            set
            {
                if (value < 0) return;
                if (_TrafficBHT1 != value)
                {
                    _TrafficBHT1 = value;
                    CalcTrafficTotal();
                    // Raise event.
                    this.RaiseChanged("TrafficBHT1");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 2 baht coin.
        /// </summary>
        [Category("Traffic")]
        [Description("Gets or sets number of 2 baht coin.")]
        [PropertyMapName("TrafficBHT2")]
        public int TrafficBHT2
        {
            get { return _TrafficBHT2; }
            set
            {
                if (value < 0) return;
                if (_TrafficBHT2 != value)
                {
                    _TrafficBHT2 = value;
                    CalcTrafficTotal();
                    // Raise event.
                    this.RaiseChanged("TrafficBHT2");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 5 baht coin.
        /// </summary>
        [Category("Traffic")]
        [Description("Gets or sets number of 5 baht coin.")]
        [PropertyMapName("TrafficBHT5")]
        public int TrafficBHT5
        {
            get { return _TrafficBHT5; }
            set
            {
                if (value < 0) return;
                if (_TrafficBHT5 != value)
                {
                    _TrafficBHT5 = value;
                    CalcTrafficTotal();
                    // Raise event.
                    this.RaiseChanged("TrafficBHT5");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 10 baht coin.
        /// </summary>
        [Category("Traffic")]
        [Description("Gets or sets number of 10 baht coin.")]
        [PropertyMapName("TrafficBHT10")]
        public int TrafficBHT10
        {
            get { return _TrafficBHT10; }
            set
            {
                if (value < 0) return;
                if (_TrafficBHT10 != value)
                {
                    _TrafficBHT10 = value;
                    CalcTrafficTotal();
                    // Raise event.
                    this.RaiseChanged("TrafficBHT10");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 20 baht bill.
        /// </summary>
        [Category("Traffic")]
        [Description("Gets or sets number of 20 baht bill.")]
        [PropertyMapName("TrafficBHT20")]
        public int TrafficBHT20
        {
            get { return _TrafficBHT20; }
            set
            {
                if (value < 0) return;
                if (_TrafficBHT20 != value)
                {
                    _TrafficBHT20 = value;
                    CalcTrafficTotal();
                    // Raise event.
                    this.RaiseChanged("TrafficBHT20");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 50 baht bill.
        /// </summary>
        [Category("Traffic")]
        [Description("Gets or sets number of 50 baht bill.")]
        [PropertyMapName("TrafficBHT50")]
        public int TrafficBHT50
        {
            get { return _TrafficBHT50; }
            set
            {
                if (value < 0) return;
                if (_TrafficBHT50 != value)
                {
                    _TrafficBHT50 = value;
                    CalcTrafficTotal();
                    // Raise event.
                    this.RaiseChanged("TrafficBHT50");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 100 baht bill.
        /// </summary>
        [Category("Traffic")]
        [Description("Gets or sets number of 100 baht bill.")]
        [PropertyMapName("TrafficBHT100")]
        public int TrafficBHT100
        {
            get { return _TrafficBHT100; }
            set
            {
                if (value < 0) return;
                if (_TrafficBHT100 != value)
                {
                    _TrafficBHT100 = value;
                    CalcTrafficTotal();
                    // Raise event.
                    this.RaiseChanged("TrafficBHT100");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 500 baht bill.
        /// </summary>
        [Category("Traffic")]
        [Description("Gets or sets number of 500 baht bill.")]
        [PropertyMapName("TrafficBHT500")]
        public int TrafficBHT500
        {
            get { return _TrafficBHT500; }
            set
            {
                if (value < 0) return;
                if (_TrafficBHT500 != value)
                {
                    _TrafficBHT500 = value;
                    CalcTrafficTotal();
                    // Raise event.
                    this.RaiseChanged("TrafficBHT500");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 1000 baht bill.
        /// </summary>
        [Category("Traffic")]
        [Description("Gets or sets number of 1000 baht bill.")]
        [PropertyMapName("TrafficBHT1000")]
        public int TrafficBHT1000
        {
            get { return _TrafficBHT1000; }
            set
            {
                if (value < 0) return;
                if (_TrafficBHT1000 != value)
                {
                    _TrafficBHT1000 = value;
                    CalcTrafficTotal();
                    // Raise event.
                    this.RaiseChanged("TrafficBHT1000");
                }
            }
        }
        /// <summary>
        /// Gets or sets total value in baht.
        /// </summary>
        [Category("Traffic")]
        [Description("Gets or sets total value in baht.")]
        [ReadOnly(true)]
        [PropertyMapName("TrafficBHTTotal")]
        public decimal TrafficBHTTotal
        {
            get { return _TrafficBHTTotal; }
            set { }
        }
        /// <summary>
        /// Gets or sets Traffic Remark.
        /// </summary>
        [Category("Traffic")]
        [Description("Gets or sets Traffic Remark.")]
        [MaxLength(255)]
        [PropertyMapName("TrafficRemark")]
        public string TrafficRemark
        {
            get { return _TrafficRemark; }
            set
            {
                if (_TrafficRemark != value)
                {
                    _TrafficRemark = value;
                    // Raise event.
                    this.RaiseChanged("TrafficRemark");
                }
            }
        }

        #endregion

        #region Other

        /// <summary>
        /// Gets or sets total value in baht (Other).
        /// </summary>
        [Category("Other")]
        [Description("Gets or sets total value in baht (Other).")]
        [PropertyMapName("OtherBHTTotal")]
        public decimal OtherBHTTotal
        {
            get { return _OtherBHTTotal; }
            set
            {
                if (value < decimal.Zero) return;
                if (_OtherBHTTotal != value)
                {
                    _OtherBHTTotal = value;
                    // Raise event.
                    this.RaiseChanged("OtherBHTTotal");
                }
            }
        }
        /// <summary>
        /// Gets or sets Other Remark.
        /// </summary>
        [Category("Other")]
        [Description("Gets or sets Other Remark.")]
        [MaxLength(255)]
        [PropertyMapName("OtherRemark")]
        public string OtherRemark
        {
            get { return _OtherRemark; }
            set
            {
                if (_OtherRemark != value)
                {
                    _OtherRemark = value;
                    // Raise event.
                    this.RaiseChanged("OtherRemark");
                }
            }
        }

        #endregion

        #region NonRevenue

        /// <summary>
        /// Gets or sets total value in baht (NonRevenue).
        /// </summary>
        [Category("NonRevenue")]
        [Description("Gets or sets total value in baht (NonRevenue).")]
        [PropertyMapName("NonRevenueBHTTotal")]
        public decimal NonRevenueBHTTotal
        {
            get { return _NonRevenueBHTTotal; }
            set
            {
                if (value < decimal.Zero) return;
                if (_NonRevenueBHTTotal != value)
                {
                    _NonRevenueBHTTotal = value;
                    // Raise event.
                    this.RaiseChanged("NonRevenueBHTTotal");
                }
            }
        }
        /// <summary>
        /// Gets or sets NonRevenue Remark.
        /// </summary>
        [Category("NonRevenue")]
        [Description("Gets or sets NonRevenue Remark.")]
        [MaxLength(255)]
        [PropertyMapName("NonRevenueRemark")]
        public string NonRevenueRemark
        {
            get { return _NonRevenueRemark; }
            set
            {
                if (_NonRevenueRemark != value)
                {
                    _NonRevenueRemark = value;
                    // Raise event.
                    this.RaiseChanged("NonRevenueRemark");
                }
            }
        }

        #endregion

        #region Coupon Usage

        /// <summary>
        /// Gets or sets number of 30 BHT coupon.
        /// </summary>
        [Category("Coupon Usage")]
        [Description("Gets or sets number of 30 BHT coupon.")]
        [PropertyMapName("CouponUsageBHT30")]
        public int CouponUsageBHT30
        {
            get { return _CouponUsageBHT30; }
            set
            {
                if (value < 0) return;
                if (_CouponUsageBHT30 != value)
                {
                    _CouponUsageBHT30 = value;
                    // Raise event.
                    this.RaiseChanged("CouponUsageBHT30");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 35 BHT coupon.
        /// </summary>
        [Category("Coupon Usage")]
        [Description("Gets or sets number of 35 BHT coupon.")]
        [PropertyMapName("CouponUsageBHT35")]
        public int CouponUsageBHT35
        {
            get { return _CouponUsageBHT35; }
            set
            {
                if (value < 0) return;
                if (_CouponUsageBHT35 != value)
                {
                    _CouponUsageBHT35 = value;
                    // Raise event.
                    this.RaiseChanged("CouponUsageBHT35");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 60 BHT coupon.
        /// </summary>
        [Category("Coupon Usage")]
        [Description("Gets or sets number of 60 BHT coupon.")]
        [PropertyMapName("CouponUsageBHT60")]
        public int CouponUsageBHT60
        {
            get { return _CouponUsageBHT60; }
            set
            {
                if (value < 0) return;
                if (_CouponUsageBHT60 != value)
                {
                    _CouponUsageBHT60 = value;
                    // Raise event.
                    this.RaiseChanged("CouponUsageBHT60");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 70 BHT coupon.
        /// </summary>
        [Category("Coupon Usage")]
        [Description("Gets or sets number of 70 BHT coupon.")]
        [PropertyMapName("CouponUsageBHT70")]
        public int CouponUsageBHT70
        {
            get { return _CouponUsageBHT70; }
            set
            {
                if (value < 0) return;
                if (_CouponUsageBHT70 != value)
                {
                    _CouponUsageBHT70 = value;
                    // Raise event.
                    this.RaiseChanged("CouponUsageBHT70");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 80 BHT coupon.
        /// </summary>
        [Category("Coupon Usage")]
        [Description("Gets or sets number of 80 BHT coupon.")]
        [PropertyMapName("CouponUsageBHT80")]
        public int CouponUsageBHT80
        {
            get { return _CouponUsageBHT80; }
            set
            {
                if (value < 0) return;
                if (_CouponUsageBHT80 != value)
                {
                    _CouponUsageBHT80 = value;
                    // Raise event.
                    this.RaiseChanged("CouponUsageBHT80");
                }
            }
        }

        #endregion

        #region FreePass (Usage)

        /// <summary>
        /// Gets or sets number of FreePass Class A (4 wheel).
        /// </summary>
        [Category("FreePass")]
        [Description("Gets or sets number of FreePass Class A (4 wheel).")]
        [PropertyMapName("FreePassUsageClassA")]
        public int FreePassUsageClassA
        {
            get { return _FreePassUsageClassA; }
            set
            {
                if (value < 0) return;
                if (_FreePassUsageClassA != value)
                {
                    _FreePassUsageClassA = value;
                    // Raise event.
                    this.RaiseChanged("FreePassUsageClassA");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of FreePass Other (> 4 wheel).
        /// </summary>
        [Category("FreePass")]
        [Description("Gets or sets number of FreePass Other (> 4 wheel).")]
        [PropertyMapName("FreePassUsageOther")]
        public int FreePassUsageOther
        {
            get { return _FreePassUsageOther; }
            set
            {
                if (value < 0) return;
                if (_FreePassUsageOther != value)
                {
                    _FreePassUsageOther = value;
                    // Raise event.
                    this.RaiseChanged("FreePassUsageOther");
                }
            }
        }

        #endregion

        #region Coupon Sold

        /// <summary>
        /// Gets or sets is coupon sold entry enable (Runtime Use only).
        /// </summary>
        [Category("Coupon Sold")]
        [Description("Gets or sets is coupon sold entry enable.")]
        [Ignore]
        [JsonIgnore]
        public bool CouponSoldEnable { get; set; }

        /// <summary>
        /// Gets or sets number of 35 BHT coupon.
        /// </summary>
        [Category("Coupon Sold")]
        [Description("Gets or sets number of 35 BHT coupon.")]
        [PropertyMapName("CouponSoldBHT35")]
        public int CouponSoldBHT35
        {
            get { return _CouponSoldBHT35; }
            set
            {
                if (value < 0) return;
                if (_CouponSoldBHT35 != value)
                {
                    _CouponSoldBHT35 = value;
                    CalcCouponSoldTotal();
                    // Raise event.
                    this.RaiseChanged("CouponSoldBHT35");

                }
            }
        }
        /// <summary>
        /// Gets or sets number of 80 BHT coupon.
        /// </summary>
        [Category("Coupon Sold")]
        [Description("Gets or sets number of 80 BHT coupon.")]
        [PropertyMapName("CouponSoldBHT80")]
        public int CouponSoldBHT80
        {
            get { return _CouponSoldBHT80; }
            set
            {
                if (value < 0) return;
                if (_CouponSoldBHT80 != value)
                {
                    _CouponSoldBHT80 = value;
                    CalcCouponSoldTotal();
                    // Raise event.
                    this.RaiseChanged("CouponSoldBHT80");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 35 BHT coupon factor.
        /// </summary>
        [Category("Coupon Sold")]
        [Description("Gets or sets number of 35 BHT coupon factor.")]
        [PropertyMapName("CouponSoldBHT35Factor")]
        public decimal CouponSoldBHT35Factor
        {
            get { return _CouponSoldBHT35Factor; }
            set
            {
                if (value < decimal.Zero) return;
                if (_CouponSoldBHT35Factor != value)
                {
                    _CouponSoldBHT35Factor = value;
                    CalcCouponSoldTotal();
                    // Raise event.
                    this.RaiseChanged("CouponSoldBHT35Factor");
                }
            }
        }
        /// <summary>
        /// Gets or sets number of 80 BHT coupon factor.
        /// </summary>
        [Category("Coupon Sold")]
        [Description("Gets or sets number of 80 BHT coupon factor.")]
        [PropertyMapName("CouponSoldBHT80Factor")]
        public decimal CouponSoldBHT80Factor
        {
            get { return _CouponSoldBHT80Factor; }
            set
            {
                if (value < decimal.Zero) return;
                if (_CouponSoldBHT80Factor != value)
                {
                    _CouponSoldBHT80Factor = value;
                    CalcCouponSoldTotal();
                    // Raise event.
                    this.RaiseChanged("CouponSoldBHT80Factor");
                }
            }
        }
        /// <summary>
        /// Gets 35 BHT coupon total in BHT.
        /// </summary>
        [Category("Coupon Sold")]
        [Description("Gets 35 BHT coupon total in BHT.")]
        [ReadOnly(true)]
        [PropertyMapName("CouponSoldBHT35Total")]
        public decimal CouponSoldBHT35Total
        {
            get { return _CouponSoldBHT35Total; }
            set { }
        }
        /// <summary>
        /// Gets 80 BHT coupon total in BHT.
        /// </summary>
        [Category("Coupon Sold")]
        [Description("Gets 80 BHT coupon total in BHT.")]
        [ReadOnly(true)]
        [PropertyMapName("CouponSoldBHT80Total")]
        public decimal CouponSoldBHT80Total
        {
            get { return _CouponSoldBHT80Total; }
            set { }
        }
        /// <summary>
        /// Gets or sets total value in baht.
        /// </summary>
        [Category("Coupon Sold")]
        [Description("Gets or sets total value in baht.")]
        [ReadOnly(true)]
        [PropertyMapName("CouponSoldBHTTotal")]
        public decimal CouponSoldBHTTotal
        {
            get { return _CouponSoldBHTTotal; }
            set { }
        }

        #endregion

        #region Server Status

        /// <summary>
        /// Gets or sets Status (1 = Generated, 0 = None)
        /// </summary>
        [Category("Server Status")]
        [Description("Gets or sets Status (1 = Send, 0 = Unsend)")]
        [ReadOnly(true)]
        [PropertyMapName("Status", typeof(RevenueEntry))]
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
        /// Gets or sets LastUpdated (generated date).
        /// </summary>
        [Category("Server Status")]
        [Description("Gets or sets LastUpdated (generated date).")]
        [ReadOnly(true)]
        [PropertyMapName("LastUpdate", typeof(RevenueEntry))]
        [PropertyOrder(10002)]
        public DateTime? LastUpdate
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

        #region ToTAServer

        /// <summary>
        /// Gets or sets ToTAServer
        /// </summary>
        [Category("User")]
        [Description("Gets or sets ToTAServer.")]
        [PropertyMapName("ToTAServer")]
        public int? ToTAServer
        {
            get
            {
                return _ToTAServer;
            }
            set
            {
                if (_ToTAServer != value)
                {
                    _ToTAServer = value;
                    this.RaiseChanged("ToTAServer");
                }
            }
        }

        #endregion

        #endregion

        #region Internal Class

        /// <summary>
        /// The internal FKs class for query data.
        /// </summary>
        public class FKs : RevenueEntry, IFKs<RevenueEntry>
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

            #region Shift

            /// <summary>
            /// Gets or sets Shift Name TH.
            /// </summary>
            [MaxLength(50)]
            [PropertyMapName("ShiftNameTH")]
            public override string ShiftNameTH
            {
                get { return base.ShiftNameTH; }
                set { base.ShiftNameTH = value; }
            }
            /// <summary>
            /// Gets or sets Shift Name EN.
            /// </summary>
            [MaxLength(50)]
            [PropertyMapName("ShiftNameEN")]
            public override string ShiftNameEN
            {
                get { return base.ShiftNameEN; }
                set { base.ShiftNameEN = value; }
            }

            #endregion
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets all Revenue Enties.
        /// </summary>
        /// <returns>Returns List of RevenueEntry.</returns>
        public static NDbResult<List<RevenueEntry>> GetRevenueEnties()
        {
            var result = new NDbResult<List<RevenueEntry>>();
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
                    string cmd = string.Empty;
                    cmd += "SELECT * ";
                    cmd += "  FROM RevenueEntryView ";

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
        /// <summary>
        /// Gets Revenue Enties by TSBId.
        /// </summary>
        /// <param name="tsbid">The TSB Id.</param>
        /// <returns>Returns List of RevenueEntry.</returns>
        public static NDbResult<List<RevenueEntry>> GetRevenueEnties(string tsbid)
        {
            var result = new NDbResult<List<RevenueEntry>>();
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
                    string cmd = string.Empty;
                    cmd += "SELECT * ";
                    cmd += "  FROM RevenueEntryView ";
                    cmd += " WHERE TSBId = ? ";

                    var rets = NQuery.Query<FKs>(cmd, tsbid).ToList();
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
        /// Find Revenue Enties by revenue date.
        /// </summary>
        /// <param name="begin">The Begin Revenue Date Time.</param>
        /// <param name="end">The End Revenue Date Time.</param>
        /// <param name="shiftId">The Shift Id filter.</param>
        /// <returns>Returns List of RevenueEntry.</returns>
        public static NDbResult<List<RevenueEntry>> FindByRevnueDate(DateTime begin, DateTime end, int? shiftId)
        {
            var result = new NDbResult<List<RevenueEntry>>();
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
                    string cmd = string.Empty;
                    cmd += "SELECT * ";
                    cmd += "  FROM RevenueEntryView ";
                    cmd += " WHERE RevenueDate >= ? ";
                    cmd += "   AND RevenueDate <= ? ";
                    if (shiftId.HasValue && shiftId.Value > 0)
                    {
                        cmd += "   AND ShiftId = ? ";
                    }
                    cmd += "   ORDER BY ShiftId, BagNo ";

                    if (shiftId.HasValue && shiftId.Value > 0)
                    {
                        // Has Shift Filter.
                        var rets = NQuery.Query<FKs>(cmd, begin, end, shiftId.Value).ToList();
                        var results = rets.ToModels();
                        result.Success(results);
                    }
                    else
                    {
                        // No Shift Filter.
                        var rets = NQuery.Query<FKs>(cmd, begin, end).ToList();
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
        /// Find Revenue Enties by date.
        /// </summary>
        /// <param name="date">The Revenue Date.</param>
        /// <param name="shiftId">The Shift Id filter.</param>
        /// <returns>Returns List of RevenueEntry.</returns>
        public static NDbResult<List<RevenueEntry>> FindByRevnueDate(DateTime date, int? shiftId)
        {
            DateTime begin = date.Date;
            DateTime end = date.Date.AddDays(1).AddMilliseconds(-1);
            return FindByRevnueDate(begin, end, shiftId);
        }

        /*
        /// <summary>
        /// Find Unsend Revenue Enties by date.
        /// </summary>
        /// <returns>Returns List of RevenueEntry.</returns>
        public static NDbResult<List<RevenueEntry>> FindUnsendRevenueEnties()
        {
            var result = new NDbResult<List<RevenueEntry>>();
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
                    string cmd = string.Empty;
                    cmd += "SELECT * ";
                    cmd += "  FROM RevenueEntryView ";
                    cmd += " WHERE Status = 0 "; // for all unsync (status = 0).
                    cmd += "   AND (RevenueId <> '' AND RevenueId IS NOT NULL) "; // has revenue id.

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
        */

        /// <summary>
        /// Gets all UnSync Revenue Enties.
        /// </summary>
        /// <returns>Returns List of RevenueEntry.</returns>
        public static NDbResult<List<RevenueEntry>> GetUnSyncRevenueEnties()
        {
            var result = new NDbResult<List<RevenueEntry>>();
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
                    int iCnt = db.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM RevenueEntry WHERE NonRevenueBHTTotal IS NULL");
                    if (iCnt > 0)
                    {
                        med.Info("SET DEFAULT VALUE FOR RevenueEntry.NonRevenueBHTTotal.");
                        db.Execute("UPDATE RevenueEntry SET NonRevenueBHTTotal = 0 WHERE NonRevenueBHTTotal IS NULL");
                    }
                }
                catch (Exception updateErr1)
                {
                    med.Err(updateErr1);
                }
                try
                {
                    int iCnt = db.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM RevenueEntry WHERE NonRevenueRemark IS NULL");
                    if (iCnt > 0)
                    {
                        med.Info("SET DEFAULT VALUE FOR RevenueEntry.NonRevenueRemark.");
                        db.Execute("UPDATE RevenueEntry SET NonRevenueRemark = '' WHERE NonRevenueRemark IS NULL");
                    }
                }
                catch (Exception updateErr2)
                {
                    med.Err(updateErr2);
                }

                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * ";
                    cmd += "  FROM RevenueEntryView ";
                    cmd += " WHERE ToTAServer IS NULL ";

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
