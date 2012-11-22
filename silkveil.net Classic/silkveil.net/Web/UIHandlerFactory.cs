using cherryflavored.net;
using cherryflavored.net.ExtensionMethods.System;

using LightCore;

using silkveil.net.Application;
using silkveil.net.Contracts.Services;
using silkveil.net.Web.Controllers;

using System;
using System.Globalization;
using System.Reflection;
using System.Web;

namespace silkveil.net.Web
{
    ///<summary>
    /// Represents a factory for the silkveil UI.
    ///</summary>
    public class UIHandlerFactory : HandlerFactoryBase
    {
        /// <summary>
        /// Contains the mapping service.
        /// </summary>
        private IMappingService _mappingService;

        /// <summary>
        /// Contains the redirect service.
        /// </summary>
        private IRedirectService _redirectService;

        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public string Controller
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the controller action.
        /// </summary>
        /// <value>The controller action.</value>
        public string ControllerAction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the action data.
        /// </summary>
        public string ActionData
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIHandlerFactory" /> type.
        /// </summary>
        /// <param name="mappingService">The mapping service.</param>
        /// <param name="redirectService">The redirect service.</param>
        public UIHandlerFactory(IMappingService mappingService, IRedirectService redirectService)
        {
            this._mappingService = mappingService;
            this._redirectService = redirectService;
        }

        /// <summary>
        /// Returns an instance of a class that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Web.IHttpHandler"/> object that processes the request.
        /// </returns>
        /// <param name="context">An instance of the <see cref="T:System.Web.HttpContext"/> class that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        /// <param name="requestType">The HTTP data transfer method (GET or POST) that the client uses.</param>
        /// <param name="url">The <see cref="P:System.Web.HttpRequest.RawUrl"/> of the requested resource.</param>
        /// <param name="pathTranslated">The <see cref="P:System.Web.HttpRequest.PhysicalApplicationPath"/> to the requested resource.</param>
        /// <param name="container">The container.</param>
        protected override IHttpHandler GetHandlerCore(HttpContext context, string requestType, string url, string pathTranslated, IContainer container)
        {
            context = Enforce.NotNull(context, () => context);
            requestType = Enforce.NotNullOrEmpty(requestType, () => requestType);
            url = Enforce.NotNullOrEmpty(url, () => url);
            pathTranslated = Enforce.NotNullOrEmpty(pathTranslated, () => pathTranslated);

            // Get the parameters that are not yet given by the base type.
            this.Controller = context.Request.QueryString["Controller"];
            this.ControllerAction = context.Request.QueryString["ControllerAction"];
            this.ActionData = this.Data;

            // Create the controller typ format.
            string controllerTypeFormat = string.Concat(
                typeof(IController).Namespace,
                ".Controllers.{0}{1}");

            // Get controller instance.
            IController controller =
                (IController)Assembly.GetExecutingAssembly().CreateInstance(
                    string.Format(controllerTypeFormat, this.Controller, "Controller"),
                    false, BindingFlags.Instance | BindingFlags.Public, null, 
                    new[] { container },
                    CultureInfo.CurrentUICulture, new object[] { });

            // No controller found, return default controller.
            if (controller == null)
            {
                controller = new HomeController(container);
            }

            bool isIndexController = this.ControllerAction.Equals("Index", StringComparison.OrdinalIgnoreCase);

            // If there is no action available or the action is the default action, invoke this.
            if (this.ControllerAction.IsNullOrEmpty() || isIndexController && this.ActionData == null)
            {
                return controller.Index();
            }

            MethodInfo actionMethod;

            // Else try to get the specified action with data parameter and invoke it.
            if (!this.ActionData.IsNullOrEmpty())
            {
                actionMethod = controller.GetType().GetMethod(this.ControllerAction, new[] { typeof(string) });

                if (actionMethod != null)
                {
                    return (IHttpHandler)actionMethod.Invoke(controller, new[] { this.ActionData });
                }
            }

            actionMethod = controller.GetType().GetMethod(this.ControllerAction,
                                                                     BindingFlags.Instance | BindingFlags.Public);
            // Else try get the specified action without the data parameter and invoke it.
            if (actionMethod != null)
            {
                return (IHttpHandler)actionMethod.Invoke(controller, null);
            }

            // Return the handler to the caller.
            return controller.Index();
        }
    }
}