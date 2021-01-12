#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

using DMT.Services;

#endregion

namespace DMT.Controls.StatusBar
{
    /// <summary>
    /// Interaction logic for LocalPlazaStatus.xaml
    /// </summary>
    public partial class LocalPlazaStatus : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalPlazaStatus()
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
            string host = (null != TAConfigManager.Instance.Plaza && null != TAConfigManager.Instance.Plaza.Service) ?
                TAConfigManager.Instance.Plaza.Service.HostName : "unknown";

            ping = new NLib.Components.PingManager();
            ping.OnReply += Ping_OnReply;
            ping.Add(host);
            ping.Interval = 1000;
            ping.Start();

            UpdateUI();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
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

        private void UpdateUI()
        {
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
