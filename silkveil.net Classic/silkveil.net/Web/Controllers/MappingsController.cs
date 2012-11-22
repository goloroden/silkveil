using LightCore;

using silkveil.net.Web.Views.Mappings;

using System.Web;

namespace silkveil.net.Web.Controllers
{
    /// <summary>
    /// Represents the mappings controller.
    /// </summary>
    public class MappingsController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingsController" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public MappingsController(IContainer container)
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