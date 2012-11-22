using cherryflavored.net;

using System;
using System.Runtime.Serialization;

namespace silkveil.net.Contracts
{
    /// <summary>
    /// Represents the base exception for all silkveil.net exceptions.
    /// </summary>
    [Serializable]
    public class SilkveilException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SilkveilException"/> type.
        /// </summary>
        public SilkveilException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SilkveilException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public SilkveilException(string message)
            : base(message)
        {
            Enforce.NotNullOrEmpty(message, () => message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SilkveilException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception</param>
        public SilkveilException(string message, Exception innerException)
            : base(message, innerException)
        {
            Enforce.NotNullOrEmpty(message, () => message);
            Enforce.NotNull(innerException, () => innerException);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SilkveilException"/> type.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected SilkveilException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}