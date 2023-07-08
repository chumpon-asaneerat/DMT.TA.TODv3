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
    // Server data result.
    /*
   {
      "RowNo": "1",
      "CouponPK": 16,
      "TransactionDate": "2020-11-16T10:16:18.910Z",
      "TSBId": "319",
      "CouponType": "35",
      "SerialNo": "ข003997",
      "Price": 665,
      "UserId": "",
      "UserReceiveDate": null,
      "CouponStatus": "1",
      "SoldDate": null,
      "SoldBy": null,
      "LaneId": null,
      "FinishFlag": "1",
      "SapChooseFlag": "1",
      "SapChooseDate": null,
      "SAPSysSerial": 742111,
      "SAPWhsCode": "CAS",
      "TollWayId": 9,
      "SAPItemName": "Coupon 35 Baht",
      "sendtaflag": "1"
    }
    */

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
        #region Internal Variables

        // server pk.
        private int? _CouponPK = new int?();

        private int _TransactionId = 0;
        private DateTime _TransactionDate = DateTime.MinValue;
        private TSBCouponTransactionTypes _TransactionType = TSBCouponTransactionTypes.Stock;

        // Coupon 
        private string _CouponId = string.Empty;
        private CouponType _CouponType = CouponType.BHT35;
        private decimal _Price = 665;
        // TSB
        private string _TSBId = string.Empty;
        private string _TSBNameEN = string.Empty;
        private string _TSBNameTH = string.Empty;
        // Plaza Group
        private string _PlazaGroupId = string.Empty;
        private string _PlazaGroupNameEN = string.Empty;
        private string _PlazaGroupNameTH = string.Empty;
        // User
        private string _UserId = string.Empty;
        private string _FullNameEN = string.Empty;
        private string _FullNameTH = string.Empty;
        private DateTime? _UserReceivedDate = new DateTime?();

        private DateTime? _UserReturnDate = new DateTime?();

        // Lane
        private string _LaneId = string.Empty;

        // Sold
        private DateTime? _SoldDate = new DateTime?();
        private string _SoldBy = string.Empty;
        private string _SoldByFullNameEN = string.Empty;
        private string _SoldByFullNameTH = string.Empty;

        private string _TSBInvoiceId = string.Empty;

        private TSBCouponFinishedFlags _FinishFlag = TSBCouponFinishedFlags.Avaliable;

        private int _Status = 0;
        private DateTime? _LastUpdate = new DateTime?();

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
                bool isSold = TransactionType == TSBCouponTransactionTypes.SoldByLane || 
                    TransactionType == TSBCouponTransactionTypes.SoldByTSB;
                return (!isSold) ? BlackForeground : RedForeground; 
            }
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
        public TSBCouponTransactionTypes TransactionType
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
        public virtual TSBCouponFinishedFlags FinishFlag
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

        #region Server

        /// <summary>
        /// Gets or sets CouponPK.
        /// </summary>
        [Category("Server")]
        [Description(" Gets or sets CouponPK.")]
        [ReadOnly(true)]
        [PropertyMapName("CouponPK")]
        public int? CouponPK
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

        /// <summary>
        /// Gets or sets SapChooseFlag.
        /// </summary>
        [Category("Server")]
        [Description(" Gets or sets SapChooseFlag.")]
        [ReadOnly(true)]
        [PropertyMapName("SapChooseFlag")]
        public int? SapChooseFlag { get; set; }

        /// <summary>
        /// Gets or sets SapChooseDate.
        /// </summary>
        [Category("Server")]
        [Description(" Gets or sets SapChooseDate.")]
        [ReadOnly(true)]
        [PropertyMapName("SapChooseDate")]
        public DateTime? SapChooseDate { get; set; }

        /// <summary>
        /// Gets or sets SAPSysSerial.
        /// </summary>
        [Category("Server")]
        [Description(" Gets or sets SAPSysSerial.")]
        [ReadOnly(true)]
        [PropertyMapName("SAPSysSerial")]
        public string SAPSysSerial { get; set; }

        /// <summary>
        /// Gets or sets SAPWhsCode.
        /// </summary>
        [Category("Server")]
        [Description(" Gets or sets SAPWhsCode.")]
        [ReadOnly(true)]
        [PropertyMapName("SAPWhsCode")]
        public string SAPWhsCode { get; set; }

        /// <summary>
        /// Gets or sets TollWayId.
        /// </summary>
        [Category("Server")]
        [Description(" Gets or sets TollWayId.")]
        [ReadOnly(true)]
        [PropertyMapName("TollWayId")]
        public int? TollWayId { get; set; }

        /// <summary>
        /// Gets or sets SAPItemName.
        /// </summary>
        [Category("Server")]
        [Description(" Gets or sets SAPItemName.")]
        [ReadOnly(true)]
        [PropertyMapName("SAPItemName")]
        public string SAPItemName { get; set; }

        /// <summary>
        /// Gets or sets sendtaflag.
        /// </summary>
        [Category("Server")]
        [Description(" Gets or sets sendtaflag.")]
        [ReadOnly(true)]
        [PropertyMapName("sendtaflag")]
        public int? sendtaflag { get; set; }

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
        public DateTime? UserReceiveDate
        {
            get
            {
                return _UserReceivedDate;
            }
            set
            {
                if (_UserReceivedDate != value)
                {
                    _UserReceivedDate = (value.HasValue && value.Value != DateTime.MinValue) ?
                        value : new DateTime?();
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
                var ret = (!this.UserReceiveDate.HasValue || this.UserReceiveDate.Value == DateTime.MinValue) ?
                    string.Empty : this.UserReceiveDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
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
                var ret = (!this.UserReceiveDate.HasValue || this.UserReceiveDate.Value == DateTime.MinValue) ?
                    string.Empty : this.UserReceiveDate.Value.ToThaiTimeString();
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
                var ret = (!this.UserReceiveDate.HasValue || this.UserReceiveDate.Value == DateTime.MinValue) ?
                    string.Empty : this.UserReceiveDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
                return ret;
            }
            set { }
        }

        #endregion

        #region User Return Date (comment out)
        /*
        /// <summary>
        /// Gets or sets User Return Date.
        /// </summary>
        [Category("User")]
        [Description(" Gets or sets User Return Date")]
        [ReadOnly(true)]
        [PropertyMapName("UserReturnDate")]
        public DateTime? UserReturnDate
        {
            get
            {
                return _UserReturnDate;
            }
            set
            {
                if (_UserReturnDate != value)
                {
                    _UserReturnDate = (value.HasValue && value.Value != DateTime.MinValue) ?
                        value : new DateTime?();
                    this.RaiseChanged("UserReturnDateDate");
                    this.RaiseChanged("UserReturnDateDateString");
                    this.RaiseChanged("UserReturnDateTimeString");
                    this.RaiseChanged("UserReturnDateDateTimeString");
                }
            }
        }
        /// <summary>
        /// Gets User Return Date String.
        /// </summary>
        [Category("User")]
        [Description("Gets User Return Date String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string UserReturnDateString
        {
            get
            {
                var ret = (!this.UserReturnDate.HasValue || this.UserReturnDate.Value == DateTime.MinValue) ?
                    string.Empty : this.UserReturnDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets User Return Time String.
        /// </summary>
        [Category("User")]
        [Description("Gets User Return Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string UserReturnTimeString
        {
            get
            {
                var ret = (!this.UserReturnDate.HasValue || this.UserReturnDate.Value == DateTime.MinValue) ?
                    string.Empty : this.UserReturnDate.Value.ToThaiTimeString();
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets User Return Date Time String.
        /// </summary>
        [Category("User")]
        [Description("Gets User Return Date Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string UserReturnDateTimeString
        {
            get
            {
                var ret = (!this.UserReturnDate.HasValue || this.UserReturnDate.Value == DateTime.MinValue) ?
                    string.Empty : this.UserReturnDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
                return ret;
            }
            set { }
        }
        */
        #endregion

        #region Lane

        /// <summary>
        /// Gets or sets LaneId
        /// </summary>
        [Category("Sold")]
        [Description("Gets or sets LaneId")]
        [ReadOnly(true)]
        [MaxLength(10)]
        [PropertyMapName("LaneId")]
        public virtual string LaneId
        {
            get
            {
                return _LaneId;
            }
            set
            {
                if (_LaneId != value)
                {
                    _LaneId = value;
                    this.RaiseChanged("LaneId");
                }
            }
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
        public DateTime? SoldDate
        {
            get
            {
                return _SoldDate;
            }
            set
            {
                if (_SoldDate != value)
                {
                    _SoldDate = (value.HasValue && value.Value != DateTime.MinValue) ? 
                        value : new DateTime?();
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
                var ret = (!this.SoldDate.HasValue || this.SoldDate.Value == DateTime.MinValue) ?
                    string.Empty : this.SoldDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
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
                var ret = (!this.SoldDate.HasValue || this.SoldDate.Value == DateTime.MinValue) ?
                    string.Empty : this.SoldDate.Value.ToThaiTimeString();
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
                var ret = (!this.SoldDate.HasValue || this.SoldDate.Value == DateTime.MinValue) ?
                    string.Empty : this.SoldDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets or sets TSBInvoiceId
        /// </summary>
        [Category("Sold")]
        [Description("Gets or sets TSBInvoiceId")]
        [MaxLength(15)]
        [PropertyMapName("TSBInvoiceId")]
        public virtual string TSBInvoiceId
        {
            get
            {
                return _TSBInvoiceId;
            }
            set
            {
                if (_TSBInvoiceId != value)
                {
                    _TSBInvoiceId = value;
                    this.RaiseChanged("TSBInvoiceId");
                }
            }
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

        #region Status

        /// <summary>
        /// Gets or sets Status
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
        /// Gets or sets LastUpdated.
        /// </summary>
        [Category("DataCenter")]
        [Description("Gets or sets LastUpdated.")]
        [ReadOnly(true)]
        [PropertyMapName("LastUpdate", typeof(TSBCouponTransaction))]
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
        /// <summary>
        /// Sync Transaction
        /// </summary>
        /// <param name="value">The TSBCouponTransaction instance.</param>
        /// <returns>Returns instance of NDbResult.</returns>
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
        /// <summary>
        /// Sync Transactions
        /// </summary>
        /// <param name="values">The TSBCouponTransaction List.</param>
        /// <returns>Returns instance of NDbResult.</returns>
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
        /// <summary>
        /// Gets Coupon Count (all).
        /// </summary>
        /// <returns>Returns number of coupon records on local database.</returns>
        public static NDbResult<int> GetCouponCount()
        {
            var result = new NDbResult<int>();
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
            result = GetCouponCount(tsb);
            return result;
        }
        /// <summary>
        /// Gets Coupon Count (all).
        /// </summary>
        /// <param name="tsb">The TSB instance.</param>
        /// <returns>Returns number of coupon records on local database.</returns>
        public static NDbResult<int> GetCouponCount(TSB tsb)
        {
            NDbResult<int> result = new NDbResult<int>();
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
                    cmd += "SELECT COUNT(*) ";
                    cmd += "  FROM TSBCouponTransaction ";
                    cmd += " WHERE TSBId = ? ";

                    var rets = db.ExecuteScalar<int>(cmd, tsb.TSBId);
                    result.Success(rets);
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
        /// Gets User Coupon sold by lane transactions (all status).
        /// </summary>
        /// <param name="tsb">The target TSB to get coupon transaction.</param>
        /// <param name="plazaGroup">The target PlazaGroup to get balance.</param>
        /// <param name="user">The target User to get balance.</param>
        /// <param name="start">The start of sold date.</param>
        /// <param name="end">The end of sold date.</param>
        /// <returns>Returns User Coupon transactions. If TSB not found returns null.</returns>
        public static NDbResult<List<TSBCouponTransaction>> GetUserCouponSoldByLaneTransactions(TSB tsb,
            //PlazaGroup plazaGroup, 
            User user, DateTime? start, DateTime? end)
        {
            var result = new NDbResult<List<TSBCouponTransaction>>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (null == tsb || 
                //null == plazaGroup || 
                null == user)
            {
                result.ParameterIsNull();
                return result;
            }

            if (!start.HasValue || start.Value == DateTime.MinValue)
            {
                // Check Start Date.
                result.ParameterIsNull();
                return result;
            }

            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    DateTime dt1 = start.Value;
                    DateTime dt2 = (end.HasValue && end.Value != DateTime.MinValue) ? end.Value : DateTime.Now;

                    string cmd = string.Empty;
                    cmd += "SELECT * ";
                    cmd += "  FROM UserCouponSoldByLaneTransactionView ";
                    cmd += " WHERE TSBId = ? ";
                    //cmd += "   AND PlazaGroupId = ? "; // ignore plazagroup
                    cmd += "   AND SoldBy = ? ";
                    cmd += "   AND SoldDate >= ? ";
                    cmd += "   AND SoldDate < ? ";

                    var rets = NQuery.Query<FKs>(cmd, tsb.TSBId,
                        //plazaGroup.PlazaGroupId, // ignore plazagroup
                        user.UserId,
                        dt1, dt2).ToList();
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
        /*
        /// <summary>
        /// Gets User Coupon OnHand transactions.
        /// </summary>
        /// <param name="tsb">The target TSB to get coupon transaction.</param>
        /// <param name="plazaGroup">The target PlazaGroup to get balance.</param>
        /// <param name="user">The target User to get balance.</param>
        /// <returns>Returns User Coupon transactions. If TSB not found returns null.</returns>
        public static NDbResult<List<TSBCouponTransaction>> GetUserCouponOnHandTransactions(TSB tsb,
            //PlazaGroup plazaGroup, 
            User user)
        {
            var result = new NDbResult<List<TSBCouponTransaction>>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (null == tsb ||
                //null == plazaGroup || 
                null == user)
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
                    cmd += "  FROM UserCouponOnHandTransactionView ";
                    cmd += " WHERE TSBId = ? ";
                    //cmd += "   AND PlazaGroupId = ? "; // ignore plazagroup
                    cmd += "   AND UserId = ? ";

                    var rets = NQuery.Query<FKs>(cmd, tsb.TSBId,
                        //plazaGroup.PlazaGroupId, // ignore plazagroup
                        user.UserId).ToList();
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
        public static NDbResult<List<TSBCouponTransaction>> GetTSBCoupon35sSoldByDate(TSB tsb, DateTime? soldDate)
        {
            var result = new NDbResult<List<TSBCouponTransaction>>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }

            if (!soldDate.HasValue || soldDate.Value == DateTime.MinValue)
            {
                // Check Sold Date.
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
                    cmd += "   AND CouponType = ? ";
                    cmd += "   AND (TransactionType = ? OR TransactionType = ?) ";
                    //cmd += "   AND SapChooseFlag = 1 ";
                    cmd += "   AND SoldDate >= ? ";
                    cmd += "   AND SoldDate < ? ";

                    var dt1 = soldDate.Value.Date;
                    var dt2 = dt1.AddDays(1);
                    var rets = NQuery.Query<FKs>(cmd, tsb.TSBId, 
                        CouponType.BHT35,
                        TSBCouponTransactionTypes.SoldByLane, TSBCouponTransactionTypes.SoldByTSB,
                        dt1, dt2).ToList();
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
        public static NDbResult<List<TSBCouponTransaction>> GetTSBCoupon80sSoldByDate(TSB tsb, DateTime? soldDate)
        {
            var result = new NDbResult<List<TSBCouponTransaction>>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }

            if (!soldDate.HasValue || soldDate.Value == DateTime.MinValue)
            {
                // Check Sold Date.
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
                    cmd += "   AND CouponType = ? ";
                    cmd += "   AND (TransactionType = ? OR TransactionType = ?) ";
                    //cmd += "   AND SapChooseFlag = 1 ";
                    cmd += "   AND SoldDate >= ? ";
                    cmd += "   AND SoldDate < ? ";

                    var dt1 = soldDate.Value.Date;
                    var dt2 = dt1.AddDays(1);
                    var rets = NQuery.Query<FKs>(cmd, tsb.TSBId, 
                        CouponType.BHT80,
                        TSBCouponTransactionTypes.SoldByLane, TSBCouponTransactionTypes.SoldByTSB,
                        dt1, dt2).ToList();
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

        public static NDbResult<List<TSBCouponTransaction>> GetTSBCoupon35sStock(TSB tsb)
        {
            var result = new NDbResult<List<TSBCouponTransaction>>();
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
                    cmd += "  FROM TSBCouponTransactionView ";
                    cmd += " WHERE TSBId = ? ";
                    cmd += "   AND CouponType = ? ";
                    cmd += "   AND TransactionType = ? ";
                    //cmd += "   AND SapChooseFlag = 1 ";

                    var rets = NQuery.Query<FKs>(cmd, tsb.TSBId,
                        CouponType.BHT35,
                        TSBCouponTransactionTypes.Stock).ToList();
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
        public static NDbResult<List<TSBCouponTransaction>> GetTSBCoupon80sStock(TSB tsb)
        {
            var result = new NDbResult<List<TSBCouponTransaction>>();
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
                    cmd += "  FROM TSBCouponTransactionView ";
                    cmd += " WHERE TSBId = ? ";
                    cmd += "   AND CouponType = ? ";
                    cmd += "   AND TransactionType = ? ";
                    //cmd += "   AND SapChooseFlag = 1 ";

                    var rets = NQuery.Query<FKs>(cmd, tsb.TSBId,
                        CouponType.BHT80,
                        TSBCouponTransactionTypes.Stock).ToList();
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
