#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WebSocketSharp;

#endregion

namespace WebSocketClientTest
{
    public partial class Form1 : Form
    {
        #region Constructor

        public Form1()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private WebSocket ws = null;

        #endregion

        #region Form Load/Closing

        private void Form1_Load(object sender, EventArgs e)
        {
            cmdConnect.Enabled = true;
            cmdDisconnect.Enabled = true;
            cmdSend.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }

        #endregion

        #region Button Handlers

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void cmdDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void cmdSend_Click(object sender, EventArgs e)
        {
            SendMsg();
        }

        private void cmdClear1_Click(object sender, EventArgs e)
        {
            ClearSendMsg();
        }

        private void cmdClear2_Click(object sender, EventArgs e)
        {
            ClearRecvMsg();
        }

        private void cmdClear3_Click(object sender, EventArgs e)
        {
            ClearLogMsg();
        }

        #endregion

        #region WS Handlers

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            Log("WS OnOpen handler execute.");
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            // Cross thread wrapper.
            Invoke(new MethodInvoker(delegate () {
                string message = e.Data;
                Log(string.Format("WS OnMessage: {0}", message));
                txtRecv.Text = message;
            }));
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            //Reconnect(e.Code, e.Reason);
            Log(string.Format("WS OnClose : Code:{0}, Reason:{1}", e.Code, e.Reason));
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            Log(string.Format("WS OnError: {0}", e.Message));
        }

        #endregion

        #region Private Methods

        private void Connect()
        {
            if (null != ws)
            {
                Log("Already create web socket.");

                Log(string.Format("Ready State: {0}.", ws.ReadyState));
                if (ws.ReadyState != WebSocketState.Open)
                {
                    Log("Current connection is not in open state. Try to disconnect before send message.");
                }

                return;
            }
            else
            {
                string url = txtUrl.Text;
                if (string.IsNullOrWhiteSpace(url))
                {
                    MessageBox.Show("Please assign url.");
                    return;
                }

                Log("Create new web socket connection.");

                try
                {
                    ws = new WebSocket(url);
                    ws.OnOpen += Ws_OnOpen;
                    ws.OnMessage += Ws_OnMessage;
                    ws.OnError += Ws_OnError;
                    ws.OnClose += Ws_OnClose;
                    ws.Connect();
                }
                catch (Exception ex)
                {
                    Log(string.Format("WS Open error: {0}", ex.Message));
                }
            }

            cmdConnect.Enabled = (null == ws || ws.ReadyState != WebSocketState.Open);
            cmdDisconnect.Enabled = true;
            cmdSend.Enabled = !cmdConnect.Enabled;
        }

        private void Disconnect()
        {
            if (null != ws)
            {
                try
                {
                    ws.OnOpen -= Ws_OnOpen;
                    ws.OnMessage -= Ws_OnMessage;
                    ws.OnError -= Ws_OnError;
                    ws.OnClose -= Ws_OnClose;
                    ws.Close();
                }
                catch (Exception ex)
                {
                    Log(string.Format("WS Close error: {0}", ex.Message));
                }
                try 
                {
                    (ws as IDisposable).Dispose();
                }
                catch (Exception ex2) 
                {
                    Log(string.Format("Dispose error: {0}", ex2.Message));
                }
            }
            ws = null;

            Log("web socket connection is closed.");
        }

        private void SendMsg()
        {
            string msg = txtSend.Text;
            if (string.IsNullOrWhiteSpace(msg))
            {
                MessageBox.Show("No message to send.");
                return;
            }

            if (null == ws || ws.ReadyState != WebSocketState.Open)
            {
                MessageBox.Show("No Web socket connect.");
                return;
            }

            Log(string.Format("Sending Message: {0}", msg));
            ws.Send(msg);
        }

        private void ClearSendMsg()
        {
            txtSend.Text = string.Empty;
        }

        private void ClearRecvMsg()
        {
            txtRecv.Text = string.Empty;
        }

        private void ClearLogMsg()
        {
            txtLogs.Text = string.Empty;
        }

        private void Log(string msg)
        {
            string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", 
                System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string line = string.Format("{0} : {1}", dt, msg) + Environment.NewLine;
            txtLogs.AppendText(line);
        }

        #endregion
    }
}
