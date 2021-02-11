#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

using DMT.Configurations;
using DMT.Services;
using NLib;

#endregion

namespace DMT.Controls.StatusBar
{
    using wsOps = Services.Operations.SCW.Security;

    /// <summary>
    /// Interaction logic for SCWStatus.xaml
    /// </summary>
    public partial class SCWStatus : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SCWStatus()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private DateTime _lastUpdate = DateTime.MinValue;
        private DispatcherTimer timer = null;
        private bool isOnline = false;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CallWS();
            UpdateUI();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            AccountConfigManager.Instance.ConfigChanged += ConfigChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            AccountConfigManager.Instance.ConfigChanged -= ConfigChanged;

            if (null != timer)
            {
                timer.Tick -= timer_Tick;
                timer.Stop();
            }
            timer = null;
        }

        #endregion

        #region Timer Handler

        void timer_Tick(object sender, EventArgs e)
        {
            UpdateUI();
        }

        #endregion

        #region Config Watcher Handlers

        private void ConfigChanged(object sender, EventArgs e)
        {
            CallWS();
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
                return AccountConfigManager.Instance.Value.UIConfig.StatusBars.SCW;
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

        private void CallWS()
        {
            var ret = wsOps.passwordExpiresDays();
            isOnline = (null != ret && null != ret.status && 
                !string.IsNullOrEmpty(ret.status.code) && ret.status.code == "S200");
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

            if (isOnline)
            {
                borderStatus.Background = new SolidColorBrush(Colors.ForestGreen);
                txtStatus.Text = "Online";
            }
            else
            {
                borderStatus.Background = new SolidColorBrush(Colors.Maroon);
                txtStatus.Text = "Offline";
            }
        }
    }
}
