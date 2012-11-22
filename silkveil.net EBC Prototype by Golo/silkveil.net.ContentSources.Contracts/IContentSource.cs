using System;
using System.IO;

namespace silkveil.net.ContentSources.Contracts
{
    /// <summary>
    /// Represents a content source such as a file system or an HTTP web server.
    /// </summary>
    public interface IContentSource
    {
        /// <summary>
        /// Request content from the specified URL.
        /// </summary>
        /// <param name="uri">The content URL.</param>
        void Request(Uri uri);

        /// <summary>
        /// Raised when the content is available.
        /// </summary>
        event Action<Stream> ContentAvailable;
    }
}