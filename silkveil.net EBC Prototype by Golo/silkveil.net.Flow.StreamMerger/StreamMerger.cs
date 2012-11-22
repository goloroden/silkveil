using System;
using System.Collections.Generic;
using System.IO;
using silkveil.net.Core.Contracts;
using silkveil.net.Core;
using silkveil.net.Flow.StreamMerger.Contracts;

namespace silkveil.net.Flow.StreamMerger
{
    /// <summary>
    /// Represents a component that merges stream parts into one stream.
    /// </summary>
    public class StreamMerger : IStreamMerger
    {
        /// <summary>
        /// Merges the stream parts.
        /// </summary>
        /// <param name="streamParts">The stream parts.</param>
        public void Merge(IEnumerable<IStreamPart> streamParts)
        {
            this.OnStreamAvailable(StreamPart.Combine(streamParts));
        }

        /// <summary>
        /// Raises the StreamAvailable event.
        /// </summary>
        /// <param name="stream">The stream.</param>
        protected virtual void OnStreamAvailable(Stream stream)
        {
            var streamAvailable = this.StreamAvailable;
            if(streamAvailable != null)
            {
                streamAvailable(stream);
            }
        }

        /// <summary>
        /// Raised when the merged stream is available.
        /// </summary>
        public event Action<Stream> StreamAvailable;
    }
}