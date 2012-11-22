using System;
using System.IO;

namespace silkveil.net.ContentSources.Contracts
{
    /// <summary>
    /// Represents the base class for content sources such as a file system or an HTTP web server.
    /// </summary>
    public abstract class ContentSourceBase : IContentSource
    {
        /// <summary>
        /// Request content from the specified URL.
        /// </summary>
        /// <param name="uri">The content URL.</param>
        public void Request(Uri uri)
        {
            Stream stream;
            if (!this.RequestCore(uri, out stream))
            {
                return;
            }

            this.OnContentAvailable(stream);
        }

        /// <summary>
        /// Request content from the specified URL.
        /// </summary>
        /// <param name="uri">The content URL.</param>
        /// <param name="stream">The content stream.</param>
        /// <returns>
        /// <c>true</c> if this content source is responsible; <c>false</c> otherwise.
        /// </returns>
        protected abstract bool RequestCore(Uri uri, out Stream stream);

        /// <summary>
        /// Raises the ContentAvailable event.
        /// </summary>
        /// <param name="stream">The stream that contains the available content.</param>
        protected virtual void OnContentAvailable(Stream stream)
        {
            Action<Stream> contentAvailable = this.ContentAvailable;
            if(contentAvailable != null)
            {
                contentAvailable(stream);
            }
        }

        /// <summary>
        /// Raised when the content is available.
        /// </summary>
        public event Action<Stream> ContentAvailable;
    }
}