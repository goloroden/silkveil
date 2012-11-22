using System;

namespace silkveil.net.Core.Contracts
{
    /// <summary>
    /// Represents a part of a stream.
    /// </summary>
    public interface IStreamPart : IDisposable
    {
        /// <summary>
        /// Gets the length of the data stored within this instance.
        /// </summary>
        /// <value>The length of the data stored within this instance.</value>
        int Length
        {
            get;
        }

        /// <summary>
        /// Gets or sets the value that should be stored.
        /// </summary>
        /// <value>The value that should be stored.</value>
        byte[] Value
        {
            get; 
            set;
        }
    }
}