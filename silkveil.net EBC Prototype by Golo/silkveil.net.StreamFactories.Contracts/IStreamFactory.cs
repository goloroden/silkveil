using System;
using System.IO;

namespace silkveil.net.StreamFactories.Contracts
{
    /// <summary>
    /// Represents a stream factory.
    /// </summary>
    public interface IStreamFactory
    {
        /// <summary>
        /// Raised when the requested stream is available.
        /// </summary>
        event Action<Stream> StreamAvailable;
    }
}