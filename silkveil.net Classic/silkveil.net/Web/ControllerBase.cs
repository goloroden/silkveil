using LightCore;

using System.Web;

namespace silkveil.net.Web
{
    ///<summary>
    /// Represents a Controller base Implementation.
    ///</summary>
    public abstract class ControllerBase : IController
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        protected IContainer Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerBase" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        protected ControllerBase(IContainer container)
        {
            this.Container = container;
        }

        /// <summary>
        /// Gets the Index View.
        /// </summary>
        public abstract IHttpHandler Index();
    }
}