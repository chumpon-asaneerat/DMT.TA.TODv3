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
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("UserShiftRevenue")]
    public class UserShiftRevenue : NTable<UserShiftRevenue>
    {
        #region Internal Variables

        private Guid _PKId = Guid.NewGuid();

        private int _UserShiftId = 0;

        private string _TSBId = string.Empty;
        private string _TSBNameEN = string.Empty;
        private string _TSBNameTH = string.Empty;

        private string _PlazaGroupId = string.Empty;
        private string _PlazaGroupNameEN = string.Empty;
        private string _PlazaGroupNameTH = string.Empty;
        private string _Direction = string.Empty;

        private int _ShiftId = 0;
        private string _ShiftNameEN = string.Empty;
        private string _ShiftNameTH = string.Empty;

        private string _UserId = string.Empty;
        private string _FullNameEN = string.Empty;
        private string _FullNameTH = string.Empty;

        private string _RevenueId = string.Empty;
        private DateTime? _RevenueDate = new DateTime?();

        #endregion

        #region Constructor

        public UserShiftRevenue() : base() { }

        #endregion

        #region Public Properties

        #region Common

        /// <summary>
        /// Gets or sets PKId
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets PK Id.")]
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

        #endregion

        #region UserShift

        /// <summary>
        /// Gets or sets User Shift Id.
        /// </summary>
        [Category("Shift")]
        [Description("Gets or sets User Shift Id.")]
        [ReadOnly(true)]
        [NotNull]
        [Indexed]
        [PropertyMapName("UserShiftId")]
        public int UserShiftId
        {
            get
            {
                return _UserShiftId;
            }
            set
            {
                if (_UserShiftId != value)
                {
                    _UserShiftId = value;
                    this.RaiseChanged("UserShiftId");
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

        #region Revenue

        /// <summary>
        /// Gets or sets RevenueId.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets or sets RevenueId.")]
        //[ReadOnly(true)]
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
        /// Gets or sets Revenue Date.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets or sets Revenue Date.")]
        //[ReadOnly(true)]
        [Indexed]
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
                    RaiseChanged("RevenueDate");
                    RaiseChanged("RevenueDateString");
                    RaiseChanged("RevenueTimeString");
                    RaiseChanged("RevenueDateTimeString");
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
                var ret = (!this.RevenueDate.HasValue || this.RevenueDate.Value == DateTime.MinValue) ?
                    "" : this.RevenueDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets Revenue Time String.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets Revenue Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string RevenueTimeString
        {
            get
            {
                var ret = (!this.RevenueDate.HasValue || this.RevenueDate.Value == DateTime.MinValue) ?
                    "" : this.RevenueDate.Value.ToThaiTimeString();
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets Revenue Date Time String.
        /// </summary>
        [Category("Revenue")]
        [Description("Gets Revenue Date Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string RevenueDateTimeString
        {
            get
            {
                var ret = (!this.RevenueDate.HasValue || this.RevenueDate.Value == DateTime.MinValue) ?
                    "" : this.RevenueDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
                return ret;
            }
            set { }
        }

        #endregion

        #endregion

        #region Internal Class

        /// <summary>
        /// The internal FKs class for query data.
        /// </summary>
        public class FKs : UserShiftRevenue, IFKs<UserShiftRevenue>
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
            /// Gets or sets Shift Name EN.
            /// </summary>
            [MaxLength(50)]
            [PropertyMapName("ShiftNameEN")]
            public override string ShiftNameEN
            {
                get { return base.ShiftNameEN; }
                set { base.ShiftNameEN = value; }
            }
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

            #endregion
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Create Plaza Revenue.
        /// </summary>
        /// <param name="shift">The UserShift instance.</param>
        /// <param name="plazaGroup">The PlazaGroup instance.</param>
        /// <returns>Returns UserShiftRevenue instance.</returns>
        public static NDbResult<UserShiftRevenue> CreatePlazaRevenue(
            UserShift shift, PlazaGroup plazaGroup)
        {
            var result = new NDbResult<UserShiftRevenue>();

            if (null == shift || null == plazaGroup)
            {
                result.ParameterIsNull();
                result.data = null;
            }
            else
            {
                UserShiftRevenue inst = Create();
                plazaGroup.AssignTo(inst);
                shift.AssignTo(inst);
                result.data = inst;
                result.Success();
            }
            return result;
        }
        /// <summary>
        /// Save Plaza Revenue.
        /// </summary>
        /// <param name="value">The UserShiftRevenue instance.</param>
        /// <param name="revenueDate">The Revenue Date.</param>
        /// <param name="revenueId">The Revenue Id.</param>
        /// <returns>Returns save UserShiftRevenue instance.</returns>
        public static NDbResult<UserShiftRevenue> SavePlazaRevenue(UserShiftRevenue value,
            DateTime revenueDate, string revenueId)
        {
            var result = new NDbResult<UserShiftRevenue>();
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
                    value.RevenueDate = revenueDate;
                    value.RevenueId = revenueId;
                    // save.
                    result = Save(value);
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
        /// Gets Plaza Revenue.
        /// </summary>
        /// <param name="shift">The Shift instance.</param>
        /// <param name="plazaGroup">The PlazaGroup instance.</param>
        /// <returns>Returns UserShiftRevenue instance.</returns>
        public static NDbResult<UserShiftRevenue> GetPlazaRevenue(
            UserShift shift, PlazaGroup plazaGroup)
        {
            var result = new NDbResult<UserShiftRevenue>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (null == shift || null == plazaGroup)
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
                    cmd += "  FROM UserShiftRevenueView ";
                    cmd += " WHERE UserShiftId = ? ";
                    cmd += "   AND PlazaGroupId = ? ";
                    var ret = NQuery.Query<FKs>(cmd,
                        shift.UserShiftId, plazaGroup.PlazaGroupId).FirstOrDefault();
                    result.data = (null != ret) ? ret.ToModel() : null;
                    result.Success();
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
}
