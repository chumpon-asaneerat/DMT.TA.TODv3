#region Using

using NLib.ServiceProcess;

#endregion

namespace DMT.Services
{
    #region PlazaDataService (Core service)

    /// <summary>
    /// Plaza Data Service. (Core service).
    /// </summary>
    public class PlazaDataService : NServiceBase
    {
        #region Internal Variables

        private bool _running = false;
        private bool _pause = true;
        private LocalDatabaseWebServer _server = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlazaDataService() : base()
        {
            _server = new LocalDatabaseWebServer();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~PlazaDataService()
        {
            if (null != _server)
            {
                _server.Shutdown();
            }
            _server = null;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// OnStart
        /// </summary>
        /// <param name="args">The service event args.</param>
        protected override void OnStart(string[] args)
        {
            _running = true;
            _pause = false;
            // Load Config service.
            PlazaServiceConfigManager.Instance.LoadConfig();

            if (null != _server) _server.Start();
        }
        /// <summary>
        /// OnPause
        /// </summary>
        protected override void OnPause()
        {
            _pause = true;
        }
        /// <summary>
        /// OnContinue
        /// </summary>
        protected override void OnContinue()
        {
            _pause = false;
            // Load Config service.
            PlazaServiceConfigManager.Instance.LoadConfig();
        }
        /// <summary>
        /// OnStop.
        /// </summary>
        protected override void OnStop()
        {
            _running = false;
            _pause = true;
            if (null != _server) _server.Shutdown();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Checks is service running.
        /// </summary>
        public bool IsRunning { get { return _running; } }
        /// <summary>
        /// Checks is service pause.
        /// </summary>
        public bool IsPause { get { return _pause; } }

        #endregion
    }

    #endregion
}
