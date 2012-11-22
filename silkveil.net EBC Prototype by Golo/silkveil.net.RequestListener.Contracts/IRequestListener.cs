using System;

namespace silkveil.net.RequestListener.Contracts
{
    /// <summary>
    /// Represents a component that listens for incoming requests, detects whether it is
    /// responsible for the request, and - if so - extracts the request's parameters.
    /// </summary>
    public interface IRequestListener
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
        /// Raised when a URI is detected this request listener is responsible for.
        /// </summary>
        event Action<Guid> ValidUriDetected;
    }
}