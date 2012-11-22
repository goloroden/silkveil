using System;

namespace silkveil.net.Providers.Contracts
{
    /// <summary>
    /// Represents the exception that is thrown when a mapping can not be found by a mapping provider.
    /// </summary>
    [Serializable]
    public class MappingNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingNotFoundException" /> type.
        /// </summary>
        public MappingNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingNotFoundException" /> type.
        /// </summary>
        /// <param name="message">The message.</param>
        public MappingNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingNotFoundException" /> type.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public MappingNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}