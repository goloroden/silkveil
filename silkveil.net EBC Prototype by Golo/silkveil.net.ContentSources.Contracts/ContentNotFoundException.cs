using System;

namespace silkveil.net.ContentSources.Contracts
{
    /// <summary>
    /// Represents the exception that is thrown when content can not be found by a content source.
    /// </summary>
    [Serializable]
    public class ContentNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentNotFoundException" /> type.
        /// </summary>
        public ContentNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentNotFoundException" /> type.
        /// </summary>
        /// <param name="message">The message.</param>
        public ContentNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentNotFoundException" /> type.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ContentNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}