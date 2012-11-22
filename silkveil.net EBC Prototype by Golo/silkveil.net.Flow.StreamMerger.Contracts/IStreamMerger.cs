using System;
using System.Collections.Generic;
using System.IO;
using silkveil.net.Core.Contracts;

namespace silkveil.net.Flow.StreamMerger.Contracts
{
    /// <summary>
    /// Represents a component that merges stream parts into one stream.
    /// </summary>
    public interface IStreamMerger
    {
        /// <summary>
        /// Merges the stream parts.
        /// </summary>
        /// <param name="streamParts">The stream parts.</param>
        void Merge(IEnumerable<IStreamPart> streamParts);

        /// <summary>
        /// Raised when the merged stream is available.
        /// </summary>
        event Action<Stream> StreamAvailable;
    }
}