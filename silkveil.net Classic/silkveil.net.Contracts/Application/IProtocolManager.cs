using System.Web;

namespace silkveil.net.Contracts.Application
{
    /// <summary>
    /// Contains all methods for a protocol manager.
    /// </summary>
    public interface IProtocolManager
    {
        /// <summary>
        /// Checks the protocol of the specified request and redirects, if neccessary.
        /// </summary>
        /// <param name="request">The current request.</param>
        void RedirectIfNeccessary(HttpRequest request);
    }
}