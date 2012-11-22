using cherryflavored.net;

using System;
using System.Runtime.Serialization;

namespace silkveil.net.Contracts
{
    /// <summary>
    /// Thrown when a mapping is not found.
    /// </summary>
    [Serializable]
    public class InvalidMappingException : SilkveilException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMappingException"/> type.
        /// </summary>
        public InvalidMappingException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMappingException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public InvalidMappingException(string message)
            : base(message)
        {
            Enforce.NotNullOrEmpty(message, () => message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMappingException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception</param>
        public InvalidMappingException(string message, Exception innerException)
            : base(message, innerException)
        {
            Enforce.NotNullOrEmpty(message, () => message);
            Enforce.NotNull(innerException, () => innerException);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMappingException"/> type.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected InvalidMappingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}