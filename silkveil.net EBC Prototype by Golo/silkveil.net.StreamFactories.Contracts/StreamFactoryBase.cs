using System;
using System.IO;

namespace silkveil.net.StreamFactories.Contracts
{
    /// <summary>
    /// Represents the base class for stream factories.
    /// </summary>
    public abstract class StreamFactoryBase : IStreamFactory
    {
        /// <summary>
        /// Raised when the requested stream is available.
        /// </summary>
        public event Action<Stream> StreamAvailable;

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
    }
}