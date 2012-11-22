using System;

namespace silkveil.net.Containers.Contracts
{
    /// <summary>
    /// Represents the exception that is thrown when an event can not be found by the container binder.
    /// </summary>
    [Serializable]
    public class EventNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventNotFoundException" /> type.
        /// </summary>
        public EventNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventNotFoundException" /> type.
        /// </summary>
        /// <param name="message">The message.</param>
        public EventNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventNotFoundException" /> type.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public EventNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}