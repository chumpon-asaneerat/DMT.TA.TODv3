#region Using

using System;
using System.Reflection;
using Microsoft.Owin.Hosting;
using System.Web.Http;
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

        internal static class MapControllers
        {
            internal static class Infrastructure 
            {
                internal static class TSB 
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.Infrastructure.TSB.ControllerName;

                        // Gets
                        actionName = RouteConsts.Infrastructure.TSB.Gets.Name;
                        actionUrl = RouteConsts.Infrastructure.TSB.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Current
                        actionName = RouteConsts.Infrastructure.TSB.Current.Name;
                        actionUrl = RouteConsts.Infrastructure.TSB.Current.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // SetActive
                        actionName = RouteConsts.Infrastructure.TSB.SetActive.Name;
                        actionUrl = RouteConsts.Infrastructure.TSB.SetActive.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.Infrastructure.TSB.Save.Name;
                        actionUrl = RouteConsts.Infrastructure.TSB.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }
                
                internal static class PlazaGroup 
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.Infrastructure.PlazaGroup.ControllerName;

                        // Gets
                        actionName = RouteConsts.Infrastructure.PlazaGroup.Gets.Name;
                        actionUrl = RouteConsts.Infrastructure.PlazaGroup.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.Infrastructure.PlazaGroup.Save.Name;
                        actionUrl = RouteConsts.Infrastructure.PlazaGroup.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByTSB
                        actionName = RouteConsts.Infrastructure.PlazaGroup.Search.ByTSB.Name;
                        actionUrl = RouteConsts.Infrastructure.PlazaGroup.Search.ByTSB.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }

                internal static class Plaza 
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.Infrastructure.Plaza.ControllerName;

                        // Gets
                        actionName = RouteConsts.Infrastructure.Plaza.Gets.Name;
                        actionUrl = RouteConsts.Infrastructure.Plaza.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.Infrastructure.Plaza.Save.Name;
                        actionUrl = RouteConsts.Infrastructure.Plaza.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByTSB
                        actionName = RouteConsts.Infrastructure.Plaza.Search.ByTSB.Name;
                        actionUrl = RouteConsts.Infrastructure.Plaza.Search.ByTSB.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByPlazaGroup
                        actionName = RouteConsts.Infrastructure.Plaza.Search.ByPlazaGroup.Name;
                        actionUrl = RouteConsts.Infrastructure.Plaza.Search.ByPlazaGroup.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }

                internal static class Lane 
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.Infrastructure.Lane.ControllerName;

                        // Gets
                        actionName = RouteConsts.Infrastructure.Lane.Gets.Name;
                        actionUrl = RouteConsts.Infrastructure.Lane.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.Infrastructure.Lane.Save.Name;
                        actionUrl = RouteConsts.Infrastructure.Lane.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByTSB
                        actionName = RouteConsts.Infrastructure.Lane.Search.ByTSB.Name;
                        actionUrl = RouteConsts.Infrastructure.Lane.Search.ByTSB.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByPlazaGroup
                        actionName = RouteConsts.Infrastructure.Lane.Search.ByPlazaGroup.Name;
                        actionUrl = RouteConsts.Infrastructure.Lane.Search.ByPlazaGroup.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByPlaza
                        actionName = RouteConsts.Infrastructure.Lane.Search.ByPlaza.Name;
                        actionUrl = RouteConsts.Infrastructure.Lane.Search.ByPlaza.Url;
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
                        controllerName = RouteConsts.Security.Role.ControllerName;

                        // Gets
                        actionName = RouteConsts.Security.Role.Gets.Name;
                        actionUrl = RouteConsts.Security.Role.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.Security.Role.Save.Name;
                        actionUrl = RouteConsts.Security.Role.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }
                internal static class User
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.Security.User.ControllerName;

                        // Gets
                        actionName = RouteConsts.Security.User.Gets.Name;
                        actionUrl = RouteConsts.Security.User.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.Security.User.Save.Name;
                        actionUrl = RouteConsts.Security.User.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ById
                        actionName = RouteConsts.Security.User.Search.ById.Name;
                        actionUrl = RouteConsts.Security.User.Search.ById.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByCardId
                        actionName = RouteConsts.Security.User.Search.ByCardId.Name;
                        actionUrl = RouteConsts.Security.User.Search.ByCardId.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByLogIn
                        actionName = RouteConsts.Security.User.Search.ByLogIn.Name;
                        actionUrl = RouteConsts.Security.User.Search.ByLogIn.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByRoleId
                        actionName = RouteConsts.Security.User.Search.ByRoleId.Name;
                        actionUrl = RouteConsts.Security.User.Search.ByRoleId.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByGroupId
                        actionName = RouteConsts.Security.User.Search.ByGroupId.Name;
                        actionUrl = RouteConsts.Security.User.Search.ByGroupId.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByFilter
                        actionName = RouteConsts.Security.User.Search.ByFilter.Name;
                        actionUrl = RouteConsts.Security.User.Search.ByFilter.Url;
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
                    controllerName = RouteConsts.Shift.ControllerName;

                    // Gets
                    actionName = RouteConsts.Shift.Gets.Name;
                    actionUrl = RouteConsts.Shift.Gets.Url;
                    Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                }

                internal static class TSB
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.Shift.TSB.ControllerName;

                        // Change
                        actionName = RouteConsts.Shift.TSB.Change.Name;
                        actionUrl = RouteConsts.Shift.TSB.Change.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Current
                        actionName = RouteConsts.Shift.TSB.Current.Name;
                        actionUrl = RouteConsts.Shift.TSB.Current.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
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
