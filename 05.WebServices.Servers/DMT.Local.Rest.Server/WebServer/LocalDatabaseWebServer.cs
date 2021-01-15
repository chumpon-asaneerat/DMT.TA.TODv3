#region Using

using System;
using System.Reflection;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using NLib.Reflection;

#endregion

namespace DMT.Services
{
    /// <summary>
    /// Web Server StartUp class.
    /// </summary>
    public class StartUp : DMTRestServerStartUp
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public StartUp() : base()
        {
            this.AuthenticationValidator = (string userName, string password) =>
            {
                var svr = (null != PlazaServiceConfigManager.Instance.Plaza &&
                           null != PlazaServiceConfigManager.Instance.Plaza.Service) ?
                    PlazaServiceConfigManager.Instance.Plaza.Service : null;
                if (null != svr)
                {
                    return (userName == svr.UserName && password == Models.Utils.MD5.Encrypt(svr.Password));
                }
                else
                {
                    return true;
                }
            };
            this.EnableSwagger = true;
            this.ApiName = "TOD&TA Local Server API";
            this.ApiVersion = "v1";
        }

        #endregion
    }

    /// <summary>
    /// Local Database Web Server (Self Host).
    /// </summary>
    public class LocalDatabaseWebServer
    {
        #region Internal Variables

        private WebServiceConfig _cfg = null;
        private IDisposable server = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalDatabaseWebServer() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~LocalDatabaseWebServer()
        {
            Shutdown();
        }

        #endregion

        #region Private Methods

        private void CheckConfig()
        {
            // Gets Plaz local service config.
            _cfg = (null != PlazaServiceConfigManager.Instance.Plaza) ? 
                PlazaServiceConfigManager.Instance.Plaza.Service : null;
        }

        private string BaseAddress
        {
            get
            {
                string result = string.Empty;
                if (null != _cfg)
                {
                    result = string.Format(@"{0}://{1}:{2}", _cfg.Protocol, "+", _cfg.PortNumber);
                }
                return result;
            }
        }

        private void InitOwinFirewall()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null == _cfg)
            {
                med.Err("Server Configuration is null.");
                return;
            }
            string portNum = _cfg.PortNumber.ToString();
            string appName = "DMT Plaza Local Service (REST)";
            var nash = new CommandLine();
            nash.Run("http add urlacl url=http://+:" + portNum + "/ user=Everyone");
            nash.Run("advfirewall firewall add rule dir=in action=allow protocol=TCP localport=" + portNum + " name=\"" + appName  + "\" enable=yes profile=Any");
        }

        private void ReleaseOwinFirewall()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null == _cfg)
            {
                med.Err("Server Configuration is null.");
                return;
            }
            string portNum = _cfg.PortNumber.ToString();
            string appName = "DMT Plaza Local Service (REST)";
            var nash = new CommandLine();
            nash.Run("http delete urlacl url=http://+:" + portNum + "/");
            nash.Run("advfirewall firewall delete rule name=\"" + appName + "\"");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start service.
        /// </summary>
        public void Start()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            CheckConfig(); // Check Config.
            if (null == _cfg)
            {
                med.Err("Server Configuration is null.");
                return;
            }
            // Setup config reference to all rest client class.
            // SCW
            Operations.SCW.Config = PlazaServiceConfigManager.Instance;
            Operations.SCW.DMT = PlazaServiceConfigManager.Instance; // required for NetworkId
            // TA
            Operations.TA.Config = PlazaServiceConfigManager.Instance;
            Operations.TA.DMT = PlazaServiceConfigManager.Instance; // required for NetworkId
            // TAxTOD
            Operations.TAxTOD.Config = PlazaServiceConfigManager.Instance;
            Operations.TAxTOD.DMT = PlazaServiceConfigManager.Instance; // required for NetworkId
            // TOD
            Operations.TOD.Config = PlazaServiceConfigManager.Instance;
            Operations.TOD.DMT = PlazaServiceConfigManager.Instance; // required for NetworkId

            // Start database server.
            LocalDbServer.Instance.Start();
            if (LocalDbServer.Instance.Connected)
            {
                med.Info("Plaza local database connected.");
            }
            else
            {
                med.Info("Plaza local database connect failed.");
            }

            // Start Local Web Service.
            if (null == server)
            {
                InitOwinFirewall();
                server = WebApp.Start<StartUp>(url: BaseAddress);
                med.Info("Plaza local web service started.");
            }
            else
            {
                med.Info("Plaza local web service failed.");
            }

            // Start SCWMQ service.
            SCWMQService.Instance.Start();
            med.Info("SCWMQ Service start.");

            // Start rabbit service.
            RabbitMQService.Instance.RabbitMQ = PlazaServiceConfigManager.Instance.RabbitMQ;
            RabbitMQService.Instance.Start();
            if (RabbitMQService.Instance.Connected)
            {
                med.Info("RabbitMQ Client service connected.");
            }
            else
            {
                med.Info("RabbitMQ Client service connect failed.");
            }
        }
        /// <summary>
        /// Shutdown service.
        /// </summary>
        public void Shutdown()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            // Shutdown Rabbit MQ Service.
            RabbitMQService.Instance.Shutdown();
            med.Info("RabbitMQ Client service disconnected.");

            // Shutdown SCWMQ service.
            SCWMQService.Instance.Shutdown();
            med.Info("SCWMQ Service shutdown.");

            // Shutdown Local Web Service.
            if (null != server)
            {
                server.Dispose();
            }
            server = null;
            ReleaseOwinFirewall();
            med.Info("Plaza local web service shutdown.");

            // Shutdown database server.
            LocalDbServer.Instance.Shutdown();
            med.Info("Plaza local database disconnected.");
        }

        #endregion
    }
}
