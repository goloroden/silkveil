using System.Web;

namespace silkveil.net.Web
{
    ///<summary>
    /// Represents the interface for any controller in silkveil.net.
    ///</summary>
    public interface IController
    {
        /// <summary>
        /// Gets the Index View.
        /// </summary>
        IHttpHandler Index();
    }
}