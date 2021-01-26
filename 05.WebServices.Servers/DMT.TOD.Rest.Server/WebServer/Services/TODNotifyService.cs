#region Using

using System;
using System.Collections.Generic;
using System.Linq;

using DMT.Models;
using NLib;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// The TODNotifyService class.
    /// </summary>
    public class TODNotifyService
    {
        #region Singelton

        private static TODNotifyService _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static TODNotifyService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TODNotifyService))
                    {
                        _instance = new TODNotifyService();
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
        private TODNotifyService() : base()
        {
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TODNotifyService()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Raise TSB Changed event.
        /// </summary>
        public void RaiseTSBChanged()
        {
            TSBChanged.Call(this, EventArgs.Empty);
        }
        /// <summary>
        /// Raise TSB Shift Changed event.
        /// </summary>
        public void RaiseTSBShiftChanged()
        {
            TSBShiftChanged.Call(this, EventArgs.Empty);
        }
        /// <summary>
        /// Raise User Shift Changed event.
        /// </summary>
        public void RaiseUserShiftChanged()
        {
            UserShiftChanged.Call(this, EventArgs.Empty);
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The TSBChanged Event Handler
        /// </summary>
        public event EventHandler TSBChanged;
        /// <summary>
        /// The TSBShiftChanged Event Handler
        /// </summary>
        public event EventHandler TSBShiftChanged;
        /// <summary>
        /// The UserShiftChanged Event Handler
        /// </summary>
        public event EventHandler UserShiftChanged;

        #endregion
    }
}
