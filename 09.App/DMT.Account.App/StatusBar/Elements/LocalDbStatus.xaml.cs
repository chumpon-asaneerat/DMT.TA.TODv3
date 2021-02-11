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
    /// <summary>
    /// Interaction logic for LocalDbStatus.xaml
    /// </summary>
    public partial class LocalDbStatus : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalDbStatus()
        {
            InitializeComponent();
        }

        #endregion

        //private DispatcherTimer timer = null;
        private DateTime _lastUpdate = DateTime.MinValue;
        private int _interval = 1 * 1000;

        private bool isOnline = false;

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUI();

            ApplicationManager.Instance.Tick += Instance_Tick;
            /*
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            */

            AccountConfigManager.Instance.ConfigChanged += ConfigChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            AccountConfigManager.Instance.ConfigChanged -= ConfigChanged;

            ApplicationManager.Instance.Tick -= Instance_Tick;
            /*
            if (null != timer)
            {
                timer.Tick -= timer_Tick;
                timer.Stop();
            }
            timer = null;
            */
        }

        #endregion

        #region Timer Handler

        void timer_Tick(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void Instance_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - _lastUpdate;
            if (ts.TotalMilliseconds > _interval)
            {
                Console.WriteLine("Local Db Begin: {0:HH:mm:ss.fff}", DateTime.Now);
                UpdateUI();
                Console.WriteLine("Local Db End: {0:HH:mm:ss.fff}", DateTime.Now);
                _lastUpdate = DateTime.Now;
            }
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
                return AccountConfigManager.Instance.Value.UIConfig.StatusBars.LocalDb;
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

            isOnline = AccountDbServer.Instance.Connected;
            if (isOnline)
            {
                borderStatus.Background = new SolidColorBrush(Colors.ForestGreen);
                txtStatus.Text = "Connected";
            }
            else
            {
                borderStatus.Background = new SolidColorBrush(Colors.Maroon);
                txtStatus.Text = "Disconnected";
            }
        }
    }
}
