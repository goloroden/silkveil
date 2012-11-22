using System;
using System.Collections.Generic;
using silkveil.net.Core.Contracts;

namespace silkveil.net.Containers.Contracts
{
    /// <summary>
    /// Represents the silkveil.net container which contains the whole application logic.
    /// </summary>
    public interface ISilkveilContainer : IDisposable
    {
        /// <summary>
        /// Handles an incoming request with the specified relative URI.
        /// </summary>
        /// <param name="relativeUri">A URI relative to the application root.</param>
        /// <remarks>
        /// To get a well-formed relative URI the client may use the request's AppRelativeCurrentExecutionFilePath property.
        /// </remarks>
        void Handle(string relativeUri);

        /// <summary>
        /// Raised when a part of the response is available.
        /// </summary>
        event Action<IStreamPart> ResponsePartAvailable;

        /// <summary>
        /// Raised when an HTTP status code is available.
        /// </summary>
        event Action<int> StatusCodeAvailable;

        /// <summary>
        /// Raised when HTTP headers are available.
        /// </summary>
        event Action<IDictionary<string, string>> HeadersAvailable;

        /// <summary>
        /// Raised when this instance is disposed.
        /// </summary>
        event Action<ISilkveilContainer> Disposed;
    }
}