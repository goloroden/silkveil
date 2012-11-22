using cherryflavored.net;

using System;
using System.Runtime.Serialization;

namespace silkveil.net.Contracts
{
    /// <summary>
    /// Thrown when a handler action is not supported.
    /// </summary>
    [Serializable]
    public class HandlerActionNotSupportedException : SilkveilException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerActionNotSupportedException"/> type.
        /// </summary>
        public HandlerActionNotSupportedException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerActionNotSupportedException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public HandlerActionNotSupportedException(string message)
            : base(message)
        {
            Enforce.NotNullOrEmpty(message, () => message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerActionNotSupportedException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception</param>
        public HandlerActionNotSupportedException(string message, Exception innerException)
            : base(message, innerException)
        {
            Enforce.NotNullOrEmpty(message, () => message);
            Enforce.NotNull(innerException, () => innerException);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerActionNotSupportedException"/> type.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected HandlerActionNotSupportedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}