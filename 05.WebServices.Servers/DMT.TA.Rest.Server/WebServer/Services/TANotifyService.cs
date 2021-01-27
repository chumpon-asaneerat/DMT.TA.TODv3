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
    /// The TANotifyService class.
    /// </summary>
    public class TANotifyService
    {
        #region Singelton

        private static TANotifyService _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static TANotifyService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(TANotifyService))
                    {
                        _instance = new TANotifyService();
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
        private TANotifyService() : base()
        {
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TANotifyService()
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

        #endregion
    }
}
