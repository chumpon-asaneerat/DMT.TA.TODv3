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

        private StatusBarService service = StatusBarService.Instance;

        private DateTime _lastUpdate = DateTime.MinValue;
        private DispatcherTimer timer = null;
        private bool needCallWs = false;
        private bool isOnline = false;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            needCallWs = true;
            UpdateUI();

            if (null != service) service.Register(this.ForceUpdateUI);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (null != service) service.Unregister(this.ForceUpdateUI);

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
                needCallWs = true;
                _lastUpdate = DateTime.Now;
            }
            else
            {
                needCallWs = false;
            }
            UpdateUI();
        }

        #endregion

        private int Interval
        {
            get
            {
                int interval = (null != service && null != service.SCW) ? service.SCW.IntervalSeconds : 5;
                if (interval < 0) interval = 5;
                return interval;
            }
        }

        private void CallWS()
        {
            if (!needCallWs) return;
            var ret = wsOps.passwordExpiresDays();
            isOnline = (null != ret && null != ret.status &&
                !string.IsNullOrEmpty(ret.status.code) && ret.status.code == "S200");
            needCallWs = false;
        }

        private void ForceUpdateUI()
        {
            needCallWs = true;
            UpdateUI();
        }

        private void UpdateUI()
        {
            var statusCfg = (null != service) ? service.SCW : null;

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
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

                CallWS();

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

                // Check has message remain in folder.
                if (SCWMQService.Instance.TotalCount > 0)
                {
                    msginfo.Visibility = Visibility.Visible;
                    txtCnt.Text = SCWMQService.Instance.TotalCount.ToString("n0");
                }
                else
                {
                    msginfo.Visibility = Visibility.Collapsed;
                    txtCnt.Text = "";
                }
            }));
        }
    }
}
