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

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUI();
            TODConfigManager.Instance.ConfigChanged += ConfigChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            TODConfigManager.Instance.ConfigChanged -= ConfigChanged;
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
                if (null == TODConfigManager.Instance.Value ||
                    null == TODConfigManager.Instance.Value.UIConfig ||
                    null == TODConfigManager.Instance.Value.UIConfig.StatusBars) return null;
                return TODConfigManager.Instance.Value.UIConfig.StatusBars.ClientInfo;
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
