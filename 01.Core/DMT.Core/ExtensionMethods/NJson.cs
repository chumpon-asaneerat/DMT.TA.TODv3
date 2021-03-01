//#define USE_PROGRAM_DATA

#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using System.Configuration;
using System.Globalization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using NLib;
using NLib.IO;

#endregion

namespace DMT
{
    #region IsoDateTimeConverter (Original https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Converters/IsoDateTimeConverter.cs)

    /*
    /// <summary>
    /// Converts a <see cref="DateTime"/> to and from the ISO 8601 date format (e.g. <c>"2008-04-12T12:53Z"</c>).
    /// </summary>
    public class IsoDateTimeConverter : DateTimeConverterBase
    {
        private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

        private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
        private string? _dateTimeFormat;
        private CultureInfo? _culture;

        /// <summary>
        /// Gets or sets the date time styles used when converting a date to and from JSON.
        /// </summary>
        /// <value>The date time styles used when converting a date to and from JSON.</value>
        public DateTimeStyles DateTimeStyles
        {
            get => _dateTimeStyles;
            set => _dateTimeStyles = value;
        }

        /// <summary>
        /// Gets or sets the date time format used when converting a date to and from JSON.
        /// </summary>
        /// <value>The date time format used when converting a date to and from JSON.</value>
        public string? DateTimeFormat
        {
            get => _dateTimeFormat ?? string.Empty;
            set => _dateTimeFormat = (StringUtils.IsNullOrEmpty(value)) ? null : value;
        }

        /// <summary>
        /// Gets or sets the culture used when converting a date to and from JSON.
        /// </summary>
        /// <value>The culture used when converting a date to and from JSON.</value>
        public CultureInfo Culture
        {
            get => _culture ?? CultureInfo.CurrentCulture;
            set => _culture = value;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            string text;

            if (value is DateTime dateTime)
            {
                if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                    || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
                {
                    dateTime = dateTime.ToUniversalTime();
                }

                text = dateTime.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);
            }
#if HAVE_DATE_TIME_OFFSET
            else if (value is DateTimeOffset dateTimeOffset)
            {
                if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                    || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
                {
                    dateTimeOffset = dateTimeOffset.ToUniversalTime();
                }

                text = dateTimeOffset.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);
            }
#endif
            else
            {
                throw new JsonSerializationException("Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {0}.".FormatWith(CultureInfo.InvariantCulture, ReflectionUtils.GetObjectType(value)!));
            }

            writer.WriteValue(text);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            bool nullable = ReflectionUtils.IsNullableType(objectType);
            if (reader.TokenType == JsonToken.Null)
            {
                if (!nullable)
                {
                    throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
                }

                return null;
            }

#if HAVE_DATE_TIME_OFFSET
            Type t = (nullable)
                ? Nullable.GetUnderlyingType(objectType)
                : objectType;
#endif

            if (reader.TokenType == JsonToken.Date)
            {
#if HAVE_DATE_TIME_OFFSET
                if (t == typeof(DateTimeOffset))
                {
                    return (reader.Value is DateTimeOffset) ? reader.Value : new DateTimeOffset((DateTime)reader.Value!);
                }

                // converter is expected to return a DateTime
                if (reader.Value is DateTimeOffset offset)
                {
                    return offset.DateTime;
                }
#endif

                return reader.Value;
            }

            if (reader.TokenType != JsonToken.String)
            {
                throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected String, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
            }

            string? dateText = reader.Value?.ToString();

            if (StringUtils.IsNullOrEmpty(dateText) && nullable)
            {
                return null;
            }

#if HAVE_DATE_TIME_OFFSET
            if (t == typeof(DateTimeOffset))
            {
                if (!StringUtils.IsNullOrEmpty(_dateTimeFormat))
                {
                    return DateTimeOffset.ParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles);
                }
                else
                {
                    return DateTimeOffset.Parse(dateText, Culture, _dateTimeStyles);
                }
            }
#endif

            if (!StringUtils.IsNullOrEmpty(_dateTimeFormat))
            {
                return DateTime.ParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles);
            }
            else
            {
                return DateTime.Parse(dateText, Culture, _dateTimeStyles);
            }
        }
    }
    */

    #endregion

    #region CorrectedIsoDateTimeConverter

    /// <summary>
    /// The CorrectedIsoDateTimeConverter class.
    /// </summary>
    public class CorrectedIsoDateTimeConverter : IsoDateTimeConverter
    {
        #region Consts

        //private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
        //private const string DefaultDateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffK";
        //private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFK";
        private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";

        #endregion

        #region Override methods

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime)
            {
                DateTime dateTime = (DateTime)value;

                if (dateTime.Kind == DateTimeKind.Unspecified)
                {
                    if (DateTimeStyles.HasFlag(DateTimeStyles.AssumeUniversal))
                    {
                        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    }
                    else if (DateTimeStyles.HasFlag(DateTimeStyles.AssumeLocal))
                    {
                        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
                    }
                    else
                    {
                        // Force local
                        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
                    }
                }

                if (DateTimeStyles.HasFlag(DateTimeStyles.AdjustToUniversal))
                {
                    dateTime = dateTime.ToUniversalTime();
                }

                string format = string.IsNullOrEmpty(DateTimeFormat) ? DefaultDateTimeFormat : DateTimeFormat;
                string str = dateTime.ToString(format, DateTimeFormatInfo.InvariantInfo);
                writer.WriteValue(str);
                //writer.WriteValue(dateTime.ToString(format, Culture));
            }
            else
            {
                base.WriteJson(writer, value, serializer);
            }
        }

        #endregion
    }

    #endregion

    #region NJson

    /// <summary>
    /// The Json Extension Methods.
    /// </summary>
    public static class NJson
    {
        private static JsonSerializerSettings _defaultSettings = null;
        private static CorrectedIsoDateTimeConverter _dateConverter = new CorrectedIsoDateTimeConverter();
        /// <summary>
        /// Gets Default JsonSerializerSettings.
        /// </summary>
        public static JsonSerializerSettings DefaultSettings
        {
            get
            {
                if (null == _defaultSettings)
                {
                    lock (typeof(NJson))
                    {
                        _defaultSettings = new JsonSerializerSettings()
                        {
                            DateFormatHandling = DateFormatHandling.IsoDateFormat,
                            DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                            DateParseHandling = DateParseHandling.DateTimeOffset,
                            //DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK"
                            //DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK"
                            //DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFK"
                            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK"
                        };
                        if (null == _defaultSettings.Converters)
                        {
                            // Create new List of not exists.
                            _defaultSettings.Converters = new List<JsonConverter>();
                        }
                        if (null != _defaultSettings.Converters && 
                            !_defaultSettings.Converters.Contains(_dateConverter))
                        {
                            _defaultSettings.Converters.Add(_dateConverter);
                        }
                    }
                }
                return _defaultSettings;
            }
        }

        /// <summary>
        /// Convert Object to Json String.
        /// </summary>
        /// <param name="value">The object instance.</param>
        /// <param name="minimized">True for minimize output.</param>
        /// <returns>Returns json string.</returns>
        public static string ToJson(this object value, bool minimized = false)
        {
            string result = string.Empty;
            if (null == value) return result;
            try
            {
                var settings = NJson.DefaultSettings;
                result = JsonConvert.SerializeObject(value, 
                    (minimized) ? Formatting.None : Formatting.Indented, settings);
            }
            catch (Exception ex)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                ex.Err(med);
            }
            return result;
        }
        /// <summary>
        /// Convert Object from Json String.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="value">The json string.</param>
        /// <returns>Returns json string.</returns>
        public static T FromJson<T>(this string value)
        {
            T result = default;
            try
            {
                var settings = NJson.DefaultSettings;
                result = JsonConvert.DeserializeObject<T>(value, settings);
            }
            catch (Exception ex)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                ex.Err(med);
            }
            return result;
        }
        /// <summary>
        /// Save object to json file.
        /// </summary>
        /// <param name="value">The object instance.</param>
        /// <param name="fileName">The target file name.</param>
        /// <param name="minimized">True for minimize output.</param>
        /// <returns>Returns true if save success.</returns>
        public static bool SaveToFile(this object value, string fileName,
            bool minimized = false)
        {
            bool result = true;
            try
            {
                // serialize JSON directly to a file
                using (StreamWriter file = File.CreateText(fileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    serializer.Formatting = (minimized) ? Formatting.None : Formatting.Indented;
                    serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    serializer.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    //serializer.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
                    //serializer.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK";
                    //serializer.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFK";
                    serializer.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
                    serializer.Serialize(file, value);

                    try 
                    {
                        file.Flush();
                        file.Close();
                        file.Dispose();
                    }
                    catch (Exception ex2)
                    {
                        MethodBase med = MethodBase.GetCurrentMethod();
                        ex2.Err(med);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                MethodBase med = MethodBase.GetCurrentMethod();
                ex.Err(med);
            }
            return result;
        }
        /// <summary>
        /// Load Object from Json file.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="fileName">The target file name.</param>
        /// <returns>Returns object instance if load success.</returns>
        public static T LoadFromFile<T>(string fileName)
        {
            T result = default(T);

            Stream stm = null;
            int iRetry = 0;
            int maxRetry = 5;

            try
            {
                while (null == stm && iRetry < maxRetry)
                {
                    try
                    {
                        stm = new FileStream(fileName, FileMode.Open, FileAccess.Read,
                            FileShare.Read);
                    }
                    catch (Exception ex2)
                    {
                        MethodBase med = MethodBase.GetCurrentMethod();
                        ex2.Err(med);

                        if (null != stm)
                        {
                            stm.Close();
                            stm.Dispose();
                        }
                        stm = null;
                        ++iRetry;

                        ApplicationManager.Instance.Sleep(50);
                    }
                }

                if (null != stm)
                {
                    // deserialize JSON directly from a file
                    using (StreamReader file = new StreamReader(stm))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        //serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                        serializer.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                        //serializer.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
                        //serializer.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK";
                        //serializer.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFK";
                        serializer.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
                        result = (T)serializer.Deserialize(file, typeof(T));

                        file.Close();
                        file.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                result = default(T);
                MethodBase med = MethodBase.GetCurrentMethod();
                ex.Err(med);
            }

            if (null != stm)
            {
                try
                {
                    stm.Close();
                    stm.Dispose();
                }
                catch { }
            }
            stm = null;

            return result;
        }
        /// <summary>
        /// Gets local data json folder path name.
        /// </summary>
        public static string LocalDataFolder
        {
            get
            {
                string localFilder = Folders.Combine(
                    Folders.Assemblies.CurrentExecutingAssembly, "data");
                if (!Folders.Exists(localFilder))
                {
                    Folders.Create(localFilder);
                }
                return localFilder;
            }
        }
        /// <summary>
        /// Gets local configs json folder path name.
        /// </summary>
        public static string LocalConfigFolder
        {
            get
            {
#if !USE_PROGRAM_DATA
                string localFilder = Folders.Combine(
                    Folders.Assemblies.CurrentExecutingAssembly, "configs");
#else
                string localFilder = ApplicationManager.Instance.Environments.Company.Configs.FullName;
#endif
                if (!Folders.Exists(localFilder))
                {
                    Folders.Create(localFilder);
                }
                return localFilder;
            }
        }
        /// <summary>
        /// Gets full file name for file in json data local folder.
        /// </summary>
        /// <param name="fileName">The file name (not include folder).</param>
        /// <returns>Returns full path to access file in json local folder</returns>
        public static string LocalDataFile(string fileName)
        {
            return Folders.Combine(NJson.LocalDataFolder, fileName);
        }
        /// <summary>
        /// Checks is local data file exists.
        /// </summary>
        /// <param name="fileName">The file name (not include folder).</param>
        /// <returns>Returns true if file in json local data folder</returns>
        public static bool DataExists(string fileName)
        {
            string localFile = NJson.LocalDataFile(fileName);
            return Files.Exists(localFile);
        }

        /// <summary>
        /// Gets full file name for file in json local configs folder.
        /// </summary>
        /// <param name="fileName">The file name (not include folder).</param>
        /// <returns>Returns full path to access file in json local folder</returns>
        public static string LocalConfigFile(string fileName)
        {
            return Folders.Combine(NJson.LocalConfigFolder, fileName);
        }
        /// <summary>
        /// Checks is local config file exists.
        /// </summary>
        /// <param name="fileName">The file name (not include folder).</param>
        /// <returns>Returns true if file in json local configs folder</returns>
        public static bool ConfigExists(string fileName)
        {
            string localFile = NJson.LocalConfigFile(fileName);
            return Files.Exists(localFile);
        }
    }

    #endregion
}
