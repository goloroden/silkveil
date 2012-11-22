using System;
using silkveil.net.Core.Contracts;

namespace silkveil.net.Core
{
    /// <summary>
    /// Represents a download mapping.
    /// </summary>
    public class DownloadMapping : IDownloadMapping
    {
        /// <summary>
        /// Gets or sets the GUID that identifies this download mapping.
        /// </summary>
        /// <value>A GUID.</value>
        public Guid Guid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URI that this download mapping points to.
        /// </summary>
        /// <value>An URI.</value>
        public Uri Uri
        {
            get;
            set;
        }
    }
}