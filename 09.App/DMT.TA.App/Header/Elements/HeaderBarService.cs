#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

using NLib;
//using NLib.Services;

using DMT.Configurations;
using DMT.Services;
using System.Threading.Tasks;

#endregion

namespace DMT.Controls.Header
{
    /// <summary>
    /// The HeaderBarService class.
    /// </summary>
    public class HeaderBarService
    {
        #region Singelton

        private static HeaderBarService _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static HeaderBarService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(HeaderBarService))
                    {
                        _instance = new HeaderBarService();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private TAConfigManager _cfgMgr = TAConfigManager.Instance;
        private List<Action> _actions = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private HeaderBarService() : base()
        {
            _actions = new List<Action>();
            if (null != _cfgMgr)
            {
                _cfgMgr.ConfigChanged += ConfigChanged;
            }
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~HeaderBarService()
        {
            if (null != _cfgMgr)
            {
                _cfgMgr.ConfigChanged -= ConfigChanged;
            }
            if (null != _actions)
            {
                _actions.Clear();
            }
            _actions = null;
        }

        #endregion

        #region Private Methods

        private void RunAction(Action action)
        {
            if (null != action)
            {
                //Console.WriteLine("Begin : {0:HH:mm:ss.fff}", DateTime.Now);
                action.Call(); // call action on UI Thread.
                //Console.WriteLine("End : {0:HH:mm:ss.fff}", DateTime.Now);
            }
        }

        private async void RunActionAsync(Action action)
        {
            await Task.Run(() => { RunAction(action); });
        }

        private void ConfigChanged(object sender, EventArgs e)
        {
            if (null == _actions || _actions.Count <= 0) return;
            _actions.ForEach(action =>
            {
                // Call Update UI action.
                RunActionAsync(action);
            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Register UpdateUI actiion.
        /// </summary>
        /// <param name="action">The action.</param>
        public void Register(Action action)
        {
            if (null == action) return;
            if (null != _actions && !_actions.Contains(action))
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
            if (null != _actions && _actions.Contains(action))
            {
                _actions.Remove(action);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets HeaderBar Configs.
        /// </summary>
        public TAHeaderBars HeaderBarConfigs
        {
            get
            {
                if (null == _cfgMgr || null == _cfgMgr.Value || null == _cfgMgr.Value.UIConfig) return null;
                return _cfgMgr.Value.UIConfig.HeaderBars;
            }
        }

        #endregion
    }
}
