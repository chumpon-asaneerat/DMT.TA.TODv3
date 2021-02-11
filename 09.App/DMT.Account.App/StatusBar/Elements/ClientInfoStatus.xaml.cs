#region Using

using System;
using System.Windows;
using System.Windows.Controls;

using NLib.Utils;

using DMT.Configurations;
using DMT.Services;

#endregion

namespace DMT.Controls.StatusBar
{
    /// <summary>
    /// Interaction logic for ClientInfoStatus.xaml
    /// </summary>
    public partial class ClientInfoStatus : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ClientInfoStatus()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private DateTime _lastUpdate = DateTime.MinValue;

        #endregion

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUI();
            AccountConfigManager.Instance.ConfigChanged += ConfigChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            AccountConfigManager.Instance.ConfigChanged -= ConfigChanged;
        }

        #endregion

        #region Config Watcher Handlers

        private void ConfigChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        #endregion

        private StatusBarConfig Config
        {
            get
            {
                if (null == AccountConfigManager.Instance.Value ||
                    null == AccountConfigManager.Instance.Value.UIConfig ||
                    null == AccountConfigManager.Instance.Value.UIConfig.StatusBars) return null;
                return AccountConfigManager.Instance.Value.UIConfig.StatusBars.ClientInfo;
            }
        }

        private int Interval
        {
            get
            {
                int interval = (null != Config) ? Config.IntervalSeconds : 5;
                if (interval < 0) interval = 5;
                return interval;
            }
        }

        private void UpdateUI()
        {
            var statusCfg = Config;
            if (null == statusCfg || !statusCfg.Visible)
            {
                // Hide Control.
                if (this.Visibility == Visibility.Visible) this.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Show Control.
                if (this.Visibility != Visibility.Visible) this.Visibility = Visibility.Visible;
            }

            var ipaddr = NetworkUtils.GetLocalIPAddress();
            txtStatus.Text = (null != ipaddr) ? ipaddr.ToString() : "0.0.0.0";
        }
    }
}
