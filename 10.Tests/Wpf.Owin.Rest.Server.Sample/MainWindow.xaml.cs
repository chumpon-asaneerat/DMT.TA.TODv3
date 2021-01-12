#region Using

using System;
using System.Linq;
using System.Windows;
using System.Reflection;
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

namespace Wpf.Owin.Rest.Server.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        private IDisposable server = null;

        #region Loaded/Unloader

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Shutdown();
        }

        #endregion

        private void Start()
        {
            if (null == server)
            {
                string baseAddress = @"http://+:8000";
                //string baseAddress = @"http://localhost:8000";
                server = WebApp.Start<StartUp>(url: baseAddress);
            }
        }

        private void Shutdown()
        {
            if (null != server)
            {
                server.Dispose();
            }
            server = null;
        }
    }

    #region Original Code
    /*
    /// <summary>
    /// Web Server StartUp class.
    /// </summary>
    public class StartUp
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Setup Authentication for listener.
            HttpListener listener =
                    (HttpListener)appBuilder.Properties["System.Net.HttpListener"];
            listener.AuthenticationSchemes =
                AuthenticationSchemes.Basic |
                //AuthenticationSchemes.IntegratedWindowsAuthentication |
                AuthenticationSchemes.Anonymous;

            // used Authentication middleware.
            appBuilder.Use(typeof(AuthenticationMiddleware));

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            // Enable Attribute routing.
            config.MapHttpAttributeRoutes();

            // Enable Cors and Authorize middleware.
            config.EnableCors();
            config.Filters.Add(new AuthorizeAttribute()); // Set Filter for Authorize Attribute.

            // Controllers with Actions
            // To handle routes like `/api/controller/action`
            //config.Routes.MapHttpRoute(
            //    name: "ControllerAndAction",
            //    routeTemplate: "api/{controller}/{action}"
            //);

            // Handle route by specificed controller (Route Order is important).
            // Calculator2 Controller
            config.Routes.MapHttpRoute(
                name: "Calc2ApiAdd",
                routeTemplate: "api/Calc2/Add",
                defaults: new { controller = "Calculator2", action = "Add" });
            config.Routes.MapHttpRoute(
                name: "Calc2ApiSub",
                routeTemplate: "api/Calc2/Sub",
                defaults: new { controller = "Calculator2", action = "Sub" });
            // Default Setting to handle routes like `/api/controller/action`
            config.Routes.MapHttpRoute(
                name: "ControllerAndAction",
                routeTemplate: "api/{controller}/{action}");

            config.Formatters.Clear();
            config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings =
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            // Replace IBodyModelValidator to Custom Model Validator to prevent
            // Insufficient Stack problem.
            config.Services.Replace(typeof(IBodyModelValidator), new CustomBodyModelValidator());

            // Enable Swashbuckle (swagger) 
            // for more information see: https://github.com/domaindrivendev/Swashbuckle.WebApi
            // to see api document goto: http://your-root-url/swagger
            config
                .EnableSwagger(c => c.SingleApiVersion("v1", "A title for your API"))
                .EnableSwaggerUi(x => x.DisableValidator());

            appBuilder.UseWebApi(config);
        }
    }

    internal class CustomBodyModelValidator : DefaultBodyModelValidator
    {
        public override bool ShouldValidateType(Type type)
        {
            // Ignore validation on all DMTModelBase subclasses.
            return base.ShouldValidateType(type);
        }
    }

    internal class AuthenticationMiddleware : OwinMiddleware
    {
        public AuthenticationMiddleware(OwinMiddleware next) :
            base(next)
        {
        }

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
            if (!String.IsNullOrWhiteSpace(header))
            {
                var authHeader = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header);
                request.User = null;
                if ("Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
                {
                    string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
                    var parts = parameter.Split(':');

                    string userName = parts[0];
                    string password = parts[1];

                    if (userName == "DMTUSER" && password == "DMTPASS2")
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, userName)
                        };
                        var identity = new ClaimsIdentity(claims, "Basic");
                        request.User = new ClaimsPrincipal(identity);
                    }
                }
            }

            await Next.Invoke(context);
        }
    }
    */
    #endregion

    #region New Code

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
            return base.ShouldValidateType(type);
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
                //med.Err(ex);
                Console.WriteLine(ex);
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
    }

    #endregion

    // Auth: Basic RE1UVVNFUjpETVRQQVNTMg==
    // See swagger doc in url: http://localhost:8000/swagger

    public class StartUp : DMTRestServerStartUp
    {
        public StartUp() : base()
        {
            this.AuthenticationValidator = (string userName, string password) =>
            {
                return (userName == "DMTUSER" && password == "DMTPASS2");
            };
            this.EnableSwagger = true;
            this.ApiName = "Owin Sample API";
            this.ApiVersion = "v1";
        }

        protected override void InitMapRoutes(HttpConfiguration config)
        {
            // Handle route by specificed controller (Route Order is important).
            // Calculator2 Controller
            /*
            config.Routes.MapHttpRoute(
                name: "Calc2ApiAdd",
                routeTemplate: "api/Calc2/Add",
                defaults: new { controller = "Calculator2", action = "Add" });
            config.Routes.MapHttpRoute(
                name: "Calc2ApiSub",
                routeTemplate: "api/Calc2/Sub",
                defaults: new { controller = "Calculator2", action = "Sub" });
            */
            // Calculator2 Controller
            config.Routes.MapHttpRoute(
                name: "Calc2ApiAdd",
                routeTemplate: RouteConsts2.Calculator.Add.Url,
                defaults: new { controller = RouteConsts2.Calculator.ControllerName, action = RouteConsts2.Calculator.Add.Name });
            config.Routes.MapHttpRoute(
                name: "Calc2ApiSub",
                routeTemplate: RouteConsts2.Calculator.Sub.Url,
                defaults: new { controller = RouteConsts2.Calculator.ControllerName, action = RouteConsts2.Calculator.Sub.Name });

            // If comment below line the Test Controller and Calculator Controller will not load and cannot access.
            //InitDefaultMapRoute(config);
        }
    }

    #endregion

    // url: http://localhost:8000/api/Test/getsample
    // body: { "name": "Job" }

    #region Test

    public class TestController : ApiController
    {
        [HttpPost]
        [ActionName(@"getsample")]
        public SampleResult getsample([FromBody] SampleRequest value)
        {
            SampleResult result;
            if (null == value)
            {
                result = new SampleResult();
                result.Greating = "Welcome [anonymous].";
            }
            else
            {
                result = new SampleResult();
                result.Greating = "Welcome [" + value.Name + "].";
            }
            return result;
        }
    }

    public class SampleRequest
    {
        public string Name { get; set; }
    }

    public class SampleResult
    {
        public string Greating { get; set; }
    }

    #endregion

    // url: http://localhost:8000/api/Calculator/Add
    // body: { "num1": 1, "num2": 2 }
    // url: http://localhost:8000/api/Calculator/Sub
    // body: { "num1": 1, "num2": 2 }

    #region Calculator

    public static class RouteConsts
    {
        public const string Url = "api";

        public static class Calculator
        {
            public const string Url = RouteConsts.Url + @"/Calculator";

            public static class Add
            {
                public const string Name = "Add";
                public const string Url = Calculator.Url + @"/" + Name;
            }

            public static class Sub
            {
                public const string Name = "Sub";
                public const string Url = Calculator.Url + @"/" + Name;
            }
        }
    }


    public class CalcRequest
    {
        public int Num1 { get; set; }
        public int Num2 { get; set; }
    }

    public class CalcResult
    {
        public int Result { get; set; }
    }

    [Authorize] // Authorize Attribute can set here or set in each method(s).
    public partial class CalculatorController : ApiController { }

    partial class CalculatorController
    {
        [AllowAnonymous]
        [HttpPost]
        [ActionName(RouteConsts.Calculator.Add.Name)]
        public CalcResult add([FromBody] CalcRequest value)
        {
            if (null == value)
                return new CalcResult() { Result = 0 };
            else return new CalcResult() { Result = value.Num1 + value.Num2 };
        }
    }

    partial class CalculatorController
    {
        [HttpPost]
        [ActionName(RouteConsts.Calculator.Sub.Name)]
        public CalcResult sub([FromBody] CalcRequest value)
        {
            if (null == value)
                return new CalcResult() { Result = 0 };
            else return new CalcResult() { Result = value.Num1 - value.Num2 };
        }
    }

    #endregion

    // See MapHttpRoute in Startup class.
    // url: http://localhost:8000/api/Calc2/Add
    // body: { "num1": 1, "num2": 2 }
    // url: http://localhost:8000/api/Calc2/Sub
    // body: { "num1": 1, "num2": 2 }

    #region Calculator2

    public static class RouteConsts2
    {
        public const string Url = "api";

        public static class Calculator
        {
            // Match MapHttpRoute in Startup class.
            // Trick for export controller we can create new class that inherited from 
            // the nested controller class for map to route.
            public const string ControllerName = "CalculatorX";
            public const string Url = RouteConsts2.Url + @"/Calc2";

            public static class Add
            {
                public const string Name = "Add";
                public const string Url = Calculator.Url + @"/" + Name;
            }

            public static class Sub
            {
                public const string Name = "Sub";
                public const string Url = Calculator.Url + @"/" + Name;
            }
        }
    }


    public class NestedControllers
    {
        [Authorize] // Authorize Attribute can set here or set in each method(s).
        public partial class Calculator2Controller : ApiController { }

        partial class Calculator2Controller
        {
            [AllowAnonymous]
            [HttpPost]
            [ActionName(RouteConsts2.Calculator.Add.Name)]
            public CalcResult addMe([FromBody] CalcRequest value)
            {
                if (null == value)
                    return new CalcResult() { Result = 0 };
                else return new CalcResult() { Result = value.Num1 + value.Num2 };
            }
        }

        partial class Calculator2Controller
        {
            [HttpPost]
            [ActionName(RouteConsts2.Calculator.Sub.Name)]
            public CalcResult subMe([FromBody] CalcRequest value)
            {
                if (null == value)
                    return new CalcResult() { Result = 0 };
                else return new CalcResult() { Result = value.Num1 - value.Num2 };
            }
        }
    }

    // The Export Controller class.
    public class CalculatorXController : NestedControllers.Calculator2Controller { }

    #endregion
}
