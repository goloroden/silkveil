using System;
using System.IO;
using silkveil.net.Core.Contracts;
using silkveil.net.Core;
using silkveil.net.Flow.StreamSplitter.Contracts;

namespace silkveil.net.Flow.StreamSplitter
{
    /// <summary>
    /// Represents a component that splits a stream into stream parts.
    /// </summary>
    public class StreamSplitter : IStreamSplitter
    {
        /// <summary>
        /// Splits the specified stream into stream parts.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Split(Stream stream)
        {
            foreach(var streamPart in StreamPart.Create(stream))
            {
                this.OnStreamPartAvailable(streamPart);
            }
        }

        /// <summary>
        /// Raises the StreamPartAvailableEvent.
        /// </summary>
        /// <param name="streamPart">The stream part.</param>
        protected virtual void OnStreamPartAvailable(IStreamPart streamPart)
        {
            var streamPartAvailable = this.StreamPartAvailable;
            if(streamPartAvailable != null)
            {
                streamPartAvailable(streamPart);
            }
        }

        /// <summary>
        /// Raised when the next part of the stream is available.
        /// </summary>
        public event Action<IStreamPart> StreamPartAvailable;
    }
}