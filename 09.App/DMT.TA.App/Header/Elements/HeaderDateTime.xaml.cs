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

        private DispatcherTimer timer = null;
        private NLib.Components.PingManager ping = null;
        private bool isOnline = false;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string host = (null != TAConfigManager.Instance.SCW && null != TAConfigManager.Instance.SCW.Service) ?
                TAConfigManager.Instance.SCW.Service.HostName : "unknown";
            int interval = (null != Config) ? Config.IntervalSeconds : 5;
            if (interval < 0) interval = 5;

            ping = new NLib.Components.PingManager();
            ping.OnReply += Ping_OnReply;
            ping.Add(host);
            ping.Interval = interval * 1000;
            ping.Start();

            CallWS();
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
                // Call WS
                //CallWS();
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
                string host = (null != TAConfigManager.Instance.SCW && null != TAConfigManager.Instance.SCW.Service) ?
                    TAConfigManager.Instance.SCW.Service.HostName : "unknown";
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

        private void CallWS()
        {
            // Do not call because Statusbar is already called.
            /*
            var ret = wsOps.passwordExpiresDays();
            isOnline = (null != ret && null != ret.status &&
                !string.IsNullOrEmpty(ret.status.code) && ret.status.code == "S200");
            */
        }

        private void UpdateUI()
        {
            borderDT.Background = (isOnline) ? OnlineColor : OfflineColor;
            DateTime dt = DateTime.Now;
            txtCurrentDate.Text = dt.ToThaiDateTimeString("dd/MM/yyyy");
            txtCurrentTime.Text = dt.ToThaiDateTimeString("HH:mm:ss");
        }
    }
}
