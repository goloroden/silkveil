using System;

namespace silkveil.net.Core.Contracts
{
    /// <summary>
    /// Represents a mapping.
    /// </summary>
    public interface IMapping
    {
        /// <summary>
        /// Gets or sets the GUID that identifies this download mapping.
        /// </summary>
        /// <value>A GUID.</value>
        Guid Guid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URI that this download mapping points to.
        /// </summary>
        /// <value>An URI.</value>
        Uri Uri
        {
            get;
            set;
        }
    }
}