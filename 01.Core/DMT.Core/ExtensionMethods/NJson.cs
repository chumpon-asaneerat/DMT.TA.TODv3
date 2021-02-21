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
    #region CorrectedIsoDateTimeConverter

    public class CorrectedIsoDateTimeConverter : IsoDateTimeConverter
    {
        //private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
        private const string DefaultDateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffK";

        /// <summary>
        /// Write Json.
        /// </summary>
        /// <param name="writer">The JsonWriter instance.</param>
        /// <param name="value">The object instance.</param>
        /// <param name="serializer">The Serializer instance.</param>
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
                writer.WriteValue(dateTime.ToString(format, Culture));
                //base.WriteJson(writer, dateTime, serializer);
            }
            else
            {
                base.WriteJson(writer, value, serializer);
            }
        }
    }

    #endregion

    #region NJson

    /// <summary>
    /// The Json Extension Methods.
    /// </summary>
    public static class NJson
    {
        private static JsonSerializerSettings _defaultSettings = null;
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
                            //DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'K'"
                            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK"
                        };
                        if (null == _defaultSettings.Converters) _defaultSettings.Converters = new List<JsonConverter>();
                        if (null != _defaultSettings.Converters)
                        {
                            _defaultSettings.Converters.Add(new CorrectedIsoDateTimeConverter());
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
            try
            {
                var settings = NJson.DefaultSettings;
                settings.Converters.Add(new CorrectedIsoDateTimeConverter());
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
                    serializer.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'K'";
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
                        serializer.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'K'";
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
