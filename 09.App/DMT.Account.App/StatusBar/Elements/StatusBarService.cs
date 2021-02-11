#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using NLib;

using DMT.Configurations;
using DMT.Services;
using System.Threading.Tasks;

#endregion

namespace DMT.Controls.StatusBar
{
    /// <summary>
    /// The StatusBarService class.
    /// </summary>
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

        private AccountConfigManager _cfgMgr = AccountConfigManager.Instance;
        private List<Action> _actions = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private StatusBarService() : base() 
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
        ~StatusBarService()
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
        /// Gets or sets StatusBar Configs.
        /// </summary>
        public AccountStatusBars StatusBarConfigs
        {
            get
            {
                if (null == _cfgMgr || null == _cfgMgr.Value || null == _cfgMgr.Value.UIConfig) return null;
                return _cfgMgr.Value.UIConfig.StatusBars;
            }
        }
        /// <summary>
        /// Gets AppInfo Status Bar Config.
        /// </summary>
        public StatusBarConfig AppInfo
        {
            get
            {
                var cfgs = StatusBarConfigs;
                if (null == cfgs) return null;
                return cfgs.AppInfo;
            }
        }
        /// <summary>
        /// Gets ClientInfo Status Bar Config.
        /// </summary>
        public StatusBarConfig ClientInfo
        {
            get
            {
                var cfgs = StatusBarConfigs;
                if (null == cfgs) return null;
                return cfgs.ClientInfo;
            }
        }
        /// <summary>
        /// Gets RabbitMQ Status Bar Config.
        /// </summary>
        public StatusBarConfig RabbitMQ
        {
            get
            {
                var cfgs = StatusBarConfigs;
                if (null == cfgs) return null;
                return cfgs.RabbitMQ;
            }
        }
        /// <summary>
        /// Gets SCW Status Bar Config.
        /// </summary>
        public StatusBarConfig SCW
        {
            get
            {
                var cfgs = StatusBarConfigs;
                if (null == cfgs) return null;
                return cfgs.SCW;
            }
        }
        /// <summary>
        /// Gets LocalDb Status Bar Config.
        /// </summary>
        public StatusBarConfig LocalDb
        {
            get
            {
                var cfgs = StatusBarConfigs;
                if (null == cfgs) return null;
                return cfgs.LocalDb;
            }
        }
        /// <summary>
        /// Gets TAServer Status Bar Config.
        /// </summary>
        public StatusBarConfig TAServer
        {
            get
            {
                var cfgs = StatusBarConfigs;
                if (null == cfgs) return null;
                return cfgs.TAServer;
            }
        }

        #endregion
    }
}
