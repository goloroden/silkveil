using cherryflavored.net;

using System;
using System.Runtime.Serialization;

namespace silkveil.net.Contracts
{
    /// <summary>
    /// Thrown when a constraint is violated.
    /// </summary>
    [Serializable]
    public class ConstraintViolationException : SilkveilException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintViolationException"/> type.
        /// </summary>
        public ConstraintViolationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintViolationException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ConstraintViolationException(string message)
            : base(message)
        {
            Enforce.NotNullOrEmpty(message, () => message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintViolationException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception</param>
        public ConstraintViolationException(string message, Exception innerException)
            : base(message, innerException)
        {
            Enforce.NotNullOrEmpty(message, () => message);
            Enforce.NotNull(innerException, () => innerException);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintViolationException"/> type.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected ConstraintViolationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}