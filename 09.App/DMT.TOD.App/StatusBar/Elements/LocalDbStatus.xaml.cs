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

        #region Internal Variables

        private StatusBarService service = StatusBarService.Instance;

        private DateTime _lastUpdate = DateTime.MinValue;
        private DispatcherTimer timer = null;
        private bool isOnline = false;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUI();

            if (null != service) service.Register(this.UpdateUI);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (null != service) service.Unregister(this.UpdateUI);

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
            TimeSpan ts = DateTime.Now - _lastUpdate;
            if (ts.TotalSeconds > this.Interval)
            {
                UpdateUI();
                _lastUpdate = DateTime.Now;
            }
        }

        #endregion

        private int Interval
        {
            get
            {
                int interval = (null != service.LocalDb) ? service.LocalDb.IntervalSeconds : 5;
                if (interval < 0) interval = 5;
                return interval;
            }
        }

        private void UpdateUI()
        {
            var statusCfg = service.LocalDb;
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

            isOnline = TODLocalDbServer.Instance.Connected;
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
