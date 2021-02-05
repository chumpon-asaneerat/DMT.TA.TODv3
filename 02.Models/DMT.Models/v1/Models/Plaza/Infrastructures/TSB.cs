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
    #region TSB

    /// <summary>
    /// The TSB Data Model class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("TSB")]
    public class TSB : NTable<TSB>
    {
        #region Intenral Variables

        private string _TSBId = string.Empty;
        private string _TSBNameEN = string.Empty;
        private string _TSBNameTH = string.Empty;
        private string _NetworkId = string.Empty;

        private bool _Active = false;

        // Update User
        private string _UserId = string.Empty;
        private string _FullNameEN = string.Empty;
        private string _FullNameTH = string.Empty;

        private DateTime? _UpdateDate = new DateTime?();

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

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TSB() : base() { }

        #endregion

        #region Public Proprties

        #region Common

        /// <summary>
        /// Gets or sets TSBId.
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets TSBId.")]
        [PrimaryKey, MaxLength(10)]
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
        /// Gets or sets NetworkId.
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets NetworkId.")]
        [MaxLength(10)]
        [PropertyMapName("NetworkId")]
        public string NetworkId
        {
            get
            {
                return _NetworkId;
            }
            set
            {
                if (_NetworkId != value)
                {
                    _NetworkId = value;
                    this.RaiseChanged("NetworkId");
                }
            }
        }
        /// <summary>
        /// Gets or sets TSBNameEN.
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets TSBNameEN.")]
        [MaxLength(100)]
        [PropertyMapName("TSBNameEN")]
        public string TSBNameEN
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
        /// Gets or sets TSBNameTH.
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets TSBNameTH.")]
        [MaxLength(100)]
        [PropertyMapName("TSBNameTH")]
        public string TSBNameTH
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
        /// Gets or sets is active TSB.
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets is active TSB.")]
        [ReadOnly(true)]
        [PropertyMapName("Active")]
        public bool Active
        {
            get
            {
                return _Active;
            }
            set
            {
                if (_Active != value)
                {
                    _Active = value;
                    this.RaiseChanged("Active");
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
        [Indexed]
        [MaxLength(10)]
        [PropertyMapName("UserId")]
        public virtual string UserId
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
        /// <summary>
        /// Gets or sets Update Date.
        /// </summary>
        [Category("User")]
        [Description("Gets or sets Update Date.")]
        [ReadOnly(true)]
        [PropertyMapName("UpdateDate")]
        public virtual DateTime? UpdateDate
        {
            get
            {
                return _UpdateDate;
            }
            set
            {
                if (_UpdateDate != value)
                {
                    _UpdateDate = value;
                    this.RaiseChanged("UpdateDate");
                }
            }
        }

        #endregion

        #region Credit

        /// <summary>
        /// Gets or sets Max TSB Credit.
        /// </summary>
        [Category("Credits")]
        [Description("Gets or sets Max TSB Credit.")]
        [PropertyMapName("MaxCredit")]
        public decimal MaxCredit
        {
            get
            {
                return _MaxCredit;
            }
            set
            {
                if (_MaxCredit != value)
                {
                    _MaxCredit = value;
                    this.RaiseChanged("MaxCredit");
                }
            }
        }

        #region Credit

        /// <summary>
        /// Gets or sets Low Limit for ST25.
        /// </summary>
        [Category("Credits")]
        [Description("Gets or sets Low Limit for ST25.")]
        [PropertyMapName("LowLimitST25")]
        public decimal LowLimitST25
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
        public decimal LowLimitST50
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
        public decimal LowLimitBHT1
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
        public decimal LowLimitBHT2
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
        public decimal LowLimitBHT5
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
        public decimal LowLimitBHT10
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
        public decimal LowLimitBHT20
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
        public decimal LowLimitBHT50
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
        public decimal LowLimitBHT100
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
        public decimal LowLimitBHT500
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
        public decimal LowLimitBHT1000
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

        #endregion

        #region Static Methods

        #region Get All TSBs

        /// <summary>
        /// Gets TSBs.
        /// </summary>
        /// <param name="db">The database connection.</param>
        /// <returns>Returns List of TSB.</returns>
        public static NDbResult<List<TSB>> GetTSBs(SQLiteConnection db)
        {
            var result = new NDbResult<List<TSB>>();
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
                    cmd += "SELECT * FROM TSB ";
                    result.Success();
                    var data = NQuery.Query<TSB>(cmd);
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
        /// Gets TSBs.
        /// </summary>
        /// <returns>Returns List of TSB.</returns>
        public static NDbResult<List<TSB>> GetTSBs()
        {
            lock (sync)
            {
                SQLiteConnection db = Default;
                return GetTSBs(db);
            }
        }

        #endregion

        #region Get TSB By TSBId

        /// <summary>
        /// Gets TSB By TSB Id.
        /// </summary>
        /// <param name="db">The database connection.</param>
        /// <param name="tsbId">The TSB Id.</param>
        /// <returns>Returns TSB instance.</returns>
        public static NDbResult<TSB> GetTSB(SQLiteConnection db, string tsbId)
        {
            var result = new NDbResult<TSB>();
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
                    cmd += "SELECT * FROM TSB ";
                    cmd += " WHERE TSBId = ? ";
                    var data = NQuery.Query<TSB>(cmd, tsbId).FirstOrDefault();
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
        /// Gets TSB By TSB Id.
        /// </summary>
        /// <param name="tsbId">The TSB Id.</param>
        /// <returns>Returns TSB instance.</returns>
        public static NDbResult<TSB> GetTSB(string tsbId)
        {
            lock (sync)
            {
                SQLiteConnection db = Default;
                return GetTSB(db, tsbId);
            }
        }

        #endregion

        #region Get Current (Active) TSB

        /// <summary>
        /// Gets Active TSB.
        /// </summary>
        /// <returns>Returns Active TSB instance.</returns>
        public static NDbResult<TSB> GetCurrent()
        {
            var result = new NDbResult<TSB>();
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
                    // inactive all TSBs
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM TSB ";
                    cmd += " WHERE Active = 1 ";
                    var results = NQuery.Query<TSB>(cmd);
                    var data = (null != results) ? results.FirstOrDefault() : null;
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

        #region Set Active TSB

        /// <summary>
        /// Set Active by TSB Id.
        /// </summary>
        /// <param name="tsbId">The TSB Id.</param>
        /// <returns>Returns Set Active status.</returns>
        public static NDbResult SetActive(string tsbId)
        {
            var result = new NDbResult();
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
                    // inactive all TSBs
                    string cmd = string.Empty;
                    cmd += "UPDATE TSB ";
                    cmd += "   SET Active = 0";
                    NQuery.Execute(cmd);
                    // Set active TSB
                    cmd = string.Empty;
                    cmd += "UPDATE TSB ";
                    cmd += "   SET Active = 1 ";
                    cmd += " WHERE TSBId = ? ";
                    NQuery.Execute(cmd, tsbId);
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

        #region Save TSB

        /// <summary>
        /// Save TSB.
        /// </summary>
        /// <param name="value">The TSB instance.</param>
        /// <returns>Returns TSB instance.</returns>
        public static NDbResult<TSB> SaveTSB(TSB value)
        {
            var result = new NDbResult<TSB>();
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
                    if (null != value && !value.UpdateDate.HasValue)
                    {
                        value.UpdateDate = DateTime.Now;

                    }
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

        #endregion

        #endregion
    }

    #endregion
}
