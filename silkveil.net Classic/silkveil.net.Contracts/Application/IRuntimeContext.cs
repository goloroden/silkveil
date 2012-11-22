using System.Web;

namespace silkveil.net.Contracts.Application
{
    /// <summary>
    /// Contains the methods for runtime contexts.
    /// </summary>
    public interface IRuntimeContext
    {
        /// <summary>
        /// Gets the current response stream.
        /// </summary>
        /// <value>The current response stream.</value>
        HttpResponseBase HttpResponse
        {
            get;
        }
    }
}