#region Using

using System;
using NLib;
using DMT.Models;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// The Runtime Manager class.
    /// </summary>
    public class RuntimeManager
    {
        #region Singelton

        private static RuntimeManager _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static RuntimeManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(RuntimeManager))
                    {
                        _instance = new RuntimeManager();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private RuntimeManager() : base()
        {
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~RuntimeManager()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Raise TSB Changed
        /// </summary>
        public void RaiseTSBChanged()
        {
            TSBChanged.Call(this, EventArgs.Empty);
        }

        public void RaiseShiftChanged()
        {
            ShiftChanged.Call(this, EventArgs.Empty);
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The TSBChanged Event Handler.
        /// </summary>
        public event EventHandler TSBChanged;
        /// <summary>
        /// The ShiftChanged Event Handler.
        /// </summary>
        public event EventHandler ShiftChanged;

        #endregion
    }
}
