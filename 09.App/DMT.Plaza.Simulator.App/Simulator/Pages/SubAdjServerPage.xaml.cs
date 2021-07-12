#region Using

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

#endregion

namespace DMT.Simulator.Pages
{
    /// <summary>
    /// Interaction logic for SubAdjServerPage.xaml
    /// </summary>
    public partial class SubAdjServerPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SubAdjServerPage()
        {
            InitializeComponent();
            Application.Current.Exit += Current_Exit; // make sure stop server when app is exit.
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            Shutdown();
        }

        #endregion

        #region Internal Variables

        private WebSocketServer wssv = null;

        #endregion

        #region Private Methods

        private void Start()
        {
            var cfg = Configurations.PlazaSupAdjConfigManager.Instance.SupAdj;
            if (null == cfg)
            {
                return;
            }

            if (null != wssv) Shutdown();

            string wsurl = string.Format("{0}://{1}:{2}", cfg.Protocol, cfg.HostName, cfg.PortNumber);
            txtUrl.Text = wsurl;

            wssv = new WebSocketServer(wsurl);
            wssv.AddWebSocketService<SubAdj>("/", subadj => subadj.Setup(LogMessage, OnRequest));
            wssv.Start();

            cmdStart.IsEnabled = false;
            cmdShutdown.IsEnabled = true;
        }

        private void Shutdown()
        {
            if (null != wssv)
            {
                wssv.Stop();
            }
            wssv = null;

            cmdStart.IsEnabled = true;
            cmdShutdown.IsEnabled = false;
        }

        private void LogMessage(string msg)
        {
            // Cross thread wrapper.
            Dispatcher.Invoke(() =>
            {
                string message = string.Empty;
                message += DateTime.Now.ToString("HH:mm:ss: ");
                message += msg;
                message += Environment.NewLine;
                txtMessages.Text += message;
            });
        }

        private string OnRequest(string request)
        {
            string ret = string.Empty;
            JObject obj = JObject.Parse(request);
            if (null != obj && obj.Property("method").HasValues &&
                obj.Property("method").Value.ToString() == "TOD_adjustSizeRequest")
            {
                Dispatcher.Invoke(() => { ret = txtAnswer.Text; });
            }

            return ret;
        }

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtAnswer.Text = @"{""jsonrpc"":""2.0"",""method"":""TOD_adjustSizeResponse"",""staffId"":123456,""adjustSize"":0}";
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Shutdown();
        }

        #endregion

        #region Button Handlers

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void cmdShutdown_Click(object sender, RoutedEventArgs e)
        {
            Shutdown();
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            Shutdown();
        }

        #endregion
    }

    public class SubAdj : WebSocketBehavior
    {
        Func<string, string> _req = null;
        private Action<string> _action = null;

        public SubAdj() : base()
        {
        }

        public void Setup(Action<string> action, Func<string, string> req)
        {
            _action = action;
            _req = req;
            LogMessage("New Behavior init.");
        }

        private void LogMessage(string msg)
        {
            if (null != _action)
            {
                _action(msg);
            }
        }

        private string OnRequest(string request)
        {
            if (null != _req)
            {
                return _req(request);
            }
            else return request;
        }

        protected override void OnOpen()
        {
            LogMessage("detected connect by client.");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            LogMessage("detected disconnect by client.");
            try
            {
                Sessions.Broadcast("detected disconnect by client.");
            }
            catch (Exception ex) 
            {
                LogMessage(string.Format("detected disconnect by client error: {0}.", ex.Message));
            }
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            try
            {
                LogMessage(string.Format("detected request by client data: {0}.", e.Data));
                string ret = OnRequest(e.Data);
                LogMessage(string.Format("server response to client ack: {0}.", ret));
                Sessions.Broadcast(ret);
            }
            catch (Exception ex) 
            {
                LogMessage(string.Format("detected request by client error: {0}.", ex.Message));
            }
        }
    }
}
