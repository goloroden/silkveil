using cherryflavored.net.Contracts.Resources;
using cherryflavored.net.Contracts.Web.Handlers;

using LightCore;

using silkveil.net.Contracts;

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;

namespace silkveil.net.Web.Controllers
{
    ///<summary>
    /// Represents the ressources controller.
    ///</summary>
    public class ResourcesController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourcesController" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public ResourcesController(IContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets the Index View.
        /// </summary>
        /// <returns></returns>
        public override IHttpHandler Index()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the Index View with additional data parameter
        /// that points to a specified ressource.
        /// </summary>
        /// <param name="data">The data parameter.</param>
        public IHttpHandler Index(string data)
        {
            // Get the resource manager.
            var resourceManager = this.Container.Resolve<IResourceManager>();

            // Prepare the resource stream and the mime type.
            Stream stream;
            string mimeType;

            // Load the appropriate data.
            switch (data)
            {
                case "Stylesheet.css":
                    stream = resourceManager.GetResource(
                        Assembly.GetExecutingAssembly(), "silkveil.net.Web.Resources.Stylesheet.css");
                    mimeType = "text/css";
                    break;
                case "MasterPage.html":
                    stream = resourceManager.GetResource(
                        Assembly.GetExecutingAssembly(), "silkveil.net.Web.Resources.MasterPage.html");
                    mimeType = "text/html";
                    break;
                default:
                    throw new SilkveilException(String.Format(CultureInfo.CurrentUICulture,
                        "The resource '{0}' was not found.", data));
            }

            // Set up the handler.
            var handler = this.Container.Resolve<IGenericHandler>();
            handler.MimeType = mimeType;
            handler.Data = stream;

            // Return the handler to the caller.
            return handler;
        }
    }
}