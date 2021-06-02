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
                var svr = (null != TAConfigManager.Instance.TAApp &&
                           null != TAConfigManager.Instance.TAApp.Service) ?
                    TAConfigManager.Instance.TAApp.Service : null;
                if (null != svr)
                {
                    return (userName == svr.UserName && password == Utils.MD5.Encrypt(svr.Password));
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

                    // IsAlive
                    actionName = RouteConsts.TA.Notify.IsAlive.Name;
                    actionUrl = RouteConsts.TA.Notify.IsAlive.Url;
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
                        controllerName = RouteConsts.TA.Infrastructure.TSB.ControllerName;

                        // Gets
                        actionName = RouteConsts.TA.Infrastructure.TSB.Gets.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.TSB.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Current
                        actionName = RouteConsts.TA.Infrastructure.TSB.Current.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.TSB.Current.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // SetActive
                        actionName = RouteConsts.TA.Infrastructure.TSB.SetActive.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.TSB.SetActive.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.TA.Infrastructure.TSB.Save.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.TSB.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }

                internal static class PlazaGroup
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TA.Infrastructure.PlazaGroup.ControllerName;

                        // Gets
                        actionName = RouteConsts.TA.Infrastructure.PlazaGroup.Gets.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.PlazaGroup.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.TA.Infrastructure.PlazaGroup.Save.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.PlazaGroup.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByTSB
                        actionName = RouteConsts.TA.Infrastructure.PlazaGroup.Search.ByTSB.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.PlazaGroup.Search.ByTSB.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }

                internal static class Plaza
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TA.Infrastructure.Plaza.ControllerName;

                        // Gets
                        actionName = RouteConsts.TA.Infrastructure.Plaza.Gets.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.Plaza.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.TA.Infrastructure.Plaza.Save.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.Plaza.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByTSB
                        actionName = RouteConsts.TA.Infrastructure.Plaza.Search.ByTSB.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.Plaza.Search.ByTSB.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByPlazaGroup
                        actionName = RouteConsts.TA.Infrastructure.Plaza.Search.ByPlazaGroup.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.Plaza.Search.ByPlazaGroup.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }

                internal static class Lane
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TA.Infrastructure.Lane.ControllerName;

                        // Gets
                        actionName = RouteConsts.TA.Infrastructure.Lane.Gets.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.Lane.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.TA.Infrastructure.Lane.Save.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.Lane.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByTSB
                        actionName = RouteConsts.TA.Infrastructure.Lane.Search.ByTSB.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.Lane.Search.ByTSB.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByPlazaGroup
                        actionName = RouteConsts.TA.Infrastructure.Lane.Search.ByPlazaGroup.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.Lane.Search.ByPlazaGroup.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByPlaza
                        actionName = RouteConsts.TA.Infrastructure.Lane.Search.ByPlaza.Name;
                        actionUrl = RouteConsts.TA.Infrastructure.Lane.Search.ByPlaza.Url;
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
                        controllerName = RouteConsts.TA.Security.Role.ControllerName;

                        // Gets
                        actionName = RouteConsts.TA.Security.Role.Gets.Name;
                        actionUrl = RouteConsts.TA.Security.Role.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.TA.Security.Role.Save.Name;
                        actionUrl = RouteConsts.TA.Security.Role.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }

                internal static class User
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TA.Security.User.ControllerName;

                        // Gets
                        actionName = RouteConsts.TA.Security.User.Gets.Name;
                        actionUrl = RouteConsts.TA.Security.User.Gets.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.TA.Security.User.Save.Name;
                        actionUrl = RouteConsts.TA.Security.User.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ById
                        actionName = RouteConsts.TA.Security.User.Search.ById.Name;
                        actionUrl = RouteConsts.TA.Security.User.Search.ById.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByCardId
                        actionName = RouteConsts.TA.Security.User.Search.ByCardId.Name;
                        actionUrl = RouteConsts.TA.Security.User.Search.ByCardId.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByLogIn
                        actionName = RouteConsts.TA.Security.User.Search.ByLogIn.Name;
                        actionUrl = RouteConsts.TA.Security.User.Search.ByLogIn.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByRoleId
                        actionName = RouteConsts.TA.Security.User.Search.ByRoleId.Name;
                        actionUrl = RouteConsts.TA.Security.User.Search.ByRoleId.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByGroupId
                        actionName = RouteConsts.TA.Security.User.Search.ByGroupId.Name;
                        actionUrl = RouteConsts.TA.Security.User.Search.ByGroupId.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Search ByFilter
                        actionName = RouteConsts.TA.Security.User.Search.ByFilter.Name;
                        actionUrl = RouteConsts.TA.Security.User.Search.ByFilter.Url;
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
                    controllerName = RouteConsts.TA.Shift.ControllerName;

                    // Gets
                    actionName = RouteConsts.TA.Shift.Gets.Name;
                    actionUrl = RouteConsts.TA.Shift.Gets.Url;
                    Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                }

                internal static class TSB
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TA.Shift.TSB.ControllerName;

                        // Current
                        actionName = RouteConsts.TA.Shift.TSB.Current.Name;
                        actionUrl = RouteConsts.TA.Shift.TSB.Current.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Update
                        actionName = RouteConsts.TA.Shift.TSB.Update.Name;
                        actionUrl = RouteConsts.TA.Shift.TSB.Update.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }

                internal static class User
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TA.Shift.User.ControllerName;

                        // Update
                        actionName = RouteConsts.TA.Shift.User.Update.Name;
                        actionUrl = RouteConsts.TA.Shift.User.Update.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }
            }

            internal static class RevenueEntry
            {
                internal static void MapRoutes(HttpConfiguration config)
                {
                    string controllerName, actionName, actionUrl;

                    // Set Controller Name.
                    controllerName = RouteConsts.TA.Revenue.ControllerName;

                    // Update
                    actionName = RouteConsts.TA.Revenue.Update.Name;
                    actionUrl = RouteConsts.TA.Revenue.Update.Url;
                    Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                }
            }

            internal static class Credit
            {
                internal static class User
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TA.Credit.User.ControllerName;

                        // Current
                        actionName = RouteConsts.TA.Credit.User.Current.Name;
                        actionUrl = RouteConsts.TA.Credit.User.Current.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Completed
                        actionName = RouteConsts.TA.Credit.User.Completed.Name;
                        actionUrl = RouteConsts.TA.Credit.User.Completed.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.

                        // Save
                        actionName = RouteConsts.TA.Credit.User.Save.Name;
                        actionUrl = RouteConsts.TA.Credit.User.Save.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
                    }
                }
            }

            internal static class Coupon
            {
                internal static class TSB { }

                internal static class User 
                {
                    internal static void MapRoutes(HttpConfiguration config)
                    {
                        string controllerName, actionName, actionUrl;

                        // Set Controller Name.
                        controllerName = RouteConsts.TA.Coupon.User.ControllerName;

                        // Current
                        actionName = RouteConsts.TA.Coupon.User.Sold.Name;
                        actionUrl = RouteConsts.TA.Coupon.User.Sold.Url;
                        Helper.MapRoute(config, controllerName, actionName, actionUrl); // Map Route.
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
            MapControllers.Shift.TSB.MapRoutes(config);
            MapControllers.Shift.User.MapRoutes(config);

            // Revenue Entry
            MapControllers.RevenueEntry.MapRoutes(config);

            // Credit (User)
            MapControllers.Credit.User.MapRoutes(config);
            // Coupon (User)
            MapControllers.Coupon.User.MapRoutes(config);

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

        private void ConfigChanged(object sender, EventArgs e)
        {
            // When Service Config file changed.
            // SCW
            Operations.SCW.Config = TAConfigManager.Instance;
            Operations.SCW.DMT = TAConfigManager.Instance; // required for NetworkId
            // TAxTOD
            Operations.TAxTOD.Config = TAConfigManager.Instance;
            Operations.TAxTOD.DMT = TAConfigManager.Instance; // required for NetworkId

            // RabbitMQ
            RabbitMQService.Instance.Shutdown();
            RabbitMQService.Instance.RabbitMQ = TAConfigManager.Instance.RabbitMQ;
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
            TAConfigManager.Instance.ConfigChanged += ConfigChanged;
            // SCW
            Operations.SCW.Config = TAConfigManager.Instance;
            Operations.SCW.DMT = TAConfigManager.Instance; // required for NetworkId
            // TAxTOD
            Operations.TAxTOD.Config = TAConfigManager.Instance;
            Operations.TAxTOD.DMT = TAConfigManager.Instance; // required for NetworkId

            // Start database server.
            TALocalDbServer.Instance.Start();
            if (TALocalDbServer.Instance.Connected)
            {
                med.Info("TA local database connected.");
            }
            else
            {
                med.Info("TA local database connect failed.");
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

            // Start SCWMQ service.
            SCWMQService.Instance.Start();
            med.Info("SCWMQ Service start.");

            // Start TAxTOD MQ Service
            TAxTODMQService.Instance.Start();

            // Start rabbit service.
            RabbitMQService.Instance.RabbitMQ = TAConfigManager.Instance.RabbitMQ;
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

            TAConfigManager.Instance.ConfigChanged -= ConfigChanged;

            // Shutdown Rabbit MQ Service.
            RabbitMQService.Instance.Shutdown();
            med.Info("RabbitMQ Client service disconnected.");

            // Shutdown TAxTOD MQ Service
            TAxTODMQService.Instance.Shutdown();

            // Shutdown SCWMQ service.
            SCWMQService.Instance.Shutdown();
            med.Info("SCWMQ Service shutdown.");

            if (null != server)
            {
                server.Dispose();
            }
            server = null;

            ReleaseOwinFirewall();
            med.Info("TA App local nofify service shutdown.");

            // Shutdown database server.
            TALocalDbServer.Instance.Shutdown();
            med.Info("TA local database disconnected.");
        }

        #endregion
    }
}
