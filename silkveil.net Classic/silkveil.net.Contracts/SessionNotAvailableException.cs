using cherryflavored.net;

using System;
using System.Runtime.Serialization;

namespace silkveil.net.Contracts
{
    /// <summary>
    /// Thrown when a session is not available.
    /// </summary>
    [Serializable]
    public class SessionNotAvailableException : SilkveilException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionNotAvailableException"/> type.
        /// </summary>
        public SessionNotAvailableException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionNotAvailableException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public SessionNotAvailableException(string message)
            : base(message)
        {
            Enforce.NotNullOrEmpty(message, () => message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionNotAvailableException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception</param>
        public SessionNotAvailableException(string message, Exception innerException)
            : base(message, innerException)
        {
            Enforce.NotNullOrEmpty(message, () => message);
            Enforce.NotNull(innerException, () => innerException);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionNotAvailableException"/> type.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected SessionNotAvailableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}