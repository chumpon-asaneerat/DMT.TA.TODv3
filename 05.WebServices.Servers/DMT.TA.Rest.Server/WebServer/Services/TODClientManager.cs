#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
//using System.Windows.Forms;
//using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Threading;

using DMT.Configurations;
using DMT.Services;
using DMT.Models;
using DMT.Models.ExtensionMethods;
using System.Timers;

using NLib;
using NLib.IO;
using NLib.Services;
using NLib.Reflection;

using RestSharp;
using System.Threading.Tasks;

#endregion

namespace DMT.Services
{
    using ops = Operations.TOD.Notify;

    /// <summary>
    /// The TODClientManager class.
    /// </summary>
    public class TODClientManager
    {
        #region Singelton

        private static TODClientManager _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static TODClientManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TODClientManager))
                    {
                        _instance = new TODClientManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private Timer _timer = null;
        private bool _scanning = false;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private TODClientManager() : base()
        {
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TODClientManager()
        {
            this.Shutdown();
        }

        #endregion

        #region Private Methods

        #region Timer Handlers

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_scanning) return;
            _scanning = true;

            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                ProcessTODFolders();
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }
            _scanning = false;
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Start service.
        /// </summary>
        public void Start()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            // Init Scanning Timer
            _scanning = false;
            _timer = new Timer();
            _timer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }
        /// <summary>
        /// Shutdown service.
        /// </summary>
        public void Shutdown()
        {
            // Free Scanning Timer 
            try
            {
                if (null != _timer)
                {
                    _timer.Elapsed -= _timer_Elapsed;
                    _timer.Stop();
                    _timer.Dispose();
                }
            }
            catch { }
            _timer = null;
            _scanning = false;
        }

        #endregion

        #region Private Methods

        private string GetTODFolder(string host, int portNo)
        {
            string todFolderName = string.Format("tod.{0}x{1:D5}", host, portNo);
            string localFilder = Folders.Combine(this.MessageFolder, todFolderName);
            if (!Folders.Exists(localFilder))
            {
                Folders.Create(localFilder);
            }
            return localFilder;
        }
        /// <summary>
        /// Get File Name.
        /// </summary>
        /// <param name="msgType">The message type.</param>
        /// <returns>Returns file name</returns>
        private string GetFileName(string msgType)
        {

            if (string.IsNullOrWhiteSpace(msgType))
                return string.Empty;
            // Save message.
            string fileName = "msg." + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss.ffffff",
                System.Globalization.DateTimeFormatInfo.InvariantInfo) + "." + msgType;
            return fileName;
        }

        private string GetFileName<T>(T value)
        {
            string msgType = "unknown";
            if (value is RevenueEntry)
                msgType = "revenue.entry.save";
            else if (value is TSBShift)
                msgType = "tsb.shift.change";
            else if (value is UserShift)
                msgType = "user.shift.change";
            return GetFileName(msgType);
        }
        /// <summary>
        /// Compress Files.
        /// </summary>
        private void CompressFiles(string todFolder)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            List<string> files = new List<string>();
            List<string> oldFiles = new List<string>();

            string backupFolder = Path.Combine(todFolder, "Backup");
            if (!Directory.Exists(backupFolder)) return; // No Backup Folder.

            var backupFiles = Directory.GetFiles(backupFolder, "*.json");
            if (null != backupFiles && backupFiles.Length > 0) files.AddRange(backupFiles);

            // Find old files to compress.
            DateTime today = DateTime.Today;
            DateTime firstOnMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime targetDT = firstOnMonth.AddMonths(-2); // last 2 month.

            files.ForEach(file =>
            {
                try
                {
                    string fileName = Path.GetFileName(file);
                    string fileYMD = fileName.Substring(4, 10);
                    DateTime fileDT;
                    if (DateTime.TryParseExact(fileYMD, "yyyy.MM.dd",
                        System.Globalization.DateTimeFormatInfo.InvariantInfo,
                        System.Globalization.DateTimeStyles.None,
                        out fileDT))
                    {
                        if (fileDT < targetDT)
                        {
                            oldFiles.Add(file);
                        }
                    }
                }
                catch (Exception ex1)
                {
                    med.Err(ex1);
                }
            });
            // Move old files to target folder.
            if (null != oldFiles && oldFiles.Count > 0)
            {
                string zipDir = targetDT.ToString("yyyy.MM.dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                oldFiles.ForEach(file =>
                {
                    try
                    {
                        MoveTo(file, zipDir);
                    }
                    catch (Exception ex2)
                    {
                        med.Err(ex2);
                    }
                });

                // Compress.
                string targetDir = Path.Combine(this.MessageFolder, "Backup", zipDir);
                string targetFile = zipDir + ".zip";
                string outputFile = Path.Combine(this.MessageFolder, "Backup", targetFile);
                try
                {
                    NLib.Utils.SevenZipManager.CompressDirectory(targetDir, outputFile, true);
                }
                catch (Exception ex3)
                {
                    med.Err(ex3);
                }

                // Remove Folder.
                try
                {
                    if (Directory.Exists(targetDir))
                    {
                        // Delete all sub directories and files
                        Directory.Delete(targetDir, true);
                    }
                }
                catch (Exception ex4)
                {
                    med.Err(ex4);
                }
            }
        }
        /// <summary>
        /// Read All Text from target file.
        /// </summary>
        /// <param name="fileName">The target full file name.</param>
        /// <returns>Returns text in target file.</returns>
        private string ReadAllText(string fileName)
        {
            string text = string.Empty;

            MethodBase med = MethodBase.GetCurrentMethod();
            FileStream fs = null;

            #region Open File Steram (for read)

            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception ex)
            {
                med.Err(ex);
                text = string.Empty;
            }

            #endregion

            #region Read File Content

            try
            {
                if (null != fs)
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        text = reader.ReadToEnd();
                        reader.Close();
                        reader.Dispose();
                    }
                }
            }
            catch (Exception ex2)
            {
                med.Err(ex2);
                text = string.Empty;
            }

            #endregion

            #region Close File Steram

            if (null != fs)
            {
                try
                {
                    fs.Close();
                    fs.Dispose();
                }
                catch { }
            }
            fs = null;

            #endregion

            return text;
        }
        /// <summary>
        /// Write File to target folder.
        /// </summary>
        /// <param name="folderName">The each tod root folder name.</param>
        /// <param name="fileName">The file name with no extension.</param>
        /// <param name="message">The json data in string.</param>
        private void WriteFile(string folderName, string fileName, string message)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(message))
                return;

            MethodBase med = MethodBase.GetCurrentMethod();
            string fullFileName = Path.Combine(folderName, fileName + ".json");

            med.Info("Write message: {0}", message);
            med.Info("Attemp Generate file: {0}.", fileName + ".json");
            int iRetry = 0;
            // Save message.
            while (!File.Exists(fullFileName) && iRetry < 5)
            {
                try
                {
                    using (var stream = File.CreateText(fullFileName))
                    {
                        stream.Write(message);
                        stream.Flush();
                        stream.Close();
                    }

                    var info = new FileInfo(fullFileName);
                    if (null != info)
                    {
                        med.Info("Generate file: {0}. File Size: {1:n0} bytes.", fileName + ".json", info.Length);
                    }
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    // remove if length is zero.
                    if (File.Exists(fullFileName))
                    {
                        var info = new FileInfo(fullFileName);
                        if (null == info || info.Length <= 0)
                        {
                            med.Info("Error whie Generate file: {0}.", fileName + ".json");
                            try
                            {
                                med.Info("Attemp to remove Generate file: {0}.", fileName + ".json");
                                File.Delete(fullFileName);
                            }
                            catch (Exception ex2)
                            {
                                med.Err(ex2);
                                med.Info("Failed to remove Generate file: {0}.", fileName + ".json");
                            }
                        }
                    }
                }
                ApplicationManager.Instance.Wait(50);
                iRetry++;
            }

            if (!File.Exists(fullFileName))
            {
                med.Info("Failed to Generate file: {0}.", fileName + ".json");
            }
            else
            {
                med.Info("Success to Generate file: {0}.", fileName + ".json");
            }
        }
        /// <summary>
        /// Move File to specificed sub folder.
        /// </summary>
        /// <param name="file">The target fule (Full File Name).</param>
        /// <param name="subFolder">The sub folder name.</param>
        private void MoveTo(string file, string subFolder)
        {
            string parentDir = Path.GetDirectoryName(file);
            string fileName = Path.GetFileName(file);
            string targetPath = Path.Combine(parentDir, subFolder);
            if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);
            if (!Directory.Exists(targetPath)) return;
            string targetFile = Path.Combine(targetPath, fileName);
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                if (File.Exists(targetFile)) File.Delete(targetFile);
                File.Move(file, targetFile);
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }
        }
        /// <summary>
        /// Move File to 'Backup' sub folder.
        /// </summary>
        /// <param name="file">The target fule (Full File Name).</param>
        private void MoveToBackup(string file)
        {
            MoveTo(file, "Backup");
        }
        /// <summary>
        /// Move File to 'Invalid' sub folder.
        /// </summary>
        /// <param name="file">The target fule (Full File Name).</param>
        private void MoveToInvalid(string file)
        {
            MoveTo(file, "Invalid");
        }
        /// <summary>
        /// Move File to 'Error' sub folder.
        /// </summary>
        /// <param name="file">The target fule (Full File Name).</param>
        private void MoveToError(string file)
        {
            MoveTo(file, "Error");
        }
        /// <summary>
        /// Move File to 'NotSupports' sub folder.
        /// </summary>
        /// <param name="file">The target fule (Full File Name).</param>
        private void MoveToNotSupports(string file)
        {
            MoveTo(file, "NotSupports");
        }

        private void ProcessTODFolders()
        {
            if (null == TAConfigManager.Instance.Value || null == TAConfigManager.Instance.Value.Services) return;

            MethodBase med = MethodBase.GetCurrentMethod();

            var configs = TAConfigManager.Instance.Value.Services.TODApps;

            // Set operation DMT.
            Operations.TOD.DMT = TAConfigManager.Instance; // required for NetworkId

            if (null != configs && configs.Count > 0)
            {
                _scanning = true;

                configs.ForEach(cfg => 
                {
                    Operations.TOD.Config = cfg; // varies by client config
                    try
                    {
                        // auto create folder for each TOD client.
                        string todFolder = GetTODFolder(cfg.Service.HostName, cfg.Service.PortNumber);
                        // Compress
                        CompressFiles(todFolder);
                        // Process file in folder.
                        ProcessJsonFiles(todFolder);
                    }
                    catch (Exception ex)
                    {
                        med.Err(ex);
                    }
                });

                _scanning = false;
            }
        }

        private void ProcessJsonFiles(string todFolder)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            List<string> files = new List<string>();
            var msgFiles = Directory.GetFiles(todFolder, "*.json");
            if (null != msgFiles && msgFiles.Length > 0) files.AddRange(msgFiles);
            files.ForEach(file => 
            {
                try 
                {
                    string json = ReadAllText(file);
                    ProcessJson(file, json);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            });
        }

        /// <summary>
        /// Process Json (string).
        /// </summary>
        /// <param name="fullFileName">The json full file name.</param>
        /// <param name="jsonString">The json data in string.</param>
        private void ProcessJson(string fullFileName, string jsonString) 
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            // Extract File Name.
            if (string.IsNullOrEmpty(fullFileName)) return; // skip if file name is empty.

            if (fullFileName.Contains("tsb.shift.change"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.TSBShift>();
                    //SendChangeTSBShift(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToError(fullFileName);
                }
            }
            else if (fullFileName.Contains("user.shift.change"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.UserShift>();
                    //SendChangeTSBShift(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToError(fullFileName);
                }
            }
            else if (fullFileName.Contains("revenue.entry.save"))
            {
                try
                {
                    var value = jsonString.FromJson<Models.RevenueEntry>();
                    //SendChangeTSBShift(fullFileName, value);
                }
                catch (Exception ex)
                {
                    // Parse Error.
                    med.Err(ex);
                    med.Err("message is null or cannot convert to json object.");
                    MoveToError(fullFileName);
                }
            }
            else
            {
                // process not Supports file.
                med.Err("Not Supports message.");
                MoveToNotSupports(fullFileName);
            }
        }

        /// <summary>
        /// Write Json object to all TOD queues.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        private void WtiteQueues<T>(T value)
        {
            if (null == value) return;
            if (null == TAConfigManager.Instance.Value || null == TAConfigManager.Instance.Value.Services) return;

            MethodBase med = MethodBase.GetCurrentMethod();

            var configs = TAConfigManager.Instance.Value.Services.TODApps;

            // Set operation DMT.
            Operations.TOD.DMT = TAConfigManager.Instance; // required for NetworkId

            if (null != configs && configs.Count > 0)
            {
                string fileName = GetFileName(value);
                string msg = value.ToJson(false); // generate json string.

                configs.ForEach(cfg => 
                {
                    Operations.TOD.Config = cfg; // varies by client config
                    try
                    {
                        // auto create folder for each TOD client.
                        string todFolder = GetTODFolder(cfg.Service.HostName, cfg.Service.PortNumber);
                        // write to each TOD queue folder
                        WriteFile(todFolder, fileName, msg);
                    }
                    catch (Exception ex)
                    {
                        med.Err(ex);
                    }
                });
            }
        }

        #endregion

        #region Protected method and properties

        #region FolderName property

        /// <summary>
        /// Gets Folder Name (sub directory name).
        /// </summary>
        protected string FolderName { get { return "tod.ws.msgs"; } }

        #endregion

        #endregion

        #region Public Methods

        #region TSB Shift Change -> Sent to all TOD apps.

        /*
        /// <summary>
        /// Raise all TOD TSBShiftChanged.
        /// </summary>
        /// <param name="value"></param>
        public void TODTSBShiftChanged()
        {
            if (null == TAConfigManager.Instance.Value || null == TAConfigManager.Instance.Value.Services) return;
            var configs = TAConfigManager.Instance.Value.Services.TODApps;

            // Set operation DMT.
            Operations.TOD.DMT = TAConfigManager.Instance; // required for NetworkId

            if (null != configs && configs.Count > 0)
            {
                configs.ForEach(cfg =>
                {
                    //TODO: TO Fixed.
                    Operations.TOD.Config = cfg; // varies by client config
                    // Notify TOD - TSBShiftChanged.
                    var ret = ops.TSBShiftChanged();
                    if (null == ret || ret.Failed)
                    {
                        Console.WriteLine("Send to TOD failed.");
                    }
                });
            }
        }
        */

        #endregion

        public void SendToTOD(TSBShift value)
        {
            WtiteQueues(value);
        }

        public void SendToTOD(UserShift value)
        {
            WtiteQueues(value);
        }

        public void SendToTOD(RevenueEntry value)
        {
            WtiteQueues(value);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets root message folder path name.
        /// </summary>
        public string MessageFolder
        {
            get
            {
                string localFilder = Folders.Combine(
                    Folders.Assemblies.CurrentExecutingAssembly, this.FolderName);
                if (!Folders.Exists(localFilder))
                {
                    Folders.Create(localFilder);
                }
                return localFilder;
            }
        }

        #endregion
    }
}
