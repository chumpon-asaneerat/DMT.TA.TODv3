//#define USE_PROGRAM_DATA

#region Using

using System;
using System.IO;
using System.Reflection;
using NLib;
using Newtonsoft.Json;
using NLib.IO;
using System.Configuration;

#endregion

namespace DMT
{
    /// <summary>
    /// The Json Extension Methods.
    /// </summary>
    public static class NJson
    {
        public static JsonSerializerSettings DefaultSettings = new JsonSerializerSettings()
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            //DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'K'"
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK"
        };

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
            try
            {
                // deserialize JSON directly from a file
                using (StreamReader file = File.OpenText(fileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    serializer.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    serializer.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'K'";
                    result = (T)serializer.Deserialize(file, typeof(T));
                }
            }
            catch (Exception ex)
            {
                result = default(T);
                MethodBase med = MethodBase.GetCurrentMethod();
                ex.Err(med);
            }
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
}
