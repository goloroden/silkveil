using cherryflavored.net;
using cherryflavored.net.ExtensionMethods.System;
using cherryflavored.net.ExtensionMethods.System.Web;

using LightCore;

using silkveil.net.Contracts.Application;

using System.Web;

namespace silkveil.net.Application
{
    /// <summary>
    /// Represents a base class for handlers.
    /// </summary>
    public abstract class HandlerFactoryBase : IHttpHandlerFactory
    {
        /// <summary>
        /// Gets or sets the action for the handler factory.
        /// </summary>
        /// <value>The action.</value>
        public HandlerAction Action
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the request data for the handler factory.
        /// </summary>
        /// <value>The request data.</value>
        public string Data
        {
            get;
            set;
        }

        /// <summary>
        /// Returns an instance of a class that implements the
        /// <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Web.IHttpHandler" /> object that processes the request.
        /// </returns>
        /// <param name="context">
        /// An instance of the <see cref="T:System.Web.HttpContext" /> class that provides
        /// references to intrinsic server objects (for example, Request, Response, Session,
        ///  and Server) used to service HTTP requests. 
        /// </param>
        /// <param name="requestType">
        /// The HTTP data transfer method (GET or POST) that the client uses. 
        /// </param>
        /// <param name="url">
        /// The <see cref="P:System.Web.HttpRequest.RawUrl" /> of the requested resource. 
        /// </param>
        /// <param name="pathTranslated">
        /// The <see cref="P:System.Web.HttpRequest.PhysicalApplicationPath" /> to the requested 
        /// resource. 
        /// </param>
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            context = Enforce.NotNull(context, () => context);
            requestType = Enforce.NotNullOrEmpty(requestType, () => requestType);
            url = Enforce.NotNullOrEmpty(url, () => url);
            pathTranslated = Enforce.NotNullOrEmpty(pathTranslated, () => pathTranslated);

            // Get the action and the data of the silkveil request.
            this.Action = context.Request.QueryString["Action"].ToOrDefault<HandlerAction>();
            this.Data = context.Request.QueryString["Data"];

            // Get the container from the application.
            IContainer container =
                context.ApplicationInstance.GetModule<IApplicationModule>().Container;

            // Delegate the work to the abstract method.
            return GetHandlerCore(context, requestType, url, pathTranslated, container);
        }

        /// <summary>
        /// Returns an instance of a class that implements the
        /// <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Web.IHttpHandler" /> object that processes the request.
        /// </returns>
        /// <param name="context">
        /// An instance of the <see cref="T:System.Web.HttpContext" /> class that provides
        /// references to intrinsic server objects (for example, Request, Response, Session,
        ///  and Server) used to service HTTP requests. 
        /// </param>
        /// <param name="requestType">
        /// The HTTP data transfer method (GET or POST) that the client uses. 
        /// </param>
        /// <param name="url">
        /// The <see cref="P:System.Web.HttpRequest.RawUrl" /> of the requested resource. 
        /// </param>
        /// <param name="pathTranslated">
        /// The <see cref="P:System.Web.HttpRequest.PhysicalApplicationPath" /> to the requested 
        /// resource. 
        /// </param>
        /// <param name="container">The container.</param>
        protected abstract IHttpHandler GetHandlerCore(HttpContext context, string requestType,
            string url, string pathTranslated, IContainer container);

        /// <summary>
        /// Enables a factory to reuse an existing handler instance.
        /// </summary>
        /// <param name="handler">
        /// The <see cref="T:System.Web.IHttpHandler" /> object to reuse. 
        /// </param>
        public virtual void ReleaseHandler(IHttpHandler handler)
        {
            // At the moment, nothing needs to be done here.
        }
    }
}