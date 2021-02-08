#region Using

using System;
using System.Windows;
using System.Windows.Controls;

using NLib;

using DMT.Configurations;
using DMT.Services;

#endregion

namespace DMT.Controls.StatusBar
{
    /// <summary>
    /// Interaction logic for AppInfoStatus.xaml
    /// </summary>
    public partial class AppInfoStatus : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public AppInfoStatus()
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
                return TODConfigManager.Instance.Value.UIConfig.StatusBars.AppInfo;
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

            txtAppInfo.Text = ApplicationManager.Instance.Environments.Options.AppInfo.DisplayText;
        }
    }
}
