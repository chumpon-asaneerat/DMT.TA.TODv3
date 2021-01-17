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

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUI();
            AccountConfigManager.Instance.ConfigChanged += ConfigChanged;
            AccountUIConfigManager.Instance.ConfigChanged += UI_ConfigChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            AccountUIConfigManager.Instance.ConfigChanged -= UI_ConfigChanged;
            AccountConfigManager.Instance.ConfigChanged -= ConfigChanged;
        }

        #endregion

        #region Config Watcher Handlers

        private void ConfigChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UI_ConfigChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        #endregion

        private void UpdateUI()
        {
            var statusCfg = AccountUIConfigManager.Instance.ClientInfo;
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
