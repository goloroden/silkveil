using System;
using silkveil.net.RequestListener.Contracts;

namespace silkveil.net.RequestListener.Redirect
{
    /// <summary>
    /// Represents a component that listens for incoming redirect requests.
    /// </summary>
    public class RedirectRequestListener : RequestListenerBase
    {
        /// <summary>
        /// Handles an incoming request with the specified relative URI.
        /// </summary>
        /// <param name="relativeUri">A URI relative to the application root.</param>
        /// <param name="guid">The internal GUID for the requested content.</param>
        /// <returns>
        /// <c>true</c> if this request listener is responsible for the request; <c>false</c>
        /// otherwise.
        /// </returns>
        /// <remarks>
        /// To get a well-formed relative URI the client may use the request's AppRelativeCurrentExecutionFilePath property.
        /// </remarks>
        protected override bool HandleCore(string relativeUri, out Guid guid)
        {
            const string uriPrefix = "~/Redirect/";

            if (!relativeUri.StartsWith(uriPrefix))
            {
                guid = Guid.Empty;
                return false;
            }

            try
            {
                guid = new Guid(relativeUri.Substring(uriPrefix.Length));
            }
            catch (FormatException)
            {
                guid = Guid.Empty;
                return false;
            }

            return true;
        }
    }
}