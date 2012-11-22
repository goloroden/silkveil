using cherryflavored.net.Contracts.Resources;
using cherryflavored.net.Contracts.Web.Handlers;

using LightCore;

using silkveil.net.Application;
using silkveil.net.Contracts.Identity;
using silkveil.net.Identity.SecurityTokenService.Views;

using System;
using System.Globalization;
using System.Reflection;
using System.Web;

namespace silkveil.net.Identity.SecurityTokenService
{
    /// <summary>
    /// Represents the handler factory for the security token service.
    /// </summary>
    public class SecurityTokenServiceHandlerFactory : HandlerFactoryBase, ISecurityTokenServiceHandlerFactory
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
            // Get the data from the request.
            string data = this.Data.Trim('/');

            // If data is requested, send the appropriate data.
            IHttpHandler handler;
            switch (data)
            {
                case "Background.jpg":
                case "Stylesheet.css":
                    handler = container.Resolve<IGenericHandler>();

                    // Set the appropriate mime type.
                    switch (data.Substring(data.LastIndexOf('.')))
                    {
                        case ".jpg":
                            ((IGenericHandler)handler).MimeType = "image/jpeg";
                            break;
                        case ".css":
                            ((IGenericHandler)handler).MimeType = "text/css";
                            break;
                    }

                    // Set the data to stream.
                    ((IGenericHandler)handler).Data =
                        container.Resolve<IResourceManager>().GetResource(
                            Assembly.GetExecutingAssembly(), "silkveil.net.Identity.SecurityTokenService.Resources." + data);
                    break;
                case "Logon":
                    handler = container.Resolve<LogonView>();
                    break;
                case "Logoff":
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                        "The data '{0}' is not supported.", data));
            }

            // Return the handler to the caller.
            return handler;
        }
    }
}