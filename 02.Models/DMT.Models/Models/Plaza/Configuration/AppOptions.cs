#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using NLib;
using NLib.Design;
using NLib.Reflection;

using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
// required for JsonIgnore attribute.
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

#endregion

namespace DMT.Models
{
    #region AppOption

    /// <summary>
    /// The AppOption Data Model Class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("AppOption")]
    public class AppOption : NTable<AppOption>
    {
        #region Intenral Variables

        private string _Key = string.Empty;
        private string _Value = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public AppOption() : base() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Key
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets Key")]
        [PrimaryKey, MaxLength(80)]
        [PropertyMapName("Key")]
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                if (_Key != value)
                {
                    _Key = value;
                    this.RaiseChanged("Key");
                }
            }
        }
        /// <summary>
        /// Gets or sets Value
        /// </summary>
        [Category("Common")]
        [Description("Gets or sets Value")]
        [MaxLength(200)]
        [PropertyMapName("Value")]
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    this.RaiseChanged("Key");
                }
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets App Option By Key.
        /// </summary>
        /// <param name="key">The Key.</param>
        /// <returns>Returns App Option instance.</returns>
        public static NDbResult<AppOption> GetOption(string key)
        {
            var result = new NDbResult<AppOption>();
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
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        result.ParameterIsNull();
                        return result;
                    }

                    string cmd = @"
                    SELECT *
                      FROM AppOption
                     WHERE Key = ? ";

                    var ret = NQuery.Query<AppOption>(cmd,
                        key).FirstOrDefault();
                    var inst = ret;
                    if (null == ret)
                    {
                        // not found key
                        inst = new AppOption();
                        inst.Key = key;
                        inst.Value = null;
                        Save(inst);
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
        /// Set App Option By Key.
        /// </summary>
        /// <param name="key">The Key.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>Returns Changed App Option instance.</returns>
        public static NDbResult<AppOption> SetOption(string key, string value)
        {
            var result = new NDbResult<AppOption>();
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
                    var inst = GetOption(key).Value();
                    if (null != inst)
                    {
                        inst.Value = value;
                        Save(inst);

                        result.Success(inst);
                    }
                    else
                    {
                        result.Error(new Exception("No instance found."));
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

        #endregion
    }

    #endregion

    #region Extension Method(s)

    /// <summary>
    /// The AppOption Extension Methods
    /// </summary>
    public static class AppOptionExtensionMethods
    {
        /// <summary>
        /// Convert AppOption's value to datetime.
        /// </summary>
        /// <param name="inst">The AppOption instance.</param>
        /// <returns>Returns AppOption's value in DateTime instance.</returns>
        public static DateTime? ToDateTime(this AppOption inst)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            DateTime? ret = new DateTime?();
            if (null != inst && !string.IsNullOrWhiteSpace(inst.Value))
            {
                try
                {
                    string[] parts = inst.Value.Split(new char[] { ':', '-', '.', 'T', '/', '\\', ',', ' ', ';' },
                        StringSplitOptions.RemoveEmptyEntries);

                    int yr = DateTime.MinValue.Year;
                    int mn = DateTime.MinValue.Month;
                    int dy = DateTime.MinValue.Day;
                    int val;
                    int hr = 12;
                    if (parts.Length > 0)
                    {
                        if (int.TryParse(parts[0], out val) && (val >= 0 && val <= 23))
                        {
                            hr = val;
                        }
                        else
                        {
                            // Parse Error, out of range.
                            val = 12;
                            med.Err("Hour: Parse error or out of range. Used default Hour = 12.");
                        }
                    }

                    int min = 0;
                    if (parts.Length > 1)
                    {
                        if (int.TryParse(parts[1], out val) && (val >= 0 && val <= 59))
                        {
                            min = val;
                        }
                        else
                        {
                            // Parse Error, out of range.
                            val = 0;
                            med.Err("Minute: Parse error or out of range. Used default Minute = 0.");
                        }
                    }

                    int sec = 0;
                    if (parts.Length > 2)
                    {
                        if (int.TryParse(parts[2], out val) && (val >= 0 && val <= 59))
                        {
                            sec = val;
                        }
                        else
                        {
                            // Parse Error, out of range.
                            val = 0;
                            med.Err("Second: Parse error or out of range. Used default Second = 0.");
                        }
                    }

                    int msec = 0;
                    if (parts.Length > 3)
                    {
                        if (int.TryParse(parts[3], out val) && (val >= 0 && val <= 999))
                        {
                            msec = val;
                        }
                        else
                        {
                            // Parse Error, out of range.
                            val = 0;
                            med.Err("Millisecond: Parse error or out of range. Used default Millisecond = 0.");
                        }
                    }

                    DateTime dt = new DateTime(yr, mn, dy, hr, min, sec, msec);
                    ret = new DateTime?(dt);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    ret = new DateTime?();
                }
            }

            return ret;
        }
    }

    #endregion
}
