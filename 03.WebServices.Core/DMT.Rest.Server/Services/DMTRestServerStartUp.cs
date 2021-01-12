#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLib;
// Owin SelfHost
using Owin;
using Microsoft.Owin; // for OwinStartup attribute.
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.Web.Http;
using System.Web.Http.Validation;
// Owin Authentication
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
// Swagger
using System.Web.Http.Description;
using System.Web.Http.Filters;
using Swashbuckle.Swagger;
using Swashbuckle.Application;

#endregion

namespace DMT.Services
{
    #region BasicAuthenticationMiddleware

    /// <summary>
    /// The Basic Authentication Owin Middleware.
    /// </summary>
    internal class BasicAuthenticationMiddleware : OwinMiddleware
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next">The OwinMiddleware instance.</param>
        public BasicAuthenticationMiddleware(OwinMiddleware next) : base(next) { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invoke.
        /// </summary>
        /// <param name="context">The OwinContext.</param>
        /// <returns>Returns Task instance.</returns>
        public async override Task Invoke(IOwinContext context)
        {
            var request = context.Request;
            var response = context.Response;

            response.OnSendingHeaders(state =>
            {
                var resp = (OwinResponse)state;
                if (resp.StatusCode == 401)
                {
                    resp.Headers.Add("WWW-Authenticate", new string[] { "Basic" });
                }
            }, response);

            var header = request.Headers.Get("Authorization");
            if (!string.IsNullOrWhiteSpace(header))
            {
                var authHeader = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header);

                request.User = null; // set request user

                if ("Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase) &&
                    null != Validator)
                {
                    string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
                    var parts = parameter.Split(':');

                    string userName = parts[0];
                    string password = parts[1];

                    if (Validator(userName, password))
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, userName) };
                        var identity = new ClaimsIdentity(claims, "Basic");
                        request.User = new ClaimsPrincipal(identity);
                    }
                }
            }

            await Next.Invoke(context);
        }

        #endregion

        #region Static Propertiess

        /// <summary>
        /// Gets or sets validator function.
        /// </summary>
        public static Func<string, string, bool> Validator { get; set; }

        #endregion
    }

    #endregion

    #region CustomBodyModelValidator

    /// <summary>
    /// The Custom Body Model Validator class.
    /// </summary>
    internal class CustomBodyModelValidator : DefaultBodyModelValidator
    {
        /// <summary>
        /// Should Validate Type.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <returns>Returns true if specificed need to validate.</returns>
        public override bool ShouldValidateType(Type type)
        {
            bool isNotDMTModel = !type.IsSubclassOf(typeof(Models.DMTModelBase));
            // Ignore validation on all DMTModelBase subclasses.
            return isNotDMTModel && base.ShouldValidateType(type);
        }
    }

    #endregion

    #region AddAuthorizationHeaderParameterOperationFilter

    /// <summary>
    /// AddAuthorizationHeaderParameterOperationFilter class for swagger.
    /// </summary>
    internal class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var filterPipeline = apiDescription.ActionDescriptor.GetFilterPipeline();
            var isAuthorized = filterPipeline
                .Select(filterInfo => filterInfo.Instance)
                .Any(filter => filter is IAuthorizationFilter);

            var allowAnonymous = apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();

            if (isAuthorized && !allowAnonymous)
            {
                // in some case operation.parameters is null so create new list.
                if (null == operation.parameters) operation.parameters = new List<Parameter>();

                operation.parameters.Add(new Parameter
                {
                    name = "Authorization",
                    @in = "header",
                    description = "access token",
                    required = true,
                    type = "string"
                });
            }
        }
    }

    #endregion

    #region DMTRestServerStartUp

    /// <summary>
    /// The DMT Rest Server StartUp class (abstract).
    /// </summary>
    public abstract class DMTRestServerStartUp
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public DMTRestServerStartUp() : base()
        {
            // Authentication
            this.HasAuthentication = true;
            this.AuthenticationSchemes = AuthenticationSchemes.Basic |
                //AuthenticationSchemes.IntegratedWindowsAuthentication |
                AuthenticationSchemes.Anonymous;
            this.AuthenticationValidator = (string userName, string password) =>
            {
                return userName != "" && password != "";
            };
            // Swagger
            this.EnableSwagger = true;
        }

        #endregion

        #region Protected Methods

        #region Authentication

        /// <summary>
        /// Init Authentication.
        /// </summary>
        /// <param name="app">The IAppBuilder instance.</param>
        protected virtual void InitAuthentication(IAppBuilder app)
        {
            if (null == app) return;
            if (!HasAuthentication) return;
            // Setup Authentication for listener.
            HttpListener listener;
            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                listener = app.Properties["System.Net.HttpListener"] as HttpListener;
                if (null != listener)
                {
                    listener.AuthenticationSchemes = AuthenticationSchemes;
                    // setup validate function.
                    BasicAuthenticationMiddleware.Validator = AuthenticationValidator;
                    // used Authentication middleware.
                    app.Use(typeof(BasicAuthenticationMiddleware));
                }
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }
        }
        /// <summary>
        /// Gets or sets Authentication Validator function.
        /// </summary>
        protected Func<string, string, bool> AuthenticationValidator { get; set; }

        #endregion

        #region Default Http Configuration

        /// <summary>
        /// Get Default Http Configuration.
        /// </summary>
        /// <returns>Returns default HttpConfiguration instance.</returns>
        protected virtual HttpConfiguration GetDefaultHttpConfiguration()
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            // Enable Cors.
            config.EnableCors();
            // Enable Attribute routing.
            config.MapHttpAttributeRoutes();
            // Add Filter for Authorize Attribute.
            config.Filters.Add(new AuthorizeAttribute());

            return config;
        }

        #endregion

        #region Formatter

        /// <summary>
        /// Init Custom Formatter.
        /// </summary>
        /// <param name="config">The HttpConfiguration instance.</param>
        protected virtual void InitCustomFormatter(HttpConfiguration config)
        {
            if (null == config) return;
            // Add new formatter.
            config.Formatters.Clear();
            config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            // Replace IBodyModelValidator to Custom Model Validator to prevent insufficient stack problem.
            config.Services.Replace(typeof(IBodyModelValidator), new CustomBodyModelValidator());
        }

        #endregion

        #region Routes

        /// <summary>
        /// Init Map Routes
        /// </summary>
        /// <param name="config">The HttpConfiguration instance.</param>
        protected virtual void InitMapRoutes(HttpConfiguration config)
        {
            if (null == config) return;
            InitDefaultMapRoute(config);
        }
        /// <summary>
        /// Init Default Map Route.
        /// </summary>
        /// <param name="config">The HttpConfiguration instance.</param>
        protected virtual void InitDefaultMapRoute(HttpConfiguration config)
        {
            if (null == config) return;
            // Default Setting to handle routes like `/api/controller/action`
            config.Routes.MapHttpRoute(
                name: "ControllerAndAction",
                routeTemplate: "api/{controller}/{action}"
            );
        }

        #endregion

        #region Swagger

        /// <summary>
        /// Init Swagger UI
        /// </summary>
        /// <param name="config">The HttpConfiguration instance.</param>
        protected virtual void InitSwagger(HttpConfiguration config)
        {
            if (null == config) return;
            if (!EnableSwagger) return;
            string version = (string.IsNullOrEmpty(ApiVersion)) ? "v1" : ApiVersion;
            string title = (string.IsNullOrEmpty(ApiName)) ? "REST Api." : ApiName;
            // Enable Swashbuckle (swagger) 
            // for more information see: https://github.com/domaindrivendev/Swashbuckle.WebApi
            // to see api document goto: http://your-root-url/swagger
            config
                .EnableSwagger(c =>
                {
                    c.BasicAuth("basic").Description("Basic HTTP Authentication");
                    c.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>();
                    c.SingleApiVersion(version, title);
                    c.PrettyPrint();
                })
                .EnableSwaggerUi(x => x.DisableValidator());
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Configuration.
        /// </summary>
        /// <param name="app">The IAppBuilder instance.</param>
        public virtual void Configuration(IAppBuilder app)
        {
            if (null == app) return;
            // Configure Web API for self-host. 
            HttpConfiguration config = GetDefaultHttpConfiguration();
            InitAuthentication(app);
            InitCustomFormatter(config);
            InitMapRoutes(config);
            InitSwagger(config);
            // set configuration to app builder.
            app.UseWebApi(config);
        }

        #endregion

        #region Public Properties

        #region Authentication

        /// <summary>
        /// Gets or (protected sets) has authentication.
        /// </summary>
        public bool HasAuthentication { get; protected set; }
        /// <summary>
        /// Gets or (protected sets) Authentication Schemes.
        /// </summary>
        public AuthenticationSchemes AuthenticationSchemes { get; protected set; }

        #endregion

        #region Swagger

        /// <summary>
        /// Gets or (protected sets) Enable Swagger UI.
        /// </summary>
        public bool EnableSwagger { get; protected set; }
        /// <summary>
        /// Gets or (protected sets) Server API version.
        /// </summary>
        public string ApiVersion { get; protected set; }
        /// <summary>
        /// Gets or (protected sets) Server API Name or Title.
        /// </summary>
        public string ApiName { get; protected set; }

        #endregion

        #endregion

        #region Static Helper class

        /// <summary>
        /// The route helper class.
        /// </summary>
        public static class Helper
        {
            /// <summary>
            /// Map Route.
            /// </summary>
            /// <param name="config">The HttpConfiguration instance.</param>
            /// <param name="controllerName">The controller class name (without Controller suffix).</param>
            /// <param name="actionName">The action name.</param>
            /// <param name="actionUrl">The action url.</param>
            public static void MapRoute(HttpConfiguration config, string controllerName, string actionName, string actionUrl)
            {
                if (null == config ||
                    string.IsNullOrWhiteSpace(controllerName) ||
                    string.IsNullOrWhiteSpace(actionName) ||
                    string.IsNullOrWhiteSpace(actionUrl)) return;

                config.Routes.MapHttpRoute(
                    name: controllerName + "." + actionName,
                    routeTemplate: actionUrl,
                    defaults: new { controller = controllerName, action = actionName });
            }
        }

        #endregion
    }

    #endregion
}
