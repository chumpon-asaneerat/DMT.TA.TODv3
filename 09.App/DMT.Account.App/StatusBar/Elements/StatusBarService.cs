#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using NLib;

using DMT.Configurations;
using DMT.Services;

#endregion

namespace DMT.Controls.StatusBar
{
    public class StatusBarService
    {
        #region Singelton

        private static StatusBarService _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static StatusBarService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(StatusBarService))
                    {
                        _instance = new StatusBarService();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private List<Action> _actions = new List<Action>();

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private StatusBarService() : base() 
        {
            AccountConfigManager.Instance.ConfigChanged += ConfigChanged;
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~StatusBarService()
        {
            AccountConfigManager.Instance.ConfigChanged -= ConfigChanged;
        }

        #endregion

        #region Private Methods

        #region Config Watcher Handlers

        private void ConfigChanged(object sender, EventArgs e)
        {
            //UpdateUI();
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Register UpdateUI actiion.
        /// </summary>
        /// <param name="action">The action.</param>
        public void Register(Action action)
        {
            if (null == action) return;
            if (!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        /// <summary>
        /// Unregister UpdateUI actiion.
        /// </summary>
        public void Unregister(Action action)
        {
            if (null == action) return;
            if (_actions.Contains(action))
            {
                _actions.Remove(action);
            }
        }

        #endregion
    }
}
