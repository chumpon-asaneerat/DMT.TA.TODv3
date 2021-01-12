#region Using

using System;
using System.Reflection;
using Microsoft.Owin.Hosting;
using System.Web.Http;
using NLib;

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
                var svr = (null != TAConfigManager.Instance.TAApp &&
                           null != TAConfigManager.Instance.TAApp.Service) ?
                    TAConfigManager.Instance.TAApp.Service : null;
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
            this.ApiName = "TA Application API";
            this.ApiVersion = "v1";
        }

        #endregion

        internal static class MapControllers
        {
            internal static class Notify
            {
                internal static void MapRoutes(HttpConfiguration config)
                {
                    string controllerName, actionName, actionUrl;

                    // Set Controller Name.
                    controllerName = RouteConsts.TA.Notify.ControllerName;

                    // TSB Changed
                    actionName = RouteConsts.TA.Notify.TSBChanged.Name;
                    actionUrl = RouteConsts.TA.Notify.TSBChanged.Url;
                    Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                    // Shift Changed
                    actionName = RouteConsts.TA.Notify.ShiftChanged.Name;
                    actionUrl = RouteConsts.TA.Notify.ShiftChanged.Url;
                    Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                }
            }
        }

        #region Override Methods

        /// <summary>
        /// Init Map Routes.
        /// </summary>
        /// <param name="config">The HttpConfiguration instance.</param>
        protected override void InitMapRoutes(HttpConfiguration config)
        {
            // Handle route by specificed controller (Route Order is important).

            // Notify
            MapControllers.Notify.MapRoutes(config);

            #region Default Route (do not used)

            // If comment below line the auto map default controllers will not load and cannot access.
            //InitDefaultMapRoute(config);

            #endregion
        }

        #endregion
    }

    /// <summary>
    /// TA WebServer Web Server (Self Host).
    /// </summary>
    public class TAWebServer
    {
        #region Internal Variables

        private WebServiceConfig _cfg = null;
        private IDisposable server = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TAWebServer() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TAWebServer()
        {
            Shutdown();
        }

        #endregion

        #region Private Methods

        private void CheckConfig()
        {
            // Gets TA App local server config.
            _cfg = (null != TAConfigManager.Instance.TAApp) ? TAConfigManager.Instance.TAApp.Service : null;
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
            string appName = "DMT TA App Service (REST)";
            var nash = new CommandLine();
            nash.Run("http add urlacl url=http://+:" + portNum + "/ user=Everyone");
            nash.Run("advfirewall firewall add rule dir=in action=allow protocol=TCP localport=" + portNum + " name=\"" + appName + "\" enable=yes profile=Any");
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
            string appName = "DMT TA App Service (REST)";
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
            if (null == server)
            {
                InitOwinFirewall();
                server = WebApp.Start<StartUp>(url: BaseAddress);
                med.Info("TA App local nofify service started.");
            }
            else
            { 
                med.Info("TA App local nofify service failed."); 
            }
            
        }
        /// <summary>
        /// Shutdown service.
        /// </summary>
        public void Shutdown()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null != server)
            {
                server.Dispose();
            }
            server = null;

            ReleaseOwinFirewall();
            med.Info("TA App local nofify service shutdown.");
        }

        #endregion
    }
}
