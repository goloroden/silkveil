using LightCore;

using silkveil.net.Web.Views.Home;

using System.Web;

namespace silkveil.net.Web.Controllers
{
    /// <summary>
    /// Represents the home controller.
    /// </summary>
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public HomeController(IContainer container)
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