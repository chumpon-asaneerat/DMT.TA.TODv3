#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
//using System.Windows.Forms;
//using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Reflection;
using System.Globalization;

using DMT.Configurations;
using DMT.Controls;
using DMT.Services;
using DMT.Models;
using DMT.Models.ExtensionMethods;

using NLib;
using NLib.IO;
using NLib.Services;
using NLib.Reflection;

using RestSharp;

using WebSocketSharp;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

#endregion

namespace DMT.Services
{
    #region SupAdjClient

    /// <summary>
    /// The SupAdj Client.
    /// </summary>
    public class SupAdjClient
    {
        #region Static Variables

        /// <summary>Set default timeout in second. default is 15 s.</summary>
        public static int TimeoutInSecond = 15;

        #endregion

        #region Internal Variables

        private int iRetry = 0;
        private bool _reconnect = false;
        private WebSocket ws = null;

        private DateTime _sendMsgTime = DateTime.MinValue;
        private int _sendCnt = 0;
        private int _recvCnt = 0;

        private Models.User _user = null;
        private int? adjCnt = new int?();
        private bool isEnabled = true;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SupAdjClient() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~SupAdjClient()
        {
            Disconnect(false);
        }

        #endregion

        #region Private Methods

        private static JsonSerializerSettings DefaultSettings = new JsonSerializerSettings()
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            //DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK"
            //DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK"
            //DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFK"
            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK"
        };

        #region WS Handlers

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Info("SUPADJ - WS OnOpen detected.");
        }

        private void Ws_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            // increase received counter
            _recvCnt++;

            if (null == _user)
            {
                med.Info("SUPADJ - WS OnMessage: No user assigned.");
            }

            string json = e.Data;
            adjCnt = new int?(); // Reset

            med.Info(string.Format("SUPADJ - RECV: {0}", json));

            // {"jsonrpc":"2.0","method":"TOD_adjustSizeResponse","staffId":123456,"adjustSize":0}
            try
            {
                //File.WriteAllText(@"C:\Users\chump\Desktop\resp.json", json); // write file on desktop.
                JObject jobj = JObject.Parse(json);

                string ver = jobj.Property("jsonrpc").HasValues ?
                    jobj.Property("jsonrpc").Value.ToString() : string.Empty;
                string method = jobj.Property("method").HasValues ?
                    jobj.Property("method").Value.ToString() : string.Empty;
                if (ver != "2.0" || method != "TOD_adjustSizeResponse")
                {
                    med.Info("SUPADJ - WS OnMessage: Invalid version or method.");
                    med.Info(string.Format("     jsonrpc: {0}.", ver));
                    med.Info(string.Format("     method: {0}.", method));
                    return;
                }
                int? staffId = new int?();
                if (jobj.Property("staffId").HasValues)
                {
                    int id;
                    if (int.TryParse(jobj.Property("staffId").Value.ToString(), out id))
                    {
                        staffId = id;
                        med.Info(string.Format("     staffId: {0}.", id));
                    }
                    else
                    {
                        med.Info(string.Format("     staffId: {0} -> parse error.", id));
                    }
                }

                if (!staffId.HasValue)
                {
                    med.Info("SUPADJ - WS OnMessage: staffId is null or invalid.");
                    return;
                }

                if (jobj.Property("adjustSize").HasValues)
                {
                    int sz;
                    if (int.TryParse(jobj.Property("adjustSize").Value.ToString(), out sz))
                    {
                        adjCnt = new int?(sz);
                        med.Info(string.Format("     adjustSize: {0}.", sz));
                    }
                    else
                    {
                        med.Info(string.Format("     adjustSize: {0} -> parse error.", sz));
                    }
                }

                if (!adjCnt.HasValue)
                {
                    med.Info("SUPADJ - WS OnMessage: adjustSize is null or invalid.");
                    return;
                }

            }
            catch (Exception ex)
            {
                med.Err(string.Format("SUPADJ - WS OnMessage Error: {0}", ex.Message));
            }
        }

        private void Ws_OnClose(object sender, WebSocketSharp.CloseEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (_reconnect)
            {
                Reconnect(e.Code, e.Reason);
            }
            else
            {
                med.Info(string.Format("SUPADJ - WS OnClose : Code:{0}, Reason:{1}", e.Code, e.Reason));
            }
        }

        private void Ws_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            med.Err(string.Format("SUPADJ - WS OnError: {0}", e.Message));
            Disconnect(true);
        }

        #endregion

        #endregion

        #region Public Method

        #region Connect/Reconnect/Disconnect/Send

        /// <summary>
        /// Connect
        /// </summary>
        public void Connect()
        {
            // reset counter.
            _sendCnt = 0;
            _recvCnt = 0;

            MethodBase med = MethodBase.GetCurrentMethod();

            if (null != ws)
            {
                med.Info("SUPADJ - Already create web socket.");
                Disconnect(false);
            }

            iRetry = 0; // reset retry,

            PlazaSupAdjConfigManager.Instance.LoadConfig(); // reload.
            var cfg = Configurations.PlazaSupAdjConfigManager.Instance.SupAdj;
            if (null == cfg)
            {
                med.Err("SUPADJ - Config is null.");
                return;
            }

            SupAdjClient.TimeoutInSecond = cfg.TimeoutInSeconds;

            isEnabled = cfg.Enabled;
            if (!isEnabled)
            {
                med.Err("SUPADJ - Connection is disable by config.");
                return;
            }

            string url = string.Format("{0}://{1}:{2}", cfg.Protocol, cfg.HostName, cfg.PortNumber); ;
            if (string.IsNullOrWhiteSpace(url))
            {
                med.Err(string.Format("SUPADJ - Invalid Url. Url: {0}", url));
                return;
            }

            med.Info("SUPADJ - Create new web socket connection.");

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
                med.Err(string.Format("SUPADJ - WS Open error: {0}", ex.Message));
            }
        }
        /// <summary>
        /// Disconnect.
        /// </summary>
        public void Disconnect()
        {
            Disconnect(false);
        }

        private void Disconnect(bool reconnect)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            _reconnect = reconnect; // update flag.

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
                    med.Err(ex);
                }
                try
                {
                    (ws as IDisposable).Dispose();
                }
                catch (Exception)
                {
                    //med.Err(ex2);
                }
            }
            ws = null;

            if (!reconnect)
            {
                med.Info("SUPADJ - successfully disconnected.");
            }

            _reconnect = false; // reset flag.
        }

        private void Reconnect(ushort code, string error)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            string msg = string.Format("SUPADJ - Reconnect: last status {0}, {1}", code, error);
            med.Info(msg);

            if (!isEnabled)
            {
                med.Info("SUPADJ - Re-conenct to ws is disable by config.");
                return;
            }

            if (iRetry >= 3)
            {
                med.Info("SUPADJ - Re-conenct to ws server failed.");
                Disconnect(false); // force disconnect.
                return;
            }

            iRetry++;
            med.Info(string.Format("SUPADJ - Retry {0}.", iRetry));
            if (null != ws && code != (ushort)CloseStatusCode.Normal)
            {
                ws.Connect();
            }
            else
            {
                med.Info(string.Format("SUPADJ - Retry failed close status: {0} or ws is null.", code));
            }
        }
        /// <summary>
        /// Send.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="jobBegin"></param>
        public void Send(Models.User user, DateTime jobBegin)
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null == ws || ws.ReadyState != WebSocketState.Open)
            {
                med.Info("SUPADJ - SEND: No websocket connection. The connection is null or connection state is not ready.");
                return;
            }
            if (null == user)
            {
                med.Info("SUPADJ - SEND: No User assign.");
                return;
            }

            _user = user;
            //{"jsonrpc":"2.0","method":"TOD_adjustSizeRequest","staffId":123456,"staffName":"สมชาย","staffLastName":"เข็มขัด","bojDateTime":"2021-05-31T22:55:31.000Z"}
            var jobj = new
            {
                jsonrpc = "2.0",
                method = "TOD_adjustSizeRequest",
                staffId = Convert.ToInt32(user.UserId),
                staffName = user.FirstNameTH,
                staffLastName = user.LastNameTH,
                bojDateTime = new DateTime(jobBegin.Ticks, DateTimeKind.Local) // mark as Local so when convert will substract with -7:00
                //bojDateTime = new DateTime(jobBegin.Ticks, DateTimeKind.Utc) // mark as UTC without substract timezone
            };


            string json = JsonConvert.SerializeObject(jobj, SupAdjClient.DefaultSettings);

            //File.WriteAllText(@"C:\Users\chump\Desktop\ws.json", json); // write file on desktop.

            med.Info(string.Format("SUPADJ - SEND: {0}", json));
            med.Info(string.Format("     jsonrpc: {0}", jobj.jsonrpc));
            med.Info(string.Format("     method: {0}", jobj.method));
            med.Info(string.Format("     staffId: {0}", jobj.staffId));
            med.Info(string.Format("     staffName: {0}", jobj.staffName));
            med.Info(string.Format("     staffLastName: {0}", jobj.staffLastName));
            med.Info(string.Format("     bojDateTime: {0:yyyy-MM-dd HH:mm:ss.fff}", jobj.bojDateTime));

            adjCnt = new int?(); // reset count.
            try
            {
                // increase send counter.
                _sendCnt++;
                // update time.
                _sendMsgTime = DateTime.Now;

                ws.Send(json); // sending
            }
            catch (Exception ex)
            {
                med.Err(string.Format("SUPADJ - SEND Error: {0}", ex.Message));
            }
        }

        #endregion

        #region HasAdjustEvents

        /// <summary>
        /// Checks has adjust event.
        /// </summary>
        /// <returns>Returns true if has one or more adjust events.</returns>
        public bool HasAdjustEvents()
        {
            bool ret = false;
            if (isEnabled)
            {
                ret = adjCnt.HasValue && adjCnt.Value > 0;
            }
            return ret;
        }
        /// <summary>
        /// Gets Adjust Count.
        /// </summary>
        public int AdjustCount
        {
            get
            {
                int ret = 0;
                if (isEnabled && adjCnt.HasValue)
                {
                    ret = adjCnt.Value;
                }
                return ret;
            }
        }

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        /// Checks is connected.
        /// </summary>
        public bool Connected
        {
            get { return (null != ws && ws.ReadyState == WebSocketState.Open); }
        }
        /// <summary>
        /// Checks is timeout after send message.
        /// </summary>
        public bool IsTimeout
        {
            get 
            {
                bool ret = false;
                if (_sendMsgTime != DateTime.MinValue)
                {
                    // Message send.
                    TimeSpan ts = DateTime.Now - _sendMsgTime;
                    if (ts.TotalSeconds > SupAdjClient.TimeoutInSecond)
                    {
                        // Timeout.
                        ret = true;
                    }
                }
                return ret;
            }
        }
        /// <summary>
        /// Gets number of send message to server.
        /// </summary>
        public int SendCount { get { return _sendCnt; } }
        /// <summary>
        /// Gets number of receive message from server.
        /// </summary>
        public int RecvCount { get { return _recvCnt; } }
        /// <summary>
        /// Checks is number of send equals to recv counter.
        /// </summary>
        public bool AllAck
        {
            get { return _sendCnt > 0 && _recvCnt == _sendCnt; }
        }

        #endregion

        #region Public Properties (static)

        /// <summary>
        /// Check is Enable (by config).
        /// </summary>
        public static bool Enabled
        {
            get 
            {
                PlazaSupAdjConfigManager.Instance.LoadConfig(); // reload.
                var cfg = Configurations.PlazaSupAdjConfigManager.Instance.SupAdj;
                if (null == cfg)
                {
                    return false;
                }

                return cfg.Enabled;
            }
        }

        #endregion
    }

    #endregion
}
