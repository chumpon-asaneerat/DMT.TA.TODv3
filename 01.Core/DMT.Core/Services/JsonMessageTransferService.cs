#region Using

using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Timers;
using System.Threading.Tasks;

using NLib;
using NLib.IO;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// The Json Message Transfer Service class.
    /// </summary>
    public abstract class JsonMessageTransferService
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public JsonMessageTransferService() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~JsonMessageTransferService() 
        {
            this.Shutdown();
        }

        #endregion

        #region Internal Variables

        private Timer _timer = null;
        private bool _scanning = false;

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
                List<string> files = new List<string>();
                var msgFiles = Directory.GetFiles(this.MessageFolder, "*.json");
                if (null != msgFiles && msgFiles.Length > 0) files.AddRange(msgFiles);
                files.ForEach(file =>
                {
                    try
                    {
                        string json = File.ReadAllText(file);
                        ProcessJson(file, json);
                    }
                    catch (Exception ex2)
                    {
                        // Invalid. Read file error.
                        MoveToInvalid(file);
                        med.Err(ex2);
                    }
                });
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }
            _scanning = false;
        }

        #endregion

        #endregion

        #region Protected Methods and Properties

        #region FolderName property

        /// <summary>
        /// Gets Folder Name (sub directory name).
        /// </summary>
        protected abstract string FolderName { get; }

        #endregion

        #region ProcessJson

        /// <summary>
        /// Process Json (string).
        /// </summary>
        /// <param name="fullFileName">The json full file name.</param>
        /// <param name="jsonString">The json data in string.</param>
        protected abstract void ProcessJson(string fullFileName, string jsonString);

        #endregion

        #region File Managements

        /// <summary>
        /// Write File to target folder.
        /// </summary>
        /// <param name="fileName">The file name with no extension.</param>
        /// <param name="message">The json data in string.</param>
        public void WriteFile(string fileName, string message)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(message)) 
                return;

            string fullFileName = Path.Combine(this.FolderName, fileName +".json");

            MethodBase med = MethodBase.GetCurrentMethod();
            // Save message.
            try
            {
                using (var stream = File.CreateText(fullFileName))
                {
                    stream.Write(message);
                    stream.Flush();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }
        }
        /// <summary>
        /// Move File to specificed sub folder.
        /// </summary>
        /// <param name="file">The target fule (Full File Name).</param>
        /// <param name="subFolder">The sub folder name.</param>
        protected void MoveTo(string file, string subFolder)
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
        protected void MoveToBackup(string file)
        {
            MoveTo(file, "Backup");
        }
        /// <summary>
        /// Move File to 'Invalid' sub folder.
        /// </summary>
        /// <param name="file">The target fule (Full File Name).</param>
        protected void MoveToInvalid(string file)
        {
            MoveTo(file, "Invalid");
        }
        /// <summary>
        /// Move File to 'Error' sub folder.
        /// </summary>
        /// <param name="file">The target fule (Full File Name).</param>
        protected void MoveToError(string file)
        {
            MoveTo(file, "Error");
        }
        /// <summary>
        /// Move File to 'NotSupports' sub folder.
        /// </summary>
        /// <param name="file">The target fule (Full File Name).</param>
        protected void MoveToNotSupports(string file)
        {
            MoveTo(file, "NotSupports");
        }

        #endregion

        #region Start/Shutdown

        /// <summary>
        /// OnStart.
        /// </summary>
        protected virtual void OnStart() { }
        /// <summary>
        /// OnShutdown.
        /// </summary>
        protected virtual void OnShutdown() { }

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
            _timer.Interval = 1000;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();

            OnStart(); // call virtual method.
        }
        /// <summary>
        /// Shutdown service.
        /// </summary>
        public void Shutdown()
        {
            //MethodBase med = MethodBase.GetCurrentMethod();
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

            OnShutdown(); // call virtual method.
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
