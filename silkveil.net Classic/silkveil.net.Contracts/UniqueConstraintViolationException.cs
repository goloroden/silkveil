using cherryflavored.net;

using System;
using System.Runtime.Serialization;

namespace silkveil.net.Contracts
{
    /// <summary>
    /// Thrown when a unique constraint is violated.
    /// </summary>
    [Serializable]
    public class UniqueConstraintViolationException : SilkveilException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueConstraintViolationException"/> type.
        /// </summary>
        public UniqueConstraintViolationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueConstraintViolationException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public UniqueConstraintViolationException(string message)
            : base(message)
        {
            Enforce.NotNullOrEmpty(message, () => message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueConstraintViolationException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception</param>
        public UniqueConstraintViolationException(string message, Exception innerException)
            : base(message, innerException)
        {
            Enforce.NotNullOrEmpty(message, () => message);
            Enforce.NotNull(innerException, () => innerException);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueConstraintViolationException"/> type.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected UniqueConstraintViolationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}