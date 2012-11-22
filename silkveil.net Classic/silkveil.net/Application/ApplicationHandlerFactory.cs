using cherryflavored.net;

using LightCore;

using silkveil.net.Contracts;
using silkveil.net.Contracts.Identity;
using silkveil.net.Downloads;
using silkveil.net.Redirects;
using silkveil.net.Web;

using System.Web;
using System.Globalization;

namespace silkveil.net.Application
{
    /// <summary>
    /// Represents the main handler factory that handles any requests that are made to
    /// silkveil.net.
    /// </summary>
    public class ApplicationHandlerFactory : HandlerFactoryBase
    {
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
        protected override IHttpHandler GetHandlerCore(HttpContext context, string requestType, string url, string pathTranslated, IContainer container)
        {
            context = Enforce.NotNull(context, () => context);
            requestType = Enforce.NotNullOrEmpty(requestType, () => requestType);
            url = Enforce.NotNullOrEmpty(url, () => url);
            pathTranslated = Enforce.NotNullOrEmpty(pathTranslated, () => pathTranslated);

            // Depending on the handler, delegate.
            IHttpHandlerFactory handlerFactory;
            switch (this.Action)
            {
                case HandlerAction.Download:
                    // Delegate execution to the download handler factory.
                    handlerFactory = container.Resolve<DownloadHandlerFactory>();
                    break;
                case HandlerAction.Redirect:
                    // Delegate execution to the redirect handler factory.
                    handlerFactory = container.Resolve<RedirectHandlerFactory>();
                    break;
                case HandlerAction.SecurityTokenService:
                    // Delegate execution to the security token service handler factory.
                    handlerFactory = container.Resolve<ISecurityTokenServiceHandlerFactory>();
                    break;
                case HandlerAction.UI:
                    // Delegate execution to the silkveil GUI handler factory.
                    handlerFactory = container.Resolve<UIHandlerFactory>();
                    break;
                default:
                    throw new HandlerActionNotSupportedException(
                        string.Format(CultureInfo.CurrentUICulture,
                            "The handler action '{0}' is not supported.", this.Action));
            }

            // Get the appropriate handler and return it to the caller.
            return handlerFactory.GetHandler(
                context, requestType, url, pathTranslated);
        }
    }
}