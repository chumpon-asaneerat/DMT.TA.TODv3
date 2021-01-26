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
    #region UserShift

    /// <summary>
    /// The UserShift Data Model class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("UserShift")]
    public class UserShift : NTable<UserShift>
    {
        #region Intenral Variables

        private int _UserShiftId = 0;

        private string _TSBId = string.Empty;
        private string _TSBNameEN = string.Empty;
        private string _TSBNameTH = string.Empty;

        private int _ShiftId = 0;
        private string _ShiftNameTH = string.Empty;
        private string _ShiftNameEN = string.Empty;

        private string _UserId = string.Empty;
        private string _FullNameEN = string.Empty;
        private string _FullNameTH = string.Empty;

        private DateTime? _Begin = new DateTime?();
        private DateTime? _End = new DateTime?();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserShift() : base() { }

        #endregion

        #region Public Properties

        #region Common

        /// <summary>
        /// Gets or sets PK Id.
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets PK Id.")]
        [ReadOnly(true)]
        [PrimaryKey, AutoIncrement]
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

        #region Begin/End

        /// <summary>
        /// Gets or sets Begin Date.
        /// </summary>
        [Category("Shift")]
        [Description("Gets or sets Begin Date.")]
        //[ReadOnly(true)]
        [Indexed]
        [PropertyMapName("Begin")]
        public DateTime? Begin
        {
            get { return _Begin; }
            set
            {
                if (_Begin != value)
                {
                    _Begin = value;
                    // Raise event.
                    RaiseChanged("Begin");
                    RaiseChanged("BeginDateString");
                    RaiseChanged("BeginTimeString");
                    RaiseChanged("BeginDateTimeString");
                }
            }
        }
        /// <summary>
        /// Gets or sets End Date.
        /// </summary>
        [Category("Shift")]
        [Description("Gets or sets End Date.")]
        //[ReadOnly(true)]
        [Indexed]
        [PropertyMapName("End")]
        public DateTime? End
        {
            get { return _End; }
            set
            {
                if (_End != value)
                {
                    _End = value;
                    // Raise event.
                    RaiseChanged("End");
                    RaiseChanged("EndDateString");
                    RaiseChanged("EndTimeString");
                    RaiseChanged("EndDateTimeString");
                }
            }
        }
        /// <summary>
        /// Gets Begin Date String.
        /// </summary>
        [Category("Shift")]
        [Description("Gets Begin Date String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string BeginDateString
        {
            get
            {
                var ret = (!this.Begin.HasValue || this.Begin.Value == DateTime.MinValue) ?
                    "" : this.Begin.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets End Date String.
        /// </summary>
        [Category("Shift")]
        [Description("Gets End Date String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string EndDateString
        {
            get
            {
                var ret = (!this.End.HasValue || this.End.Value == DateTime.MinValue) ?
                    "" : this.End.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets Begin Time String.
        /// </summary>
        [Category("Shift")]
        [Description("Gets Begin Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string BeginTimeString
        {
            get
            {
                var ret = (!this.Begin.HasValue || this.Begin.Value == DateTime.MinValue) ?
                    "" : this.Begin.Value.ToThaiTimeString();
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets End Time String.
        /// </summary>
        [Category("Shift")]
        [Description("Gets End Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string EndTimeString
        {
            get
            {
                var ret = (!this.End.HasValue || this.End.Value == DateTime.MinValue) ?
                    "" : this.End.Value.ToThaiTimeString();
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets or sets Begin Date Time String..
        /// </summary>
        [Category("Shift")]
        [Description("Gets or sets Begin Date Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string BeginDateTimeString
        {
            get
            {
                var ret = (!this.Begin.HasValue || this.Begin.Value == DateTime.MinValue) ?
                    "" : this.Begin.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets End Date Time String.
        /// </summary>
        [Category("Shift")]
        [Description("Gets End Date Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string EndDateTimeString
        {
            get
            {
                var ret = (!this.End.HasValue || this.End.Value == DateTime.MinValue) ?
                    "" : this.End.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
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
        public class FKs : UserShift, IFKs<UserShift>
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
        /// Create UserShift.
        /// </summary>
        /// <param name="shift">The Shift instance.</param>
        /// <param name="supervisor">The User instance.</param>
        /// <returns>Returns UserShift instance.</returns>
        public static NDbResult<UserShift> Create(Shift shift, User supervisor)
        {
            var result = new NDbResult<UserShift>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            UserShift inst = Create();
            var tsb = TSB.GetCurrent().Value();
            if (null != tsb) tsb.AssignTo(inst);
            if (null != shift) shift.AssignTo(inst);
            if (null != supervisor) supervisor.AssignTo(inst);
            result.Success(inst);
            return result;
        }
        /// <summary>
        /// Gets UserShift by User Id.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>Returns UserShift instance.</returns>
        public static NDbResult<UserShift> GetUserShift(string userId)
        {
            var result = new NDbResult<UserShift>();
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
            result = GetUserShift(tsb.TSBId, userId);
            return result;
        }
        /// <summary>
        /// Gets UserShift by TSBId and UserId.
        /// </summary>
        /// <param name="tsbId">Ths TSB id.</param>
        /// <param name="userId">The User Id.</param>
        /// <returns>Returns UserShift instance.</returns>
        public static NDbResult<UserShift> GetUserShift(string tsbId, string userId)
        {
            var result = new NDbResult<UserShift>();
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
                    cmd += "  FROM UserShiftView ";
                    cmd += " WHERE TSBId = ? ";
                    cmd += "   AND UserId = ? ";
                    cmd += "   AND (End IS NULL OR End = ?) ";

                    var ret = NQuery.Query<FKs>(cmd,
                        tsbId, userId, DateTime.MinValue).FirstOrDefault();
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
        /// <summary>
        /// Begin User Shift.
        /// </summary>
        /// <param name="value">The UserShift instance.</param>
        /// <returns>Returns Begin User Shift Result.</returns>
        public static NDbResult BeginUserShift(UserShift value)
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
                    var last = GetUserShift(value.TSBId, value.UserId);
                    if (null != last && null != last.data && !last.errors.hasError)
                    {
                        // not enter revenue entry.
                        result.Error(new Exception("ยังมีกะที่ยังไม่ได้ป้อนรายได้"));
                        result.errors.errNum = -10;
                        return result;
                    }
                    // Begin new shift.
                    if (!value.Begin.HasValue || value.Begin == DateTime.MinValue)
                        value.Begin = DateTime.Now;

                    var saveRet = Save(value);
                    result.errors = saveRet.errors;
                    if (!result.errors.hasError)
                    {
                        result.Success();
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
        /// End User Shift.
        /// </summary>
        /// <param name="value">The UserShift instance.</param>
        /// <returns>Returns End User Shift Result.</returns>
        public static NDbResult EndUserShift(UserShift value)
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
                    // End shift.
                    if (!value.End.HasValue || value.End == DateTime.MinValue)
                        value.End = DateTime.Now;

                    var saveRet = Save(value);
                    result.errors = saveRet.errors;
                    if (!result.errors.hasError)
                    {
                        result.Success();
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
        /// Gets User Shifts (Active TSB).
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>Returns List of User Shifts.</returns>
        public static NDbResult<List<UserShift>> GetUserShifts(string userId)
        {
            var result = new NDbResult<List<UserShift>>();
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
            result = GetUserShifts(tsb.TSBId, userId);
            return result;
        }
        /// <summary>
        /// Gets User Shifts.
        /// </summary>
        /// <param name="tsbId">The TSB Id.</param>
        /// <param name="userId">The User Id.</param>
        /// <returns>Returns List of User Shifts.</returns>
        public static NDbResult<List<UserShift>> GetUserShifts(string tsbId, string userId)
        {
            var result = new NDbResult<List<UserShift>>();
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
                    cmd += "  FROM UserShiftView ";
                    cmd += " WHERE TSBId = ? ";
                    cmd += "   AND UserShift.UserId = ? ";

                    var rets = NQuery.Query<FKs>(cmd, tsbId, userId).ToList();
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
        /// Gets Un Close User Shifts (Active TSB).
        /// </summary>
        /// <returns>Returns List of User Shifts.</returns>
        public static NDbResult<List<UserShift>> GetUnCloseUserShifts()
        {
            var result = new NDbResult<List<UserShift>>();
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
            result = GetUnCloseUserShifts(tsb.TSBId);
            return result;
        }
        /// <summary>
        /// Gets Un Close User Shifts.
        /// </summary>
        /// <param name="tsbid">The TSB Id.</param>
        /// <returns>Returns List of User Shifts.</returns>
        public static NDbResult<List<UserShift>> GetUnCloseUserShifts(string tsbid)
        {
            var result = new NDbResult<List<UserShift>>();
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
                    cmd += "  FROM UserShiftView ";
                    cmd += " WHERE TSBId = ? ";
                    cmd += "   AND (End IS NULL OR End = ?) ";

                    var rets = NQuery.Query<FKs>(cmd, tsbid, DateTime.MinValue).ToList();
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
