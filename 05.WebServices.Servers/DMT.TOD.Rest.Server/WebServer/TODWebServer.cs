#region Using

using System;
using System.Reflection;
using System.Web.Http;
using Microsoft.Owin.Hosting;

using NLib;
using NLib.Reflection;

using DMT.Configurations;
using DMT.Services;

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
                var svr = (null != TODConfigManager.Instance.TODApp &&
                           null != TODConfigManager.Instance.TODApp.Service) ?
                    TODConfigManager.Instance.TODApp.Service : null;
                if (null != svr)
                {
                    //return (userName == svr.UserName && password == svr.Password);
                    return (userName == svr.UserName && password == Models.Utils.MD5.Encrypt(svr.Password));
                }
                else
                {
                    return true;
                }
            };
            this.EnableSwagger = true;
            this.ApiName = "TOD Application API";
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
                    controllerName = RouteConsts.TOD.Notify.ControllerName;

                    // TSB Changed
                    actionName = RouteConsts.TOD.Notify.TSBChanged.Name;
                    actionUrl = RouteConsts.TOD.Notify.TSBChanged.Url;
                    Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                    // Shift Changed
                    actionName = RouteConsts.TOD.Notify.ShiftChanged.Name;
                    actionUrl = RouteConsts.TOD.Notify.ShiftChanged.Url;
                    Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                }
            }

            internal static class Infrastructure
            {
                internal static class TSB
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TOD.Infrastructure.TSB.ControllerName;

                        // Gets
                        actionName = RouteConsts.TOD.Infrastructure.TSB.Gets.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.TSB.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Current
                        actionName = RouteConsts.TOD.Infrastructure.TSB.Current.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.TSB.Current.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // SetActive
                        actionName = RouteConsts.TOD.Infrastructure.TSB.SetActive.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.TSB.SetActive.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.TOD.Infrastructure.TSB.Save.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.TSB.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }

                internal static class PlazaGroup
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TOD.Infrastructure.PlazaGroup.ControllerName;

                        // Gets
                        actionName = RouteConsts.TOD.Infrastructure.PlazaGroup.Gets.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.PlazaGroup.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.TOD.Infrastructure.PlazaGroup.Save.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.PlazaGroup.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByTSB
                        actionName = RouteConsts.TOD.Infrastructure.PlazaGroup.Search.ByTSB.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.PlazaGroup.Search.ByTSB.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }

                internal static class Plaza
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TOD.Infrastructure.Plaza.ControllerName;

                        // Gets
                        actionName = RouteConsts.TOD.Infrastructure.Plaza.Gets.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.Plaza.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.TOD.Infrastructure.Plaza.Save.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.Plaza.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByTSB
                        actionName = RouteConsts.TOD.Infrastructure.Plaza.Search.ByTSB.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.Plaza.Search.ByTSB.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByPlazaGroup
                        actionName = RouteConsts.TOD.Infrastructure.Plaza.Search.ByPlazaGroup.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.Plaza.Search.ByPlazaGroup.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }

                internal static class Lane
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TOD.Infrastructure.Lane.ControllerName;

                        // Gets
                        actionName = RouteConsts.TOD.Infrastructure.Lane.Gets.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.Lane.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.TOD.Infrastructure.Lane.Save.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.Lane.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByTSB
                        actionName = RouteConsts.TOD.Infrastructure.Lane.Search.ByTSB.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.Lane.Search.ByTSB.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByPlazaGroup
                        actionName = RouteConsts.TOD.Infrastructure.Lane.Search.ByPlazaGroup.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.Lane.Search.ByPlazaGroup.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByPlaza
                        actionName = RouteConsts.TOD.Infrastructure.Lane.Search.ByPlaza.Name;
                        actionUrl = RouteConsts.TOD.Infrastructure.Lane.Search.ByPlaza.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }
            }

            internal static class Security
            {
                internal static class Role
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TOD.Security.Role.ControllerName;

                        // Gets
                        actionName = RouteConsts.TOD.Security.Role.Gets.Name;
                        actionUrl = RouteConsts.TOD.Security.Role.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.TOD.Security.Role.Save.Name;
                        actionUrl = RouteConsts.TOD.Security.Role.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }
                internal static class User
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TOD.Security.User.ControllerName;

                        // Gets
                        actionName = RouteConsts.TOD.Security.User.Gets.Name;
                        actionUrl = RouteConsts.TOD.Security.User.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.TOD.Security.User.Save.Name;
                        actionUrl = RouteConsts.TOD.Security.User.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ById
                        actionName = RouteConsts.TOD.Security.User.Search.ById.Name;
                        actionUrl = RouteConsts.TOD.Security.User.Search.ById.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByCardId
                        actionName = RouteConsts.TOD.Security.User.Search.ByCardId.Name;
                        actionUrl = RouteConsts.TOD.Security.User.Search.ByCardId.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByLogIn
                        actionName = RouteConsts.TOD.Security.User.Search.ByLogIn.Name;
                        actionUrl = RouteConsts.TOD.Security.User.Search.ByLogIn.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByRoleId
                        actionName = RouteConsts.TOD.Security.User.Search.ByRoleId.Name;
                        actionUrl = RouteConsts.TOD.Security.User.Search.ByRoleId.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByGroupId
                        actionName = RouteConsts.TOD.Security.User.Search.ByGroupId.Name;
                        actionUrl = RouteConsts.TOD.Security.User.Search.ByGroupId.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByFilter
                        actionName = RouteConsts.TOD.Security.User.Search.ByFilter.Name;
                        actionUrl = RouteConsts.TOD.Security.User.Search.ByFilter.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }
            }

            internal static class Shift
            {
                internal static void MapRoutes(HttpConfiguration config)
                {
                    string controllerName, actionName, actionUrl;

                    // Set Controller Name.
                    controllerName = RouteConsts.TOD.Shift.ControllerName;

                    // Gets
                    actionName = RouteConsts.TOD.Shift.Gets.Name;
                    actionUrl = RouteConsts.TOD.Shift.Gets.Url;
                    Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                }

                internal static class TSB
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TOD.Shift.TSB.ControllerName;

                        // Current
                        actionName = RouteConsts.TOD.Shift.TSB.Current.Name;
                        actionUrl = RouteConsts.TOD.Shift.TSB.Current.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Change
                        //actionName = RouteConsts.TOD.Shift.TSB.Change.Name;
                        //actionUrl = RouteConsts.TOD.Shift.TSB.Change.Url;
                        //Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }

                internal static class User
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        /*
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.Shift.User.ControllerName;
                        */
                    }
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

            // Infrastructure (TSB/PlazaGroup/Plaza/Lane)
            MapControllers.Infrastructure.TSB.MapRoutes(config);
            MapControllers.Infrastructure.PlazaGroup.MapRoutes(config);
            MapControllers.Infrastructure.Plaza.MapRoutes(config);
            MapControllers.Infrastructure.Lane.MapRoutes(config);

            // Security
            MapControllers.Security.Role.MapRoutes(config);
            MapControllers.Security.User.MapRoutes(config);

            // Shift
            MapControllers.Shift.MapRoutes(config);
            // Shift (TSB/User)
            MapControllers.Shift.TSB.MapRoutes(config);
            MapControllers.Shift.User.MapRoutes(config);

            #region Default Route (do not used)

            // If comment below line the auto map default controllers will not load and cannot access.
            //InitDefaultMapRoute(config);

            #endregion
        }

        #endregion
    }

    /// <summary>
    /// TOD WebServer Web Server (Self Host).
    /// </summary>
    public class TODWebServer
    {
        #region Internal Variables

        private WebServiceConfig _cfg = null;
        private IDisposable server = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TODWebServer() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~TODWebServer()
        {
            Shutdown();
        }

        #endregion

        #region Private Methods

        private void CheckConfig()
        {
            // Gets TOD App local server config.
            _cfg = (null != TODConfigManager.Instance.TODApp) ? TODConfigManager.Instance.TODApp.Service : null;
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
            string appName = "DMT TOD App Service (REST)";
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
            string appName = "DMT TOD App Service (REST)";
            var nash = new CommandLine();
            nash.Run("http delete urlacl url=http://+:" + portNum + "/");
            nash.Run("advfirewall firewall delete rule name=\"" + appName + "\"");
        }

        private void ConfigChanged(object sender, EventArgs e)
        {
            // When Service Config file changed.
            // SCW
            Operations.SCW.Config = TODConfigManager.Instance;
            Operations.SCW.DMT = TODConfigManager.Instance; // required for NetworkId
            // TA
            Operations.TA.Config = TODConfigManager.Instance;
            Operations.TA.DMT = TODConfigManager.Instance; // required for NetworkId
            // TAxTOD
            Operations.TAxTOD.Config = TODConfigManager.Instance;
            Operations.TAxTOD.DMT = TODConfigManager.Instance; // required for NetworkId

            // RabbitMQ
            RabbitMQService.Instance.Shutdown();
            RabbitMQService.Instance.RabbitMQ = TODConfigManager.Instance.RabbitMQ;
            RabbitMQService.Instance.Start();
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
            TODConfigManager.Instance.ConfigChanged += ConfigChanged;
            // SCW
            Operations.SCW.Config = TODConfigManager.Instance;
            Operations.SCW.DMT = TODConfigManager.Instance; // required for NetworkId
            // TA
            Operations.TA.Config = TODConfigManager.Instance;
            Operations.TA.DMT = TODConfigManager.Instance; // required for NetworkId
            // TAxTOD
            Operations.TAxTOD.Config = TODConfigManager.Instance;
            Operations.TAxTOD.DMT = TODConfigManager.Instance; // required for NetworkId

            // Start database server.
            TODLocalDbServer.Instance.Start();
            if (TODLocalDbServer.Instance.Connected)
            {
                med.Info("TOD local database connected.");
            }
            else
            {
                med.Info("TOD local database connect failed.");
            }

            if (null == server)
            {
                InitOwinFirewall();
                server = WebApp.Start<StartUp>(url: BaseAddress);
                med.Info("TOD App local nofify service started.");
            }
            else
            {
                med.Info("TOD App local nofify service failed.");
            }

            // Start SCWMQ service.
            SCWMQService.Instance.Start();
            med.Info("SCWMQ Service start.");

            // Start rabbit service.
            RabbitMQService.Instance.RabbitMQ = TODConfigManager.Instance.RabbitMQ;
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

            TODConfigManager.Instance.ConfigChanged -= ConfigChanged;

            // Shutdown Rabbit MQ Service.
            RabbitMQService.Instance.Shutdown();
            med.Info("RabbitMQ Client service disconnected.");

            // Shutdown SCWMQ service.
            SCWMQService.Instance.Shutdown();
            med.Info("SCWMQ Service shutdown.");

            if (null != server)
            {
                server.Dispose();
            }
            server = null;
            ReleaseOwinFirewall();
            med.Info("TOD App local nofify service shutdown.");

            // Shutdown database server.
            TODLocalDbServer.Instance.Shutdown();
            med.Info("TOD local database disconnected.");
        }

        #endregion
    }
}
