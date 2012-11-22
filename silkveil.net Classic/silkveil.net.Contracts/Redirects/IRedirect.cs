using cherryflavored.net.Contracts.Web;

using silkveil.net.Contracts.Authentication;
using silkveil.net.Contracts.Constraints;

using System;
using System.Collections.Generic;

namespace silkveil.net.Contracts.Redirects
{
    /// <summary>
    /// Contains methods for redirects.
    /// </summary>
    public interface IRedirect
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the redirect mode.
        /// </summary>
        /// <value>The redirect mode.</value>
        RedirectMode RedirectMode { get; set; }

        /// <summary>
        /// The target for the redirect.
        /// </summary>
        /// <value>The target.</value>
        Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the authentication the user must pass.
        /// </summary>
        IAuthentication SourceAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the list of constraints for the download.
        /// </summary>
        /// <value>The list of constraints.</value>
        ICollection<IConstraint> Constraints { get; }
    }
}