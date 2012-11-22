using System;

namespace silkveil.net.Providers.Contracts
{
    /// <summary>
    /// Represents the exception that is thrown when a mapping already exists.
    /// </summary>
    [Serializable]
    public class DuplicateMappingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateMappingException" /> type.
        /// </summary>
        public DuplicateMappingException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateMappingException" /> type.
        /// </summary>
        /// <param name="message">The message.</param>
        public DuplicateMappingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateMappingException" /> type.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DuplicateMappingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}