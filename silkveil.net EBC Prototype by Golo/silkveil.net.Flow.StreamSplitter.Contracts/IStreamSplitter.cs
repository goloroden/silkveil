using System;
using System.IO;
using silkveil.net.Core.Contracts;

namespace silkveil.net.Flow.StreamSplitter.Contracts
{
    /// <summary>
    /// Represents a component that splits a stream into stream parts.
    /// </summary>
    public interface IStreamSplitter
    {
        /// <summary>
        /// Splits the specified stream into stream parts.
        /// </summary>
        /// <param name="stream">The stream.</param>
        void Split(Stream stream);

        /// <summary>
        /// Raised when the next part of the stream is available.
        /// </summary>
        event Action<IStreamPart> StreamPartAvailable;
    }
}