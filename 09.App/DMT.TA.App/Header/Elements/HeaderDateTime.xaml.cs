#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

//using NLib.Services;
using DMT.Configurations;
using DMT.Services;

#endregion

namespace DMT.Controls.Header
{
    using wsOps = Services.Operations.SCW.Security;

    /// <summary>
    /// Interaction logic for HeaderDateTime.xaml
    /// </summary>
    public partial class HeaderDateTime : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public HeaderDateTime()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private SolidColorBrush OnlineColor = new SolidColorBrush(Colors.Transparent);
        private SolidColorBrush OfflineColor = new SolidColorBrush(Colors.Maroon);

        private HeaderBarService service = HeaderBarService.Instance;

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
            CallWS();

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                borderDT.Background = (isOnline) ? OnlineColor : OfflineColor;
                DateTime dt = DateTime.Now;
                txtCurrentDate.Text = dt.ToThaiDateTimeString("dd/MM/yyyy");
                txtCurrentTime.Text = dt.ToThaiDateTimeString("HH:mm:ss");
            }));
        }
    }
}
