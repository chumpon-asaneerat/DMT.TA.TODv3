#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

//using NLib.Services;
//using DMT.Services;

#endregion

namespace DMT.Controls.Header
{
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

        private DispatcherTimer timer = null;
        private NLib.Components.PingManager ping = null;

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: Required to load SCW host, user, pwd from Config Manager.
            string host = @"www.google.com2";
            //string host = ConfigManager.Instance.Plaza.SCW.Http.HostName;

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
                borderDT.Background = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                borderDT.Background = new SolidColorBrush(Colors.Maroon);
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
            DateTime dt = DateTime.Now;
            txtCurrentDate.Text = dt.ToThaiDateTimeString("dd/MM/yyyy");
            txtCurrentTime.Text = dt.ToThaiDateTimeString("HH:mm:ss");
        }
    }
}
