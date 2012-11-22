using System;
using System.Collections.Generic;
using System.IO;
using silkveil.net.Core.Contracts;
using silkveil.net.StreamFactories.Contracts;

namespace silkveil.net.StreamFactories.Http.Contracts
{
    /// <summary>
    /// Represents an HTTP stream factory.
    /// </summary>
    public interface IHttpStreamFactory : IStreamFactory
    {
        /// <summary>
        /// Creates a redirect.
        /// </summary>
        /// <param name="redirectMapping">The redirect mapping.</param>
        void CreateRedirect(IRedirectMapping redirectMapping);

        /// <summary>
        /// Requests the HTTP status code from a stream.
        /// </summary>
        /// <param name="stream">A stream.</param>
        void RequestStatusCode(Stream stream);

        /// <summary>
        /// Raised when the status code is available.
        /// </summary>
        event Action<int> StatusCodeAvailable;

        /// <summary>
        /// Requests the HTTP headers from a stream.
        /// </summary>
        /// <param name="stream">A stream.</param>
        void RequestHeaders(Stream stream);

        /// <summary>
        /// Raised when the headers are available.
        /// </summary>
        event Action<IDictionary<string, string>> HeadersAvailable;
    }
}