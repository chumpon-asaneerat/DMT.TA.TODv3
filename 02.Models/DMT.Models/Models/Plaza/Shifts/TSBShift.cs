﻿#region Using

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
    #region TSBShift

    /// <summary>
    /// The TSBShift Data Model class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("TSBShift")]
    public class TSBShift : NTable<TSBShift>
    {
        #region Intenral Variables

        private Guid _TSBShiftId = Guid.NewGuid();

        private string _TSBId = string.Empty;
        private string _TSBNameEN = string.Empty;
        private string _TSBNameTH = string.Empty;

        private int _ShiftId = 0;
        private string _ShiftNameEN = string.Empty;
        private string _ShiftNameTH = string.Empty;

        private string _UserId = string.Empty;
        private string _FullNameEN = string.Empty;
        private string _FullNameTH = string.Empty;

        private DateTime? _Begin = new DateTime?();
        private DateTime? _End = new DateTime?();

        private int? _ToTAServer = new int?();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSBShift() : base() { }

        #endregion

        #region Public Properties

        #region Common

        /// <summary>
        /// Gets or sets PK Id.
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets PK Id.")]
        [ReadOnly(true)]
        [PrimaryKey]
        [PropertyMapName("TSBShiftId")]
        public Guid TSBShiftId
        {
            get
            {
                return _TSBShiftId;
            }
            set
            {
                if (_TSBShiftId != value)
                {
                    _TSBShiftId = value;
                    this.RaiseChanged("TSBShiftId");
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
        /// Gets or sets Full Name EN.
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
        /// Gets or sets Full Name TH.
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
        public class FKs : TSBShift, IFKs<TSBShift>
        {
            #region TSB

            /// <summary>
            /// Gets or sets TSBNameEN.
            /// </summary>
            [MaxLength(100)]
            [PropertyMapName("TSBNameEN")]
            public override string TSBNameEN
            {
                get { return base.TSBNameEN; }
                set { base.TSBNameEN = value; }
            }
            /// <summary>
            /// Gets or sets TSBNameTH.
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
        /// Create new TSB Shift (not save to database).
        /// </summary>
        /// <param name="shift">The Shift instance.</param>
        /// <param name="user">The User instance.</param>
        /// <returns>Returns instance of TSBShift.</returns>
        public static NDbResult<TSBShift> Create(Shift shift, User user)
        {
            var result = new NDbResult<TSBShift>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            var tsb = TSB.GetCurrent().Value();
            TSBShift inst = Create();
            if (null != tsb) tsb.AssignTo(inst);
            if (null != shift) shift.AssignTo(inst);
            if (null != user) user.AssignTo(inst);
            result.Success(inst);

            return result;
        }
        /// <summary>
        /// Gets Current TSB Shift.
        /// </summary>
        /// <returns>Returns TSBShift instance</returns>
        public static NDbResult<TSBShift> GetTSBShift()
        {
            var result = new NDbResult<TSBShift>();
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
            result = GetTSBShift(tsb.TSBId);
            return result;
        }
        /// <summary>
        /// Gets TSBShift by TSBId.
        /// </summary>
        /// <param name="tsbid">The TSB Id.</param>
        /// <returns>Returns TSBShift instance</returns>
        public static NDbResult<TSBShift> GetTSBShift(string tsbid)
        {
            var result = new NDbResult<TSBShift>();
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
                    cmd += "  FROM TSBShiftView ";
                    cmd += " WHERE TSBId = ? ";
                    cmd += "   AND (End IS NULL OR End = ?) ";
                    cmd += " ORDER BY Begin ";

                    var ret = NQuery.Query<FKs>(cmd,
                        tsbid, DateTime.MinValue).FirstOrDefault();
                    var data = (null != ret) ? ret.ToModel() : null;
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
        /// Gets Unclose Shifts.
        /// </summary>
        /// <param name="tsbid">The TSB Id.</param>
        /// <returns>Returns List of TSBShift instance</returns>
        public static NDbResult<List<TSBShift>> GetUncloseShifts(string tsbid)
        {
            var result = new NDbResult<List<TSBShift>>();
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
                    cmd += "  FROM TSBShiftView ";
                    cmd += " WHERE TSBId = ? ";
                    cmd += "   AND (End IS NULL OR End = ?) ";
                    cmd += " ORDER BY Begin ";

                    var ret = NQuery.Query<FKs>(cmd,
                        tsbid, DateTime.MinValue).ToList();
                    var data = (null != ret) ? ret.ToModels() : null;
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
        /// Gets TSB Shifts by TSBId and Date.
        /// </summary>
        /// <param name="date">The date of shift.</param>
        /// <returns>Returns TSBShift instance</returns>
        public static NDbResult<List<TSBShift>> GetTSBShifts(DateTime? date)
        {
            var result = new NDbResult<List<TSBShift>>();
            SQLiteConnection db = Default;
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            var tsb = TSB.GetCurrent().Value();
            if (null == tsb || !date.HasValue || date.Value == DateTime.MinValue)
            {
                result.ParameterIsNull();
                return result;
            }
            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                DateTime dt = date.Value.Date;
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * ";
                    cmd += "  FROM TSBShiftView ";
                    cmd += " WHERE TSBId = ? ";
                    cmd += "   AND Begin <= ? ";
                    cmd += "   AND (End IS NULL OR END > ? ) ";
                    cmd += " ORDER BY Begin ";

                    var ret = NQuery.Query<FKs>(cmd,
                        tsb.TSBId, dt).ToList();
                    var data = (null != ret) ? ret.ToModels() : null;
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
        /// Change Shift.
        /// </summary>
        /// <param name="value">The TSBShift instance.</param>
        /// <returns>Returns Change Shift result.</returns>
        public static NDbResult ChangeShift(TSBShift value)
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
                    NDbResult<TSBShift> saveRet = null;

                    // Check Begin new shift.
                    if (!value.Begin.HasValue || value.Begin.Value == DateTime.MinValue)
                    {
                        value.Begin = DateTime.Now;
                    }

                    // End exists shift.
                    var uncloseShifts = GetUncloseShifts(value.TSBId).Value();
                    if (null != uncloseShifts && uncloseShifts.Count > 0)
                    {
                        uncloseShifts.ForEach(uncloseShift => 
                        {
                            if (value.TSBShiftId == uncloseShift.TSBShiftId) return; // Same Id ignore it.

                            if (uncloseShift.Begin.HasValue && value.Begin.HasValue)
                            {
                                if (uncloseShift.Begin.Value < value.Begin.Value)
                                {
                                    // Exists shift Begin DateTime is older than new shift.
                                    if (!uncloseShift.End.HasValue || uncloseShift.End.Value == DateTime.MinValue)
                                    {
                                        // End shift.
                                        uncloseShift.End = value.Begin;
                                        Save(uncloseShift);
                                    }
                                }
                                else
                                {
                                    // Exists shift Begin DateTime is newer than new shift.
                                    if (!value.End.HasValue || value.End.Value == DateTime.MinValue)
                                    {
                                        // End shift.
                                        value.End = uncloseShift.Begin;
                                        saveRet = Save(value);
                                        result.errors = saveRet.errors;
                                        if (!result.errors.hasError)
                                        {
                                            result.Success();
                                        }
                                    }
                                }
                            }
                        });
                    }

                    if (null == saveRet)
                    {
                        // Save current value.
                        saveRet = Save(value);
                        result.errors = saveRet.errors;
                        if (!result.errors.hasError)
                        {
                            result.Success();
                        }
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
        /// Gets UnSync TSB Shifts.
        /// </summary>
        /// <returns>Returns TSBShift instance</returns>
        public static NDbResult<List<TSBShift>> GetUnSyncTSBShifts()
        {
            var result = new NDbResult<List<TSBShift>>();
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
                    cmd += "  FROM TSBShiftView ";
                    cmd += " WHERE ToTAServer IS NULL ";

                    var ret = NQuery.Query<FKs>(cmd).ToList();
                    var data = (null != ret) ? ret.ToModels() : null;
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

        #endregion
    }

    #endregion
}
