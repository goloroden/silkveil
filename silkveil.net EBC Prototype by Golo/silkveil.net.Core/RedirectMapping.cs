using System;
using silkveil.net.Core.Contracts;

namespace silkveil.net.Core
{
    /// <summary>
    /// Represents a redirect mapping.
    /// </summary>
    public class RedirectMapping : IRedirectMapping
    {
        /// <summary>
        /// Gets or sets the GUID that identifies this redirect mapping.
        /// </summary>
        /// <value>A GUID.</value>
        public Guid Guid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URI that this redirect mapping points to.
        /// </summary>
        /// <value>An URI.</value>
        public Uri Uri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the redirect mapping.
        /// </summary>
        /// <value>The type of the redirect mapping.</value>
        public RedirectType RedirectType
        {
            get;
            set;
        }
    }
}