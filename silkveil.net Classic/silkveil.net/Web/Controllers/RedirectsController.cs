using LightCore;

using silkveil.net.Web.Views.Redirects;

using System.Web;

namespace silkveil.net.Web.Controllers
{
    /// <summary>
    /// Represents the redirects controller.
    /// </summary>
    public class RedirectsController : ControllerBase
    {
                /// <summary>
        /// Initializes a new instance of the <see cref="RedirectsController" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public RedirectsController(IContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets the default view.
        /// </summary>
        /// <value>The default view.</value>
        public override IHttpHandler Index()
        {
            return this.Container.Resolve<IndexView>();
        }
    }
}