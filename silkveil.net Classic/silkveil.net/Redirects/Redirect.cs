using cherryflavored.net.Contracts.Web;

using silkveil.net.Contracts.Authentication;
using silkveil.net.Contracts.Constraints;
using silkveil.net.Contracts.Redirects;

using System;
using System.Collections.Generic;

namespace silkveil.net.Redirects
{
    /// <summary>
    /// Represents a redirect.
    /// </summary>
    public class Redirect : IRedirect
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the redirection mode.
        /// </summary>
        /// <value>The redirection mode.</value>
        public RedirectMode RedirectMode
        {
            get;
            set;
        }

        /// <summary>
        /// The target for the redirect.
        /// </summary>
        /// <value>The target.</value>
        public Uri Uri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the authentication the user must pass.
        /// </summary>
        public IAuthentication SourceAuthentication
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of constraints for the download.
        /// </summary>
        /// <value>The list of constraints.</value>
        public ICollection<IConstraint> Constraints
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Redirect"/> type.
        /// </summary>
        public Redirect()
        {
            // Initialize the list of constraints.
            this.Constraints = new List<IConstraint>();
        }
    }
}