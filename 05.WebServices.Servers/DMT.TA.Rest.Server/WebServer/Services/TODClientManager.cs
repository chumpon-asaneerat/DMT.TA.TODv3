#region Using

using System;
using System.Collections.Generic;
using System.Linq;

using DMT.Configurations;
using NLib;

#endregion

namespace DMT.Services
{
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

        private List<TODAppWebServiceConfig> _clients = null;

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
            Shutdown();
        }

        #endregion

        #region Public Methods

        #region Start/Shutdown

        /// <summary>
        /// Start service.
        /// </summary>
        public void Start()
        {
            if (null == _clients) _clients = new List<TODAppWebServiceConfig>();
            _clients.Clear();
        }
        /// <summary>
        /// Shutdown service.
        /// </summary>
        public void Shutdown()
        {
            if (null != _clients)
            {
                _clients.Clear();
            }
            _clients = null;
        }

        #endregion

        #region Register

        /// <summary>
        /// Register.
        /// </summary>
        /// <param name="value"></param>
        public void Register(TODAppWebServiceConfig value)
        {
            if (null == value) return;
        }

        #endregion

        #region Unregister

        /// <summary>
        /// Unregister.
        /// </summary>
        /// <param name="value"></param>
        public void Unregister(TODAppWebServiceConfig value)
        {
            if (null == value) return;
        }

        #endregion

        #endregion
    }
}
