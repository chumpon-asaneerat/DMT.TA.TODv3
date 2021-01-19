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
    #region UserAccess

    /// <summary>
    /// The UserAccess Data Model Class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("UserAccess")]
    public class UserAccess : NTable<UserAccess>
    {
        #region Public Static Propeties (required)

        private static int _DefaultFailAllow = 3;
        /// <summary>Gets or sets Default Failed Allow count.</summary>
        public static int DefaultFailAllow
        {
            get { return _DefaultFailAllow; }
            set
            {
                if (value <= 0) return; // ignore less than zero.
                if (_DefaultFailAllow != value)
                {
                    _DefaultFailAllow = value;
                }
            }
        }

        private static int _DefaultLockHours = 90;
        /// <summary>Gets or sets Default Lock Hours.</summary>
        public static int DefaultLockHours
        {
            get { return _DefaultLockHours; }
            set
            {
                if (value <= 0) return; // ignore less than zero.
                if (_DefaultLockHours != value)
                {
                    _DefaultLockHours = value;
                }
            }
        }

        #endregion

        #region Intenral Variables

        private string _UserId = string.Empty;
        private DateTime? _LastAccessDate = new DateTime?();
        private DateTime? _LastNotifyDate = new DateTime?();
        private DateTime? _LastLockDate = new DateTime?();
        private int _FailCount = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserAccess() : base() { }

        #endregion

        #region Public Properties

        #region Common

        /// <summary>
        /// Gets or sets User Id.
        /// </summary>
        [Category("Access")]
        [Description("Gets or sets User Id.")]
        [PrimaryKey, MaxLength(10)]
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
        /// Gets or sets LastAccess Date.
        /// </summary>
        [Category("Access")]
        [Description("Gets or sets LastAccess Date.")]
        [ReadOnly(true)]
        [PropertyMapName("LastAccessDate")]
        public DateTime? LastAccessDate
        {
            get { return _LastAccessDate; }
            set
            {
                if (_LastAccessDate != value)
                {
                    _LastAccessDate = value;
                    this.RaiseChanged("LastAccessDate");
                    this.RaiseChanged("LastAccessDateString");
                    this.RaiseChanged("LastAccessTimeString");
                    this.RaiseChanged("LastAccessDateTimeString");
                }
            }
        }
        /// <summary>
        /// Gets LastAccess Date String.
        /// </summary>
        [Category("Access")]
        [Description("Gets LastAccess Date String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string LastAccessDateString
        {
            get
            {
                var ret = (!this._LastAccessDate.HasValue || this._LastAccessDate.Value == DateTime.MinValue) ?
                    "" : this._LastAccessDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets LastAccess Time String.
        /// </summary>
        [Category("Access")]
        [Description("Gets LastAccess Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string LastAccessTimeString
        {
            get
            {
                var ret = (!this._LastAccessDate.HasValue || this._LastAccessDate.Value == DateTime.MinValue) ?
                    "" : this._LastAccessDate.Value.ToThaiTimeString();
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets or sets LastAccess Date Time String..
        /// </summary>
        [Category("Access")]
        [Description("Gets or sets LastAccess Date Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string LastAccessDateTimeString
        {
            get
            {
                var ret = (!this._LastAccessDate.HasValue || this._LastAccessDate.Value == DateTime.MinValue) ?
                    "" : this._LastAccessDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
                return ret;
            }
            set { }
        }

        /// <summary>
        /// Gets or sets LastNotify Date.
        /// </summary>
        [Category("Access")]
        [Description("Gets or sets LastNotify Date.")]
        [ReadOnly(true)]
        [PropertyMapName("LastNotifyDate")]
        public DateTime? LastNotifyDate
        {
            get { return _LastNotifyDate; }
            set
            {
                if (_LastNotifyDate != value)
                {
                    _LastNotifyDate = value;
                    this.RaiseChanged("LastNotify");
                    this.RaiseChanged("LastNotifyDateString");
                    this.RaiseChanged("LastNotifyTimeString");
                    this.RaiseChanged("LastNotifyDateTimeString");
                }
            }
        }
        /// <summary>
        /// Gets LastNotify Date String.
        /// </summary>
        [Category("Access")]
        [Description("Gets LastNotify Date String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string LastNotifyDateString
        {
            get
            {
                var ret = (!this._LastNotifyDate.HasValue || this._LastNotifyDate.Value == DateTime.MinValue) ?
                    "" : this._LastNotifyDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets LastNotify Time String.
        /// </summary>
        [Category("Access")]
        [Description("Gets LastNotify Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string LastNotifyTimeString
        {
            get
            {
                var ret = (!this._LastNotifyDate.HasValue || this._LastNotifyDate.Value == DateTime.MinValue) ?
                    "" : this._LastNotifyDate.Value.ToThaiTimeString();
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets or sets LastNotify Date Time String..
        /// </summary>
        [Category("Access")]
        [Description("Gets or sets LastNotify Date Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string LastNotifyDateTimeString
        {
            get
            {
                var ret = (!this._LastNotifyDate.HasValue || this._LastNotifyDate.Value == DateTime.MinValue) ?
                    "" : this._LastNotifyDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
                return ret;
            }
            set { }
        }

        /// <summary>
        /// Gets or sets LastLock Date.
        /// </summary>
        [Category("Access")]
        [Description("Gets or sets LastLock Date.")]
        [ReadOnly(true)]
        [PropertyMapName("LastLockDate")]
        public DateTime? LastLockDate
        {
            get { return _LastLockDate; }
            set
            {
                if (_LastLockDate != value)
                {
                    _LastLockDate = value;
                    this.RaiseChanged("LastLock");
                    this.RaiseChanged("LastLockDateString");
                    this.RaiseChanged("LastLockTimeString");
                    this.RaiseChanged("LastLockDateTimeString");
                }
            }
        }
        /// <summary>
        /// Gets LastLock Date String.
        /// </summary>
        [Category("Access")]
        [Description("Gets LastLock Date String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string LastLockDateString
        {
            get
            {
                var ret = (!this._LastLockDate.HasValue || this._LastLockDate.Value == DateTime.MinValue) ?
                    "" : this._LastLockDate.Value.ToThaiDateTimeString("dd/MM/yyyy");
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets LastLock Time String.
        /// </summary>
        [Category("Access")]
        [Description("Gets LastLock Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string LastLockTimeString
        {
            get
            {
                var ret = (!this._LastLockDate.HasValue || this._LastLockDate.Value == DateTime.MinValue) ?
                    "" : this._LastLockDate.Value.ToThaiTimeString();
                return ret;
            }
            set { }
        }
        /// <summary>
        /// Gets or sets LastLock Date Time String..
        /// </summary>
        [Category("Access")]
        [Description("Gets or sets LastLock Date Time String.")]
        [ReadOnly(true)]
        [JsonIgnore]
        [Ignore]
        public string LastLockDateTimeString
        {
            get
            {
                var ret = (!this._LastLockDate.HasValue || this._LastLockDate.Value == DateTime.MinValue) ?
                    "" : this._LastLockDate.Value.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
                return ret;
            }
            set { }
        }

        /// <summary>
        /// Gets or sets LogIn Fail Count.
        /// </summary>
        [Category("Access")]
        [Description("Gets or sets LogIn Fail Count.")]
        [ReadOnly(true)]
        [PropertyMapName("FailCount")]
        public int FailCount
        {
            get { return _FailCount; }
            set
            {
                if (_FailCount != value)
                {
                    _FailCount = value;
                    this.RaiseChanged("FailCount");
                }
            }
        }

        #endregion

        #endregion

        #region Internal Class

        /// <summary>
        /// The internal FKs class for query data.
        /// </summary>
        public class FKs : UserAccess, IFKs<UserAccess> { }

        #endregion

        #region Static Methods

        /// <summary>
        /// Save User Access.
        /// </summary>
        /// <param name="value">The UserAccess instance.</param>
        /// <returns>Returns UserAccess instance.</returns>
        public static NDbResult<UserAccess> SaveUserAccess(UserAccess value)
        {
            var result = new NDbResult<UserAccess>();
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
        /// Gets User Access.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult<UserAccess> GetUserAccess(string userId)
        {
            lock (sync)
            {
                SQLiteConnection db = Default;
                return GetUserAccess(db, userId);
            }
        }
        /// <summary>
        /// Gets User Access.
        /// </summary>
        /// <param name="db">The database connection.</param>
        /// <param name="userId">The User Id.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult<UserAccess> GetUserAccess(SQLiteConnection db, string userId)
        {
            var result = new NDbResult<UserAccess>();
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                result.ParameterIsNull();
                return result;
            }
            var user = User.GetByUserId(db, userId).Value();
            if (null == user)
            {
                // User Not found.
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
                    cmd += "  FROM UserAccess ";
                    cmd += " WHERE UserId = ? ";

                    var ret = NQuery.Query<FKs>(cmd, userId).FirstOrDefault();
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
        /// Mark Success Access.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult<UserAccess> Success(string userId)
        {
            lock (sync)
            {
                SQLiteConnection db = Default;
                return Success(db, userId);
            }
        }
        /// <summary>
        /// Mark Success Access.
        /// </summary>
        /// <param name="db">The database connection.</param>
        /// <param name="userId">The User Id.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult<UserAccess> Success(SQLiteConnection db, string userId) 
        {
            var result = new NDbResult<UserAccess>();
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                result.ParameterIsNull();
                return result;
            }
            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    UserAccess access = GetUserAccess(db, userId).Value();
                    if (null == access)
                    {
                        access = new UserAccess();
                        access.UserId = userId;
                    }
                    access.FailCount = 0; // Reset failed counter.
                    access.LastLockDate = new DateTime?(); // Reset lock date.

                    access.LastAccessDate = DateTime.Now;

                    result = Save(access);
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
        /// Mark Filed Access.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult<UserAccess> Failed(string userId)
        {
            lock (sync)
            {
                SQLiteConnection db = Default;
                return Failed(db, userId);
            }
        }
        /// <summary>
        /// Mark Filed Access.
        /// </summary>
        /// <param name="db">The database connection.</param>
        /// <param name="userId">The User Id.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult<UserAccess> Failed(SQLiteConnection db, string userId) 
        {
            var result = new NDbResult<UserAccess>();
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                result.ParameterIsNull();
                return result;
            }
            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    UserAccess access = GetUserAccess(db, userId).Value();
                    if (null == access)
                    {
                        access = new UserAccess();
                        access.UserId = userId;
                        access.FailCount = 0; // init default before increase counter.
                    }
                    access.FailCount++; // increase failed count.
                    if (access.FailCount >= DefaultFailAllow)
                    {
                        // Set lock date.
                        access.LastLockDate = DateTime.Now;
                    }

                    access.LastAccessDate = DateTime.Now;

                    result = Save(access);
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
