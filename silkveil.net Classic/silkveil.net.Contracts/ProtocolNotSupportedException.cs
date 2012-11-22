using cherryflavored.net;

using System;
using System.Runtime.Serialization;

namespace silkveil.net.Contracts
{
    /// <summary>
    /// Thrown when a protocol is not supported.
    /// </summary>
    [Serializable]
    public class ProtocolNotSupportedException : SilkveilException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolNotSupportedException"/> type.
        /// </summary>
        public ProtocolNotSupportedException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolNotSupportedException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ProtocolNotSupportedException(string message)
            : base(message)
        {
            Enforce.NotNullOrEmpty(message, () => message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolNotSupportedException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception</param>
        public ProtocolNotSupportedException(string message, Exception innerException)
            : base(message, innerException)
        {
            Enforce.NotNullOrEmpty(message, () => message);
            Enforce.NotNull(innerException, () => innerException);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolNotSupportedException"/> type.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected ProtocolNotSupportedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}