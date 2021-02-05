#region Using

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using NLib;
using NLib.IO;
using Newtonsoft.Json;
using NLib.Controls.Design;

#endregion

namespace DMT.Configurations
{
    #region JsonConfigFileManger (abstract)

    /// <summary>
    /// The JsonConfigFileManger abstract class.
    /// </summary>
    /// <typeparam name="T">The Config Class Type.</typeparam>
    public abstract class JsonConfigFileManger<T>
        where T : new()
    {
        #region Internal Variables

        private FileSystemWatcher _watcher = null;
        private T _cfg = new T();
        private DateTime _lastRead = DateTime.MinValue;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public JsonConfigFileManger() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~JsonConfigFileManger()
        {
            Shutdown();
        }

        #endregion

        #region Private Methods

        private void InitWacther(bool reCreated = false)
        {
            if (null != _watcher && !reCreated)
            {
                return;
            }
            if (null != _watcher)
            {
                ReleaseWacther();
            }

            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                _watcher = new FileSystemWatcher();
                _watcher.Path = NJson.LocalConfigFolder;
                _watcher.NotifyFilter = NotifyFilters.LastWrite;
                _watcher.Filter = "*.json";
                _watcher.Changed += _watcher_Changed;
                _watcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                med.Err(ex);
                ReleaseWacther();
            }
        }

        private void ReleaseWacther()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                if (null != _watcher)
                {
                    _watcher.Changed -= _watcher_Changed;
                    _watcher.Dispose();
                }
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }
            _watcher = null;
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.FullPath) && !string.IsNullOrWhiteSpace(this.FileName) &&
                e.FullPath.Trim().ToLower() == this.FileName.Trim().ToLower())
            {
                // Gets Last Write Time.
                DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
                TimeSpan ts = lastWriteTime - _lastRead;
                if (ts.TotalMilliseconds > 0)
                {
                    Console.WriteLine("Detected File '{0}' Changed.", e.Name);
                    // Reload config.
                    this.LoadConfig();
                    // Set last read.
                    _lastRead = lastWriteTime;
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets Config File Name.
        /// </summary>
        public abstract string FileName { get; }
        /// <summary>
        /// Raise Config Changed Event.
        /// </summary>
        protected void RaiseConfigChanged()
        {
            // Raise event.
            ConfigChanged.Call(this, EventArgs.Empty);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load Config from file.
        /// </summary>
        public virtual void LoadConfig()
        {
            lock (this)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    // save back to file.
                    if (!NJson.ConfigExists(FileName))
                    {
                        // File not exist.
                        if (null == _cfg)
                        {
                            _cfg = new T();
                        }
                        NJson.SaveToFile(_cfg, FileName);
                    }
                    else
                    {
                        // Check file size.
                        long len = new FileInfo(FileName).Length;
                        if (len <= 0)
                        {
                            // File size is zero.
                            if (null == _cfg)
                            {
                                _cfg = new T();
                            }
                            NJson.SaveToFile(_cfg, FileName);
                        }
                        else
                        {
                            _cfg = NJson.LoadFromFile<T>(FileName);
                        }
                    }
                    // Raise event.
                    RaiseConfigChanged();
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            }
        }
        /// <summary>
        /// Save Config to file.
        /// </summary>
        public virtual void SaveConfig()
        {
            lock (this)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    // save back to file.
                    if (null == _cfg)
                    {
                        _cfg = new T();
                    }
                    NJson.SaveToFile(_cfg, FileName);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            }
        }
        /// <summary>
        /// Start File Watcher Service.
        /// </summary>
        public void Start()
        {
            InitWacther();
        }
        /// <summary>
        /// Shutdown File Watcher Service.
        /// </summary>
        public void Shutdown()
        {
            ReleaseWacther();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets current config.
        /// </summary>
        public T Value { get { return _cfg; } set { } }

        #endregion

        #region Public Events

        /// <summary>
        /// The ConfigChanged Event Handler.
        /// </summary>
        public event EventHandler ConfigChanged;

        #endregion
    }

    #endregion
}
