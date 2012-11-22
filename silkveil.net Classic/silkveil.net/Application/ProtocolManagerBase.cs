using silkveil.net.Contracts.Application;

using System;
using System.Web;

namespace silkveil.net.Application
{
    /// <summary>
    /// Represents the base class for protocol managers.
    /// </summary>
    public abstract class ProtocolManagerBase : IProtocolManager
    {
        /// <summary>
        /// Checks the protocol of the specified request and redirects, if neccessary.
        /// </summary>
        /// <param name="request">The current request.</param>
        public void RedirectIfNeccessary(HttpRequest request)
        {
            // Get the current uri.
            Uri currentUri = request.Url;

            // Transform the uri.
            Uri transformedUri;
            bool isTransformed = this.Transform(currentUri, out transformedUri);

            // If the current uri was not transformed, nothing needs to be done.
            if(!isTransformed)
            {
                return;
            }

            // Otherwise, redirect to the transformed url.
            HttpContext.Current.Response.Redirect(transformedUri.ToString(), true);
        }

        /// <summary>
        /// Transforms the source uri.
        /// </summary>
        /// <param name="sourceUri">The source uri.</param>
        /// <param name="transformedUri">The transformed uri.</param>
        /// <returns><c>true</c> if the source uri was transformed; <c>false</c> otherwise.</returns>
        protected abstract bool Transform(Uri sourceUri, out Uri transformedUri);
    }
}