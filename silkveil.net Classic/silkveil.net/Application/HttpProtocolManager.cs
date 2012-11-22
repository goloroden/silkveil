using System;

namespace silkveil.net.Application
{
    /// <summary>
    /// Represents a protocol manager that works only with the HTTP protocol.
    /// </summary>
    public class HttpProtocolManager : ProtocolManagerBase
    {
        /// <summary>
        /// Transforms the source uri.
        /// </summary>
        /// <param name="sourceUri">The source uri.</param>
        /// <param name="transformedUri">The transformed uri.</param>
        /// <returns><c>true</c> if the source uri was transformed; <c>false</c> otherwise.</returns>
        protected override bool Transform(Uri sourceUri, out Uri transformedUri)
        {
            // Check whether the uri already starts with http://. If so, simply return.
            if(sourceUri.ToString().StartsWith("http://"))
            {
                transformedUri = sourceUri;
                return false;
            }

            // Otherwise, transform the uri.
            transformedUri = new Uri(sourceUri.ToString().Replace("https://", "http://"));
            return true;
        }
    }
}