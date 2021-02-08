#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

using DMT.Configurations;
using DMT.Services;

#endregion

namespace DMT.Controls.StatusBar
{
    /// <summary>
    /// Interaction logic for TODAppStatus.xaml
    /// </summary>
    public partial class TODAppStatus : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TODAppStatus()
        {
            InitializeComponent();
        }

        #endregion

        private DispatcherTimer timer = null;
        private NLib.Components.PingManager ping = null;
        private bool isOnline = false;

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO: Need Check TOD App Implements
            /*
            string host = (null != TAConfigManager.Instance.TODApp && null != TAConfigManager.Instance.TODApp.Service) ?
                TAConfigManager.Instance.TODApp.Service.HostName : "unknown";
            */
            string host = "127.0.0.1";
            int interval = (null != Config) ? Config.IntervalSeconds : 5;
            if (interval < 0) interval = 5;

            ping = new NLib.Components.PingManager();
            ping.OnReply += Ping_OnReply;
            ping.Add(host);
            ping.Interval = interval * 1000;
            ping.Start();

            UpdateUI();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            TAConfigManager.Instance.ConfigChanged += ConfigChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            TAConfigManager.Instance.ConfigChanged -= ConfigChanged;

            if (null != ping)
            {
                ping.OnReply -= Ping_OnReply;
                ping.Stop();
                ping.Dispose();
            }
            ping = null;

            if (null != timer)
            {
                timer.Tick -= timer_Tick;
                timer.Stop();
            }
            timer = null;
        }

        #endregion

        #region Ping Reply Handler

        private void Ping_OnReply(object sender, NLib.Networks.PingResponseEventArgs e)
        {
            if (null != e.Reply &&
                e.Reply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                isOnline = true;
            }
            else
            {
                isOnline = false;
            }
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
            if (null != ping)
            {
                // TODO: Need Check TOD App Implements
                /*
                string host = (null != TAConfigManager.Instance.TODApp && null != TAConfigManager.Instance.TODApp.Service) ?
                    TAConfigManager.Instance.TODApp.Service.HostName : "unknown";
                */
                string host = "127.0.0.1";
                int interval = (null != Config) ? Config.IntervalSeconds : 5;
                if (interval < 0) interval = 5;

                // Stop ping service.
                ping.Stop();
                ping.Interval = interval * 1000;
                // Clear and add new host.
                ping.Clear();
                ping.Add(host);
                // Restart ping service.
                ping.Start();
            }
            UpdateUI();
        }

        #endregion

        private StatusBarConfig Config
        {
            get
            {
                if (null == TAConfigManager.Instance.Value ||
                    null == TAConfigManager.Instance.Value.UIConfig ||
                    null == TAConfigManager.Instance.Value.UIConfig.StatusBars) return null;
                return TAConfigManager.Instance.Value.UIConfig.StatusBars.TODApp;
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
