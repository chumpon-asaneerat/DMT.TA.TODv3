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
    using taops = Services.Operations.TA;

    /// <summary>
    /// Interaction logic for TAAppStatus.xaml
    /// </summary>
    public partial class TAAppStatus : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TAAppStatus()
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
                int interval = (null != service.TAApp) ? service.TAApp.IntervalSeconds : 5;
                if (interval < 0) interval = 5;
                return interval;
            }
        }

        private void CallWS()
        {
            var ret = taops.Notify.IsAlive();
            isOnline = (null != ret && ret.Ok);
        }

        private void UpdateUI()
        {
            CallWS();

            var statusCfg = service.TAApp;
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
