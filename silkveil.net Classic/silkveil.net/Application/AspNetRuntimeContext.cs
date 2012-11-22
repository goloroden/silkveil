using silkveil.net.Contracts.Application;

using System.Web;

namespace silkveil.net.Application
{
    /// <summary>
    /// Represents the ASP.NET runtime context.
    /// </summary>
    public class AspNetRuntimeContext : IRuntimeContext
    {
        /// <summary>
        /// Gets the current response stream.
        /// </summary>
        /// <value>The current response stream.</value>
        public HttpResponseBase HttpResponse
        {
            get
            {
                return new HttpResponseWrapper(HttpContext.Current.Response);
            }
        }
    }
}