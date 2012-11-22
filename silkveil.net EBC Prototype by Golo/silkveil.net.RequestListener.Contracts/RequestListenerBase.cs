using System;

namespace silkveil.net.RequestListener.Contracts
{
    /// <summary>
    /// Represents the base class for a component that listens for incoming requests, detects
    /// whether it is responsible for the request, and - if so - extracts the request's parameters.
    /// </summary>
    public abstract class RequestListenerBase : IRequestListener
    {
        /// <summary>
        /// Handles an incoming request with the specified relative URI.
        /// </summary>
        /// <param name="relativeUri">A URI relative to the application root.</param>
        /// <remarks>
        /// To get a well-formed relative URI the client may use the request's FilePath property.
        /// </remarks>
        public void Handle(string relativeUri)
        {
            Guid guid;
            if(!this.HandleCore(relativeUri, out guid))
            {
                return;
            }

            this.OnValidUriDetected(guid);
        }

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
        /// To get a well-formed relative URI the client may use the request's FilePath property.
        /// </remarks>
        protected abstract bool HandleCore(string relativeUri, out Guid guid);

        /// <summary>
        /// Raises the ValidUriDetected event.
        /// </summary>
        /// <param name="guid">The internal GUID for the requested content.</param>
        protected virtual void OnValidUriDetected(Guid guid)
        {
            Action<Guid> validUriDetected = this.ValidUriDetected;
            if (validUriDetected != null)
            {
                validUriDetected(guid);
            }
        }

        /// <summary>
        /// Raised when a URI is detected this request listener is responsible for.
        /// </summary>
        public event Action<Guid> ValidUriDetected;
    }
}